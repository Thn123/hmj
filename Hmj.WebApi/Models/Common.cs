using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.Entity;
using log4net;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using WeChatCRM.Common.Utils;

namespace Hmj.WebApi.Models
{
    public class Common
    {
        private static ILog _logerror = LogManager.GetLogger("logerror");
        /// <summary>
        /// 生成带参数二维码
        /// </summary>
        /// <returns></returns>
        public static bool GetOauthCode(string code,string filePath)
        {
            try
            {
                // 生成二维码的内容
                
                QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
                QRCode qrcode = new QRCode(qrCodeData);

                // qrcode.GetGraphic 方法可参考最下发“补充说明”
                Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
                MemoryStream ms = new MemoryStream();
                qrCodeImage.Save(ms, ImageFormat.Jpeg);

                // 如果想保存图片 可使用  
                qrCodeImage.Save(filePath+ code+".jpg");
                return true;
            }
            catch(Exception ex) {
                _logerror.Error("生成二维码异常："+ex.Message);
                return false;
            }
        }

    }
}