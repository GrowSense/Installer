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
      get { return Path.Combine(ParentDirectory.TrimEnd('/'), ProjectFamily); }
    }
    public string InstallerDirectory
    {
      get { return Path.Combine(GrowSenseDirectory, "Installer"); }
    }
    public string IndexDirectory
    {
      get { return Path.Combine(GrowSenseDirectory, ProjectName); }
    }

    public bool IsTest { get; set; }

    public bool EnableDownload = true;

    public string Version = "0.0.0.0";
    public bool VersionIsSpecified
    {
      get { return !String.IsNullOrEmpty(Version) && Version != "0.0.0.0" && Version != "latest"; }
    }
    
    public bool AllowSkipDownload = false;
  }
}
