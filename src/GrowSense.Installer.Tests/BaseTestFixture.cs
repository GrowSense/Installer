using System;
using System.IO;
using NUnit.Framework;
using GrowSense.Installer.Web.GitHub;
using GrowSense.Installer.Web;


namespace GrowSense.Installer.Tests
{
    public class BaseTestFixture
    {
        public string ProjectDirectory;
        public string TemporaryDirectory;
        public TemporaryDirectoryCreator TmpDirCreator = new TemporaryDirectoryCreator();
        public bool ForceDownload = false;
        public bool ForceUpgrade = false;

        //public TestHelper Helper = new TestHelper();

        [SetUp]
        public void Initialize()
        {
            Console.WriteLine("");
            Console.WriteLine("=== Starting test");
            Console.WriteLine("Test: " + TestContext.CurrentContext.Test.FullName);

            var dir = Environment.CurrentDirectory;

            dir = dir.Replace("/tests/nunit/bin/Release", "");
            dir = dir.Replace("/tests/nunit/bin/Debug", "");

            dir = dir.Replace("/bin/Release", "");
            dir = dir.Replace("/bin/Debug", "");

            ProjectDirectory = dir;
            Console.WriteLine("Project directory: ");
            Console.WriteLine("  " + ProjectDirectory);
            Console.WriteLine("");

            Directory.SetCurrentDirectory(ProjectDirectory);

            ClearDevices();
        }

        /*public void CopyDirectory (string source, string destination)
        {

          var starter = new ProcessStarter ();
          starter.Start ("rsync -arh --exclude='.git' --exclude='.pioenvs' " + source + "/ " + destination + "/");
          Console.WriteLine (starter.Output);
        }*/


        public void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }


        [TearDown]
        public void Finish()
        {
            Directory.SetCurrentDirectory(ProjectDirectory);

            Console.WriteLine("=== Finished test");
            Console.WriteLine("");
        }

        public void ClearDevices()
        {
            // Clear all devices for the test
            var devicesPath = Path.GetFullPath("devices");
            if (Directory.Exists(devicesPath))
                Directory.Delete(devicesPath, true);
        }

        public TestProcessStarter GetTestProcessStarter()
        {
            return GetTestProcessStarter(true, Environment.CurrentDirectory);
        }

        public TestProcessStarter GetTestProcessStarter(string workingDirectory)
        {
            return GetTestProcessStarter(true, workingDirectory);
        }

        public TestProcessStarter GetTestProcessStarter(bool initializeStarter, string workingDirectory)
        {
            var starter = new TestProcessStarter();

            starter.WorkingDirectory = workingDirectory;

            if (initializeStarter)
                starter.Initialize();

            return starter;
        }

        public void CheckDeviceInfoWasCreated(string deviceBoard, string deviceGroup, string deviceProject, string deviceLabel, string deviceName, string devicePort)
        {
            var devicesDir = Path.GetFullPath("devices");
            var deviceDir = Path.Combine(devicesDir, deviceName);

            Console.WriteLine("Device dir:");
            Console.WriteLine(deviceDir);

            var deviceDirExists = Directory.Exists(deviceDir);

            Assert.IsTrue(deviceDirExists, "Device directory not found: " + deviceDir);

            var foundBoard = File.ReadAllText(Path.Combine(deviceDir, "board.txt")).Trim();
            Assert.AreEqual(deviceBoard, foundBoard, "Device board doesn't match.");

            var foundGroup = File.ReadAllText(Path.Combine(deviceDir, "group.txt")).Trim();
            Assert.AreEqual(deviceGroup, foundGroup, "Device group doesn't match.");

            var foundProject = File.ReadAllText(Path.Combine(deviceDir, "project.txt")).Trim();
            Assert.AreEqual(deviceProject, foundProject, "Device project doesn't match.");

            var foundLabel = File.ReadAllText(Path.Combine(deviceDir, "label.txt")).Trim();
            Assert.AreEqual(deviceLabel, foundLabel, "Device label doesn't match.");

            var foundName = File.ReadAllText(Path.Combine(deviceDir, "name.txt")).Trim();
            Assert.AreEqual(deviceName, foundName, "Device name doesn't match.");

            var foundPort = File.ReadAllText(Path.Combine(deviceDir, "port.txt")).Trim();
            Assert.AreEqual(devicePort, foundPort, "Device port doesn't match.");
        }

        public void CheckMqttBridgeServiceFileWasCreated(string deviceName)
        {
            var serviceFile = Path.Combine(GetServicesDirectory(), "growsense-mqtt-bridge-" + deviceName + ".service");

            var fileExists = File.Exists(serviceFile);

            Assert.IsTrue(fileExists, "MQTT bridge service file not created: " + serviceFile);
        }

        public void CheckFlagWasCreated(string deviceName)
        {
            var deviceInfoDir = Path.Combine(ProjectDirectory, "devices/" + deviceName);

            var flagFile = Path.Combine(deviceInfoDir, "is-ui-created.txt");

            var fileExists = File.Exists(flagFile);

            Assert.IsTrue(fileExists, "'UI is created' flag file not found: " + flagFile);
        }

        public string GetServicesDirectory()
        {
            var servicesDirectory = "";
            if (File.Exists(Path.GetFullPath(Path.Combine(ProjectDirectory, "is-mock-systemctl.txt"))))
            {
                servicesDirectory = Path.Combine(ProjectDirectory, "mock/services");
            }
            else
            {
                servicesDirectory = "/lib/systemd/system/";
            }
            return servicesDirectory;
        }

        public void PullFileFromProject(string fileName)
        {
            PullFileFromProject(fileName, false);
        }

        public void PullFileFromProject(string fileName, bool removeDestinationDirectory)
        {
            var sourceFile = Path.Combine(ProjectDirectory, fileName);
            var destinationFile = Path.Combine(Environment.CurrentDirectory, fileName);

            if (removeDestinationDirectory)
            {
                var shortenedFileName = Path.GetFileName(fileName);
                destinationFile = Path.Combine(Environment.CurrentDirectory, shortenedFileName);
            }

            if (!Directory.Exists(Path.GetDirectoryName(destinationFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));

            File.Copy(sourceFile, destinationFile);
        }

        public void PullDirectoryFromProject(string directoryPath)
        {
            var sourceDir = ProjectDirectory + "/" + directoryPath;
            var destinationDir = TemporaryDirectory + "/" + directoryPath;

            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            CopyDirectory(sourceDir, destinationDir, true);
        }

        public void MoveToProjectDirectory()
        {
            Directory.SetCurrentDirectory(ProjectDirectory);
        }

        public void MoveToTemporaryDirectory()
        {
            var tmpTestDir = TmpDirCreator.Create(ProjectDirectory);

            if (!Directory.Exists(tmpTestDir))
                Directory.CreateDirectory(tmpTestDir);

            TemporaryDirectory = tmpTestDir;

            Directory.SetCurrentDirectory(tmpTestDir);
        }

        public void CleanTemporaryDirectory()
        {
            var tmpDir = TemporaryDirectory;

            Directory.SetCurrentDirectory(ProjectDirectory);

            Console.WriteLine("Cleaning temporary directory:");
            Console.WriteLine(tmpDir);

            //Directory.Delete (tmpDir, true);
        }

        public void EnableMocking(string path, string key)
        {
            var filePath = Path.GetFullPath(path) + "/is-mock-" + key + ".txt";
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, 1.ToString());

        }

        public string GetBranch()
        {
            Console.WriteLine("Getting git branch...");
            var branchDetector = new BranchDetector(ProjectDirectory);
            return branchDetector.Branch;
        }

        public void CreateGrowSenseIndexReleaseZipAndPullToInstallerDirectory(string version)
        {
            Console.WriteLine("Creating GrowSense Index release zip file...");

            var testIndexPath = Path.GetFullPath(Environment.CurrentDirectory + "/../../Index");

            Console.WriteLine("  Test index path: " + testIndexPath);

            var sourceIndexPath = GetSourceIndexPath();

            var starter = new ProcessStarter(sourceIndexPath);
            starter.Start("bash build-cli.sh");
            starter.Start("bash create-release-zip.sh");

            var sourceReleaseFilePath = Directory.GetFiles(sourceIndexPath + "/releases/")[0];

            var releaseFileName = Path.GetFileName(sourceReleaseFilePath);

            var destinationReleaseFilePath = Environment.CurrentDirectory + "/" + releaseFileName;

            File.Copy(sourceReleaseFilePath, destinationReleaseFilePath);
        }

        public bool UseLocalGrowSenseIndex()
        {
            Console.WriteLine("Checking if local GrowSense Index files should be used...");

            var sourceIndexPath = GetSourceIndexPath();

            var indexIsFoundLocally = Directory.Exists(sourceIndexPath);

            Console.WriteLine("  GrowSense Index is found locally: " + indexIsFoundLocally);
            Console.WriteLine("  Force download: " + ForceDownload);

            var useLocal = indexIsFoundLocally && !ForceDownload;

            Console.WriteLine("  Use local GrowSense Index files: " + useLocal);

            return useLocal;
        }

        public string GetSourceIndexPath()
        {
            var sourceIndexPath = Path.GetFullPath(Environment.CurrentDirectory + "/../../../../Index");

            Console.WriteLine("  Source index path: " + sourceIndexPath);

            return sourceIndexPath;
        }

        public void PullInstaller()
        {
#if DEBUG
            var release = "Debug";
#else
      var release = "Release";
#endif
            var installerPath = ProjectDirectory + "/bin/" + release + "/GSInstaller.exe";
            File.Copy(installerPath, TemporaryDirectory + "/GSInstaller.exe");
        }

        public string GetGrowSenseVersion(string branch)
        {

            Console.WriteLine("Getting GrowSense Index version...");

            var testIndexPath = Path.GetFullPath(Environment.CurrentDirectory + "/../Index");

            Console.WriteLine("  Test index path: " + testIndexPath);

            var sourceIndexPath = Path.GetFullPath(Environment.CurrentDirectory + "/../../../../Index");

            Console.WriteLine("  Source index path: " + sourceIndexPath);

            var indexIsFoundLocally = Directory.Exists(sourceIndexPath);

            var version = "";
            // If found locally (on dev machine)
            if (indexIsFoundLocally && !ForceDownload)
            {
                Console.WriteLine("  From local files...");
                Console.WriteLine("    " + sourceIndexPath);

                var versionPath = sourceIndexPath + "/full-version.txt";
                version = File.ReadAllText(versionPath).Trim();
            }
            else // On build server, pull it from GitHub
            {
                Console.WriteLine("  From GitHub repository...");
                var settings = new Settings();
                settings.Branch = new BranchDetector(ProjectDirectory).Branch;
                //var versionDetector = new VersionDetector(settings);
                version = "latest"; //versionDetector.Detect();
            }

            Console.WriteLine("  Version: " + version);

            return version;

        }

        public Settings GetSettings(string branch, string version)
        {

            var indexDir = Path.Combine(ProjectDirectory, "../Index");

            var isOnDevEnvironment = Directory.Exists(indexDir);

            var destination = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));

            var settings = new Settings();
            settings.Branch = branch;
            settings.ParentDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));
            settings.IsTest = true;
            settings.Version = version;

            if (ForceDownload)
            {
                settings.EnableDownload = true;
                settings.AllowSkipDownload = false;
            }
            else
            {
                settings.EnableDownload = !isOnDevEnvironment; // When on local dev environment, disable downloads to ensure release is pulled from local GrowSense Index
                settings.AllowSkipDownload = true;
            }

            settings.GitHubUsername = GetEnvironmentVariable("GITHUB_USERNAME");
            settings.GitHubToken = GetEnvironmentVariable("GITHUB_TOKEN");

            return settings;
        }

        public void SetGrowSenseIndexVersion(string indexDirectory, string version)
        {
            File.WriteAllText(indexDirectory + "/full-version.txt", "0-0-0-1");
        }

        public string GetEnvironmentVariable(string name)
        {
            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                return Environment.GetEnvironmentVariable(name);

            var envFilePath = ProjectDirectory + "/.env/" + name + ".txt";

            if (File.Exists(envFilePath))
                return File.ReadAllText(envFilePath).Trim();

            return String.Empty;
        }
    }
}
