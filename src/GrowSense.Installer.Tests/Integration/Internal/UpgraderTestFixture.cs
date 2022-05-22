using System;
using NUnit.Framework;
using System.IO;
using GrowSense.Installer.Tests.Web.GitHub;

namespace GrowSense.Installer.Tests.Integration.Internal
{
    [TestFixture]
    public class UpgraderTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_Upgrade_NewVersionAvailable_PerformsUpgrade()
        {
            ForceDownload = false; // Set this to true to test the download functionality. Otherwise leave it as false for faster tests.
            ForceUpgrade = false;

            var supervisorUpgradeFrequency = 10;

            var version = "latest"; //GetGrowSenseVersion(branch);      
            var branch = GetBranch();

            MoveToTemporaryDirectory();

            if (UseLocalGrowSenseIndex())
            {
                CreateGrowSenseIndexReleaseZipAndPullToInstallerDirectory(version);
            }
            else
                Console.WriteLine("  Skipping create release zip from local index files...");

            var settings = GetSettings(branch, version);

            // Install GrowSense Index to prepare for an upgrade
            var installer = new Installer(settings);
            installer.Install();

            // Set the installed version to a low value to force an upgrade
            SetGrowSenseIndexVersion(settings.IndexDirectory, "0-0-0-1");

            // Apply settings to the GrowSense index
            File.WriteAllText(Path.GetFullPath("../Index/supervisor-upgrade-frequency.txt"), supervisorUpgradeFrequency.ToString());

            // Run the upgrader
            var upgrader = new Upgrader(settings);
            var didUpgrade = upgrader.Upgrade();


            Assert.IsTrue(didUpgrade, "Didn't upgrade when it should have");


            var detectedVersion = File.ReadAllText(settings.IndexDirectory + "/full-version.txt");

            Console.WriteLine("  Version after upgrade: " + detectedVersion);

            Assert.AreNotEqual(detectedVersion, "0-0-0-1");

            // Check the settings were kept
            Assert.AreEqual(supervisorUpgradeFrequency.ToString(), File.ReadAllText(Path.GetFullPath("../Index/supervisor-upgrade-frequency.txt")), "Supervisor upgrade frequency was reset after upgrade");
        }


        [Test]
        public void Test_Upgrade_NewVersionNotAvailable_SkipsUpgrade()
        {
            ForceDownload = false; // Set this to true to test the download functionality. Otherwise leave it as false for faster tests.
            ForceUpgrade = false;

            var version = "latest"; //GetGrowSenseVersion(branch);      
            var branch = GetBranch();

            MoveToTemporaryDirectory();

            if (UseLocalGrowSenseIndex())
            {
                CreateGrowSenseIndexReleaseZipAndPullToInstallerDirectory(version);
            }
            else
                Console.WriteLine("  Skipping create release zip from local index files...");

            var settings = GetSettings(branch, version);

            // Install GrowSense Index to prepare for an upgrade
            var installer = new Installer(settings);
            installer.Install();

            var currentVersion = File.ReadAllText(Path.GetFullPath("../Index/full-version.txt"));

            var upgrader = new Upgrader(settings);
            upgrader.Identifier = new MockReleaseIdentifier(settings, currentVersion);

            // Run the upgrader
            var didUpgrade = upgrader.Upgrade();

            Assert.IsFalse(didUpgrade, "Upgraded when it shouldn't.");
        }
    }
}
