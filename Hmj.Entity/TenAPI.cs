namespace Hmj.Entity
{
    public class TenAPI
    {
        /// <summary>
        /// 状态码
        /// 状态码，0为正常
        //310请求参数信息有误
        //311Key格式错误
        //306请求有护持信息请检查字符串
        //110请求来源未被授权
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 状态说明
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public TenAPI_EX result { get; set; }
    }
}
