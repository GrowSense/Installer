using System;
using Newtonsoft.Json;
using System.Linq;
namespace GrowSense.Installer.GitHub
{
  public class ReleaseIdentifier
  {
    public string ReleaseUrl;
    public string Version;
    
    public ReleaseIdentifier()
    {
    }
    
    public void Initialize(string branch, string version)
    {
      var request = new WebRequestHelper();
      var json = request.HttpGet("https://api.github.com/repos/GrowSense/Index/releases");

      var releases = JsonConvert.DeserializeObject<ReleaseInfo[]>(json);
      var matchingReleases = releases.Where(r => r.TagName.IndexOf(branch) > -1).ToArray();
      
      ReleaseInfo latestRelease = null;

      if (String.IsNullOrEmpty(version) || version == "latest")
        latestRelease = matchingReleases.OrderByDescending(r => r.CreatedAt).FirstOrDefault();
      else
        latestRelease = matchingReleases.Where(r => r.TagName.IndexOf(version) > -1).FirstOrDefault();
        
      ReleaseUrl = latestRelease.AssetsUrl.ToString();
      Version = latestRelease.TagName.Replace("v", "").Replace("-" + branch, "");
    }
  }
}
