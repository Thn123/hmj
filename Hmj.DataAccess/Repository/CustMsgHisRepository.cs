using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using System.Text;

namespace Hmj.DataAccess.Repository
{
    public class CustMsgHisRepository : BaseRepository
    {
        //查询粉丝发送给商户的消息记录
        public PagedList<CUST_MSG_HIS_EX> QueryFansMessages(PageView view, string toUserName, string msgType, bool? isStar, bool? isReturn, bool? hasRemark, string searchText)
        {
            StringBuilder sqlWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(toUserName))
                sqlWhere.Append(string.Format(" AND cmh.TOUSERNAME ='{0}'", toUserName));
            if (!string.IsNullOrEmpty(msgType))
                sqlWhere.Append(string.Format(" AND cmh.MSGTYPE ='{0}'", msgType));
            if (isStar.HasValue)
                sqlWhere.Append(string.Format(" AND cmh.IS_STAR = '{0}'", isStar.Value));
            if (isReturn.HasValue)
            {
                if (isReturn.Value)
                    sqlWhere.Append(string.Format(" AND cmh.IS_RETURN = '{0}'", isReturn.Value));
                else
                {
                    sqlWhere.Append(string.Format(" AND (cmh.IS_RETURN = '{0}' or cmh.IS_RETURN IS NULL)", isReturn.Value));
                }
            }

            if (hasRemark.HasValue)
            {
                if (hasRemark.Value)
                {
                    sqlWhere.Append(string.Format(" AND cmh.RETURN_CON IS NOT NULL"));
                }
                else
                {
                    sqlWhere.Append(string.Format(" AND cmh.RETURN_CON IS NULL"));
                }
            }
            if (!string.IsNullOrEmpty(searchText))
                sqlWhere.Append(string.Format(" AND cf.NAME LIKE '%{0}%' ", searchText));
            return base.PageGet<CUST_MSG_HIS_EX>(view, @" cf.NAME as NICKNAME,cf.IMAGE,cmh.*",
                "[CUST_MSG_HIS] cmh LEFT JOIN [CUST_FANS] cf ON cmh.FROMUSERNAME = cf.FROMUSERNAME", sqlWhere.ToString(), "cmh.ID", "ORDER BY cmh.CREATE_DATE DESC");
        }

        public CUST_MSG_HIS_EX GetFansMessage(int id)
        {
            var sql = @"SELECT cf.NAME as NICKNAME,cf.IMAGE,cmh.* FROM [CUST_MSG_HIS] cmh LEFT JOIN [CUST_FANS] cf ON cmh.FROMUSERNAME = cf.FROMUSERNAME WHERE cmh.ID=@ID";
            return base.Get<CUST_MSG_HIS_EX>(sql, new { ID = id });
        }
    }
}
