using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using System;
using System.Collections.Generic;

namespace Hmj.WebApp.wechat.Area
{

    public partial class Area : System.Web.UI.Page
    {
        MySmallShopService mss = new MySmallShopService();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["para"] != null)
            {
                string name = Request.QueryString["para"].ToString();
                string html = "";
                List<AreaManage> list = new List<AreaManage>();
                if (name == "")
                {
                    list = mss.GetAllArea();
                }
                else
                {
                    list = mss.GetAllAreaByName(name);
                }
                if (list.Count > 0)
                {
                    html = "<ul>";
                    foreach (AreaManage area in list)
                    {
                        if (area.URL.ToString().Trim() != "" && area.URL != null)
                        {
                            if (!area.URL.StartsWith("http", true, null))
                            {
                                area.URL = "http://" + area.URL;
                            }
                            html += string.Format("<li><a href='{0}'>{1}</a></li>", area.URL, area.Name);
                        }
                        else
                        {
                            html += string.Format("<li><a href='Html.aspx?Id={0}'>{1}</a></li>", area.Id, area.Name);
                        }
                    }
                    html += "</ul>";
                }

                Response.Write(html);
                Response.End();
            }
            else
            {
                BasePage();
            }
        }
        private void BasePage()
        {
            if (!IsPostBack)
            {
                List<AreaManage> list = mss.GetAllArea();
                if (list.Count > 0)
                {
                    string html = "<ul>";
                    foreach (AreaManage area in list)
                    {
                        if (area.URL.ToString().Trim() != "" && area.URL != null)
                        {
                            if (!area.URL.StartsWith("http", true, null))
                            {
                                area.URL = "http://" + area.URL;
                            }
                            html += string.Format("<li><a href='{0}'>{1}</a></li>", area.URL, area.Name);
                        }
                        else
                        {
                            html += string.Format("<li><a href='Html.aspx?Id={0}'>{1}</a></li>", area.Id, area.Name);
                        }
                    }
                    html += "</ul>";
                    con.InnerHtml = html;
                }

            }
        }
    }
}