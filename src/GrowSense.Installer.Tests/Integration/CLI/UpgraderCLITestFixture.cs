﻿
using System;
using NUnit.Framework;
using System.IO;
namespace GrowSense.Installer.Tests.Integration.CLI
{
    [TestFixture]
    public class UpgraderCLITestFixture : BaseTestFixture
    {
        [Test]
        public void Test_Upgrade()
        {
            ForceDownload = false; // Set this to true to test the download functionality. Otherwise leave it as false for faster tests.

            var version = "latest";
            var branch = GetBranch();

            MoveToTemporaryDirectory();

            if (UseLocalGrowSenseIndex())
            {
                CreateGrowSenseIndexReleaseZipAndPullToInstallerDirectory(version);
            }
            else
                Console.WriteLine("  Skipping create release zip from local index files...");

            var starter = new ProcessStarter();

            var destination = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));

            var settings = GetSettings(branch, version);

            var releaseMode = "Release";
#if DEBUG
            releaseMode = "Debug";
#endif

            PullFileFromProject("bin/" + releaseMode + "/GSInstaller.exe", true);
            PullFileFromProject("bin/" + releaseMode + "/Newtonsoft.Json.dll", true);

            starter.Start("mono GSInstaller.exe install --to=../ --version=latest --test=true");

            Assert.IsFalse(starter.IsError, "An error occurred");

            // Set the installed version to a low value to force an upgrade
            SetGrowSenseIndexVersion(settings.IndexDirectory, "0-0-0-1");

            starter.Start("mono GSInstaller.exe upgrade --to=../ --test=true");

            Assert.IsFalse(starter.IsError, "An error occurred");

            var detectedVersion = File.ReadAllText(settings.IndexDirectory + "/full-version.txt");

            Console.WriteLine("  Version after upgrade: " + detectedVersion);

            Assert.AreNotEqual(detectedVersion, "0-0-0-1");

        }
    }
}
