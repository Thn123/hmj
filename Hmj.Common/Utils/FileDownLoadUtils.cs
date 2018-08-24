using System.Web;

namespace Hmj.Common.Utils
{
    public class FileDownLoadUtils
    {
        public static void DownloadFile(byte[] datas, string fileName)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            response.ContentType = "application/octet-stream";

            response.ContentEncoding = System.Text.Encoding.UTF8; //.GetEncoding("GB2312");

            response.BinaryWrite(datas);
            response.Flush();
            response.End();
        }
    }
}
