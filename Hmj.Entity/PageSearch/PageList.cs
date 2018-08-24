using System.Collections.Generic;

namespace Hmj.Entity.PageSearch
{
    public class PagedList<T> //where T : class
    {
        private List<T> _dataList;
        public List<T> DataList
        {
            get
            {
                if (_dataList == null) _dataList = new List<T>();
                return _dataList;
            }
            set
            {
                _dataList = value;
            }
        }
        public int PageSize
        {
            get;
            set;
        }
        public int PageIndex
        {
            get;
            set;
        }
        public int Total
        { get; set; }
    }
}
