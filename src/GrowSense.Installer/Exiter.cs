using System;
namespace GrowSense.Installer
{
  public class Exiter
  {
    public Settings Settings;
    
    public Exiter(Settings settings)
    {
      Settings = settings;
    }
    
    public void Exit(int exitCode)
    {
      if (!Settings.IsTest)
        Environment.Exit(exitCode);
      else
        throw new Exception("Failed");
    }
  }
}
