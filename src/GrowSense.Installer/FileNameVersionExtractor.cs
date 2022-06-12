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

            Console.WriteLine("    Extracting version from file name...");
            Console.WriteLine("      File name: " + fileName);
            Console.WriteLine("      Version: " + version);

            return version;
        }
    }
}
