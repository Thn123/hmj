using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using System.Collections.Generic;
using System.Text;

namespace Hmj.DataAccess.Repository
{
    public class WXMessageRecordRepository : BaseRepository
    {
        //查询粉丝发送给商户的消息记录
        public PagedList<CUST_MSG_RECORD_EX> QueryFansMessages(PageView view, string toUserName, bool? isStar, bool? isReturn, string searchText)
        {
            StringBuilder sqlWhere = new StringBuilder(" AND cmr.State = 0");
            if (!string.IsNullOrEmpty(toUserName))
                sqlWhere.Append(string.Format(" AND cmr.TOUSERNAME ='{0}'", toUserName));
            if (isStar.HasValue)
                sqlWhere.Append(string.Format(" AND cmr.IS_STAR = '{0}'", isStar.Value));
            if (isReturn.HasValue)
            {
                if (isReturn.Value)
                    sqlWhere.Append(string.Format(" AND cmr.IS_RETURN = '{0}'", isReturn.Value));
                else
                {
                    sqlWhere.Append(string.Format(" AND (cmr.IS_RETURN = '{0}' or cmr.IS_RETURN IS NULL)", isReturn.Value));
                }
            }
            if (!string.IsNullOrEmpty(searchText))
                sqlWhere.Append(string.Format(" AND cf.NAME LIKE '%{0}%' ", searchText));
            return base.PageGet<CUST_MSG_RECORD_EX>(view, @" cf.NAME as Fname,cf.IMAGE,cmr.*",
                "[WXCUST_MSG_RECORD] cmr LEFT JOIN [WXCUST_FANS] cf ON cmr.FROMUSERNAME = cf.FROMUSERNAME", sqlWhere.ToString(), "cmr.ID", "ORDER BY cmr.CREATE_DATE DESC");
        }
        //查询粉丝发送给商户的消息记录
        public PagedList<CUST_MSG_RECORD_EX> QueryFansMessagesN(PageView view, string toUserName, bool? isStar, bool? isReturn, string searchText)
        {
            StringBuilder sqlWhere = new StringBuilder(" AND cmr.State = 0");
            sqlWhere.Append(string.Format(" AND cmr.FROMUSERNAME not in (select FROMUSERNAME from Emp_Cust)"));


            if (!string.IsNullOrEmpty(toUserName))
                sqlWhere.Append(string.Format(" AND cmr.TOUSERNAME ='{0}'", toUserName));
            if (isStar.HasValue)
                sqlWhere.Append(string.Format(" AND cmr.IS_STAR = '{0}'", isStar.Value));
            if (isReturn.HasValue)
            {
                if (isReturn.Value)
                    sqlWhere.Append(string.Format(" AND cmr.IS_RETURN = '{0}'", isReturn.Value));
                else
                {
                    sqlWhere.Append(string.Format(" AND (cmr.IS_RETURN = '{0}' or cmr.IS_RETURN IS NULL)", isReturn.Value));
                }
            }
            if (!string.IsNullOrEmpty(searchText))
                sqlWhere.Append(string.Format(" AND cf.NAME LIKE '%{0}%' ", searchText));
            return base.PageGet<CUST_MSG_RECORD_EX>(view, @" cf.NAME as Fname,cf.IMAGE,cmr.*",
                "[WXCUST_MSG_RECORD] cmr LEFT JOIN [WXCUST_FANS] cf ON cmr.FROMUSERNAME = cf.FROMUSERNAME", sqlWhere.ToString(), "cmr.ID", "ORDER BY cmr.CREATE_DATE DESC");
        }
        public CUST_MSG_RECORD_EX GetFansMessage(int id)
        {
            var sql = @"SELECT cf.NAME as NICKNAME,cf.IMAGE,cmr.* FROM [CUST_MSG_HIS] cmr LEFT JOIN [CUST_FANS] cf ON cmr.FROMUSERNAME = cf.FROMUSERNAME WHERE cmr.ID=@ID";
            return base.Get<CUST_MSG_RECORD_EX>(sql, new { ID = id });
        }

        public CUST_MSG_RECORD_EX GetMessage(int id)
        {
            var sql = @"SELECT * FROM [WXCUST_MSG_RECORD] WHERE ID=@ID";
            return base.Get<CUST_MSG_RECORD_EX>(sql, new { ID = id });
        }

        //得到一条消息和它的所有回复
        public List<CUST_MSG_RECORD_EX> QueryReplyMessages(int id)
        {
            var sql = @" SELECT CMR.*,CF.NAME as Fname,CF.IMAGE
 FROM [WXCUST_MSG_RECORD] CMR 
 LEFT JOIN [WXCUST_FANS] CF ON CMR.FROMUSERNAME = CF.FROMUSERNAME 
 WHERE CMR.ID=@ID OR ReturnID=@ID
 ORDER BY CMR.State ASC,CMR.CREATE_DATE DESC";
            return base.Query<CUST_MSG_RECORD_EX>(sql, new { ID = id });
        }
    }
}
