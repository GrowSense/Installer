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

      ReleaseInfo latestRelease = null;

      var versionIsSpecified = !String.IsNullOrEmpty(version) && version != "latest";

      if (versionIsSpecified)
        latestRelease = GetSpecificReleaseForVersion(branch, version);
      else
        latestRelease = GetLatestRelease(branch, version);

      if (latestRelease == null || latestRelease.Assets.Length == 0)
      {
        throw new Exception("Can't find release for version '" + version + "' and branch '" + branch + "'.");
      }

      ReleaseUrl = latestRelease.Assets[0].BrowserDownloadUrl.ToString();
      Version = latestRelease.TagName.Replace("-" + branch, "").Replace("v", "");
    }

    public ReleaseInfo GetLatestRelease(string branch, string version)
    {
      var matchingReleases = GetReleasesForBranch(branch);

      return matchingReleases.OrderByDescending(r => r.CreatedAt).Where(r => r.Assets.Length > 0).FirstOrDefault();
    }


    public ReleaseInfo GetSpecificReleaseForVersion(string branch, string version)
    {
      var matchingReleases = GetReleasesForBranch(branch);

      return matchingReleases.Where(r => r.TagName.IndexOf(version) > -1 && r.Assets.Length > 0).FirstOrDefault();
    }

    public ReleaseInfo[] GetReleasesForBranch(string branch)
    {
      var request = new WebRequestHelper();
      var json = request.HttpGet("https://api.github.com/repos/GrowSense/Index/releases");

      var releases = JsonConvert.DeserializeObject<ReleaseInfo[]>(json);
      var matchingReleases = releases.Where(r => r.TagName.IndexOf(branch) > -1).ToArray();

      return matchingReleases;
    }
  }
}
