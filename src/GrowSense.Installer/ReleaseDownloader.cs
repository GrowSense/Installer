using System;
using System.IO;
namespace GrowSense.Installer
{
  public class ReleaseDownloader
  {
    public Settings Settings;
    public FileDownloader Downloader = new FileDownloader();
    
    public ReleaseDownloader(Settings settings)
    {
      Settings = settings;
    }

    public string DownloadRelease()
    {
      if (String.IsNullOrEmpty(Settings.InstallerDirectory))
        throw new Exception("Settings.InstallerDirectory is not set.");

      var releaseUrl = "https://github.com/" + Settings.ProjectFamily + "/" + Settings.ProjectName + "/releases/download/v" + Settings.Version + "-" + Settings.Branch + "/" + Settings.ProjectFamily + "-" + Settings.ProjectName + "." + Settings.Version + "-" + Settings.Branch + ".zip";

      var fileName = "GrowSenseIndex.zip";

      var installerDir = Settings.InstallerDirectory;

      if (!installerDir.TrimEnd('/').EndsWith("GrowSense/Installer"))
        throw new Exception("Path doesn't end with GrowSense/Installer: " + installerDir);

      if (!Directory.Exists(installerDir))
        Directory.CreateDirectory(installerDir);

      var localZipFilePath = Path.Combine(Settings.InstallerDirectory, fileName);
      //destination = "test.zip"; //Path.GetFullPath("test.zip");

      if (!Settings.EnableDownload)
        Console.WriteLine("  Download not enabled. Skipping download.");
      else if (File.Exists(localZipFilePath) && Settings.AllowSkipDownload)
        Console.WriteLine("  Zip file exists. Skipping download...");
      else
        Downloader.Download(releaseUrl, localZipFilePath);

      return localZipFilePath;
    }
  }
}
