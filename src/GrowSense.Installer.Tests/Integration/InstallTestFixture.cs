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
      MoveToTemporaryDirectory();

      //PullInstaller();
      
      PullGrowSenseIndexRelease();

      var version = GetGrowSenseVersion();

      var starter = new ProcessStarter();

      var destination = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));

      var branchDetector = new BranchDetector(Environment.CurrentDirectory);
      var branch = branchDetector.Branch;

      //starter.Start("mono GSInstaller.exe install --to=" + destination + " --branch=" + branch + " --enable-download=false --allow-skip-download=true");

      var settings = new Settings();
      settings.Branch = branch;
      settings.ParentDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory));
      //settings.EnableDownload = false;
      //settings.AllowSkipDownload = true;
      settings.IsTest = true;
      //settings.Version = version;
      
      var installer = new Installer(settings);
      installer.Install();

      //Assert.IsFalse(starter.IsError, "An error occurred");
    }
  }
}
