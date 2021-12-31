using System;
using NUnit.Framework;
using System.IO;
namespace GrowSense.Installer.Tests.Integration
{
[TestFixture]
  public class InstallerTestFixture : BaseTestFixture
  {
    [Test]
    public void Test_Install()
    {
      ForceDownload = false; // Set this to true to test the download functionality. Otherwise leave it as false for faster tests.
      
      
      var version = "latest"; //GetGrowSenseVersion(branch);
      var branch = GetBranch();
      
      MoveToTemporaryDirectory();

// TODO: Clean up
      //CreateGrowSenseIndexReleaseZipAndPullToInstallerDirectory(version);
      
      if (UseLocalGrowSenseIndex())
      {
        CreateGrowSenseIndexReleaseZipAndPullToInstallerDirectory(version);
      }
      else
        Console.WriteLine("  Skipping create release zip from local index files...");

      var starter = new ProcessStarter();

      var destination = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));

      var settings = GetSettings(branch, version);
      
      var installer = new Installer(settings);
      installer.Install();

      //Assert.IsFalse(starter.IsError, "An error occurred");
    }
  }
}
