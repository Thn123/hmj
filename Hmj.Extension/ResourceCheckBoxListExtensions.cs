using Hmj.Common;
using Hmj.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Hmj.Extension
{
    public static class ResourceCheckBoxListExtensions
    { 

        public static MvcHtmlString ResourceCheckBoxList(
          this HtmlHelper html, string name, string resourceName, object[] selectedValue, object htmlAttributes)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName);

            if (rlist != null)
            {
                StringBuilder sb = new StringBuilder();
                // put a similar foreach here
                foreach (var item in rlist)
                {
                    sb.Append("<label>");
                    //sb.Append(html.CheckBox(name, item.Code == (selectedValue == null ? selectedValue : selectedValue.ToString()), htmlAttributes2));
                    sb.Append(CreateCheckBoxTag(name, item.Value, selectedValue, htmlAttributes));
                    sb.AppendFormat("{0}</label>", item.Name);
                }
                return new MvcHtmlString(sb.ToString());

            }
            return new MvcHtmlString("");
        }
        public static MvcHtmlString ResourceCheckBoxList(
       this HtmlHelper html, string name, string resourceName, object[] selectedValue, object htmlAttributes, string parentCode, bool hasAllOption)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName, parentCode, hasAllOption);
            //SelectList list = new SelectList(rlist, "Value", "Name", selectedValue);
            if (rlist != null)
            {
                StringBuilder sb = new StringBuilder();
                // put a similar foreach here
                foreach (var item in rlist)
                {
                    sb.Append("<label class=\"checkbox-inline\">");
                    //sb.Append(html.CheckBox(name, item.Code == (selectedValue == null ? selectedValue : selectedValue.ToString()), htmlAttributes2));
                    sb.Append(CreateCheckBoxTag(name, item.Value, selectedValue, htmlAttributes));
                    sb.AppendFormat("{0}</label>", item.Name);
                }
                return new MvcHtmlString(sb.ToString());

            }
            return new MvcHtmlString("");
        }
        private static string CreateCheckBoxTag(string name, object value, object[] selectedValue, object htmlAttributes)
        {
            IDictionary<string, object> htmlAttributes2 = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            htmlAttributes2.Add("type", "checkbox");
            htmlAttributes2.Add("id", name);
            htmlAttributes2.Add("name", name);
            htmlAttributes2.Add("value", value);

            if (selectedValue != null && value != null && selectedValue.Where(s => s.ToString() == value.ToString()).Count() > 0)
            {
                if (htmlAttributes2.ContainsKey("checked"))
                    htmlAttributes2["checked"] = "checked";
                else
                    htmlAttributes2.Add("checked", "checked");
            }

            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes<string, object>(htmlAttributes2);
            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }
    }
}
