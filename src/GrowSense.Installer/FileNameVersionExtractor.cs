using System;
namespace GrowSense.Installer
{
  public class FileNameVersionExtractor
  {
    public Settings Settings;
    
    public FileNameVersionExtractor(Settings settings)
    {
      Settings = settings;
    }

    public string ExtractVersionFromFileName(string fileName)
    {
      var version = fileName.Replace("GrowSense", "").Replace("Index", "").Replace("-" + Settings.Branch, "").Trim('.').Trim('-').Trim('.');

      return version;
    }
  }
}
