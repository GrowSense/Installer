using System;
namespace GrowSense.Installer
{
  public class ReleaseFinder
  {
    public ReleaseFinder(Settings settings)
    {
    }

    public string FindReleaseUrl()
    {
      throw new NotImplementedException();
   /*var client = new GitHubClient(new ProductHeaderValue("GrowSense"), new Uri("https://github.com/"));
      var releases = client.Repository.Release.GetAll("octokit", "octokit.net").Result;
var latest = releases[0];
Console.WriteLine(
    "The latest release is tagged at {0} and is named {1}", 
    latest.TagName, 
    latest.Name);

      throw new NotImplementedException();*/
      /*var requestHelper = new WebRequestHelper();
      var output = requestHelper.HttpGet("https://api.github.com/repos/GrowSense/Index/releases");

      return output;*/
    }
  }
}
