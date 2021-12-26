using System;
using System.IO;
using System.IO.Compression;
using GrowSense.Installer.GitHub;
namespace GrowSense.Installer
{
  public class Installer
  {
    public Settings Settings;
    public ReleaseDownloader Downloader;
    public CoreCommandExecutor Executor;
    public Exiter Exiter;
    
    public Installer(Settings settings)
    {
      Settings = settings;
      Downloader = new ReleaseDownloader(settings);
      Executor = new CoreCommandExecutor(settings);
      Exiter = new Exiter(settings);
    }

    public void Install()
    {
      Console.WriteLine("Installing GrowSense...");

      EnsureDirectoryExists(Settings.ParentDirectory);

      var localZipFile = GetReleaseZipFile();

      var indexDir = ExtractReleaseZip(localZipFile);

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

    public string GetReleaseZipFile()
    {
      if (SkipReleaseDownload())
      {
        Console.WriteLine("  Skipping download.");

        var fileName = "GrowSenseIndex.zip";

        var localZipFilePath = Path.Combine(Settings.InstallerDirectory, fileName);

        return localZipFilePath;
      }
      else
      {
        var releaseIdentifier = new ReleaseIdentifier();
        releaseIdentifier.Initialize(Settings.Branch, Settings.Version);

        if (!Settings.VersionIsSpecified)
          Settings.Version = releaseIdentifier.Version;

        return Downloader.DownloadRelease(releaseIdentifier.ReleaseUrl);
      }
    }

    public bool SkipReleaseDownload()
    {
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
        return false;
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
  }
}
