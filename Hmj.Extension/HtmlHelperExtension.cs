using System;
using System.Text;
using System.Web.Mvc;

namespace Hmj.Extension
{
    public static class HtmlHelperExtension
    {

        public static MvcHtmlString Js(this HtmlHelper html, params string[] jsFileName)
        {
            if (jsFileName != null)
            {
                string folderpath = "../../assets";
                string jslink = "<script src='{0}' type='text/javascript'></script>";
                StringBuilder sb = new StringBuilder();
                foreach (string file in jsFileName)
                {
                    if (string.IsNullOrEmpty(file))
                        continue;
#if DEBUG
                    sb.AppendFormat(jslink, UrlHelper.GenerateContentUrl(folderpath + "/" + file + ".js?v=" + DateTime.Now.Ticks, html.ViewContext.HttpContext));
#else
                    sb.AppendFormat(jslink, UrlHelper.GenerateContentUrl(folderpath + "/" + file + ".js?v=1.0.1", html.ViewContext.HttpContext));
#endif
                }
                return MvcHtmlString.Create(sb.ToString());
            }
            return MvcHtmlString.Empty;

        }


        public static MvcHtmlString Css(this HtmlHelper html, params string[] cssfilename)
        {
            if (cssfilename != null)
            {
                string folderpath = "~/assets/css";
                string csslink = "<link href=\"{0}\" rel=\"Stylesheet\" type=\"text/css\" />";
                StringBuilder sb = new StringBuilder();
                foreach (string filename in cssfilename)
                {
#if DEBUG
                    sb.AppendFormat(csslink, UrlHelper.GenerateContentUrl(folderpath + "/" + filename + ".css?v="+DateTime.Now.Ticks, html.ViewContext.HttpContext));
#else
                    sb.AppendFormat(csslink, UrlHelper.GenerateContentUrl(folderpath + "/" + filename + ".css?v=2014033001", html.ViewContext.HttpContext));
#endif
                }

                return MvcHtmlString.Create(sb.ToString());
            }
            return MvcHtmlString.Empty;

        }
        public static MvcHtmlString Css2(this HtmlHelper html, params string[] cssfilename)
        {
            if (cssfilename != null)
            {
                string folderpath = "../../assets";
                string csslink = "<link href=\"{0}\" rel=\"Stylesheet\" type=\"text/css\" />";
                StringBuilder sb = new StringBuilder();
                foreach (string filename in cssfilename)
                {
#if DEBUG
                    sb.AppendFormat(csslink, UrlHelper.GenerateContentUrl(folderpath + "/" + filename + ".css?v=" + DateTime.Now.Ticks, html.ViewContext.HttpContext));
#else
                    sb.AppendFormat(csslink, UrlHelper.GenerateContentUrl(folderpath + "/" + filename + ".css?v=2014033001", html.ViewContext.HttpContext));
#endif
                }

                return MvcHtmlString.Create(sb.ToString());
            }
            return MvcHtmlString.Empty;

        }
    }
}
