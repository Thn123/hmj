using Newtonsoft.Json;
using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Hmj.WebApp.wechat
{
    /// <summary>
    /// Actives 的摘要说明
    /// </summary>
    public class Actives : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        MySmallShopService mss = new MySmallShopService();
        OpinionService op = new OpinionService();
        LoginService lgo = new LoginService();
        public void ProcessRequest(HttpContext context)
        {
            if (GetQeuryString("para", context) == "jsapi") //获取js接口凭证
            {
                try
                {
                    ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                    if (m != null)
                    {

                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write(ex.Message + "," + ex.StackTrace);
                }

            }
            else if (GetQeuryString("para", context) == "SaveLocation") //保存地理位置
            {
                try
                {
                    ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                    if (m != null)
                    {
                        WD_Location l = new WD_Location();
                        l.accuracy = context.Request.Params["accuracy"];
                        l.Createdate = DateTime.Now;
                        l.FromUserName = context.Session["FromUserName"].ToString();
                        l.latitude = context.Request.Params["latitude"];
                        l.longitude = context.Request.Params["longitude"];
                        l.speed = context.Request.Params["speed"];
                        l.ToUserName = context.Session["ToUserName"].ToString();
                        new WeiPage().GetBaiDuMap(ref l);
                        mss.SaveLocation(l);
                        context.Response.Write("0");
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write(ex.Message + "," + ex.StackTrace);
                }

            }
            else if (GetQeuryString("para", context) == "GetScratch") //获取刮刮乐数据
            {
                try
                {
                    if ((context.Request.QueryString["FromUserName"] != null || context.Session["FromUserName"] != null) && (context.Request.QueryString["ToUserName"] != null || context.Session["ToUserName"] != null))
                    {
                        string user = context.Request.QueryString["FromUserName"] == null ? context.Session["FromUserName"].ToString() : context.Request.QueryString["FromUserName"].ToString();
                        string user2 = context.Request.QueryString["ToUserName"] == null ? context.Session["ToUserName"].ToString() : context.Request.QueryString["ToUserName"].ToString();

                        CUST_INFO cust = op.GetCustinfoByFromusername(user);
                        if (cust == null)
                        {
                            string url = "../../Member/Index.aspx?tousername=gh_60153a15d878&fromusername=" + user;
                            context.Response.Write("{\"message\":\"你尚未绑定，请先绑定\",\"url\":\"" + url + "\",\"status\":\"" + 0 + "\"}");
                        }
                        else if (cust.LAST_MODI_DATE != null && (cust.LAST_MODI_DATE < DateTime.Parse("2015-05-5") || cust.LAST_MODI_DATE > DateTime.Parse("2015-06-16")))
                        { //5月15至7月15
                            context.Response.Write("{\"message\":\"仅5.13-6.15期间绑定的会员方可参与此活动。\",\"url\":\"\",\"status\":\"" + 1 + "\"}");
                        }
                        else
                        {
                            Scratch scr = mss.GetScratch(user);
                            if (scr != null)
                            {
                                context.Response.Write("{\"message\":\"每人仅可参加一次此活动，感谢您的参与。\",\"url\":\"\",\"status\":\"" + 1 + "\"}");
                            }
                            else
                            {
                                Random r = new Random();
                                int num = r.Next(0, 50);
                                switch (num)
                                {
                                    case 0:
                                        List<Scratch> slist = mss.GetScratchList("1", DateTime.Now.AddDays(-3)); //每三天只中一个
                                        if (slist.Count > 0)
                                        {
                                            mss.SaveScratch(new Scratch { CreateDate = DateTime.Now, FromUserName = user, JP = "0",Code=-1 });
                                            context.Response.Write("{\"message\":\"未中奖\",\"status\":\"" + -1 + "\"}");
                                        }
                                        else
                                        {
                                            try
                                            {

                                                Hmj.WebApp.TicketService.VoucherWebServiceSoapClient d = new Hmj.WebApp.TicketService.VoucherWebServiceSoapClient();
                                                string a = d.SendSignVoucherByFromUserName(user, 3924);
                                                server ser = JsonConvert.DeserializeObject<server>(a);
                                                //if (ser.Code == 0)
                                                //{
                                                mss.SaveScratch(new Scratch { CreateDate = DateTime.Now, FromUserName = user, JP = "1", Code = ser.Code });
                                                context.Response.Write("{\"message\":\"价值680元的护理免券1张\",\"status\":\"" + 3 + "\"}");
                                                //}

                                            }
                                            catch (Exception)
                                            {
                                                mss.SaveScratch(new Scratch { CreateDate = DateTime.Now, FromUserName = user, JP = "0",Code=-1 });
                                                context.Response.Write("{\"message\":\"未中奖\",\"status\":\"" + -1 + "\"}");
                                            }
                                        }
                                        break;
                                    default:
                                        mss.SaveScratch(new Scratch { CreateDate = DateTime.Now, FromUserName = user, JP = "0",Code=-1 });
                                        context.Response.Write("{\"message\":\"未中奖\",\"status\":\"" + -1 + "\"}");
                                        break;
                                }
                            }
                        }
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


        public static string HttpXmlPostRequest(string postUrl, string postXml, Encoding encoding, string contype = "text/xml")
        {
            if (string.IsNullOrEmpty(postUrl))
            {
                throw new ArgumentNullException("HttpXmlPost ArgumentNullException :  postUrl IsNullOrEmpty");
            }

            if (string.IsNullOrEmpty(postXml))
            {
                throw new ArgumentNullException("HttpXmlPost ArgumentNullException : postXml IsNullOrEmpty");
            }

            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            byte[] byteArray = encoding.GetBytes(postXml);
            request.ContentLength = byteArray.Length;
            request.Method = "post";
            request.ContentType = contype;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                return new StreamReader(responseStream, encoding).ReadToEnd();
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        class server
        {
            public int Code { get; set; }
            public string Msg { get; set; }
        }
    }
}