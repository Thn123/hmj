using Hmj.Common;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Interface;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Hmj.WebApp
{
    /// <summary>
    /// SendTmps 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SendTmps : System.Web.Services.WebService
    {

        [WebMethod]
        public JsonSMsg SendTmpMsg(BCJ_TMP_DETAIL request)
        {
            IBcjStoreService _bcjStore = ObjectFactory.GetInstance<IBcjStoreService>();
            JsonSMsg msg = new JsonSMsg();
            try
            {
                if (string.IsNullOrEmpty(request.Template_Code))
                {
                    msg.Status = 0;
                    msg.Message = "模板ID不能是空";
                    return msg;
                }

                BasePage bpage = new BasePage();
                string access_token = bpage.MyToken(AppConfig.FWHOriginalID);

                int str = _bcjStore.SendTmp(request, access_token);

                if (str == -1)
                {
                    msg.Status = 0;
                    msg.Message = "没有该模板，请查看模板ID";
                    return msg;
                }

                msg.Status = 1;
                msg.Message = "OK";
                return msg;
            }
            catch (Exception ex)
            {
                msg.Status = 0;
                msg.Message = ex.Message;
                return msg;
            }
        }
    }
}
