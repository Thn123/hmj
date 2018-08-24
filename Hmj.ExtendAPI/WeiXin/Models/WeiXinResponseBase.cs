using Newtonsoft.Json;

namespace Hmj.ExtendAPI.WeiXin.Models
{
    public class WeiXinResponseBase
    {
        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }

        [JsonProperty("errmsg")]
        public string ErrorMessage { get; set; }
    }
}
