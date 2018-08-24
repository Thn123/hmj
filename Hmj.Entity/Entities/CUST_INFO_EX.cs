using System;

namespace Hmj.Entity.Entities
{
    public class CUST_INFO_EX : CUST_INFO
    {
        //会籍店
        public string STORE_NAME { get; set; }
        //注册门店
        public string REG_STORE_NAME { get; set; }
        public DateTime LAST_BUY_DATE { get; set; }

        public int BUY_COUNT { get; set; }

        public decimal BUY_MONEY { get; set; }

        public decimal PAR_AMT { get; set; }

        public decimal BALANCE { get; set; }

        public decimal ARREARS { get; set; }

        //会员卡级
        public int PCID { get; set; }//prod_card ID
        public int CARD_ID { get; set; } //CUST_CARD_ID
        public string CARD_NAME { get; set; }

        //会员积分
        public int AVA_POINTS { get; set; }
        //字符串时间
        public string ONDATE { get; set; }

        public string MobilePhone { get; set; }
        public string IMAGE_URL { get; set; }


    }
}
