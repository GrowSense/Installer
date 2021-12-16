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
      var output = "";
        var request = HttpWebRequest.Create(url);
        //request.ContentType = "application/json; charset=utf-8";
        //request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("username:password"));
        //request.PreAuthenticate = true;
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
