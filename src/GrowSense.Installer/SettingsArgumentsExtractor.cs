using System;
using System.IO;
namespace GrowSense.Installer
{
  public class SettingsArgumentsExtractor
  {
    public SettingsArgumentsExtractor()
    {
    }
    
    public void ExtractArgumentsFromSettings(Arguments arguments, Settings settings)
    {
      settings.Branch = arguments["branch"];
      
      settings.ParentDirectory = Path.GetFullPath(arguments["install-to"]);
      if (arguments.Contains("to")) // Argument alias
        settings.ParentDirectory = Path.GetFullPath(arguments["to"]);
        
      if (arguments.Contains("enable-download"))
        settings.EnableDownload = Convert.ToBoolean(arguments["enable-download"]);
      else
        settings.EnableDownload = true;

      if (arguments.Contains("allow-skip-download"))
        settings.AllowSkipDownload = Convert.ToBoolean(arguments["allow-skip-download"]);

      if (arguments.Contains("version"))
        settings.Version = arguments["version"].Replace(".", "-");
                
      if (arguments.Contains("test"))
        settings.IsTest = Convert.ToBoolean(arguments["test"]);

      Console.WriteLine("  Branch: " + settings.Branch);
      Console.WriteLine("  Parent install dir: " + settings.ParentDirectory);
      Console.WriteLine("  GrowSense base dir: " + settings.GrowSenseDirectory);
      Console.WriteLine("  GrowSense index dir: " + settings.IndexDirectory);
      Console.WriteLine("  Installer dir: " + settings.InstallerDirectory);
      Console.WriteLine("  Version (target): " + (settings.VersionIsSpecified ? settings.Version : "latest"));
      Console.WriteLine("  Allow skip download (if file is found locally): " + settings.AllowSkipDownload);
    }
  }
}
