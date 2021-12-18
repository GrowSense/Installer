using System;
namespace GrowSense.Installer
{
  public class VersionDetector
  {
    public Settings Settings;
    public VersionDetector(Settings settings)
    {
      Settings = settings;
    }

    public string Detect()
    {
      Console.WriteLine("  Detecting GrowSense version from GitHub repo...");
      
      var requestHelper = new WebRequestHelper();
      var version = requestHelper.HttpGet("https://raw.githubusercontent.com/" + Settings.ProjectFamily + "/" + Settings.ProjectName + "/" + Settings.Branch + "/full-version.txt").Trim();

      Console.WriteLine("    Version: " + version);
      return version;
    }
  }
}
