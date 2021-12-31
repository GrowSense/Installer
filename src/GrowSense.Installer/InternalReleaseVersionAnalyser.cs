using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace GrowSense.Installer
{
  public class InternalReleaseVersionAnalyser
  {
    public Settings Settings;

    public FileNameVersionExtractor VersionExtractor;
    
    public InternalReleaseVersionAnalyser(Settings settings)
    {
      Settings = settings;
      VersionExtractor = new FileNameVersionExtractor(settings);
    }
    
    public string GetLatestVersionFromInternalReleaseZipFiles()
    {
      var versions = new List<string>();

      var releaseZipFilePaths = Directory.GetFiles(Settings.InstallerDirectory, "GrowSense-Index*.zip");

      if (releaseZipFilePaths.Length == 0)
        throw new FileNotFoundException("No local release zip file found at " + Settings.InstallerDirectory);

      foreach (var releaseFilePath in releaseZipFilePaths)
      {
        var fileName = Path.GetFileNameWithoutExtension(releaseFilePath);
        var version = VersionExtractor.ExtractVersionFromFileName(fileName);

        versions.Add(version.Replace("-", "."));
      }

      return versions.OrderByDescending(v => Version.Parse(v)).FirstOrDefault().Replace(".", "-");
    }
  }
}
