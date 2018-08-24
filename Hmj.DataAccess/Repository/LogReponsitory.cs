using Hmj.Entity;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class LogReponsitory : BaseRepository
    {
        /// <summary>
        /// 查询未激活的记录
        /// </summary>
        /// <returns></returns>
        public List<MEMBER_CHANGESTATUS_LOG> QueryMemberActivateList()
        {
            string sql = "select ID,ZTYPE,STATUS,MESSAGE,ZPASS,MOBILE,CREATE_DATE,USED from MEMBER_CHANGESTATUS_LOG where isnull(USED,0)=0";
            return base.Query<MEMBER_CHANGESTATUS_LOG>(sql, null);
        }

        /// <summary>
        /// 更新激活状态
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int UpdateMemberActivateUse(int ID)
        {
            string sql = "update MEMBER_CHANGESTATUS_LOG set USED=1 where ID=@ID";
            return base.Excute(sql, new { ID = ID });
        }

        /// <summary>
        /// 添加失败的记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public int InsertFailRecord(MEMBER_CHANGESTATUS_LOG record)
        {
            string sql = "insert MEMBER_CHANGESTATUS_LOG(ZTYPE,STATUS,MESSAGE,ZPASS,MOBILE,CREATE_DATE,USED) values(@ZTYPE,@STATUS,@MESSAGE,@ZPASS,@MOBILE,@CREATE_DATE,0);";
            return base.Excute(sql, new { ZTYPE = record.ZTYPE, STATUS = record.STATUS, MESSAGE = record.MESSAGE, ZPASS = record.ZPASS, MOBILE = record.MOBILE, CREATE_DATE = record.CREATE_DATE });
        }
    }
}
