using System;
using System.Collections.Specialized;

namespace Hmj.Entity.PageSearch
{
    public class PageView
    {
        public PageView()
        {

        }
        public PageView(NameValueCollection form)
        {
            this.PageIndex = Convert.ToInt32(form["page"]) - 1;
            this.PageSize = Convert.ToInt32(form["rp"]);
            SortName = form["sortname"];
            SortOrder = form["sortorder"];
        }
        //public PageView(int pageIndex, int pageSize)
        //{
        //    PageIndex = pageIndex;
        //    PageSize = pageSize;
        //}
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SortName { get; set; }

        public string SortOrder { get; set; }

        public string GetSqlOrder()
        {
            if (!string.IsNullOrEmpty(SortName) && !string.IsNullOrEmpty(SortOrder))
                return string.Format("ORDER BY {0} {1} ", SortName, SortOrder);
            return string.Empty;
        }
    }
}
