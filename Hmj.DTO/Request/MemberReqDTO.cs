namespace Hmj.DTO
{
    public class MemberUpdateReqDTO
    {
        /// <summary>
        /// 姓氏
        /// </summary>
        public string NAME_LAST { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string NAME_FIRST { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NICK_NAME { get; set; }

        /// <summary>
        /// 性别 0：女 1：男
        /// </summary>
        public string GENDER { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string BIRTHDAY { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MOBILE { get; set; }

        /// <summary>
        /// OPENID
        /// </summary>
        public string OPENID { get; set; }

    }
}
