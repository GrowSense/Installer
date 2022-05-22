using System;
using System.IO;
using GrowSense.Installer.Web.GitHub;

namespace GrowSense.Installer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Preparing to install GrowSense...");

            var arguments = new Arguments(args);

            var settings = new Settings();
            //var versionDetector = new VersionDetector(settings);


            var settingsExtractor = new SettingsArgumentsExtractor();
            settingsExtractor.ExtractArgumentsFromSettings(arguments, settings);

            FixBaseInstallDirectory(settings);

            VerifySettingsAndArguments(settings, arguments);

            var command = arguments.KeylessArguments[0];

            switch (command)
            {
                case "install":
                    var installer = new Installer(settings);
                    installer.Install();
                    break;
                case "upgrade":
                    var upgrader = new Upgrader(settings);
                    upgrader.Upgrade();
                    break;
                case "uninstall":
                    var uninstaller = new Uninstaller(settings);
                    uninstaller.Uninstall();
                    break;
                case "reinstall":
                    var reinstaller = new Reinstaller(settings);
                    reinstaller.Reinstall();
                    break;
                case "version":
                    Console.WriteLine("");
                    Console.WriteLine("Installer version: " + File.ReadAllText("full-version.txt"));
                    break;
                case "help":
                    ConsoleWriteHelp();
                    break;
                default:
                    Console.WriteLine("  Unknown command: " + command);
                    Environment.Exit(1);
                    break;
            }
        }

        static public void FixBaseInstallDirectory(Settings settings)
        {
            if (settings.ParentDirectory.TrimEnd('/').EndsWith("Index"))
                settings.ParentDirectory = Path.GetDirectoryName(settings.ParentDirectory.TrimEnd('/'));

            if (settings.ParentDirectory.TrimEnd('/').EndsWith("GrowSense"))
                settings.ParentDirectory = Path.GetDirectoryName(settings.ParentDirectory.TrimEnd('/'));
        }

        static public void VerifySettingsAndArguments(Settings settings, Arguments arguments)
        {
            if (String.IsNullOrEmpty(settings.Branch))
            {
                Console.WriteLine("  Branch argument not specified. Using dev branch...");
                settings.Branch = "dev";
            }

            if (String.IsNullOrEmpty(settings.ParentDirectory))
            {
                Console.WriteLine("  Parent directory not specified. Using /usr/local/...");
                settings.ParentDirectory = "/usr/local";
            }

            if (arguments.KeylessArguments.Length == 0)
            {
                Console.WriteLine("  Please specify a command as an argument: install, upgrade, uninstall, reinstall");
                Environment.Exit(1);
            }
        }

        static public void ConsoleWriteHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("GrowSense Installer Help");
            Console.WriteLine("");
            Console.WriteLine("Commands:");
            Console.WriteLine("");
            Console.WriteLine("- install - Installs the GrowSense system");
            Console.WriteLine("- upgrade - Upgrades the GrowSense system");
            Console.WriteLine("- uninstall - Uninstalls the GrowSense system");
            Console.WriteLine("- reinstall - Reinstalls the GrowSense system");
            Console.WriteLine("- version - Displays the installer version");
            Console.WriteLine("- help - Displays this help info");
        }
    }
}