using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class EmployeeInfoRepository : BaseRepository
    {

        public int GetCountByGroupID(int groupID)
        {
            string sql = @"SELECT count(1) FROM REL_EMP_GROUP WHERE GROUP_ID = @GroupID";
            
            return base.Get<int>(sql, new { GroupID = groupID });
        }

        public int Delete(int id)
        {
            string sql = @"DELETE FROM EMPLOYEE WHERE ID = @ID";

            return base.Excute(sql, new { ID = id });
        }

        public int DeleteAllRel()
        {
            string sql = @"DELETE FROM REL_EMP_GROUP";

            return base.Excute(sql, null);
        }

        public int DeleteRelByEmpID(int empId)
        {
            string sql = @"DELETE FROM REL_EMP_GROUP WHERE EMP_ID = @EmpID";

            return base.Excute(sql, new { EmpID = empId });
        }

        //public int DeleteRelByGroupID(Guid groupId)
        //{
        //    string sql = @"DELETE FROM REL_EMP_GROUP WHERE GROUP_ID = @GroupID";

        //    return base.Excute(sql, new { GroupID = groupId });
        //}
        public EMPLOYEE GetByUserId(string userId)
        {
            string sql = @"select * from EMPLOYEE where USERID = @USER_ID";

            return base.Get<EMPLOYEE>(sql, new { USER_ID = userId });
        }

        public EMPLOYEE_EX Get(int id)
        {
            string sql = @"select EMP.*,GI.ID DeptID,GI.NAME DeptName,GI.CODE DeptCode,GI.TYPE DeptType
                        from EMPLOYEE EMP inner join REL_EMP_GROUP REG on REG.EMP_ID =EMP.ID 
                        inner join GROUP_INFO GI on GI.ID = REG.GROUP_ID
                        where EMP.ID = @ID";

            return base.Get<EMPLOYEE_EX>(sql, new { ID = id });
        }
        

        public PagedList<EMPLOYEE> QueryList(int[] groupIds, EmpInfoSearch search, PageView view)
        {
            string cols = @" EMP.*  ";
            string sqlWhere = "";
            if (groupIds.Length > 0)
            {
                string ids = string.Join(",", groupIds);
                sqlWhere += " AND REG.GROUP_ID IN (" + ids + ") ";
            }
            if (!string.IsNullOrEmpty(search.EmpName))
            {
                sqlWhere += " AND (EMP.NAME LIKE  '" + search.EmpName + "%' OR EMP.MOBILE LIKE '" + search.EmpName + "%' OR EMP.USERID LIKE '" + search.EmpName + "%')";
            }


            return base.PageGet<EMPLOYEE>(view, cols,
                "EMPLOYEE EMP inner join REL_EMP_GROUP REG on REG.EMP_ID =EMP.ID"
                , sqlWhere
                , "EMP.ID",
                "");
        }
        public List<EMPLOYEE_EX> QueryList(int[] groupIds)
        {
            string sql = @"select EMP.*,GI.ID DeptID,GI.NAME DeptName,GI.CODE DeptCode,GI.TYPE DeptType
                        from EMPLOYEE EMP inner join REL_EMP_GROUP REG on REG.EMP_ID =EMP.ID 
                        inner join GROUP_INFO GI on GI.ID = REG.GROUP_ID
                        where REG.GROUP_ID IN @GroupID";

            return base.Query<EMPLOYEE_EX>(sql, new { GroupID = groupIds });
        }
        public List<EMPLOYEE_EX> QueryAllList()
        {
            string sql = @"select EMP.*,GI.ID DeptID,GI.NAME DeptName,GI.CODE DeptCode,GI.TYPE DeptType
                        from EMPLOYEE EMP inner join REL_EMP_GROUP REG on REG.EMP_ID =EMP.ID
                        inner join GROUP_INFO GI on GI.ID = REG.GROUP_ID";

            return base.Query<EMPLOYEE_EX>(sql, null);
        }

        public List<EMPLOYEE> QueryAll()
        {
            string sql = @"select * from EMPLOYEE";

            return base.Query<EMPLOYEE>(sql, null);
        }

        public List<EMPLOYEE> QueryAllNoQrCode()
        {
            string sql = "select * from EMPLOYEE where Ewmurl is null";

            return base.Query<EMPLOYEE>(sql, null);
        }

        public int GetGroupIdByEmpID(int empId)
        {
            string sql = @"select top 1 GROUP_ID from rel_emp_group reg
                            where reg.emp_id=@EmpID";
            return base.Get<int>(sql, new { EmpID = empId });
        }

        public REL_EMP_GROUP GetRelByEmpID(int empId)
        {
            string sql = @"select top 1 * from rel_emp_group reg
                            where reg.emp_id=@EmpID";
            return base.Get<REL_EMP_GROUP>(sql, new { EmpID = empId });
        }
    }
}
