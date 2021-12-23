using System;
using System.Net;
namespace GrowSense.Installer
{
  public class FileDownloader
  {
    public FileDownloader()
    {
    }

    public void Download(string url, string destination)
    {
      Console.WriteLine("  Downloading file...");
      Console.WriteLine("    URL: " + url);
      Console.WriteLine("    Destination: " + destination);

      try
      {
        WebClient webClient = new WebClient();
        webClient.DownloadFile(url, destination);
      }
      catch (WebException wex)
      {
        if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)
        {
          throw new Exception("File not found at: " + url);
        }
      }
      catch (Exception ex)
      {
        var starter = new ProcessStarter();
        starter.Start("wget " + url + " -O " + destination);

      }
    }
  }
}
