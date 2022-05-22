using System;
using System.IO;
using GrowSense.Installer.Web.GitHub;

namespace GrowSense.Installer.Web
{
    public class ReleaseDownloader
    {
        public Settings Settings;
        public FileDownloader Downloader = new FileDownloader();
        public ReleaseIdentifier Identifier;

        public ReleaseDownloader(Settings settings)
        {
            Settings = settings;
            Identifier = new ReleaseIdentifier(settings);
        }

        public string DownloadLatestReleaseZipFile()
        {
            Console.WriteLine("  Downloading latest release zip file...");

            Identifier.Initialize(Settings.Branch, Settings.Version);

            if (!Settings.VersionIsSpecified)
                Settings.Version = Identifier.Version;

            Console.WriteLine("    Version: " + Identifier.Version);
            Console.WriteLine("    URL: " + Identifier.ReleaseUrl);

            return DownloadRelease(Identifier.ReleaseUrl);
        }

        public string DownloadRelease(string releaseUrl)
        {
            Console.WriteLine("  Downloading release...");
            Console.WriteLine("    Release URL: " + releaseUrl);

            if (String.IsNullOrEmpty(Settings.InstallerDirectory))
                throw new Exception("Settings.InstallerDirectory is not set.");

            var fileName = Path.GetFileName(releaseUrl);

            var installerDir = Settings.InstallerDirectory;

            if (!installerDir.TrimEnd('/').EndsWith("GrowSense/Installer"))
                throw new Exception("Path doesn't end with GrowSense/Installer: " + installerDir);

            if (!Directory.Exists(installerDir))
                Directory.CreateDirectory(installerDir);

            var localZipFilePath = Path.Combine(Settings.InstallerDirectory, fileName);

            Downloader.Download(releaseUrl, localZipFilePath);

            return localZipFilePath;
        }
    }
}