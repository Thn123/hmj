using Hmj.Common;

namespace Hmj.WebApp.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class PostRedirectModel
    {
        /// <summary>
        /// 1 会员checkin 2 非会员checkin 3 会员详情， 4 储值卡  5 诊疗卡开卡
        /// </summary>
        /// <value>
        /// The type of the redirect.
        /// </value>
        public int RedirectType { get; set; }
        public string CardCheckInUrl
        {
            get
            {
                return AppConfig.CardCheckInUrl;
            }
        }
        public string CashCheckInUrl
        {
            get
            {
                return AppConfig.CashCheckInUrl;
            }
        }
        public string UserEditUrl
        {
            get
            {
                return AppConfig.UserEditUrl;
            }
        }
        public string CreateCardUrl
        {
            get
            {
                return AppConfig.CreateCardUrl;
            }
        }
        public string CardRechargeUrl
        {
            get
            {
                return AppConfig.CardRechargeUrl;
            }
        }
        public string CheckInUrl
        {
            get
            {
                return AppConfig.CheckInUrl;
            }
        }
        public  string CardAddUrl
        {
            get
            {
                return AppConfig.CardAddUrl;
            }
        }
        public string ViewBillUrl
        {
            get
            {
                return AppConfig.ViewBillUrl;
            }
        }
        /// <summary>
        /// 单据号
        /// </summary>
        public string tcfdOpeid { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>      
        public string tcfdScatter { get; set; }
        /// <summary>
        /// 顾客电话
        /// </summary>      
        public string tcfdScatterTel { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>      
        public string tcfdPaperId { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>      
        public string tcfdMemberId { get; set; }

        /// <summary>
        /// 会员卡号
        /// </summary>
        /// <value>
        /// The TCFD card identifier.
        /// </value>
        public string tcfdCardId { get; set; }

        /// <summary>
        /// 预约号
        /// </summary>
        /// <value>
        /// The tifd reservation identifier.
        /// </value>
        public string tifdReservationID { get; set; }

        public string tifdRoomId { get; set; }
    }
}