using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.Store
{

    public partial class Update : System.Web.UI.Page
    {
        MySmallShopService mss = new MySmallShopService();
        ISystemService _service = new SystemService();
        protected void Page_Load(object sender, EventArgs e)
        {

            //            List<MDSearch> list = mss.GetAll();
            //            foreach (MDSearch item in list)
            //            {
            //                QMActivity CuObj = new QMActivity();
            //                CuObj.MdName = item.Name;
            //                CuObj.Id = 0;
            //                CuObj.Ticket = "";
            //                CuObj.MdSel = "";
            //                CuObj.CreateTime = DateTime.Now;
            //                CuObj.CID = 0;
            //                int num = _service.SaveQMActivity(CuObj);
            //                WeiPage wp = new WeiPage();
            //                string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + wp.Token(AppConfig.FWHOriginalID);
            //                //这里需要修改
            //                string d = @"{ ""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"": {0}}}
            //        
            //        }";
            //                d = d.Replace("{0}", num.ToString());
            //                string mes = WeiPage.HttpXmlPostRequest(url, d, Encoding.UTF8);
            //                //Response.Write(mes);
            //                string[] b = mes.Split('\"');
            //                string ticket = Server.UrlEncode(b[3]);
            //                CuObj.Id = num;
            //                CuObj.Ticket = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket;
            //                CuObj.MdSel = "qrscene_" + num.ToString();
            //                num = _service.SaveQMActivity(CuObj);
            //           }
        }
    }
}