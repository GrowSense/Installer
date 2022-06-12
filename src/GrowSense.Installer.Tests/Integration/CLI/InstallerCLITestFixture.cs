
using System;
using NUnit.Framework;
using System.IO;
namespace GrowSense.Installer.Tests.Integration.CLI
{
    [TestFixture]
    public class InstallerCLITestFixture : BaseTestFixture
    {
        [Test]
        public void Test_Install_VersionLatest()
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

            var cmd = String.Format("mono GSInstaller.exe install --to=../ --version=latest --test=true --allow-skip-download=true --github-username={0} --github-token={1}", settings.GitHubUsername, settings.GitHubToken);
            starter.Start(cmd);

            Assert.IsFalse(starter.IsError, "An error occurred");
        }

        [Test]
        public void Test_Install_VersionNotSpecified()
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

            var cmd = String.Format("mono GSInstaller.exe install --to=../ --test=true --allow-skip-download=true --github-username={0} --github-token={1}", settings.GitHubUsername, settings.GitHubToken);
            starter.Start(cmd);

            Assert.IsFalse(starter.IsError, "An error occurred");
        }
    }
}
