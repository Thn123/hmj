namespace Hmj.Entity.Entities
{
    public class ORG_STORE_EX : ORG_STORE
    { 
        public string ORG_NAME
        {
            get;
            set;
        }
        public string REGION_NAME
        {
            get;
            set;
        }
        #region 卡适用的门店
        public string STORE_ID { get; set; }
        public bool IS_SALES { get; set; }
        public bool IS_USERD { get; set; }
        public bool IsChecked { get; set; }
        public int SALE_MAX { get; set; }
        #endregion 

        #region 产品，项目适用的门店
        public decimal PRICE { get; set; }
        public decimal BUSY_PRICE { get; set; }
        public decimal POLD_PRICE { get; set; }
        public decimal SOLD_PRICE { get; set; }
        #endregion
        /// <summary>
        /// 开店时间字符串
        /// </summary>
        public string OPEN_DATE_Str
        {
            get;
            set;
        } 
        /// <summary>
        /// 关店时间字符串
        /// </summary>
        public string CLOSE_DATE_Str
        {
            get;
            set;
        }

        public string IMAGE_ID_URL { get; set; }
        public string MIMAGE_ID_URL { get; set; }
        public string SIMAGE_ID_URL { get; set; }

        public decimal jl { get; set; }
    }
}
