using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class RoleRepository : BaseRepository
    {

        public PagedList<SYS_ROLE> QueryRoleList(RoleSearch search, PageView view)
        {
            string cols = @" * ";
            string sqlWhere = " AND ORG_ID=" + search.ORG_ID + " AND ROLE_TYPE<>0";
            if (!string.IsNullOrEmpty(search.ROLE_NAMES))
            {
                sqlWhere += " AND A.[ROLE_NAME] LIKE '%" + search.ROLE_NAMES + "%'";
            }
            return base.PageGet<SYS_ROLE>(view, cols,
                "[SYS_ROLE] A " //table
                , sqlWhere
                , "A.ROLE_ID DESC",
                "");
        }

        public List<LISTRIGHT_EX> QueryRightList()
        {
            List<LISTRIGHT_EX> listright = new List<LISTRIGHT_EX>();
            List<SYS_RIGHT> listsys = new List<SYS_RIGHT>();

            string sql = @"SELECT * FROM SYS_RIGHT WHERE PARENT_ID = 0";
            listsys = base.Query<SYS_RIGHT>(sql, null);
            foreach (var item in listsys)
            {
                LISTRIGHT_EX right = new LISTRIGHT_EX();
                right.RIGHT_ID = item.RIGHT_ID;
                right.RIGHT_NAME = item.RIGHT_NAME;
                right.RIGHT_DSC = item.RIGHT_DSC;
                right.MENU_CODE = item.MENU_CODE;
                //right.IS_MENU = item.IS_MENU;
                //right.IS_RIGHT = item.IS_RIGHT;
                right.PARENT_ID = item.PARENT_ID;
                right.TARGET = item.TARGET;
                right.URL_LINK_TO = item.URL_LINK_TO;
                string listsql = @"SELECT * FROM SYS_RIGHT WHERE PARENT_ID = " + item.RIGHT_ID;
                right.LISTRIGHT = base.Query<SYS_RIGHT>(listsql, null);
                listright.Add(right);
            }
            return listright;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SYS_ROLE_EX GetRole(int id)
        {
            string sql = @"SELECT *
                          FROM [SYS_ROLE] WHERE [ROLE_ID]=@ROLE_ID";
            return base.Get<SYS_ROLE_EX>(sql, new { ROLE_ID = id });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteRole(int id)
        {
            string sql = "SELECT COUNT(0) FROM dbo.USER_INFO WHERE ROLE_ID=" + id;
            var cuC = base.Get<int>(sql, null);
            if (cuC > 0)
                return -2;

            sql = "DELETE FROM [SYS_ROLE_RIGHT] WHERE [ROLE_ID]=@ID";
            base.Excute(sql, new { ID = id });

            sql = "DELETE FROM [SYS_ROLE] WHERE [ROLE_ID]=@ID";
            return base.Excute(sql, new { ID = id });
        }

        /// <summary>
        /// 获取权限可选择的菜单
        /// </summary>
        /// <param name="orgId">公司编号</param>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public List<SYS_RIGHT_EX> GetRIGHTSByOrgId(int orgId, int roleId)
        {
            string SQLS = @"select distinct re.* from 
                (
                select DISTINCT A.*  
                ,D.ID ROLE_RIGHT_ID 
                from dbo.SYS_RIGHT A 
                LEFT JOIN dbo.SYS_ROLE_RIGHT B on A.RIGHT_ID=B.RIGHT_ID 
                AND B.ROLE_ID=(select top 1 role_id from sys_role r where r.org_id=" + orgId + @" and r.role_type=0) 
                LEFT JOIN SYS_ROLE_RIGHT D ON D.RIGHT_ID=B.RIGHT_ID 
                AND D.ROLE_ID=" + roleId + @"  
                WHERE B.ROLE_ID IS NOT NULL

                UNION ALL

                select r.*,null ROLE_RIGHT_ID from SYS_RIGHT r 
                where R.RIGHT_ID in 
                (
                select DISTINCT A.parent_id
                from dbo.SYS_RIGHT A 
                LEFT JOIN dbo.SYS_ROLE_RIGHT B on A.RIGHT_ID=B.RIGHT_ID 
                AND B.ROLE_ID=(select top 1 role_id from sys_role r where r.org_id=" + orgId + @" and r.role_type=0) 
                LEFT JOIN SYS_ROLE_RIGHT D ON D.RIGHT_ID=B.RIGHT_ID 
                AND D.ROLE_ID=" + roleId + @" 
                WHERE B.ROLE_ID IS NOT NULL)
               AND R.RIGHT_ID NOT IN 
                (
                  select DISTINCT A.RIGHT_ID
                
                from dbo.SYS_RIGHT A 
                LEFT JOIN dbo.SYS_ROLE_RIGHT B on A.RIGHT_ID=B.RIGHT_ID 
                AND B.ROLE_ID=(select top 1 role_id from sys_role r where r.org_id=" + orgId + @" and r.role_type=0) 
                LEFT JOIN SYS_ROLE_RIGHT D ON D.RIGHT_ID=B.RIGHT_ID 
                AND D.ROLE_ID=" + roleId + @"   
                WHERE B.ROLE_ID IS NOT NULL
                )        
                ) re";
            return base.Query<SYS_RIGHT_EX>(SQLS, null);
        }

        /// <summary>
        /// 检查名称重复
        /// </summary>
        /// <param name="cuObj"></param>
        /// <returns></returns>
        public int SaveValidateRoleById(SYS_ROLE cuObj)
        {
            string sql = @"SELECT [ROLE_ID] FROM [SYS_ROLE] where ROLE_NAME=@ROLE_NAME and ROLE_ID<>@ROLE_ID AND ORG_ID=@ORG_ID ";
            return base.Get<int>(sql, new { ROLE_NAME = cuObj.ROLE_NAME, ROLE_ID = cuObj.ROLE_ID, ORG_ID = cuObj.ORG_ID });
        }
        /// <summary>
        /// 删除当前角色下的菜单权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int DeleteRIGHTSByRoleId(int roleId)
        {
            try
            {
                string SQLS = @"delete from SYS_ROLE_RIGHT where role_Id=" + roleId;
                return base.Excute(SQLS, null);
            }
            catch (Exception)
            {
                return -1;
                throw;
            }

        }
    }
}
