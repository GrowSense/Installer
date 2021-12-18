using System;
using System.Net;
using System.IO;
using System.Text;
namespace GrowSense.Installer
{
  public class WebRequestHelper
  {
    public WebRequestHelper()
    {
    }

    public string HttpGet(string url)
    {
      ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

      var output = "";
      var request = HttpWebRequest.Create(url);
      
      var response = request.GetResponse() as HttpWebResponse;
      using (var responseStream = response.GetResponseStream())
      {
        var reader = new StreamReader(responseStream, Encoding.UTF8);
        output = reader.ReadToEnd();
      }

      return output;
    }
  }
}
