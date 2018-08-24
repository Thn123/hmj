using Hmj.Entity;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class StoreInfoRepository : BaseRepository 
    {

        public MDSearch Get(int id)
        {
            string sql = @"SELECT * FROM MDSearch WHERE ID = @ID";

            return base.Get<MDSearch>(sql, new { ID = id });
        }
        public MDSearch GetCode(string code)
        {
            string sql = @"SELECT * FROM MDSearch WHERE Code = @code";

            return base.Get<MDSearch>(sql, new { code = code });
        }
        public MDSearch GetByGroupId(int groupId)
        {
            string sql = @"SELECT top 1 * FROM MDSearch WHERE GROUP_ID = @GroupID";

            return base.Get<MDSearch>(sql, new { GroupID = groupId });
        }

        public int Delete(int id)
        {
            string sql = @"DELETE FROM MDSearch WHERE ID = @ID";

            return base.Excute(sql, new { ID = id });
        }

        public int GetCountByGroupID(int groupID)
        {
            string sql = @"SELECT count(1)
                        FROM MDSearch
                        WHERE GROUP_ID = @GroupID";

            return base.Get<int>(sql, new { GroupID = groupID });
        }

        public List<MDSearch> GetByGroupID(int[] groupID)
        {
            string sql = @"SELECT *
                        FROM MDSearch
                        WHERE GROUP_ID in @GroupIDs";

            return base.Query<MDSearch>(sql, new { GroupIDs = groupID });
        }

        public List<GROUP_INFO> GetGroupInfoByCode(string code)
        {
            string sql = @"SELECT top 2 * FROM GROUP_INFO WHERE code in ( "+code+") order by name";

            return base.Query<GROUP_INFO>(sql, null);
        }
    }
}
