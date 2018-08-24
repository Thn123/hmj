using SPACRM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPACRM.WebApp.ViewModels
{
    public class GoodsIndexModel
    {
        public PROD_GOODS goods {get;set;}
        public PROD_SERVICE service { get; set; }
        //public PROD_INFO info { get; set; }
    }
}