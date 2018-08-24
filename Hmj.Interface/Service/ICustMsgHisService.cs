using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;

namespace Hmj.Interface
{
    //查询微信实时消息集合
    public interface ICustMsgHisService
    {
        //查询粉丝发送给商户的消息记录
        PagedList<CUST_MSG_HIS_EX> QueryFansMessages(PageView view, FansMessageSearch fms);

        //加星 取消加星
        int UpdateFansMessageIsStar(CUST_MSG_HIS_EX message);

        CUST_MSG_HIS_EX GetFansMessage(int id);

        int ReplyMessage(CUST_MSG_HIS_EX message);
    }
}
