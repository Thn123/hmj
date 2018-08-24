using ServiceStack.Text;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Hmj.Extension
{
    public abstract class BaseController: Controller
    {

        //重写基类的方法
        #region 重写基类的方法
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new ServiceStackJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }
        #endregion
    }

    public class ServiceStackJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json;charset=utf-8";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                response.Write(Data.ToJson());
            }
        }


    }
}
