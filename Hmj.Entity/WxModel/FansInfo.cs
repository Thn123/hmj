using Newtonsoft.Json;

namespace Hmj.Entity.WxModel
{
    public class FansInfo
    {
        /// <summary>
        /// openid
        /// </summary>
        [JsonProperty("openid")]
        public string Openid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [JsonProperty("headimgurl")]
        public string Headimgurl { get; set; }

        /// <summary>
        /// 性别 1：男性 2：女性 0：未知
        /// </summary>
        [JsonProperty("sex")]
        public string Gender { get; set; }
        
    }
}
