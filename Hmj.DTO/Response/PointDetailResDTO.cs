using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DTO
{
    public class PointDetailResDTO
    {
        public string ACCOUNT_ID { get; set; }

        public string CREATED_TIME { get; set; }

        public string ORDER_TYPE { get; set; }

        public string SAPORDER_ID { get; set; }

        public string SAPITEM_ID { get; set; }

        public string WB_ORDER_ID { get; set; }

        public string ITEM_ID { get; set; }

        public decimal POINTS { get; set; }

        public string PRODUCT_ID { get; set; }

        public string PRODUCT_DESC { get; set; }

        public string CATEGORY { get; set; }

        public string QUANTITY { get; set; }

        public string QTY_UNIT { get; set; }

        public string POINT_CODE { get; set; }

        public decimal ORGINAL_POINT { get; set; }

        public string ORGINAL_DATASOURCE { get; set; }

        public string ORGINAL_BRAND { get; set; }

        public string POINT_RULE { get; set; }

        public string EXPIRE_DATE { get; set; }
    }
}
