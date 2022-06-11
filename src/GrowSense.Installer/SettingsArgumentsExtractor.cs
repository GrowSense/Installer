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

            settings.ParentDirectory = ExtractParentDirectory(arguments);

            if (arguments.Contains("enable-download"))
                settings.EnableDownload = Convert.ToBoolean(arguments["enable-download"]);
            else
                settings.EnableDownload = true;

            if (arguments.Contains("allow-skip-download"))
                settings.AllowSkipDownload = Convert.ToBoolean(arguments["allow-skip-download"]);

            if (arguments.Contains("version"))
                settings.Version = arguments["version"].Replace(".", "-");

            if (arguments.Contains("force"))
                settings.Force = Convert.ToBoolean(arguments["force"]);

            if (arguments.Contains("test"))
                settings.IsTest = Convert.ToBoolean(arguments["test"]);

            if (arguments.Contains("github-username"))
                settings.GitHubUsername = arguments["github-username"];

            if (arguments.Contains("github-token"))
                settings.GitHubToken = arguments["github-token"];

            Console.WriteLine("  Branch: " + settings.Branch);
            Console.WriteLine("  Parent install dir: " + settings.ParentDirectory);
            Console.WriteLine("  GrowSense base dir: " + settings.GrowSenseDirectory);
            Console.WriteLine("  GrowSense index dir: " + settings.IndexDirectory);
            Console.WriteLine("  Installer dir: " + settings.InstallerDirectory);
            Console.WriteLine("  GitHub Username: " + settings.GitHubUsername);
            Console.WriteLine("  Version (target): " + (settings.VersionIsSpecified ? settings.Version : "latest"));
            Console.WriteLine("  Allow skip download (if file is found locally): " + settings.AllowSkipDownload);
        }

        public string ExtractParentDirectory(Arguments arguments)
        {
            var parentDirectory = "/usr/local/GrowSense";

            if (arguments.Contains("install-to"))
            {
                try
                {
                    parentDirectory = Path.GetFullPath(arguments["install-to"]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: Invalid 'install-to' argument...");
                    Console.WriteLine(arguments["install-to"]);
                    Console.WriteLine(ex.ToString());
                }
            }
            if (arguments.Contains("to")) // Argument alias
            {

                try
                {
                    parentDirectory = Path.GetFullPath(arguments["to"]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: Invalid 'to' argument...");
                    Console.WriteLine(arguments["to"]);
                    Console.WriteLine(ex.ToString());
                }
            }

            return parentDirectory;
        }
    }
}