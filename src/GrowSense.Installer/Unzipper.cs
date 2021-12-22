
using System.IO.Compression;
using System;
using System.IO;
namespace GrowSense.Installer
{
  public class Unzipper
  {
    public Unzipper()
    {
    }

    public void Unzip(string zipFile, string destination)
    {
      Console.WriteLine("Unzipping file...");
      Console.WriteLine("  Zip file: " + zipFile);
      Console.WriteLine("  Destination: " + destination);

      if (!Directory.Exists(destination))
        Directory.CreateDirectory(destination);

      foreach (var file in ZipFile.Open(zipFile, ZipArchiveMode.Read).Entries)
      {
        /*if (!String.IsNullOrEmpty(f.Name))
         {
           var destinationFileName = destination + "/" + f.Name;
           f.ExtractToFile(destinationFileName, true);
           //ZipFile.ExtractToDirectory(zipFile, destination);
         }*/


        string completeFileName = Path.Combine(destination, file.FullName);
        string directory = Path.GetDirectoryName(completeFileName);

        if (!Directory.Exists(directory))
          Directory.CreateDirectory(directory);

        if (file.Name != "")
          file.ExtractToFile(completeFileName, true);
      }
    }
  }
}
