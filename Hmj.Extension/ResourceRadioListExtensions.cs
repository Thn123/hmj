using Hmj.Common;
using Hmj.Interface;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Hmj.Extension
{
    public static class ResourceRadioListExtensions
    {
        public static MvcHtmlString ResourceRadioList(
           this HtmlHelper html, string name, string resourceName, object selectedValue, object htmlAttributes)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName);

            if (rlist != null)
            {
                StringBuilder sb = new StringBuilder();
                // put a similar foreach here
                foreach (var item in rlist)
                {
                    sb.Append("<label class=\"radio-inline\">");
                    sb.Append(html.RadioButton(name, item.Code, selectedValue != null && item.Value == selectedValue.ToString()));
                    sb.AppendFormat("{0}</label>", item.Name);
                }
                return new MvcHtmlString(sb.ToString());

            }
            return new MvcHtmlString("");
        }
    }
}
