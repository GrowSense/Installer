using System;
namespace GrowSense.Installer
{
  public class Upgrader
  {
    public Installer Installer;
    
    public Upgrader(Settings settings)
    {
      Installer = new Installer(settings);
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
      throw new NotImplementedException();
    }

    public void PostUpgrade()
    {
      throw new NotImplementedException();
    }
  }
}
