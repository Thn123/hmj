using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.Interface;
using Hmj.WebApp.DicountService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmj.WebApp.wechat.NewMember
{
    public partial class Discount : WeiPage
    {
        IStoreService sto = new StoreService();
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
                    CUST_INFO c = mss.GetCust(user);

                    if (c != null)
                    {
                        //获取优惠券列表
                        Z_CRM_LOY_WELFAREResponse DiscountList = SelectDiscount(c.CardNo, "");
                        var query1 = DiscountList.ET_LOY_WELFARE.Where(n => n.ZCPSTATE == "J01");
                        var query2 = DiscountList.ET_LOY_WELFARE.Where(n => n.ZCPSTATE == "J03");
                        var query3 = DiscountList.ET_LOY_WELFARE.Where(n => { return n.ZCPSTATE == "J02" || n.ZCPSTATE == "J04"; });
                        string[] str1 = new string[5] { "J01", "J02", "J03", "J03", "J03" };
                        var query4 = str1.Where(n => { return n == "J02" || n == "J01"; });
                        var query5 = str1.Where(n => { return n == "J03" || n == "J01"; });
                        var query6 = str1.Where(n => { return n == "J03" || n == "J02"; });
                        foreach (var item in query1)
                        {
                            quan1.InnerHtml += "<li>";
                            quan1.InnerHtml += "<a href=\"DiscountDetail.aspx?did=" + item.ZCPIDO + "\"><div class=\"quanbg\">";
                            //quan1.InnerHtml += "<div class=\"quanbg\">";
                            quan1.InnerHtml += "<div class=\"quanLeft\">";
                            quan1.InnerHtml += "<h2>" + item.ZCPTYPET + "<span>券号:" + item.ZCPIDO + "</span></h2>";
                            quan1.InnerHtml += "<p><strong>适用范围：</strong>" + item.ZCPMS + "<br/>";
                            quan1.InnerHtml += "<strong>适用门店：</strong>" + GetStoreName(item.ZCPSTORE) + "</p>";
                            quan1.InnerHtml += "<em></em>";
                            quan1.InnerHtml += "</div>";
                            quan1.InnerHtml += "<div class=\"quanRight\">";
                            quan1.InnerHtml += " <h2><em>￥</em>" + item.ZCPVALUE.ToString("0") + "</h2>";
                            quan1.InnerHtml += " <p class=\"padd\"></p>";
                            quan1.InnerHtml += " <p>有效期至<br/>" + item.ZCPEND + "</p>";
                            quan1.InnerHtml += "</div>";
                            quan1.InnerHtml += "</div></a>";
                            quan1.InnerHtml += "</li>";

                        }
                        foreach (var item in query2)
                        {
                            quan2.InnerHtml += "<li>";
                            quan2.InnerHtml += "<div class=\"quanBluebg\">";
                            quan2.InnerHtml += "<div class=\"quanLeft\">";
                            quan2.InnerHtml += "<h2>" + item.ZCPTYPET + "<span >券号:" + item.ZCPIDO + "</span></h2>";


                            quan2.InnerHtml += "<p><strong>适用范围：</strong>" + item.ZCPMS + "<br/>";

                            quan2.InnerHtml += "<strong>适用门店：</strong>" + GetStoreName(item.ZCPSTORE) + "</p>";
                            quan2.InnerHtml += "<em></em>";
                            quan2.InnerHtml += " </div>";
                            quan2.InnerHtml += " <div class=\"quanRight\">";
                            quan2.InnerHtml += " <h2><em>￥</em>" + item.ZCPVALUE.ToString("0") + "</h2>";
                            quan2.InnerHtml += " <p class=\"padd\"></p>";
                            quan2.InnerHtml += "  <p>有效期至<br/>" + item.ZCPEND + "</p>";
                            quan2.InnerHtml += " </div>";
                            quan2.InnerHtml += " </div>";
                            quan2.InnerHtml += " </li>";

                        }
                        foreach (var item in query3)
                        {
                            quan3.InnerHtml += "<li class=\"guoqi\">";
                            quan3.InnerHtml += "<div class=\"quanbg\">";
                            quan3.InnerHtml += "<div class=\"quanLeft\">";
                            quan3.InnerHtml += "<h2>" + item.ZCPTYPET + "<span >券号:" + item.ZCPIDO + "</span></h2>";


                            quan3.InnerHtml += "<p><strong>适用范围：</strong>" + item.ZCPMS + "<br/>";

                            quan3.InnerHtml += "<strong>适用门店：</strong>" + GetStoreName(item.ZCPSTORE) + "</p>";
                            quan3.InnerHtml += " <em></em>";
                            quan3.InnerHtml += "</div>";
                            quan3.InnerHtml += " <div class=\"quanRight\">";
                            quan3.InnerHtml += " <h2><em>￥</em>" + item.ZCPVALUE.ToString("0") + "</h2>";
                            quan3.InnerHtml += " <p class=\"padd\"></p>";
                            quan3.InnerHtml += " <p>有效期至<br/>" + item.ZCPEND + "</p>";
                            quan3.InnerHtml += " </div>";
                            quan3.InnerHtml += "</div>";
                            quan3.InnerHtml += "  <div class=\"guoqiicon\"></div>";
                            quan3.InnerHtml += "</li>";

                        }
                        if (query1.ToList<ZST_LOY_WELFARE>().Count == 0)
                        {
                            quan1.InnerHtml += "<li>";
                            quan1.InnerHtml += "<div class=\"nodata\">";
                            quan1.InnerHtml += "<p>暂无记录</p>";
                            quan1.InnerHtml += "</ div > ";
                            quan1.InnerHtml += "</li>";

                        }
                        if (query2.ToList<ZST_LOY_WELFARE>().Count == 0)
                        {
                            quan2.InnerHtml += "<li>";
                            quan2.InnerHtml += "<div class=\"nodata\">";
                            quan2.InnerHtml += "<p>暂无记录</p>";
                            quan2.InnerHtml += "</ div > ";
                            quan2.InnerHtml += "</li>";

                        }
                        if (query3.ToList<ZST_LOY_WELFARE>().Count == 0)
                        {
                            quan3.InnerHtml += "<li>";
                            quan3.InnerHtml += "<div class=\"nodata\">";
                            quan3.InnerHtml += "<p>暂无记录</p>";
                            quan3.InnerHtml += "</ div > ";
                            quan3.InnerHtml += "</li>";

                        }
                    }
                }
                else
                {
                    Response.Redirect(AuthCode(Request.Url.AbsoluteUri));
                    
                }
            }
        }



        private string GetStoreName(string codelist)
        {
            string[] st = codelist.Split(',');
            string sb = "";
            string values = "";
            foreach (var item in st)
            {
                sb += "'" + item + "',";
            }
            List<GROUP_INFO> list = sto.GetGroupInfoByCode((sb.Length==0?"0":sb.Substring(0, sb.Length - 1)));

            foreach (var item in list)
            {
                values += item.NAME + ",";
            }
            return values.Length==0?"": values.Substring(0, values.Length - 1) + "等";
        }
    }
}