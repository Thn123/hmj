namespace Hmj.Entity.Entities
{
    public class CUST_BOOKING_EX 
    {
        public string SERVICE_NAME { get; set; }

        private string _begintime;
        public string BEGINTIME { get; set; }

        public string ENDTIME { get; set; }

        public string BED_NAME { get; set; }
        public string ROOM_NAME { get; set; }
        public string CUST_NO { get; set; }
        public string CARD_NO { get; set; }
        //技师姓名
        public string STAFF_NAME { get; set; }
        //技师性别
        public string STAFF_GENDER { get; set; }
        //服务项目ID
        public string SERVICE_ID { get; set; }
        //登记人员ID
        public string CREATE_USER_ID { get; set; }
        //登记人员姓名
        public string CREATE_USER_NAME { get; set; }
        //用户类型
        public int CUST_TYPE { get; set; }
        //项目类别
        public string SERVICE_TYPE { get; set; }
        public string ORG_ID { get; set; }
        //微信确认人
        public string WX_CONFIRM_STAFFNAME { get; set; }
        //电话确认人
        public string CONFIRM_STAFFNAME { get; set; }
    }
}
