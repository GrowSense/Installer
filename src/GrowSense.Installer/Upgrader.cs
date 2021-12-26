using System;
namespace GrowSense.Installer
{
  public class Upgrader
  {
    public Installer Installer;
    public CoreCommandExecutor Executor;
    
    public Upgrader(Settings settings)
    {
      Installer = new Installer(settings);
      Executor = new CoreCommandExecutor(settings);
    }

    public void Upgrade()
    {
      Console.WriteLine("Upgrading GrowSense...");
    
      PreUpgrade();
      
      Installer.Install();

      PostUpgrade();

      Console.WriteLine("GrowSense upgrade complete.");
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
      
    }
  }
}
