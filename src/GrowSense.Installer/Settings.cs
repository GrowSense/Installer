using System;
namespace GrowSense.Installer
{
  public class Settings
  {
    public string ProjectFamily = "GrowSense";
    public string ProjectName = "Index";
    public string Branch;
    public string SourcePath;
    public string BaseInstallDirectory;

    public string Version { get; internal set; }
    public bool AllowSkipDownload { get; internal set; }
  }
}
