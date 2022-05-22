using System;
using System.IO;
using System.IO.Compression;
using GrowSense.Installer.Web.GitHub;
using System.Collections.Generic;
using GrowSense.Installer.Web;

namespace GrowSense.Installer
{
    public class Installer
    {
        public Settings Settings;
        public ReleaseDownloader Downloader;
        public CoreCommandExecutor Executor;
        public Exiter Exiter;
        public InternalReleaseVersionAnalyser InternalVersionAnalyser;

        public Installer(Settings settings)
        {
            Settings = settings;
            Downloader = new ReleaseDownloader(settings);
            Executor = new CoreCommandExecutor(settings);
            Exiter = new Exiter(settings);
            InternalVersionAnalyser = new InternalReleaseVersionAnalyser(settings);
        }

        public void Install()
        {
            Console.WriteLine("Installing GrowSense...");

            EnsureDirectoryExists(Settings.ParentDirectory);

            var localZipFile = "";

            if (SkipReleaseDownload())
                localZipFile = GetInternalReleaseZipFilePath();
            else
                localZipFile = Downloader.DownloadLatestReleaseZipFile();

            var gsSettings = LoadExistingGrowSenseSettings();

            var indexDir = ExtractReleaseZip(localZipFile);

            SaveExistingGrowSenseSettings(gsSettings);

            Verify(indexDir);

            ExecutePostInstallActions(indexDir);

            Console.WriteLine("Finished installing to:");
            Console.WriteLine("  " + indexDir);
        }

        public void ExecutePostInstallActions(string indexDir)
        {
            Console.WriteLine("Executing post install actions...");
            Console.WriteLine("  Index dir: " + indexDir);
            Console.WriteLine("  Is test: " + Settings.IsTest);

            Executor.ExecuteCoreCommand("post-install --version=" + Settings.Version + " --mock-systemctl=" + Settings.IsTest + " --mock-docker=" + Settings.IsTest);

            if (Executor.IsError)
            {
                Console.WriteLine("Error: An error occurred during installation.");
                Exiter.Exit(1);
            }

            if (Executor.Starter.Output.IndexOf("GrowSense installation verified") == -1)
            {
                Console.WriteLine("Error: GrowSense installation was not verified.");
                Exiter.Exit(1);
            }

            Console.WriteLine("Finished execuing post install actions.");
        }

        public void Verify(string indexDir)
        {
            Console.WriteLine("Verify install...");
            Console.WriteLine("  Index dir: " + indexDir);

            if (!Directory.Exists(indexDir))
                throw new DirectoryNotFoundException("Install failed. GrowSense Index directory not found: " + indexDir);

            var gsScriptPath = Path.Combine(indexDir, "gs.sh");

            if (!File.Exists(gsScriptPath))
                throw new FileNotFoundException("Install failed. GrowSense Index gs.sh script not found: " + gsScriptPath);
        }

        public string GetInternalReleaseZipFilePath()
        {
            Console.WriteLine("Getting internal release zip file path...");

            //if (SkipReleaseDownload())
            //{
            //  Console.WriteLine("  Skipping download.");

            var latestVersion = Settings.Version;

            if (latestVersion == "latest")
                latestVersion = InternalVersionAnalyser.GetLatestVersionFromInternalReleaseZipFiles();

            var fileName = "GrowSense-Index." + latestVersion + "-" + Settings.Branch + ".zip";

            var internalZipFilePath = Path.Combine(Settings.InstallerDirectory, fileName);

            return internalZipFilePath;
            // }
        }


        public bool SkipReleaseDownload()
        {
            Console.WriteLine("Checking whether release zip download should be skipped...");

            var fileName = "GrowSenseIndex.zip";

            var localZipFilePath = Path.Combine(Settings.InstallerDirectory, fileName);

            if (!Settings.EnableDownload)
            {
                Console.WriteLine("  Download not enabled.");
                return true;
            }
            else if (File.Exists(localZipFilePath) && Settings.AllowSkipDownload)
            {
                Console.WriteLine("  Zip file exists.");
                return true;
            }
            else
            {
                Console.WriteLine("  Download should not be skipped.");
                return false;
            }
        }

        public void Reinstall()
        {
            Console.WriteLine("Reinstalling GrowSense...");

            Uninstall();

            Install();
        }

        public void Uninstall()
        {
            throw new NotImplementedException();
        }

        public string ExtractReleaseZip(string localZipFile)
        {
            var growSenseIndexDir = Path.Combine(Settings.GrowSenseDirectory, Settings.ProjectName);

            if (!growSenseIndexDir.TrimEnd('/').EndsWith("GrowSense/Index"))
                throw new Exception("Path doesn't end with GrowSense/Index: " + growSenseIndexDir);

            Console.WriteLine("  Extracting release zip...");
            Console.WriteLine("    Zip file: " + localZipFile);

            var zipExtractor = new Unzipper();

            zipExtractor.Unzip(localZipFile, growSenseIndexDir);

            var versionFile = growSenseIndexDir + "/full-version.txt";
            if (!File.Exists(versionFile))
                throw new Exception("Error: Didn't find version file at " + versionFile);

            var foundVersion = File.ReadAllText(versionFile).Trim();
            Console.WriteLine("    Version (found): " + foundVersion);

            if (Settings.VersionIsSpecified && Settings.Version != foundVersion)
            {
                throw new Exception("Versions don't match... Expected: " + Settings.Version + "; Found: " + foundVersion + ";");
            }

            return growSenseIndexDir;
        }


        public void EnsureDirectoryExists(string dir)
        {

            if (!Directory.Exists(dir))
            {
                Console.WriteLine("  Directory doesn't exist. Creating: " + dir);
                Directory.CreateDirectory(dir);
            }
            else
                Console.WriteLine("  Install directory exists.");
        }

        public Dictionary<string, string> LoadExistingGrowSenseSettings()
        {
            var settings = new Dictionary<string, string>();

            LoadExistingGrowSenseSetting(settings, "supervisor-upgrade-frequency");

            return settings;
        }

        public void SaveExistingGrowSenseSettings(Dictionary<string, string> gsSettings)
        {
            SaveExistingGrowSenseSetting(gsSettings, "supervisor-upgrade-frequency");
        }

        public void LoadExistingGrowSenseSetting(Dictionary<string, string> gsSettings, string settingKey)
        {
            var filePath = Path.GetFullPath(Settings.IndexDirectory + "/" + settingKey + ".txt");

            if (File.Exists(filePath))
                gsSettings.Add(settingKey, File.ReadAllText(filePath).Trim());
        }

        public void SaveExistingGrowSenseSetting(Dictionary<string, string> gsSettings, string settingKey)
        {
            if (gsSettings.ContainsKey(settingKey))
            {
                var filePath = Path.GetFullPath(Settings.IndexDirectory + "/" + settingKey + ".txt");

                File.WriteAllText(filePath, gsSettings[settingKey]);
            }
        }

    }
}
