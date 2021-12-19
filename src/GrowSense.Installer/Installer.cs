using System;
using System.IO;
using System.IO.Compression;
namespace GrowSense.Installer
{
  public class Installer
  {
    public Settings Settings;
    public ReleaseDownloader Downloader;
    public Installer(Settings settings)
    {
      Settings = settings;
      Downloader = new ReleaseDownloader(settings);
    }

    public void Install()
    {
      Console.WriteLine("Installing GrowSense...");

      EnsureDirectoryExists(Settings.ParentDirectory);

      var localZipFile = DownloadRelease();

      var indexDir = ExtractReleaseZip(localZipFile);

      Verify(indexDir);

      ExecutePostInstallActions(indexDir);
      //var releaseFinder = new ReleaseFinder(Settings);

      //var output = releaseFinder.FindReleaseUrl();

      //Console.WriteLine(output);

      Console.WriteLine("Finished installing to:");
      Console.WriteLine("  " + indexDir);
    }

    public void ExecutePostInstallActions(string indexDir)
    {
      Console.WriteLine("Executing post install actions...");
      Console.WriteLine("  Index dir: " + indexDir);
      
      var starter = new ProcessStarter(indexDir); 
      starter.Start("bash gs.sh post-install --version=" + Settings.Version + " --mock-systemctl=true");
      Console.WriteLine(starter.Output);

      if (starter.IsError)
      {
        Console.WriteLine("Error: An error occurred during installation.");
        Exit(1);
      }

      if (starter.Output.IndexOf("GrowSense installation verified") == -1)
      {
        Console.WriteLine("Error: GrowSense installation was not verified.");
        Exit(1);
      }

      Console.WriteLine("Finished execuing post install actions.");
    }

    private void Exit(int v)
    {
      if (!Settings.IsTest)
        Environment.Exit(1);
      else
        throw new Exception("Failed");
    }

    public void Verify(string indexDir)
    {
      Console.WriteLine("Verify install...");
      Console.WriteLine("  Index dir: " + indexDir);

      if (!Directory.Exists(indexDir))
        throw new DirectoryNotFoundException("Install failed. GrowSense Index directory not found: " + indexDir);

      var gsScriptPath = Path.Combine(indexDir, "gs.sh");

      if (!File.Exists(gsScriptPath))
        throw new FileNotFoundException("Install failed. GrowSense Index gs.sh script not found: " + gsScriptPath);
    }

    public string DownloadRelease()
    {
      return Downloader.DownloadRelease();
    }

    public void Reinstall()
    {
      Console.WriteLine("Reinstalling GrowSense...");

      Uninstall();

      Install();
    }

    public void Uninstall()
    {
      throw new NotImplementedException();
    }

    public void Upgrade()
    {
      Console.WriteLine("Upgrading GrowSense...");
    
      Preupgrade();
      throw new Exception("sdf");
      Install();
    }

    public void Preupgrade()
    {
    }

    public string ExtractReleaseZip(string localZipFile)
    {
      var growSenseIndexDir = Path.Combine(Settings.GrowSenseDirectory, Settings.ProjectName);

      if (!growSenseIndexDir.TrimEnd('/').EndsWith("GrowSense/Index"))
        throw new Exception("Path doesn't end with GrowSense/Index: " + growSenseIndexDir);

      Console.WriteLine("  Extracting release zip...");
      Console.WriteLine("    Zip file: " + localZipFile);
      
      var zipExtractor = new Unzipper();

      zipExtractor.Unzip(localZipFile, growSenseIndexDir);
      
      Console.WriteLine("    Version (found): " + File.ReadAllText(growSenseIndexDir + "/full-version.txt"));

      return growSenseIndexDir;
    }
    

    public void EnsureDirectoryExists(string dir)
    {

      if (!Directory.Exists(dir))
      {
        Console.WriteLine("  Directory doesn't exist. Creating: " + dir);
        Directory.CreateDirectory(dir);
      }
      else
        Console.WriteLine("  Install directory exists.");
    }
  }
}
