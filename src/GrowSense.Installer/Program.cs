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


      Console.WriteLine("  Branch: " + settings.Branch);
      Console.WriteLine("  Parent install dir: " + settings.ParentDirectory);
      Console.WriteLine("  GrowSense base dir: " + settings.GrowSenseDirectory);
      Console.WriteLine("  GrowSense index dir: " + settings.IndexDirectory);
      Console.WriteLine("  Installer dir: " + settings.InstallerDirectory);
      Console.WriteLine("  Version (target): " + settings.Version);
      Console.WriteLine("  Allow skip download (if file is found locally): " + settings.AllowSkipDownload);

      var installer = new Installer(settings);

      installer.Install();
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
