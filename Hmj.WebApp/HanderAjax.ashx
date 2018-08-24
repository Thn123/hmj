<%@ WebHandler Language="C#" Class="HanderAjax" %>

using System;
using System.Web;
using System.Data;
using System.Web.SessionState;
using System.Collections.Generic;
using Hmj.Common;

public class HanderAjax : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        //Access access = new Access();
        context.Response.ContentType = "text/plain";
        if (GetQeuryString("para", context) == "jsapi") //获取js接口凭证
        {
            try
            {
                WeiPage w = new WeiPage();
                // string token = w.Token();
                string sj = ConvertDateTimeInt(DateTime.Now).ToString();//时间戳
                string sjm = Guid.NewGuid().ToString("d"); //随机码
                string ticket = w.GetJSAPI_Ticket(); //凭证
                string dz = context.Server.UrlDecode(context.Request.Params["apiurl"]);
                string noncestr = "noncestr=" + sjm;
                string jsapi_ticket = "jsapi_ticket=" + ticket;
                string timestamp = "timestamp=" + sj;
                string url = "url=" + dz;
                string[] ArrTmp = { noncestr, jsapi_ticket, timestamp, url };
                Array.Sort(ArrTmp);     //字典排序
                string tmpStr = string.Join("&", ArrTmp);
                tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
                tmpStr = tmpStr.ToLower();
                if (ticket == "")
                {
                    context.Response.Write("{\"status\":\"" + -1 + "\"}");
                }
                else
                {
                    var re = new
                    {
                        state = 0,
                        appId = AppConfig.WXCorpID,// "wx37a6d83af764d816",
                        timestamp = sj,
                        nonceStr = sjm,
                        signature = tmpStr,
                        url = dz,
                        link = (dz.IndexOf("&") == -1 ? dz : dz.Substring(0, dz.IndexOf("&"))) + "&id=" + GetValueFromUrl(dz, "state"),
                        title = "华美家微信",
                        imgUrl = "http://wechat.jahwa.com.cn/WX_Herborist/BCJ/images/logo.png",
                        desc = "华美家微信"
                    };

                    string ret = Newtonsoft.Json.JsonConvert.SerializeObject(re);
                    context.Response.Write(ret);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message + "," + ex.StackTrace);
            }
        }
    }


    public string GetQeuryString(string para, HttpContext context)
    {
        if (context.Request.QueryString[para] != null) return context.Request.QueryString[para].ToString();
        else return "";
    }
    /// <summary>
    /// datetime转换为unixtime
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private int ConvertDateTimeInt(System.DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;
    }
    // <summary>
    /// 获取URL中的参数
    /// </summary>
    /// <param name="url"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public string GetValueFromUrl(string url, string name)
    {
        string[] a = url.Split('?');
        if (a.Length <= 1)
            return "-1";
        else
        {
            string[] b = a[1].Split('&');
            string d = "";
            foreach (string c in b)
            {
                if (c.Split('=')[0] == name)
                {
                    d = c;
                }
            }
            if (d == "")
                return "-1";
            else
                return d.Split('=')[1];
        }
    }
    private string reverse(string value)
    {
        System.Text.StringBuilder s = new System.Text.StringBuilder();

        for (int i = value.Length - 1; i >= 0; i--)
        {
            s.Append(value[i]);

        }
        return s.ToString();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }



}