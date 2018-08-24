namespace Hmj.Entity.Entities
{
    public class Information_EX : WXInformation
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string FulltextUrl { get; set; }
        /// <summary>
        /// 图文消息id
        /// </summary>
        public int DID { get; set; }


        /// <summary>
        /// 消息类型
        /// </summary>
        public string XXType { get; set; }

        /// <summary>
        /// 关键字匹配类型
        /// </summary>
        public string PPType { get; set; }

        /// <summary>
        /// 是否链接 如果是链接则直接跳转链接，不是链接则调整到指定页面显示内容
        /// </summary>
        public bool IsURL { get; set; }

        /// <summary>
        /// 图文
        /// </summary>
        public string tuwen { get; set; }

        /// <summary>
        /// 表情
        /// </summary>
        public string Biaoqing { get; set; }
    }
}
