using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.WebApp.SelectMemberNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.Member
{
    public partial class ka : WeiPage
    {
        MySmallShopService mss = new MySmallShopService();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["FromUserName"] = "oQaIMwFdpQlde3Ko2iUCAkYhJ9JY";
            //oQaIMwG_9lanE3f5VtvOSrErmfic
            //Session["ToUserName"] = "oQaIMwFdpQlde3Ko2iUCAkYhJ9JY";
            if (!IsPostBack)
            {
                Base();
                if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
                {
                    string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
                    string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
                    Session["FromUserName"] = user;
                    Session["ToUserName"] = user2;
                    Bind(user);

                }
            }
        }
        private System.Text.Encoding encoding;
        public System.Text.Encoding Encoding
        {
            get
            {
                if (encoding == null)
                {
                    encoding = System.Text.Encoding.UTF8;
                }
                return encoding;
            }

            set
            {
                encoding = value;
            }
        }
        public string Encrypt3DES(string strString)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = Encoding.GetBytes("censhdes");
            DES.Mode = CipherMode.ECB;
            DES.Padding = PaddingMode.Zeros;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] Buffer = encoding.GetBytes(strString);

            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
        public void Bind(string FromUserName)
        {
            CUST_INFO cust = mss.GetCust(FromUserName);
            if (cust != null)
            {
                Z_LOY_BP_GETDETAILResponse zloy = SelectMember(cust.MOBILE);
                if (zloy.T_CENTRAL != null && zloy.T_CENTRAL.Length > 0)
                {
                    string DesRet = Encrypt3DES(cust.MOBILE);
                    string mobile = Server.UrlEncode(DesRet);
                    string redirectUrlSM = Server.UrlEncode("http://www.censh.com/jifen/index/list/39");
                    string redirectUrlSH = Server.UrlEncode("http://www.censh.com/jifen/index/list/40");
                    string redirectUrlLY = Server.UrlEncode("http://www.censh.com/jifen/index/list/41");
                    string redirectUrlWX = Server.UrlEncode("http://www.censh.com/jifen/index/list/42");
                    string SM = "http://www.censh.com/crm/weiXin/index?mobile=" + mobile + "&redirectUrl=" + redirectUrlSM;
                    string SH = "http://www.censh.com/crm/weiXin/index?mobile=" + mobile + "&redirectUrl=" + redirectUrlSH;
                    string LY = "http://www.censh.com/crm/weiXin/index?mobile=" + mobile + "&redirectUrl=" + redirectUrlLY;
                    string WX = "http://www.censh.com/crm/weiXin/index?mobile=" + mobile + "&redirectUrl=" + redirectUrlWX;
                    string html = @"<span><a href='" + SM + "'>数码<br />产品</a></span><span><a href='" + SH + "'>生活<br />日用</a></span><span><a href='" + LY + "'>旅游<br />健身</a></span><span><a href='" + WX + "'>维修<br />服务</a></span>";

                    conjfsc.InnerHtml = html;

                    name.InnerHtml = "姓名：" + zloy.T_CENTRALDATAPERSON[0].LASTNAME;
                    jf.InnerHtml = "积分：" + string.Format("{0:N0}", zloy.E_POINT);
                    endtime.InnerHtml = "有效期：" + zloy.E_ENDDAT;
                    phone.InnerHtml = "手机：" + cust.MOBILE;
                    cardno.InnerHtml = zloy.E_USER_NO;

                    if (zloy.E_LEVEL == "Register")
                    {
                        imagecon.InnerHtml = "<img src='images/01.png'>";
                    }
                    else if (zloy.E_LEVEL == "Common")
                    {
                        imagecon.InnerHtml = "<img src='images/02.png'>";
                    }
                    else if (zloy.E_LEVEL == "Primary")
                    {
                        imagecon.InnerHtml = "<img src='images/03.png'>";
                    }
                    else if (zloy.E_LEVEL == "Medium")
                    {
                        imagecon.InnerHtml = "<img src='images/04.png'>";
                    }
                    else if (zloy.E_LEVEL == "Senior")
                    {
                        imagecon.InnerHtml = "<img src='images/05.png'>";
                    }
                    else
                    {
                        imagecon.InnerHtml = "<img src='images/01.png'>";
                    }

                }
            }
            else
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/Member/Reg.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect");
            }
        }
    }

}