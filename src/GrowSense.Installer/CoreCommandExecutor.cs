using System;
namespace GrowSense.Installer
{
  public class CoreCommandExecutor
  {
    public ProcessStarter Starter;
    public Settings Settings;
    public bool IsError
    {
      get { return Starter.IsError; }
    }
    
    public CoreCommandExecutor(Settings settings)
    {
      Settings = settings;
      Starter = new ProcessStarter(settings.IndexDirectory);
    }

    public void ExecuteCoreCommand(string command)
    {
      if (!command.StartsWith("bash gs.sh"))
        command = "bash gs.sh " + command;
        
      Starter.Start(command);
    }
  }
}
