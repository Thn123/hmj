using Hmj.Extension;
using System.IO;
using System.Net;
using System.Text;

/// <summary>
///BasePage 的摘要说明
/// </summary>
public class PosBasePage : WXMyContext
{
    public string PostRequest(string url)
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url); 
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();   
        Stream stream = response.GetResponseStream(); 
        StreamReader sr = new StreamReader(stream); 
        string responseHTML = sr.ReadToEnd();
        return responseHTML;
    }

    //GET
    public string PostRequestGet(string url)
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        request.Method = "GET";     
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();   
        Stream stream = response.GetResponseStream();
        StreamReader sr = new StreamReader(stream, Encoding.UTF8);  
        string responseHTML = sr.ReadToEnd();
        return responseHTML;
    }
}