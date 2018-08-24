using Hmj.Common;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Hmj.Business.ServiceImpl
{
    public class RoleService : IRoleService
    {
        RoleRepository _repo;
        public RoleService()
        {
            _repo = new RoleRepository();
        }
        public PagedList<SYS_ROLE> QueryRoleList(RoleSearch search, PageView view)
        {
            search.ROLE_NAMES = Utility.ClearSafeStringParma(search.ROLE_NAMES);
            return _repo.QueryRoleList(search, view);
        }
        public List<LISTRIGHT_EX> QueryRightList()
        {
            return _repo.QueryRightList();
        }
        public SYS_ROLE_EX GetRole(int id)
        {
            return _repo.GetRole(id);
        }

        //保存
        public int SaveRole(SYS_ROLE_EX role)
        {
            var cuo = _repo.SaveValidateRoleById(role);
            if (cuo > 0)
                return -2;//重复No
            int re = 0;
            role.ROLE_TYPE = 2; //默认公司自定义角色  
            using (TransactionScope scope = new TransactionScope())
            {
                if (role.ROLE_ID <= 0) // 新增
                { 
                    role.LAST_MODI_DATE = role.CREATE_DATE = DateTime.Now;
                    re = (int)_repo.Insert(role);
                    if (re > 0)
                    {
                        //新建角色菜单
                        if (!string.IsNullOrEmpty(role.MENU_IDS))
                        {
                            var menumIds = role.MENU_IDS.Split(',');
                            foreach (var item in menumIds)
                            {
                                SYS_ROLE_RIGHT srr = new SYS_ROLE_RIGHT()
                                {
                                    RIGHT_ID = Int32.Parse(item),
                                    ROLE_ID = re
                                };
                                int srrId = (int)_repo.Insert(srr);
                                if (srrId < 1)
                                {
                                    re = srrId;
                                    break;
                                }
                            }
                        }
                    }
                    
                }
                else
                {
                    role.LAST_MODI_DATE = DateTime.Now;
                    role.RemoveUpdateColumn("CREATE_USER");
                    role.RemoveUpdateColumn("CREATE_DATE");
                    re = (int)_repo.Update(role);
                    if (re > 0)
                    {
                        //先删除
                        if (_repo.DeleteRIGHTSByRoleId(role.ROLE_ID) < 0)
                        {
                            re = 0;
                        }
                        //再添加
                        else
                        {
                            //新建角色菜单
                            if (!string.IsNullOrEmpty(role.MENU_IDS))
                            {
                                var menumIds = role.MENU_IDS.Split(',');
                                foreach (var item in menumIds)
                                {
                                    SYS_ROLE_RIGHT srr = new SYS_ROLE_RIGHT()
                                    {
                                        RIGHT_ID = Int32.Parse(item),
                                        ROLE_ID = role.ROLE_ID
                                    };
                                    int srrId = (int)_repo.Insert(srr);
                                    if (srrId < 1)
                                    {
                                        re = srrId;
                                        break;
                                    }
                                }
                            }
                        }
                    }                   
                }

                if (re > 0)
                    scope.Complete();
            }
            return re;
        }
        //保存
        public int SaveRole_Right(SYS_ROLE_RIGHT srr)
        {
            return (int)_repo.Insert(srr);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteRole(int id)
        {
            return _repo.DeleteRole(id);
        }

        /// <summary>
        /// 获取权限可选择的菜单
        /// </summary>
        /// <param name="orgId">公司编号</param>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public List<SYS_RIGHT_EX> GetRIGHTSByOrgId(int orgId, int roleId)
        {
            return _repo.GetRIGHTSByOrgId(orgId, roleId);
        }
    }
}
