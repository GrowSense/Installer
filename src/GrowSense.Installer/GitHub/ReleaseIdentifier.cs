using System;
using Newtonsoft.Json;
using System.Linq;
namespace GrowSense.Installer.GitHub
{
  public class ReleaseIdentifier
  {
    public ReleaseIdentifier()
    {
    }

    public string GetLatestReleaseUrl(string branch)
    {
      var request = new WebRequestHelper();
      var json = request.HttpGet("https://api.github.com/repos/GrowSense/Index/releases");

      var releases = JsonConvert.DeserializeObject<ReleaseInfo[]>(json);
      var matchingReleases = releases.Where(r => r.TagName.IndexOf(branch) > -1).ToArray();
      var latestRelease = matchingReleases.OrderByDescending(r => r.CreatedAt).FirstOrDefault();
      return latestRelease.AssetsUrl.ToString();
    }
  }
}
