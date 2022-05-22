using System;
using System.IO;
using GrowSense.Installer.Web.GitHub;
namespace GrowSense.Installer
{
    public class Upgrader
    {
        public Installer Installer;
        public CoreCommandExecutor Executor;
        public Settings Settings;
        public ReleaseIdentifier Identifier;

        public Upgrader(Settings settings)
        {
            Installer = new Installer(settings);
            Executor = new CoreCommandExecutor(settings);
            Identifier = new ReleaseIdentifier(settings);
            Settings = settings;
        }

        public bool Upgrade()
        {
            if (NeedsUpgrade())
            {
                Console.WriteLine("Upgrading GrowSense...");

                PreUpgrade();

                Installer.Install();

                PostUpgrade();

                Console.WriteLine("GrowSense upgrade complete.");

                return true;
            }
            else
            {
                Console.WriteLine("Upgrade not required. Skipping upgrade.");

                return false;
            }
        }

        public void PreUpgrade()
        {
            Console.WriteLine("Preparing to upgrade GrowSense...");

            Console.WriteLine("  Stopping GrowSense system services...");

            Executor.ExecuteCoreCommand("stop");

            Console.WriteLine("Finished preparing to upgrade GrowSense.");
        }

        public void PostUpgrade()
        {
            // Start not required because services are started during upgrade
        }

        public bool NeedsUpgrade()
        {
            Console.WriteLine("Checking if GrowSense needs upgrade...");

            var newVersion = Settings.Version;

            if (newVersion == "latest")
                newVersion = DetectLatestVersion();

            var existingVersion = File.ReadAllText(Settings.IndexDirectory + "/full-version.txt").Trim();

            Console.WriteLine("  Current version: " + existingVersion);
            Console.WriteLine("  New version: " + newVersion);

            if (Settings.Force)
            {
                Console.WriteLine("  Forcing upgrade.");
                return true;
            }

            if (newVersion != existingVersion)
            {
                Console.WriteLine("  New version available. Needs upgrade.");
                return true;
            }
            else
            {
                Console.WriteLine("  Versions match. Upgrade not required.");
                return false;
            }

        }

        public string DetectLatestVersion()
        {
            Identifier.Initialize(Settings.Branch, Settings.Version);

            return Identifier.Version.Trim();
        }
    }
}