﻿using System;
using System.Collections.Generic;
using Hmj.WebApp.SelectMemberNew;
using Hmj.WebApp.SelectOrderNew;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hmj.Entity;
using Hmj.Business.ServiceImpl;

namespace Hmj.WebApp.wechat.Member
{
    public partial class Order : WeiPage
    {
        MySmallShopService mss = new MySmallShopService();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["FromUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS4";
            //Session["ToUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS4";
            //oQaIMwG_9lanE3f5VtvOSrErmfic
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
                        log.CON = ex.Message.ToString() + "Order";
                        log.TIME = DateTime.Now;
                        mss.SaveLog(log);
                    }
                }
            }
        }

        public void Bind(string FromUserName)
        {
            CUST_INFO cust = mss.GetCust(FromUserName);
            if (cust != null)
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
                    //timeend.InnerHtml = string.Format(zloy.E_ENDDAT);

                    //Z_LOY_BP_GETORDERResponse zws = SelectOrder("1000000011", "20151101", "");
                    //正式的
                    Z_LOY_BP_GETORDERResponse zws = SelectOrder(zloy.E_USER_NO, "", "");
                    string conval1 = "";
                    string conval2 = "";
                    string conval3 = "";
                    string conval4 = "";
                    for (int i = 0; i < zws.T_ZCRMD_ORDER.Count(); i++)
                    {
                        if (zws.T_ZCRMD_ORDER[i].ZAUART == "ZO03" || zws.T_ZCRMD_ORDER[i].ZAUART == "ZO07" || zws.T_ZCRMD_ORDER[i].ZAUART == "ZR05" || zws.T_ZCRMD_ORDER[i].ZAUART == "ZR07")
                        {
                            if (zws.T_ZCRMD_ORDER[i].ZZBRD.Length >= 8)
                            {
                                conval1 += "<li><span>" + (mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8)) == "" ? "无" : mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8))) + "</span></li>";
                            }
                            else
                            {
                                conval1 += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZZBRD == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZZBRD) + "</span></li>";
                            }

                            conval1 += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZORZ == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORZ) + "</span></li>";
                            //conval1 += "<li style='width:12.5%'><span>" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";

                            if (zws.T_ZCRMD_ORDER[i].ZAUART == "ZR07" || zws.T_ZCRMD_ORDER[i].ZAUART == "ZR05")
                            {
                                //conval1 += "<li style='width:12.5%'><span>退货</span></li>";
                                conval1 += "<li style='width:12.5%'><span>" + "-" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";
                            }
                            else
                            {
                                //conval1 += "<li style='width:12.5%'><span>完成</span></li>";
                                conval1 += "<li style='width:12.5%'><span>" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";
                            }
                            conval1 += "<li style='width:12.5%'><span>" + (zws.T_ZCRMD_ORDER[i].ZLSK == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZLSK) + "</span></li>";
                            string sj = "";
                            if (zws.T_ZCRMD_ORDER[i].ZORE.Length == 8)
                            {
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(0, 4);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(4, 2);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(6, 2);
                            }

                            conval1 += "<li><span>" + (sj == "" ? "无" : sj) + "</span></li>";
                        }
                        else if (zws.T_ZCRMD_ORDER[i].ZAUART == "ZO08" || zws.T_ZCRMD_ORDER[i].ZAUART == "ZR08")
                        {
                            if (zws.T_ZCRMD_ORDER[i].ZZBRD.Length >= 8)
                            {
                                conval2 += "<li><span>" + (mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8)) == "" ? "无" : mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8))) + "</span></li>";
                            }
                            else
                            {
                                conval2 += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZZBRD == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZZBRD) + "</span></li>";
                            }
                            conval2 += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZORZ == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORZ) + "</span></li>";
                            //conval2 += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";
                            //conval2 += "<li style='width:12.5%'><span>" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";

                            if (zws.T_ZCRMD_ORDER[i].ZAUART == "ZR08")
                            {
                                //conval2 += "<li style='width:12.5%'><span>退货</span></li>";
                                conval2 += "<li style='width:12.5%'><span>" + "-" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";
                            }
                            else
                            {
                                conval2 += "<li style='width:12.5%'><span>" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";
                                //conval2 += "<li style='width:12.5%'><span>完成</span></li>";
                            }

                            conval2 += "<li style='width:12.5%'><span>" + (zws.T_ZCRMD_ORDER[i].ZLSK == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZLSK) + "</span></li>";

                            string sj = "";
                            if (zws.T_ZCRMD_ORDER[i].ZORE.Length == 8)
                            {
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(0, 4);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(4, 2);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(6, 2);
                            }

                            conval2 += "<li><span>" + (sj == "" ? "无" : sj) + "</span></li>";
                        }

                        else if (zws.T_ZCRMD_ORDER[i].ZAUART == "ZZ02")
                        {
                            if (zws.T_ZCRMD_ORDER[i].ZZBRD.Length >= 8)
                            {
                                conval3 += "<li><span>" + (mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8)) == "" ? "无" : mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8))) + "</span></li>";
                            }
                            else
                            {
                                conval3 += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZZBRD == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZZBRD) + "</span></li>";
                            }
                            string zorr = "";
                            if (zws.T_ZCRMD_ORDER[i].ZORR == "Z012")
                            {
                                zorr = "预约成功";
                            }
                            else if (zws.T_ZCRMD_ORDER[i].ZORR == "Z013")
                            {
                                zorr = "预约取消";
                            }
                            conval3 += "<li><span>" + (zorr == "" ? "无" : zorr) + "</span></li>";
                            conval3 += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";

                            string sj = "";
                            if (zws.T_ZCRMD_ORDER[i].ZORE.Length == 8)
                            {
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(0, 4);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(4, 2);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(6, 2);
                            }

                            conval3 += "<li><span>" + (sj == "" ? "无" : sj) + "</span></li>";
                        }

                        else if (zws.T_ZCRMD_ORDER[i].ZAUART == "ZZ01")
                        {
                            if (zws.T_ZCRMD_ORDER[i].ZZBRD.Length >= 8)
                            {
                                conval4 += "<li><span>" + (mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8)) == "" ? "无" : mss.GetNameByBrand(zws.T_ZCRMD_ORDER[i].ZZBRD.Substring(0, 8))) + "</span></li>";
                            }
                            else
                            {
                                conval4 += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZZBRD == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZZBRD) + "</span></li>";
                            }
                            string zorr = "";
                            if (zws.T_ZCRMD_ORDER[i].ZORR == "Z009")
                            {
                                zorr = "待处理";
                            }
                            else if (zws.T_ZCRMD_ORDER[i].ZORR == "Z010")
                            {
                                zorr = "维修中";
                            }
                            else if (zws.T_ZCRMD_ORDER[i].ZORR == "Z011")
                            {
                                zorr = "维修完成";
                            }
                            conval4 += "<li><span>" + (zorr == "" ? "无" : zorr) + "</span></li>";
                            conval4 += "<li><span>" + (zws.T_ZCRMD_ORDER[i].ZORA == "" ? "无" : zws.T_ZCRMD_ORDER[i].ZORA) + "</span></li>";
                            string sj = "";
                            if (zws.T_ZCRMD_ORDER[i].ZORE.Length == 8)
                            {
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(0, 4);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(4, 2);
                                sj += ".";
                                sj += zws.T_ZCRMD_ORDER[i].ZORE.ToString().Substring(6, 2);
                            }

                            conval4 += "<li><span>" + (sj == "" ? "无" : sj) + "</span></li>";
                        }

                    }

                    con1.InnerHtml = conval1;
                    con2.InnerHtml = conval2;
                    con3.InnerHtml = conval3;
                    con4.InnerHtml = conval4;
                }
            }
            else
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/Member/Band.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect");
            }
        }
    }
}