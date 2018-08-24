using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hmj.Business.ServiceImpl;
using Hmj.Entity;

namespace Hmj.WebApp.wechat.Test
{
    public partial class Prolist : WeiPage
    {
        SystemService ss = new SystemService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["para"] != null)
            {
                if (Request.Params["yl"] != null && Request.Params["ptype"] != null)
                {
                    List<TT_Detail> list = ss.GetTTDetailList(int.Parse(Request.Params["yl"]), Request.Params["ptype"]);
                    string html = "";
                    foreach (var t in list)
                    {
                        html += string.Format(@"<li>
                    <a href=""detail.aspx?id={0}"">
                        <div class=""chanpinTu"">
                            <div class=""imgbox"">
                                <img src=""{1}"">
                            </div>
                            <strong>{2}</strong>
                            <p>{3}</p>
                        </div>
                    </a>
                </li>", t.ID, WebUrl + t.CoverImg, t.Name, t.Desc);
                    }
                    Response.Write(html);
                    Response.End();
                }
                else
                {
                    Response.Write("");
                    Response.End();
                }
            }
        }
    }
}