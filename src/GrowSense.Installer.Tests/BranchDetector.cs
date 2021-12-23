using System;
namespace GrowSense.Installer.Tests
{
  public class BranchDetector
  {
    public string Branch { get; set; }

    public BranchDetector (string projectDirectory)
    {
      Initialize (projectDirectory);
    }

    public void Initialize (string projectDirectory)
    {
      Console.WriteLine("Detecting branch from git directory...");
      Console.WriteLine("  " + projectDirectory);
      
      var cmd = "/bin/bash -c \"echo $(git branch | sed -n -e 's/^\\* \\(.*\\)/\\1/p')\"";
      var starter = new ProcessStarter (projectDirectory);
      starter.WriteOutputToConsole = false;
      starter.Start (cmd);

      Validate(starter.Output, projectDirectory);
      
      Branch = starter.Output.Trim ();

      Console.WriteLine("  Branch: " + Branch);
    }

    public void Validate(string output, string projectDirectory)
    {
      if (output.IndexOf("not a git repository") > -1)
        throw new Exception("Can't detect git repository. Directory is not a git repository: " + projectDirectory);
    }
  }
}

