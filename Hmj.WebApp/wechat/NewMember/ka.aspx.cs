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

namespace Hmj.WebApp.wechat.NewMember
{
    public partial class ka : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
                else
                {
                    string host = Request.Url.Host;
                    string path = Request.Path;
                    string redirect_uri = Request.Url.AbsoluteUri;

                    Response.Redirect(AuthCode(redirect_uri));
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
            if (cust != null && !string.IsNullOrEmpty(cust.CardNo))
            {
                Z_LOY_BP_GETDETAILResponse zloy = SelectMember(cust.MOBILE);
                if (zloy.T_CENTRAL != null && zloy.T_CENTRAL.Length > 0)
                {
                    string DesRet = Encrypt3DES(cust.MOBILE);
                    string mobile = Server.UrlEncode(DesRet);
                    string redirectUrlSM = Server.UrlEncode("http://www.censh.com/jifen/index/list/39");
                    string redirectUrlSH = Server.UrlEncode("http://www.censh.com/jifen/index/list/40");
                    string redirectUrlLY = Server.UrlEncode("http://www.censh.com/jifen/index/list/41");
                    string redirectUrlWX = Server.UrlEncode("http://www.censh.com/jifen/index/list/69");
                    string SM = "http://www.censh.com/crm/weiXin/index?mobile=" + mobile + "&redirectUrl=" + redirectUrlSM;
                    string SH = "http://www.censh.com/crm/weiXin/index?mobile=" + mobile + "&redirectUrl=" + redirectUrlSH;
                    string LY = "http://www.censh.com/crm/weiXin/index?mobile=" + mobile + "&redirectUrl=" + redirectUrlLY;
                    string WX = "http://www.censh.com/crm/weiXin/index?mobile=" + mobile + "&redirectUrl=" + redirectUrlWX;

                    string html = @"<h2>积分商城</h2><ol><a href='" + WX + "' onclick='zhugeclick('wx_C-菜单-我的盛时-会员卡-品牌礼品-点击')'><span class='icon13'></span></a><p>品牌礼品</p></ol>" +
                    "<ol><a href='" + SM + "' onclick='zhugeclick('wx_C-菜单-我的盛时-会员卡-数码产品-点击')'><span class='icon01'></span></a><p>数码产品</p></ol>" +
                    "<ol><a href='" + SH + "' onclick='zhugeclick('wx_C-菜单-我的盛时-会员卡-生活日用-点击')'><span class='icon02'></span></a><p>生活日用</p></ol>" +
                    "<ol><a href='" + LY + "' onclick='zhugeclick('wx_C-菜单-我的盛时-会员卡-旅游健身-点击')'><span class='icon03'></span></a><p>旅游健身</p></ol>";

                    conjfsc.InnerHtml = html;
                    string level = "";
                    if (zloy.E_LEVEL == "Register")
                    {
                        imagecon.InnerHtml = "<img src='images/01.png'>";
                        level = "盛时会员";
                    }
                    else if (zloy.E_LEVEL == "Common")
                    {
                        imagecon.InnerHtml = "<img src='images/02.png'>";
                        level = "银卡会员";
                    }
                    else if (zloy.E_LEVEL == "Primary")
                    {
                        imagecon.InnerHtml = "<img src='images/03.png'>";
                        level = "金卡会员";
                    }
                    else if (zloy.E_LEVEL == "Medium")
                    {
                        imagecon.InnerHtml = "<img src='images/04.png'>";
                        level = "白金会员";
                    }
                    else if (zloy.E_LEVEL == "Senior")
                    {
                        imagecon.InnerHtml = "<img src='images/05.png'>";
                        level = "钻石会员";
                    }
                    else
                    {
                        imagecon.InnerHtml = "<img src='images/01.png'>";
                        level = "盛时会员";
                    }
                    name.InnerHtml = zloy.T_CENTRALDATAPERSON[0].LASTNAME + "<em>〖" + level + "〗</em>";
                    jf.InnerHtml = string.Format("{0:N0}", zloy.E_POINT);
                    if (zloy.E_ENDDAT == "9999-12-31")
                    {
                        endtime.InnerHtml = "有效期：长期有效";
                    }
                    else
                    {
                        endtime.InnerHtml = "有效期：" + zloy.E_ENDDAT;
                    }
                    phone.InnerHtml = "手机号：" + cust.MOBILE;
                    cardno.InnerHtml = "NO." + zloy.E_USER_NO;

                    string image = "";
                    if (mss.GetNickImg(FromUserName) == "" || mss.GetNickImg(FromUserName) == null)
                    {
                        image = "images/renwu.jpg";
                    }
                    else
                    {
                        image = mss.GetNickImg(FromUserName);
                    }

                    nickname.Src = image;


                }
            }
            else
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/Reg.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect");
            }
        }
    }
}