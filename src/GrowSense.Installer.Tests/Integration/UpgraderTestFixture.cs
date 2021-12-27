using System;
using NUnit.Framework;
using System.IO;

namespace GrowSense.Installer.Tests.Integration
{
[TestFixture]
  public class UpgraderTestFixture : BaseTestFixture
  {
    [Test]
    public void Test_Upgrade()
    {
      ForceDownload = false; // Set this to true to test the download functionality. Otherwise leave it as false for faster tests.
      ForceUpgrade = false;
      
      var branchDetector = new BranchDetector(ProjectDirectory);
      var branch = branchDetector.Branch;
      
      MoveToTemporaryDirectory();

      var version = "latest"; //GetGrowSenseVersion(branch);
      
      PullGrowSenseIndexReleaseZip(version);

      var destination = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));

      var settings = new Settings();
      settings.Branch = branch;
      settings.ParentDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));
      //settings.EnableDownload = false; // Commented out so release zips can be downloaded on the build server. Uncomment to force use of local zip on development workstation.
      settings.AllowSkipDownload = true;
      settings.IsTest = true;
      settings.Version = version;
      
      var installer = new Installer(settings);
      installer.Install();

      File.WriteAllText(settings.IndexDirectory + "/full-version.txt", "0-0-0-1");
      
      var upgrader = new Upgrader(settings);
      upgrader.Upgrade();

      //Assert.IsFalse(starter.IsError, "An error occurred");
    }
  }
}
