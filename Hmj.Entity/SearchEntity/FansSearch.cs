namespace Hmj.Entity.SearchEntity
{
    public class FansSearch
    {
        private string _ToUserName;
        /// <summary>
        /// 微信原始ID
        /// </summary>
        public string ToUserName
        {
            get { return _ToUserName; }
            set { _ToUserName = value; }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _ISZC;
        /// <summary>
        /// 是否注册
        /// </summary>
        public string ISZC
        {
            get { return _ISZC; }
            set { _ISZC = value; }
        }

        private int? state;

        public int? State
        {
            get { return state; }
            set { state = value; }
        }

        private string _mdsel;

        public string MdSel
        {
            get { return _mdsel; }
            set { _mdsel = value; }
        }

        private string _mobile;

        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }
        
    }
}
