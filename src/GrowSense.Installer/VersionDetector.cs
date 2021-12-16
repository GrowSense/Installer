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
      var requestHelper = new WebRequestHelper();
      var version = requestHelper.HttpGet("https://raw.githubusercontent.com/" + Settings.ProjectFamily + "/" + Settings.ProjectName + "/" + Settings.Branch + "/full-version.txt").Trim();
      return version;
    }
  }
}
