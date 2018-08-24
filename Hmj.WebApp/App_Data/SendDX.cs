using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;


public class SendDX
{
    MySmallShopService mss = new MySmallShopService();
    public SendDX()
    {
    }

    /// <summary>
    /// 发送短信
    /// </summary>
    /// <param name="Message">信息内容</param>
    /// <param name="Phone">手机号码</param>
    /// <param name="ToUserName">商户微信号或ID</param>
    /// <returns>发送结果</returns>
    public string Send(string Message, string Phone, ORG_INFO org)
    {
        //string pwd = "123456a";
        //string encodedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, FormsAuthPasswordFormat.SHA1.ToString("G"));
        //string taskName = "短信即时发送_" + DateTime.Now.Ticks.ToString();
        //string mobileList = Phone;
        //string content = Message + "【美街微店】";
        //string msg = Mt("SDK-BBX-010-19253", getMD5("SDK-BBX-010-19253e-5ec2-["), mobileList, content, "", "", "");
        //try
        //{
        //    WXDXLog d = new WXDXLog();
        //    d.JG = msg;
        //    d.Merchants_ID = Merchants_ID;
        //    d.MSTYPE = "验证码";
        //    d.CONTENT = content;
        //    d.Time = DateTime.Now;
        //    d.ToUser = mobileList;
        //    dxl.Save(d);
        //    if (msg.Contains("发送成功"))
        //        msg = "发送成功";
        //    else
        //        msg = "发送失败";
        //}
        //catch (Exception)
        //{

        //}
        try
        {
            Hmj.WebApp.SendSMS.SMSServiceSoapClient s = new Hmj.WebApp.SendSMS.SMSServiceSoapClient();
        string jg = s.SendSMS(Phone, 8, Message, DateTime.Parse("2010-01-01"), "7101038130214180");
        //string content = Message + "【美街微店】";
        //string url = "http://www.ztsms.cn:8800/sendXSms.do?username=puman&password=puman2014&mobile=" + Phone + "&content=" + content + "&dstime=&productid=676767&xh=";
        //string msg = PostRequest(url);

        
            Hmj.Entity.WXDXLog d = new Hmj.Entity.WXDXLog();
            d.JG = jg;
            d.Merchants_ID = org.ID;
            d.CONTENT = Message;
            d.Time = DateTime.Now;
            d.ToUser = Phone;
            d.MSTYPE = "微信";
            mss.SaveDX(d);
            if (jg.Contains("OK"))
                jg = "发送成功";
            else
                jg = "发送失败";
            return jg;
        }
        catch (Exception ex)
        {
            Hmj.Entity.WXDXLog d = new Hmj.Entity.WXDXLog();
            d.JG = "";
            d.Merchants_ID = org.ID;
            d.CONTENT = ex.Message+","+ex.StackTrace;
            d.Time = DateTime.Now;
            d.ToUser = Phone;
            d.MSTYPE = "微信";
            mss.SaveDX(d);
            return "";
        }
        


    }

    //public string Send(string Message, string Phone, int? hyzt)
    //{
    //    string userName = "service@crmfocussend.com";
    //    string pwd = "123456a";
    //    string encodedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, FormsAuthPasswordFormat.SHA1.ToString("G"));
    //    string taskName = "短信即时发送_" + DateTime.Now.Ticks.ToString();
    //    string mobileList = Phone;
    //    string content = Message + "【美街微店】";
    //    string priority = "1";
    //    string addrial = "1";
    //    string msg = Mt("SDK-BBX-010-19253", getMD5("SDK-BBX-010-19253e-5ec2-["), mobileList, content, "", "", "");
    //    try
    //    {
    //        WXDXLog d = new WXDXLog();
    //        d.JG = msg;
    //        d.MID = "";
    //        d.NR = content;
    //        d.Time = DateTime.Now;
    //        d.ToUser = mobileList;
    //        dxl.Save(d);
    //        if (msg.Contains("发送成功"))
    //            msg = "发送成功";
    //        else
    //            msg = "发送失败";
    //    }
    //    catch (Exception)
    //    {


    //    }
    //    return msg;
    //}

    /// <summary>
    /// 获取md5码
    /// </summary>
    /// <param name="source">待转换字符串</param>
    /// <returns>md5加密后的字符串</returns>
    public string getMD5(string source)
    {
        string result = "";
        try
        {
            MD5 getmd5 = new MD5CryptoServiceProvider();
            byte[] targetStr = getmd5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(source));
            result = BitConverter.ToString(targetStr).Replace("-", "");
            return result;
        }
        catch (Exception)
        {
            return "0";
        }

    }

    public string PostRequest(string url)
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);  //定义请求对象，并设置好请求URL地址
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();    //定义响应对象，request在调用GetResponse方法事执行请求了，而不是在HttpWebRequest.Create的时候执行。
        Stream stream = response.GetResponseStream(); //定义一个流对象，来获取响应流
        StreamReader sr = new StreamReader(stream);  //定义一个流读取对象，读取响应流
        string responseHTML = sr.ReadToEnd();
        return responseHTML;
    }

    /// <summary>
    /// 普通群发短信mt方法
    /// </summary>
    /// <param name="sn">序列号</param>
    /// <param name="pwd">密码要MD5(SN+PWD)加密，取32位大写</param>
    /// <param name="mobiles">手机号列表</param>
    /// <param name="content">短信内容</param>
    /// <param name="ext">扩展码</param>
    /// <param name="stime">定时时间</param>
    /// <param name="rrid">流水号</param>
    /// <returns>发送结果</returns>
    //public string Mt(string sn, string pwd, string mobiles, string content, string ext, string stime, string rrid)
    //{
    //    string result = sms.mt(sn, pwd, mobiles, content, ext, stime, rrid);
    //    return CheckResult(result);
    //}


    static string CheckResult(string result)
    {
        if (result.StartsWith("-"))
        {
            return "发送失败！" + GetWhy(result) + "。返回值是：" + result;
        }
        else
        {
            return "发送成功。流水号是：" + result;
        }
    }

    static string GetWhy(string code)
    {
        switch (code)
        {
            case "-2":
                return "序列号未注册或加密不对";

            case "-4":
                return "余额不足";
            case "-6":
                return "参数错误，请检测所有参数";
            case "-7":

                return "权限受限";

            case "-9":

                return "扩展码权限错误";
            case "-10":

                return "内容过长，短信不得超过500个字符";
            case "-20":

                return "相同手机号，相同内容重复提交";
            case "-22":

                return "Ip鉴权失败";

            default:
                return "错误，请调试程序";
        }
    }
}
