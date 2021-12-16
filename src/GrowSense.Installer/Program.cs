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
      
      settings.Version = arguments.Contains("version")
        ? arguments["version"]
        : versionDetector.Detect();

      settings.Version = "1-2-1-33";

      settings.AllowSkipDownload = arguments.Contains("allow-skip-download")
        ? Convert.ToBoolean(arguments["allow-skip-download"])
        : false;

      Console.WriteLine("  Branch: " + settings.Branch);
      Console.WriteLine("  Install dir: " + settings.BaseInstallDirectory);
      Console.WriteLine("  Version: " + settings.Version);

      var installer = new Installer(settings);

      installer.Install();
    }
  }
}
