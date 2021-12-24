using System;
using NUnit.Framework;
using System.IO;
namespace GrowSense.Installer.Tests.Integration
{
[TestFixture]
  public class InstallTestFixture : BaseTestFixture
  {
    [Test]
    public void Test_Install()
    {
      ForceDownload = false; // Set this to true to test the download functionality. Otherwise leave it as false for faster tests.
      
      var branchDetector = new BranchDetector(ProjectDirectory);
      var branch = branchDetector.Branch;
      
      MoveToTemporaryDirectory();

      var version = "latest"; //GetGrowSenseVersion(branch);
      
      PullGrowSenseIndexReleaseZip(version);

      var starter = new ProcessStarter();

      var destination = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));

      var settings = new Settings();
      settings.Branch = branch;
      settings.ParentDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));
      //settings.EnableDownload = false;
      settings.AllowSkipDownload = true;
      settings.IsTest = true;
      settings.Version = version;
      
      var installer = new Installer(settings);
      installer.Install();

      //Assert.IsFalse(starter.IsError, "An error occurred");
    }
  }
}
