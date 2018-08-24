using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IRoleService
    {
        PagedList<SYS_ROLE> QueryRoleList(RoleSearch search, PageView view);

        SYS_ROLE_EX GetRole(int id);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        int SaveRole(SYS_ROLE_EX role);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteRole(int id);

        List<LISTRIGHT_EX> QueryRightList();

        int SaveRole_Right(SYS_ROLE_RIGHT srr);

        /// <summary>
        /// 获取权限可选择的菜单
        /// </summary>
        /// <param name="orgId">公司编号</param>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        List<SYS_RIGHT_EX> GetRIGHTSByOrgId(int orgId, int roleId);

    }
}
