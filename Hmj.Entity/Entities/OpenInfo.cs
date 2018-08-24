namespace Hmj.Entity.Entities
{
    /// <summary>
    /// 微信用户信息
    /// </summary>
    public class OpenInfo
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        /// <summary>
        /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        public string openid { get; set; }
        public string scope { get; set; }
        public string nickname { get; set; }
        public string headimgurl { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string sex { get; set; }

        public int subscribe { get; set; }
        public string language { get; set; }
        public string country { get; set; }
        public string subscribe_time { get; set; }
    }
}
