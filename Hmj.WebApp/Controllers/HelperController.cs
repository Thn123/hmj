using Hmj.Common;
using Hmj.Entity;
using Hmj.Extension;
using Hmj.Interface;
using Hmj.WebApp.ViewModels;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace WeChat.WebApp.Controllers
{
    public class HelperController : WXMyControllerBase
    {
        private ICommonService _cservice;
        //private ICustomerService _uservice;
        public HelperController(ICommonService cservice)
        {
            _cservice = cservice;
            //_uservice = uService;
        }
        public ActionResult UploadImage(int? uid)
        {
            UploadModel mode =new UploadModel();
            mode.PostUrl = Url.Action("UploadImage");
            return View(mode);
        }


        [HttpPost]
        public ActionResult UploadImage(FormCollection form)
        {
            string state = "";           
            //判断Request中是否有接收Files文件
            if (Request.Files.Count != 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                state = CheckUpload(file);
                //保存图片,重命名下文件名
                if (string.IsNullOrEmpty(state))
                {
                    string Extension = Path.GetExtension(file.FileName).ToLower();

                    byte[] data = new byte[file.InputStream.Length];
                    file.InputStream.Read(data, 0, data.Length);
                    string url = Url.Action("ViewImage");
                    FILES fileEntity = _cservice.UploadFile(Extension, file.ContentType, data, url);
                    if (Request.Form["UID"] != null&&Request.Form["UID"].ToString()!="")  //保存顾客头像
                    {
                        //CUST_INFO c = _uservice.GetCustomer(int.Parse(Request.Form["UID"].ToString()));
                        //if (c != null)
                        //{
                        //    c.IMAGE_ID = fileEntity.ID;
                        //    _uservice.SaveCustomer(c);
                        //}
                    }
                    return Content("<script type='text/javascript'>parent.UploadCallback('SUCCESS','" + fileEntity.FILE_URL + "'," + fileEntity.ID + ");</script>"); 
                }
            }
            else
            {
                state = "请选择文件";
            }
            return Content(state);    
        }

       

        public FilePathResult ViewImage(int id)
        {
            if (id == 0)
            {
                string emptyFile = Server.MapPath("~/assets/img/icon/s.gif");
                return new FilePathResult(emptyFile, "image/gif");
            }
            FILES fileEntity = _cservice.GetUploadFile(id);
            if (fileEntity == null)
            {
                throw new FileNotFoundException();
            }
            return new FilePathResult(fileEntity.FILE_NAME, fileEntity.CONTENT_TYPE);
        }
        private static string CheckUpload(HttpPostedFileBase file)
        {            
            //获取用户上传文件的后缀名
            string Extension = Path.GetExtension(file.FileName).ToLower();

            // 安全性验证 
            if (Extension != ".jpg" && Extension != ".gif" && Extension != ".png" && Extension != ".jpeg")
            {
                return "错误的图片格式！";
            }

            if (file.ContentLength >= AppConfig.MaxUploadImageSize)
            {
                return "不允许的文件类型";
            }
            return "";
        }
    
    }
}
