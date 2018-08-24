using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.WebApp.SelectMemberNew;
using Hmj.WebApp.SelectOrderNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.NewMember
{
    public partial class Coupon : WeiPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["FromUserName"] = "oQaIMwFdpQlde3Ko2iUCAkYhJ9JY";
            //Session["ToUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS4";
            if (!IsPostBack)
            {
                Base();
                if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
                {
                    string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
                    string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
                    Session["FromUserName"] = user;
                    Session["ToUserName"] = user2;
                    try
                    {
                        Bind(user);
                    }
                    catch (Exception ex)
                    {
                        WXLOG log = new WXLOG();
                        log.CON = ex.Message.ToString() + "Coupon";
                        log.TIME = DateTime.Now;
                        mss.SaveLog(log);
                    }
                }
                else
                {
                    Response.Redirect(AuthCode(Request.Url.AbsoluteUri));
                }
            }
        }

        public void Bind(string FromUserName)
        {
            CUST_INFO cust = mss.GetCust(FromUserName);
            if (cust != null&&!string.IsNullOrEmpty(cust.CardNo))
            {
                Z_LOY_BP_GETDETAILResponse zloy = SelectMember(cust.MOBILE);
                if (zloy.T_CENTRAL != null && zloy.T_CENTRAL.Length > 0)
                {
                    cardno.InnerHtml = "<em>" + zloy.E_USER_NO + "</em>";
                    name.InnerHtml = "<em>" + zloy.T_CENTRALDATAPERSON[0].LASTNAME + "</em>";
                    phone.InnerHtml = "<em>" + zloy.T_TELEFONDATA[0].TELEPHONE + "</em>";
                    if (zloy.T_CENTRAL[0].TITLE_KEY == "Z001")
                    {
                        sex.InnerHtml = "<span class='nandeicon'><em>先生</em></span>";
                    }
                    else
                    {
                        sex.InnerHtml = "<span class='nvdeicon'><em>女士</em></span>";
                    }
                    //jf.InnerHtml = string.Format("{0:N0}", zloy.E_POINT);
                    //有效期暂且不知道哪个字段


                    //Z_LOY_BP_GETORDERResponse zws = SelectOrder("1000000011", "20150101", "20150301");
                    //正式的
                    Z_LOY_BP_GETORDERResponse zws = SelectOrder(zloy.E_USER_NO, "", "");

                    string conval = "";
                    for (int i = 0; i < zws.T_ZCRMD_ORDER.Count(); i++)
                    {
                        if (zws.T_ZCRMD_ORDER[i].ZAUART == "ZZ03")
                        {
                            if (zws.T_ZCRMD_ORDER[i].ZZBRD.Length >= 8)
                            {
                                conval += "<li><span>" + (mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8)) == "" ? "无" : mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8))) + "</span></li>";
                            }
                            else
                            {
                                conval += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZZBRD == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZZBRD) + "</span></li>";
                            }
                            string jfzoeb = zws.T_ZCRMD_ORDER[i].ZOEB;
                            int index = jfzoeb.IndexOf(".");
                            if (index >= 0)
                            {
                                jfzoeb = jfzoeb.Substring(0, index);
                            }
                            conval += "<li><span>" + (jfzoeb == "" ? "无" : jfzoeb) + "</span></li>";
                            conval += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";
                            string sj = "";
                            if (zws.T_ZCRMD_ORDER[i].ZORE.Length == 8)
                            {
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(0, 4);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(4, 2);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(6, 2);
                            }

                            conval += "<li><span>" + (sj == "" ? "无" : sj) + "</span></li>";
                        }
                    }
                    con.InnerHtml = conval;
                }
            }
            else
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/Member/Band.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect");
            }
        }
    }
}