using System;
using System.Net;
using System.IO;
using System.Text;
namespace GrowSense.Installer.Web
{
    public class WebRequestHelper
    {
        public WebRequestHelper()
        {
        }

        public string HttpGet(string url)
        {
            return HttpGet(url, "", "");
        }

        public string HttpGet(string url, string username, string password)
        {
            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            var output = "";

            try
            {
                var request = HttpWebRequest.Create(url);

                var response = request.GetResponse() as HttpWebResponse;

                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    output = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                var starter = new ProcessStarter();
                starter.WriteOutputToConsole = false;

                var userArgument = "";
                if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
                    userArgument = " -u " + username + ":" + password;

                starter.Start("curl " + userArgument + " -s " + url + "");
                output = starter.Output.Trim();
            }

            return output;
        }
    }
}