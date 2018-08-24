namespace Hmj.Entity.Entities
{
    public class book_ex
    {
        public string cust_name { get; set; }
        public string cust_mobile { get; set; }
        public string begin_time { get; set; }
        public string end_time { get; set; }
        public string service_item { get; set; }
        public string booking_num { get; set; }
        public string store_id { get; set; }
        public string store_name { get; set; }
        public string store_tel { get; set; }
        public string store_addr { get; set; }
        public string emp_name { get; set; }
        public string store_no { get; set; }
        public string category_name { get; set; }
        public int? id { get; set; }
        /// <summary>
        /// 0：未处理；1：确认；2：已到店；3：已取消
        /// </summary>
        public int? state { get; set; }

        /// <summary>
        /// 0	新预约
        ///1	已开单
        ///2	已结帐
        ///3	已到店
        ///7	未约进
        ///8	已爽约
        ///9	已取消
        /// </summary>
        public int? status { get; set; }
    }
}
