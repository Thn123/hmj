using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IWXMessageRecordService
    {
        //查询粉丝发送给商户的消息记录
        PagedList<CUST_MSG_RECORD_EX> QueryFansMessages(PageView view, FansMessageSearch fms);
        PagedList<CUST_MSG_RECORD_EX> QueryFansMessagesN(PageView view, FansMessageSearch fms);

        //加星 取消加星
        int UpdateFansMessageIsStar(CUST_MSG_RECORD_EX message);

        CUST_MSG_RECORD_EX GetFansMessage(int id);

        int ReplyMessage(CUST_MSG_RECORD_EX fanMessage, string content);

        CUST_MSG_RECORD_EX GetMessage(int id);

        List<CUST_MSG_RECORD_EX> QueryReplyMessages(int id);
    }
}
