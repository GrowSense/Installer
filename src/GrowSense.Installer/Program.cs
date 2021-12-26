using System;
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

      VerifySettingsAndArguments(settings, arguments);

        // TODO: Remove if not needed. Should be obsolete

      //var releaseUrl = releaseIdentifier.GetLatestReleaseUrl(Settings.Branch);

      /*if (!settings.AllowSkipDownload || !File.Exists(settings.InstallerDirectory + "/GrowSenseIndex.zip"))
      {
        settings.Version = versionDetector.Detect();
      }
      else
        Console.WriteLine("  Skipping detect version");*/



      var command = arguments.KeylessArguments[0];

      switch (command)
      {
        case "install":
          var installer = new Installer(settings);
          installer.Install();
          break;
        case "upgrade":
          var upgrader = new Upgrader(settings);
          upgrader.Upgrade();
          break;
        case "uninstall":
          var uninstaller = new Uninstaller(settings);
          uninstaller.Uninstall();
          break;
        case "reinstall":
          var reinstaller = new Reinstaller(settings);
          reinstaller.Reinstall();
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

    static public void VerifySettingsAndArguments(Settings settings, Arguments arguments)
    {
      if (String.IsNullOrEmpty(settings.Branch))
      {
        Console.WriteLine("  Branch argument not specified. Using dev branch...");
        settings.Branch = "dev";
      }

      if (String.IsNullOrEmpty(settings.ParentDirectory))
      {
        Console.WriteLine("  Parent directory not specified. Using /usr/local/...");
        settings.ParentDirectory = "/usr/local";
      }

      if (arguments.KeylessArguments.Length == 0)
      {
        Console.WriteLine("  Please specify a command as an argument: install, upgrade, uninstall, reinstall");
        Environment.Exit(1);
      }
    }
  }
}
