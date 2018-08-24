using Hmj.DataAccess.Repository;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Interface;

namespace Hmj.Business.ServiceImpl
{
    public class CustMsgHisService : ICustMsgHisService
    {
        private CustMsgHisRepository _cmhr;
        public CustMsgHisService()
        {
            _cmhr = new CustMsgHisRepository();
        }

        //查询粉丝发送给商户的消息记录
        public PagedList<CUST_MSG_HIS_EX> QueryFansMessages(PageView view, FansMessageSearch fms)
        {
            return _cmhr.QueryFansMessages(view, fms.ToUserName, "text", fms.IsStar, fms.IsReturn, fms.HasRemark, fms.SearchText);
        }

        //加星 取消加星
        public int UpdateFansMessageIsStar(CUST_MSG_HIS_EX message)
        {
            if (message.IS_STAR.HasValue)
            {
                return _cmhr.Update(message);
            }
            return 0;
        }

        public int ReplyMessage(CUST_MSG_HIS_EX message)
        {
            message.IS_RETURN = true;
            return _cmhr.Update(message);
        }

        public CUST_MSG_HIS_EX GetFansMessage(int id)
        {
            return _cmhr.GetFansMessage(id);
        }
    }
}
