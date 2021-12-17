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
      settings.BaseInstallDirectory = Path.GetFullPath(arguments["install-to"]);

      FixBaseInstallDirectory(settings);

      if (String.IsNullOrEmpty(settings.Branch))
      {
        Console.WriteLine("  Branch argument not specified. Choosing dev branch.");
      }

      if (String.IsNullOrEmpty(settings.BaseInstallDirectory))
      {
        throw new ArgumentException("Error: The install-to argument was not specified.");
      }
      
      
      settings.Version = arguments.Contains("version")
        ? arguments["version"]
        : versionDetector.Detect();

      settings.AllowSkipDownload = arguments.Contains("allow-skip-download")
        ? Convert.ToBoolean(arguments["allow-skip-download"])
        : false;

      Console.WriteLine("  Branch: " + settings.Branch);
      Console.WriteLine("  Install dir: " + settings.BaseInstallDirectory);
      Console.WriteLine("  Version: " + settings.Version);

      var installer = new Installer(settings);

      installer.Install();
    }

    static public void FixBaseInstallDirectory(Settings settings)
    {
      if (settings.BaseInstallDirectory.TrimEnd('/').EndsWith("Index"))
        settings.BaseInstallDirectory = Path.GetDirectoryName(settings.BaseInstallDirectory.TrimEnd('/'));
    }
  }
}
