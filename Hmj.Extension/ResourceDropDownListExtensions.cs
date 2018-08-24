using Hmj.Common;
using Hmj.Entity.Jsons;
using Hmj.Interface;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Hmj.Extension
{
    public static class ResourceDropDownListExtensions
    {
        /// <summary>
        /// Resources the drop down list.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="name">The name.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="selectedValue">The selected value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceDropDownList(
          this HtmlHelper html, string name, string resourceName,object selectedValue, object htmlAttributes)
        {
            return ResourceDropDownList(html, name, resourceName, selectedValue, htmlAttributes, false);
        }

        /// <summary>
        /// Resources the drop down list.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="name">The name.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="selectedValue">The selected value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="hasAllOption"></param>
        /// <returns></returns>
        public static MvcHtmlString ResourceDropDownList(
          this HtmlHelper html, string name, string resourceName, object selectedValue, object htmlAttributes, bool hasAllOption)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName, hasAllOption);
            SelectList list = new SelectList(rlist, "Value", "Name", selectedValue);            
            return html.DropDownList(name, list, htmlAttributes);
        }

        public static MvcHtmlString ResourceDropDownList(
        this HtmlHelper html, string name, string resourceName,  object selectedValue, object htmlAttributes,string parentCode, bool hasAllOption)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName,parentCode, hasAllOption);
            SelectList list = new SelectList(rlist, "Value", "Name", selectedValue);
            return html.DropDownList(name, list, htmlAttributes);
        }

        public static MvcHtmlString ResourceDropDownList(
        this HtmlHelper html, string name, string resourceName, object selectedValue, object htmlAttributes, string parentCode, bool hasAllOption, string Merchants_ID)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName, parentCode, hasAllOption, Merchants_ID);
            SelectList list = new SelectList(rlist, "Value", "Name", selectedValue);
            return html.DropDownList(name, list, htmlAttributes);
        }

        public static MvcHtmlString ResourceDropDownList(
       this HtmlHelper html, string name, string resourceName, object selectedValue, object htmlAttributes, string parentCode,string regionid,string store, bool hasAllOption)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName, parentCode,regionid,store, hasAllOption);
            SelectList list = new SelectList(rlist, "Value", "Name", selectedValue);
            return html.DropDownList(name, list, htmlAttributes);
        }

        public static MvcHtmlString DropDownListEx(
          this HtmlHelper html, string name, List<CodeValue> rlist, object selectedValue, object htmlAttributes)
        {
            SelectList list = new SelectList(rlist, "Code", "Value", selectedValue);
            return html.DropDownList(name, list, htmlAttributes);
        }

        /// <summary>
        /// add by cyr 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="resourceName"></param>
        /// <param name="selectedValue"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="parentCode"></param>
        /// <param name="hasAllOption"></param>
        /// <param name="LowerAmt">销售额不大于当前LowerAmt</param>
        /// <param name="type">1为储值卡 2为疗程卡</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceDropDownList(
        this HtmlHelper html, string name, string resourceName, object selectedValue, object htmlAttributes, string parentCode, bool hasAllOption, double LowerAmt, int type)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName, parentCode, hasAllOption, LowerAmt, type);
            SelectList list = new SelectList(rlist, "Value", "Name", selectedValue);
            return html.DropDownList(name, list, htmlAttributes);
        }

         
    }
}
