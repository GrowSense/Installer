using System;
using System.IO;

namespace GrowSense.Installer
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Preparing to install GrowSense...");

      var arguments = new Arguments(args);

      var settings = new Settings();
      var versionDetector = new VersionDetector(settings);

      settings.Branch = arguments["branch"];
      settings.ParentDirectory = Path.GetFullPath(arguments["install-to"]);
      if (arguments.Contains("to")) // Argument alias
        settings.ParentDirectory = Path.GetFullPath(arguments["to"]);

      FixBaseInstallDirectory(settings);

      if (String.IsNullOrEmpty(settings.Branch))
      {
        Console.WriteLine("  Branch argument not specified. Choosing dev branch.");
      }

      if (String.IsNullOrEmpty(settings.ParentDirectory))
      {
        throw new ArgumentException("Error: The install-to argument was not specified.");
      }

      if (arguments.Contains("enable-download"))
        settings.EnableDownload = Convert.ToBoolean(arguments["enable-download"]);
      else
        settings.EnableDownload = true;

      if (arguments.Contains("allow-skip-download"))
        settings.AllowSkipDownload = Convert.ToBoolean(arguments["allow-skip-download"]);

      if (arguments.Contains("version"))
        settings.Version = arguments["version"].Replace(".", "-");

      if (!settings.AllowSkipDownload || !File.Exists(settings.InstallerDirectory + "/GrowSenseIndex.zip"))
      {
        settings.Version = versionDetector.Detect();
      }
      else
        Console.WriteLine("  Skipping detect version");

      if (arguments.Contains("test"))
        settings.IsTest = Convert.ToBoolean(arguments["test"]);


      Console.WriteLine("  Branch: " + settings.Branch);
      Console.WriteLine("  Parent install dir: " + settings.ParentDirectory);
      Console.WriteLine("  GrowSense base dir: " + settings.GrowSenseDirectory);
      Console.WriteLine("  GrowSense index dir: " + settings.IndexDirectory);
      Console.WriteLine("  Installer dir: " + settings.InstallerDirectory);
      Console.WriteLine("  Version (target): " + settings.Version);
      Console.WriteLine("  Allow skip download (if file is found locally): " + settings.AllowSkipDownload);


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
