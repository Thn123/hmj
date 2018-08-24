using Hmj.DataAccess.Repository;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Hmj.Business.WXService
{
    public class WXMessageRecordService : IWXMessageRecordService
    {
        private WXMessageRecordRepository _mrr;
        public WXMessageRecordService()
        {
            _mrr = new WXMessageRecordRepository();
        }

        //查询粉丝发送给商户的消息记录
        public PagedList<CUST_MSG_RECORD_EX> QueryFansMessages(PageView view, FansMessageSearch fms)
        {
            return _mrr.QueryFansMessages(view, fms.ToUserName, fms.IsStar, fms.IsReturn, fms.SearchText);
        }
        //查询粉丝发送给商户的消息记录
        public PagedList<CUST_MSG_RECORD_EX> QueryFansMessagesN(PageView view, FansMessageSearch fms)
        {
            return _mrr.QueryFansMessagesN(view, fms.ToUserName, fms.IsStar, fms.IsReturn, fms.SearchText);
        }
        //加星 取消加星
        public int UpdateFansMessageIsStar(CUST_MSG_RECORD_EX message)
        {
            if (message.IS_STAR.HasValue)
            {
                return _mrr.Update(message);
            }
            return 0;
        }

        public int ReplyMessage(CUST_MSG_RECORD_EX fanMessage, string content)
        {
            var num = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                fanMessage.IS_RETURN = true;
                _mrr.Update(fanMessage);

                var replyMessage = new CUST_MSG_RECORD_EX()
                {
                    TOUSERNAME = fanMessage.TOUSERNAME,
                    FROMUSERNAME = fanMessage.FROMUSERNAME,
                    MSGTYPE = "text",
                    CONTENT = content,
                    ReturnID = fanMessage.ID,
                    State = 1,
                    CREATE_DATE = DateTime.Now
                };
                num = (int)_mrr.Insert(replyMessage);
                scope.Complete();
            }
            return num;
        }

        public List<CUST_MSG_RECORD_EX> QueryReplyMessages(int id)
        {
            return _mrr.QueryReplyMessages(id);
        }

        public CUST_MSG_RECORD_EX GetFansMessage(int id)
        {
            return _mrr.GetFansMessage(id);
        }

        public CUST_MSG_RECORD_EX GetMessage(int id)
        {
            return _mrr.GetMessage(id);
        }
    }
}
