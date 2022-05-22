using System;
using NUnit.Framework;
using System.IO;

namespace GrowSense.Installer.Tests.Integration.Internal
{
[TestFixture]
  public class UpgraderTestFixture : BaseTestFixture
  {
    [Test]
    public void Test_Upgrade()
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

      // Set the installed version to a low value to force an upgrade
      SetGrowSenseIndexVersion(settings.IndexDirectory, "0-0-0-1");
      
      // Run the upgrader
      var upgrader = new Upgrader(settings);
      upgrader.Upgrade();
    }
  }
}
