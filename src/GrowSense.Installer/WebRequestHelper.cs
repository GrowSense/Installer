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
      try
      {
        var request = HttpWebRequest.Create(url);

        var response = request.GetResponse() as HttpWebResponse;

//        System.Net.ServicePointManager.ServerCertificateValidationCallback +=
//  delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
//                          System.Security.Cryptography.X509Certificates.X509Chain chain,
//                          System.Net.Security.SslPolicyErrors sslPolicyErrors)
//      {
//        return true; // **** Always accept
//      };

        using (var responseStream = response.GetResponseStream())
        {
          var reader = new StreamReader(responseStream, Encoding.UTF8);
          output = reader.ReadToEnd();
        }
      }
      catch (Exception ex)
      {
        var starter = new ProcessStarter();
        starter.Start("curl -s " + url + "");
        output = starter.Output.Trim();       
      }

      return output;
    }
  }
}
