namespace Hmj.Entity.SearchEntity
{
    public class FansMessageSearch
    {
        /// <summary>
        /// 商户OPENID
        /// </summary>
        public string ToUserName;

        public string MsgType;

        /// <summary>
        /// null全部，true加星，false未加星
        /// </summary>
        public bool? IsStar;

        /// <summary>
        /// null全部，true已回复，false未回复
        /// </summary>
        public bool? IsReturn;

        /// <summary>
        /// null全部，true有备注，false无备注
        /// </summary>
        public bool? HasRemark;

        /// <summary>
        /// 查询文本
        /// </summary>
        public string SearchText;
    }
}
