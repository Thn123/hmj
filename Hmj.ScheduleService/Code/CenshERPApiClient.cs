using System;
using System.Collections.Generic;

namespace Hmj.ScheduleService.Code
{
    public class CenshERPApiClient : ServiceApiBase
    {
        protected override string ApiUrl
        {
            get { return "http://139.219.224.121:9002/Api/"; }
        }
        private static string APP_ID = "TEST10000001";
        private static string APP_SECRET = "ZHT2016123";
        private static string ACCESS_TOKEN = "DS20160814";

        private static CenshERPApiClient _client;
        private static object _lockobject = new object();

        private CenshERPApiClient()
        {

        }

        /// <summary>
        /// 实例化服务
        /// </summary>
        /// <returns></returns>
        public static CenshERPApiClient Create()
        {
            if (_client == null)
            {
                lock (_lockobject)
                {
                    if (_client == null)
                    {
                        _client = new CenshERPApiClient();
                    }
                }
            }
            return _client;
        }

        public string GetProductList()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            GetProductCondition condition = new GetProductCondition();
            condition.PAGE_INDEX = 1;
            condition.PAGE_SIZE = 10;
            condition.LAST_MODIFIED_TIME_START = DateTime.Parse("1900-1-1 00:00:00");
            condition.LAST_MODIFIED_TIME_END = DateTime.Parse("2900-1-1 00:00:00");
            condition.CODE = "";// "Z11007012A10A71A";
            condition.NAME = "";
            condition.BRAND_CODE = "";
            //condition.SERIES_ID = "10014914";
            //condition.CATAGORY_CODE = "";
            condition.ON_SALE = "T";
            condition.IS_HOT_PRODUCT = "是";
            condition.CHANNEL_ID = "8";
            //condition.STOCK_TYPE = 4;
            condition.STOCK_NUM_GREATER_ZERO = "T";
            //condition.STYLE = "女款";
            condition.MOVEMENT_TYPE = "";

            dict.Add("CONDITION", condition);

            return this.Request("Product/GetListByConditionV2", dict);

        }
        public string GetProductDetail(string codes)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("CODE", codes);

            return this.Request("Product/GetDetailByCodes", dict);

        }
        public string GetCategoryList()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            
            return this.Request("Category/GetCategoryList", dict);
        }
        public string GetAllBrandList()
        {
            //Dictionary<string, object> dict = new Dictionary<string, object>();
            //dict.Add("PAGE_INDEX", 1);
            //dict.Add("PAGE_SIZE", 500);
            //dict.Add("CLASS", -1);
            GetProductCondition condition = new GetProductCondition();
            condition.PAGE_INDEX = 1;
            condition.PAGE_SIZE = 50;
            condition.CLASS = -1;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("CONDITION", condition);

            return this.Request("Brand/GetListByCondition", dict);
        }
        public string PayAndConfirm()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("CODE", "000012310272051001");
            var payment = new
            {
                PAYMENT_TYPE = 2,
                PAYPAL_USERNAME = "13611111111",
                PAY_TIME = DateTime.Now,
                AMOUNT_PAID = 18000,
                TRADE_NO = "22",
                PAYER = "allen",
            };
            dict.Add("Payment", payment);

            return this.Request("Order/PayAndConfirm", dict);

        }
        public string UpdateItemSalesPrice()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("CODE", "O10000100");
            var item = new
            {
                ITEM_ID = 10000100,
                SALE_PRICE = 8500,
            };
            List<object> list = new List<object>();
            list.Add(item);
            dict.Add("ORDER_ITEMS", list);

            return this.Request("Order/UpdateItemSalesPrice", dict);

        }

        public string GetOrderList()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            GetOrderCondition condition = new GetOrderCondition();
            condition.ChannelTypeId = -1;
            condition.OrderTypeId = -1;
            condition.STATUS = -1;
            condition.PaymentStatus = -1;
            condition.InvoiceStatus = -1;
            condition.PAGE_INDEX = 1;
            condition.PAGE_SIZE = 50;
            condition.CustomerCode = "100";

            dict.Add("CONDITION", condition);

            return this.Request("Order/GetListByCustomerLoginName", dict);

        }
        public string GetOrderDetail()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("CODE", "O10000100");

            return this.Request("Order/GetDetailByCode", dict);

        }
        public string GetEmpSalesStatByEmpId()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var search = new
            {
                EMPLOYEE_ID = 11,
                ORDER_TIME_START = DateTime.Parse("1900-1-1 00:00:00"),
                ORDER_TIME_END = DateTime.Parse("2900-1-1 00:00:00"),
                PAGE_INDEX = 1,
                PAGE_SIZE = 50,
            };
            dict.Add("CONDITION", search);

            return this.Request("Order/GetEmpSalesStatByEmpId", dict);
        }
        public string GetEmpSalesStatByOrgId()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var search = new
            {
                ORG_ID = 1,
                ORDER_TIME_START = DateTime.Parse("1900-1-1 00:00:00"),
                ORDER_TIME_END = DateTime.Parse("2900-1-1 00:00:00"),
                PAGE_INDEX = 1,
                PAGE_SIZE = 50,
            };
            dict.Add("CONDITION", search);

            return this.Request("Order/GetEmpSalesStatByOrgId", dict);
        }
        public string Deliver()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("CODE", "O10000100");
            dict.Add("DELIVER_CODE", "YTO1000032");

            return this.Request("Order/Deliver", dict);

        }
        public string Finish()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("CODE", "O10000100");
            dict.Add("SIGN_IMAGE_URL", "aaa_sign.jpg");
            dict.Add("REMARK", "委托代签");

            return this.Request("Order/Finish", dict);

        }
        public string GetTopEmpSalesStat()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("YEAR_MONTH", 201611);

            return this.Request("Order/GetTopEmpSalesStat", dict);

        }
        public string GetAllSeriesByBrandCode()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("BRAND_CODE", "H1101002");

            return this.Request("BrandSeries/GetAllByBrandCode", dict);

        }
        public string GetTopStoreSalesStat()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("YEAR_MONTH", 201611);

            return this.Request("Order/GetTopStoreSalesStat", dict);
        }

        public string GetInventoryDetailBySkuCode()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("SKU_CODE", "R53922706");

            return this.Request("Inventory/GetDetailBySkuCode", dict);
        }

        public string GetPromotion()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            GetPromotionCondition condition = new GetPromotionCondition();
            condition.OnSale = -1;
            condition.PageIndex = 1;
            condition.PageSize = 50;
            condition.SkuCode = "1234";

            dict.Add("GetPromotionOrgInventoryCondition", condition);

            return this.Request("Promotion/GetOrgInventoryPriceListByCondition", dict);
        }
        public string GetAppointOrderList()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            GetAppointOrderCondition condition = new GetAppointOrderCondition();
            condition.CustomerCode = "100";
            condition.PageIndex = 1;
            condition.PageSize = 50;

            dict.Add("GetAppointmentOrderCondition", condition);

            return this.Request("AppointmentOrder/GetListByCondition",  dict);
        }
        public string GetAppointOrderDetail()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            GetAppointOrderCondition condition = new GetAppointOrderCondition();
            condition.AppointmentOrderNo = "YY201608281111";

            dict.Add("GetAppointmentOrderCondition", condition);

            return this.Request("AppointmentOrder/GetDetailByNo",  dict);
        }



        public string GetListByEmployeeId()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            GetOrderCondition condition = new GetOrderCondition();
            condition.EMPLOYEE_ID = 4876;
            condition.STATUS = 0;
            condition.ORDER_TIME_START = DateTime.Parse("1900-1-1");
            condition.ORDER_TIME_END = DateTime.Parse("9900-1-1");
            condition.PAGE_INDEX = 1;
            condition.PAGE_SIZE = 500;

            dict.Add("CONDITION", condition);

            return this.Request("Order/GetListByEmployeeId", dict);
        }

        public string GetListByCustomerLoginName()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            GetOrderCondition condition = new GetOrderCondition();
            condition.CUSTOMER_LOGIN_NAME = "leon999";
            condition.STATUS = -1;
            condition.ORDER_TIME_START = DateTime.Parse("1900-1-1");
            condition.ORDER_TIME_END = DateTime.Parse("9900-1-1");
            condition.PAGE_INDEX = 1;
            condition.PAGE_SIZE = 50;

            dict.Add("CONDITION", condition);

            return this.Request("Order/GetListByCustomerLoginName", dict);
        }


        public string GetOrderStatisticByEmployeeId()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("EMPLOYEE_ID", 4876);

            return this.Request("Order/GetOrderStatisticByEmployeeId", dict);

        }





        private string Request(string path, Dictionary<string, object> businessDict)
        {
            string method = JsonHelper.SerializeObject(businessDict);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("BUSINESS_PARAMETERS", businessDict);
            dict.Add("SYSTEM_PARAMETERS", this.GetSysParams(method));

            return this.DoJSONRequest(path, dict, "POST");
        }



        private SystemParameters GetSysParams(string method)
        {
            //AppId	TEST10000001
            //AccessToken	DS20160814
            //AppSecret	ZHT2016123

            var param = new SystemParameters()
            {
                APP_ID = APP_ID,
                ACCESS_TOKEN = ACCESS_TOKEN,
                TIMESTAMP = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            //AppId+GetPromotionOrgInventoryCondition节点文本+AppSecret拼接后取MD5特征码32位大写
            string tempStr = APP_ID + method + APP_SECRET;
            param.SIGN = MD5(tempStr).ToUpper();

            return param;
        }

    }
    public class GetAppointOrderCondition
    {
        public string AppointmentOrderNo { get; set; }
        public string AppointStoreCode { get; set; }
        public string AppointEmployeeCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerMobile { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
    public class GetPromotionCondition
    {
        public string OrgCode { get; set; }
        public string BrandCode { get; set; }
        public string SkuCode { get; set; }
        public string SkuSn { get; set; }
        public int OnSale { get; set; }
        public DateTime? LastModifiedTimeStart { get; set; }
        public DateTime? LastModifiedTimeEnd { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GetOrderCondition
    {
        public string OuterOrderNo { get; set; }
        public int? ChannelTypeId { get; set; }
        public int? OrderTypeId { get; set; }
        public int? STATUS { get; set; }
        public int? EMPLOYEE_ID { get; set; }
        public int? PaymentStatus { get; set; }
        public int? InvoiceStatus { get; set; }
        public string CUSTOMER_LOGIN_NAME { get; set; }
        public string EmployeeCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerMobile { get; set; }
        public DateTime? ORDER_TIME_START { get; set; }
        public DateTime? ORDER_TIME_END { get; set; }
        public int? PAGE_INDEX { get; set; }
        public int? PAGE_SIZE { get; set; }
    }

    public class GetProductCondition
    {
        public string CATAGORY_CODE { get; set; }
        public string ON_SALE { get; set; }
        public string IS_HOT_PRODUCT { get; set; }
        public string BRAND_CODE { get; set; }
        public string SERIES_ID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public int? PAGE_INDEX { get; set; }
        public int? PAGE_SIZE { get; set; }
        public DateTime? LAST_MODIFIED_TIME_START { get; set; }
        public DateTime? LAST_MODIFIED_TIME_END { get; set; }
        public int? PRICE_RANGE_ID { get; set; }
        public string STYLE { get; set; }
        public string MOVEMENT_TYPE { get; set; }
        public string CHANNEL_ID { get; set; }
        public int? STOCK_TYPE { get; set; }
        public string STOCK_NUM_GREATER_ZERO { get; set; }
        public int? CLASS { get; set; }
    }

    //public class ERPBaseModel
    //{
    //    public SystemParameters SystemParameters { get; set; }


    //}

    public class SystemParameters
    {

        //"AppId": "10000001",
        //"AccessToken": "DS20160814",
        //"Timestamp": "2016-08-14 14:56:53",
        //"Sign": "7BDE397848844D730D8FB276F1DC9554"


        public string APP_ID { get; set; }
        public string ACCESS_TOKEN { get; set; }
        public string TIMESTAMP { get; set; }
        public string SIGN { get; set; }
    }
}
