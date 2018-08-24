namespace Hmj.Entity.SearchEntity
{
    public class RoleSearch
    {
        public string ROLE_NAMES { get; set; }
        public int ORG_ID { get; set; }

        private int _replyType;
        /// <summary>
        /// 消息类型
        /// </summary>
        public int replyType
        {
            get { return _replyType; }
            set { _replyType = value; }
        }

        public string CName
        {
            get;
            set;
        }
    }
}
