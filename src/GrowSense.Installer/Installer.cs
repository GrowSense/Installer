﻿using System;
using System.IO;
using System.IO.Compression;
namespace GrowSense.Installer
{
  public class Installer
  {
    public Settings Settings;
    public FileDownloader Downloader = new FileDownloader();
    public Installer(Settings settings)
    {
      Settings = settings;
    }

    public void Install()
    {
      Console.WriteLine("Installing GrowSense...");

      EnsureDirectoryExists(Settings.BaseInstallDirectory);

      var localZipFile = DownloadRelease();

      var indexDir = ExtractReleaseZip(localZipFile);

      ExecutePostInstallActions(indexDir);
      //var releaseFinder = new ReleaseFinder(Settings);

      //var output = releaseFinder.FindReleaseUrl();

      //Console.WriteLine(output);

      Console.WriteLine("Finished installing to:");
      Console.WriteLine("  " + indexDir);
    }

    public void ExecutePostInstallActions(string indexDir)
    {
      var starter = new ProcessStarter(indexDir); 
      starter.Start("bash gs.sh post-install");
      Console.WriteLine(starter.Output);
    }

    public string DownloadRelease()
    {
    
      var releaseUrl = "https://github.com/" + Settings.ProjectFamily + "/" + Settings.ProjectName + "/releases/download/v" + Settings.Version + "-" + Settings.Branch + "/" + Settings.ProjectFamily + "-" + Settings.ProjectName + "." + Settings.Version + "-" + Settings.Branch + ".zip";

      var fileName = "GrowSenseIndex.zip";

      var installerDir = Path.Combine(Settings.BaseInstallDirectory, "Installer");

      EnsureDirectoryExists(installerDir);

      var localZipFilePath = Path.Combine(Settings.BaseInstallDirectory, "Installer/" + fileName);
      //destination = "test.zip"; //Path.GetFullPath("test.zip");

      if (File.Exists(localZipFilePath) && Settings.AllowSkipDownload)
        Console.WriteLine("  Zip file exists. Skipping download...");
      else
        Downloader.Download(releaseUrl, localZipFilePath);

      return localZipFilePath;
    }

    public string ExtractReleaseZip(string localZipFile)
    {
    
      var zipExtractor = new Unzipper();

      var growSenseIndexDir = Path.Combine(Settings.BaseInstallDirectory, Settings.ProjectName);

      zipExtractor.Unzip(localZipFile, growSenseIndexDir);

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