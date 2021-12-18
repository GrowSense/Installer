using System;
using System.IO;
namespace GrowSense.Installer
{
  public class Settings
  {
    public string ProjectFamily = "GrowSense";
    public string ProjectName = "Index";
    public string Branch;
    public string SourcePath;
    public string ParentDirectory;
    public string GrowSenseDirectory
    {
      get { return Path.Combine(ParentDirectory, ProjectFamily); }
    }
    public string InstallerDirectory
    {
      get { return Path.Combine(GrowSenseDirectory, "Installer"); }
    }
    public string IndexDirectory
    {
      get { return Path.Combine(GrowSenseDirectory, ProjectName); }
    }


    public string Version { get; internal set; }
    public bool AllowSkipDownload { get; internal set; }
  }
}
