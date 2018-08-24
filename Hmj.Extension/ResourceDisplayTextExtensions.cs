using Hmj.Common;
using Hmj.Interface;
using System.Web.Mvc;

namespace Hmj.Extension
{
    public static class ResourceDisplayTextExtensions
    {
        public static MvcHtmlString ResourceDisplayText(
        this HtmlHelper html,string resourceName, object selectedValue)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName);
            return new MvcHtmlString(rlist.GetResourceNameByValue(selectedValue.ToString()));
        }
    }
}
