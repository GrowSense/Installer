using System;
using GrowSense.Installer.Web.GitHub;

namespace GrowSense.Installer.Tests.Web.GitHub
{
    public class MockReleaseIdentifier : ReleaseIdentifier
    {   
        public MockReleaseIdentifier(string version)
        {
            Version = version;
        }

        public override void Initialize(string branch, string version)
        {
            // Skip initialization
            //base.Initialize(branch, version);
        }
    }
}
