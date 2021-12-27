using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace GrowSense.Installer
{
  public class LocalReleaseVersionAnalyser
  {
    public Settings Settings;

    public FileNameVersionExtractor VersionExtractor;
    
    public LocalReleaseVersionAnalyser(Settings settings)
    {
      Settings = settings;
      VersionExtractor = new FileNameVersionExtractor(settings);
    }
    
    public string GetLatestVersionFromLocalReleaseZipFiles()
    {
      var versions = new List<string>();

      foreach (var releaseFilePath in Directory.GetFiles(Settings.InstallerDirectory, "GrowSense-Index*.zip"))
      {
        var fileName = Path.GetFileNameWithoutExtension(releaseFilePath);
        var version = VersionExtractor.ExtractVersionFromFileName(fileName);

        versions.Add(version.Replace("-", "."));
      }

      return versions.OrderByDescending(v => Version.Parse(v)).FirstOrDefault().Replace(".", "-");
    }
  }
}
