﻿using System;
using System.IO;
using GrowSense.Installer.GitHub;

namespace GrowSense.Installer
{
  class MainClass
  {
    public static void Main(string[] args)
    {
    //Environment.SetEnvironmentVariable("MONO_TLS_PROVIDER", "btls");
    
      Console.WriteLine("Preparing to install GrowSense...");

      var arguments = new Arguments(args);

      var settings = new Settings();
      //var versionDetector = new VersionDetector(settings);


      var settingsExtractor = new SettingsArgumentsExtractor();
      settingsExtractor.ExtractArgumentsFromSettings(arguments, settings);

      FixBaseInstallDirectory(settings);

      if (String.IsNullOrEmpty(settings.Branch))
      {
        Console.WriteLine("  Branch argument not specified. Choosing dev branch.");
      }

      if (String.IsNullOrEmpty(settings.ParentDirectory))
      {
        throw new ArgumentException("Error: The install-to argument was not specified.");
      }

        

      //var releaseUrl = releaseIdentifier.GetLatestReleaseUrl(Settings.Branch);

      /*if (!settings.AllowSkipDownload || !File.Exists(settings.InstallerDirectory + "/GrowSenseIndex.zip"))
      {
        settings.Version = versionDetector.Detect();
      }
      else
        Console.WriteLine("  Skipping detect version");*/


      if (arguments.KeylessArguments.Length == 0)
      {
        Console.WriteLine("  Please specify a command as an argument: install, upgrade, uninstall, reinstall");
        Environment.Exit(1);
      }

      var command = arguments.KeylessArguments[0];

      var installer = new Installer(settings);

      switch (command)
      {
        case "install":
          installer.Install();
          break;
        case "upgrade":
          installer.Upgrade();
          break;
        case "uninstall":
          installer.Uninstall();
          break;
        case "reinstall":
          installer.Reinstall();
          break;
        default:
          Console.WriteLine("  Unknown command: " + command);
          Environment.Exit(1);
          break;
      }
    }

    static public void FixBaseInstallDirectory(Settings settings)
    {
      if (settings.ParentDirectory.TrimEnd('/').EndsWith("Index"))
        settings.ParentDirectory = Path.GetDirectoryName(settings.ParentDirectory.TrimEnd('/'));
        
      if (settings.ParentDirectory.TrimEnd('/').EndsWith("GrowSense"))
        settings.ParentDirectory = Path.GetDirectoryName(settings.ParentDirectory.TrimEnd('/'));
    }
  }
}
