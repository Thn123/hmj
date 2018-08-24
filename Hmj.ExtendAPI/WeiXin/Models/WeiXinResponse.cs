using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hmj.ExtendAPI.WeiXin.Models
{
    public class AccessTokenResponse : WeiXinResponseBase
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }


    public class DeptResponse : WeiXinResponseBase
    {
        [JsonProperty("id")]
        public int ID { get; set; }
    }


    public class DeptListResponse : WeiXinResponseBase
    {
        [JsonProperty("department")]
        public List<Department> Departments { get; set; }
    }

    public class UserListResponse : WeiXinResponseBase
    {
        [JsonProperty("userlist")]
        public List<UserInfo> Users { get; set; }

    }
    public class UserInfo
    {
        [JsonProperty("userid")]
        public string UserId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("department")]
        public int[] Department { get; set; }
        [JsonProperty("weixinid")]
        public string WeiXinId { get; set; }
        [JsonProperty("gender")]
        public byte Gender { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        //关注状态: 1=已关注，2=已冻结，4=未关注
        [JsonProperty("status")]
        public byte Status { get; set; }
    }
    public class Department
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("parentid")]
        public int ParentID { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
    }

    public class QRCodeResponse : WeiXinResponseBase
    {
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        [JsonProperty("expire_seconds")]
        public int ExpireSeconds { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
