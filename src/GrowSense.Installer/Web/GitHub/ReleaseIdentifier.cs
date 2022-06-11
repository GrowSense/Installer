using System;
using Newtonsoft.Json;
using System.Linq;
using GrowSense.Installer;

namespace GrowSense.Installer.Web.GitHub
{
    public class ReleaseIdentifier
    {
        public string ReleaseUrl;
        public string Version;
        public Settings Settings;

        public ReleaseIdentifier(Settings settings)
        {
            Settings = settings;
        }

        public virtual void Initialize(string branch, string version)
        {

            ReleaseInfo latestRelease = null;

            var versionIsSpecified = !String.IsNullOrEmpty(version) && version != "0.0.0.0" && version != "latest";

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

        public virtual ReleaseInfo GetLatestRelease(string branch, string version)
        {
            var matchingReleases = GetReleasesForBranch(branch);

            return matchingReleases.OrderByDescending(r => r.CreatedAt).Where(r => r.Assets.Length > 0).FirstOrDefault();
        }


        public virtual ReleaseInfo GetSpecificReleaseForVersion(string branch, string version)
        {
            var matchingReleases = GetReleasesForBranch(branch);

            return matchingReleases.Where(r => r.TagName.IndexOf(version) > -1 && r.Assets.Length > 0).FirstOrDefault();
        }

        public virtual ReleaseInfo[] GetReleasesForBranch(string branch)
        {
            Console.WriteLine("    Getting latest release for branch: " + branch);

            var request = new WebRequestHelper();

            var credentialsSection = "";
            if (!String.IsNullOrEmpty(Settings.GitHubUsername) && !String.IsNullOrEmpty(Settings.GitHubToken))
            {
                Console.WriteLine("GitHub credentials found. Authenticating request...");

                credentialsSection = String.Format("{0}:{1}@", Settings.GitHubUsername, Settings.GitHubToken);
            }

            var releasesUrl = String.Format("https://{0}api.github.com/repos/GrowSense/Index/releases", credentialsSection);

            var json = request.HttpGet(releasesUrl, Settings.GitHubUsername, Settings.GitHubToken);

            try
            {
                var releases = JsonConvert.DeserializeObject<ReleaseInfo[]>(json);
                var matchingReleases = releases.Where(r => r.TagName.IndexOf(branch) > -1).ToArray();

                Console.WriteLine("      Matching releases: " + matchingReleases.Length);

                return matchingReleases;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while retrieving releases for branch...");
                if (ex.Message.Contains("API rate limit exceeded"))
                    Console.WriteLine("GitHub API rate limit exceeded");
                else
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("----- Start JSON -----");
                    Console.WriteLine(json);
                    Console.WriteLine("----- Finish JSON -----");
                }
                return new ReleaseInfo[] { };
            }
        }
    }
}