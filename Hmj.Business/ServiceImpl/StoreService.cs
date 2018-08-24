using Hmj.Common;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.ExtendAPI.WeiXin;
using Hmj.ExtendAPI.WeiXin.Models;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmj.Business.ServiceImpl
{
    public class StoreService : BaseService, IStoreService
    {
        GroupInfoRepository _groupRepo;
        StoreReponsitory sReponsitory;
        ResourceRepository _rrepo;
        StoreInfoRepository _storeRepo;
        EmployeeInfoRepository _empRepo;
        public StoreService()
        {
            sReponsitory = new StoreReponsitory();
            _rrepo = new ResourceRepository();
            _groupRepo = new GroupInfoRepository();
            _storeRepo = new StoreInfoRepository();
            _empRepo = new EmployeeInfoRepository();
        }

        //获取服务类型
        public List<ORG_DICT_EX> GetServicetype(string servicetype, int org_id)
        {
            List<ORG_DICT_EX> dlist = _rrepo.GeDICTByPDicCode(servicetype, org_id);
            return dlist;
        }
        public PagedList<ORG_STORE_EX> QueryAllStoreDate(StoreSearch search, PageView view)
        {
            search.SNAME = Utility.ClearSafeStringParma(search.SNAME);
            return sReponsitory.QueryAllStoreDate(search, view);
        }


        public ORG_STORE_EX QueryStoreById(int id)
        {
            return sReponsitory.QueryStoreById(id);
        }

        public ORG_STORE_EX QueryStoreById_Ex(int id)
        {
            return sReponsitory.QueryStoreById_Ex(id);
        }

        public List<ORG_STORE_EX> QueryStoreByOrgId(int orgId)
        {
            return sReponsitory.QueryStoreByOrgId(orgId);
        }

        public int SaveStore(ORG_STORE store)
        {
            //NOTE:这里添加其他业务逻辑
            var cuo = sReponsitory.SaveValidateStoreById(store);
            if (cuo > 0)
                return -2;//重复No
            if (store.ID <= 0) // 新增
            {
                return (int)sReponsitory.Insert(store);
            }
            else
            {
                //store.RemoveUpdateColumn("CREATE_USER");
                store.RemoveUpdateColumn("OPEN_DATE_Str");
                store.RemoveUpdateColumn("CLOSE_DATE_Str");
                return sReponsitory.Update(store);
            }
        }

        public int DeleteStore(int id)
        {
            //删除，逻辑删除更好一点
            return sReponsitory.DeleteStore(id);
        }

        public PagedList<ORG_STORE_EX> QueryAllStoreDateEx(StoreSearch search, PageView view)
        {
            return sReponsitory.QueryAllStoreDateEx(search, view);
        }

        public List<ORG_INFO_EX> GetORG_INFO_BY_ID(int ORG_ID)
        {
            return sReponsitory.GetORG_INFO_BY_ID(ORG_ID);
        }

        //获取服务类型
        public List<ORG_DICT> GetServicetype(string servicetype)
        {
            List<ORG_DICT> dlist = _rrepo.GeDICTByPDicCode(servicetype);
            return dlist;
        }

        public WXStore GetStore(int id)
        {
            return sReponsitory.GetStore(id);
        }

        public List<WXStore> GetStores(int merchants_ID)
        {
            return sReponsitory.GetStores(merchants_ID);
        }

        public int AddStore(WXStore s)
        {
            return (int)_rrepo.Insert(s);
        }

        public int UpdateStore(WXStore s)
        {
            s.LastUpdateTime = DateTime.Now;
            return _rrepo.Update(s);
        }

        public List<GROUP_INFO> GetAllGroup()
        {
            return _groupRepo.GetAll();
        }

        public BaseDeptInfo GetGroupInfo(int groupId)
        {
            GROUP_INFO groupInfo = _groupRepo.Get(groupId);
            if (groupInfo != null)
            {
                BaseDeptInfo deptInfo = new BaseDeptInfo();
                deptInfo.ID = groupInfo.ID;
                deptInfo.DeptName = groupInfo.NAME;
                deptInfo.DeptCode = groupInfo.CODE;
                deptInfo.Type = groupInfo.TYPE;
                deptInfo.MagentoGroupID = groupInfo.MAGENTO_GROUP_ID;
                deptInfo.Order = groupInfo.WX_ORDER.HasValue ? groupInfo.WX_ORDER.Value : 0;
                deptInfo.ParentID = groupInfo.PARENT_ID;
                //deptInfo.DeptType = (byte)deptInfo.Type;
                deptInfo.WXGroupID = groupInfo.WX_GROUP_ID;

                return deptInfo;
            }
            return null;
        }

        public int CreateDept(DeptInfo deptInfo, string accessToken, ref string errMsg)
        {
            int groupId = 0, wxParentId = 0, rootId = AppConfig.QYDeptRootID;
            GROUP_INFO groupInfo = null;
            if (deptInfo.ParentID == 0) //创建大区
            {
                groupInfo = new GROUP_INFO();
                groupInfo.TYPE = 1;
                wxParentId = rootId;//根目录ID
            }
            else
            {
                GROUP_INFO parentDept = _groupRepo.Get(deptInfo.ParentID);
                if (parentDept != null)
                {
                    groupInfo = new GROUP_INFO();
                    wxParentId = parentDept.WX_GROUP_ID;

                    if (parentDept.TYPE == 1)   //如果父级是大区，则创建区域
                    {
                        groupInfo.TYPE = 2;
                    }
                    else if (parentDept.TYPE == 2)
                    {
                        groupInfo.TYPE = 3;
                    }
                }
            }
            if (groupInfo != null)
            {
                groupInfo.NAME = deptInfo.DeptName;
                groupInfo.CODE = deptInfo.DeptCode;
                groupInfo.PARENT_ID = deptInfo.ParentID;
                groupInfo.MAGENTO_GROUP_ID = deptInfo.MagentoGroupID;
                groupInfo.WX_ORDER = deptInfo.Order;
                groupInfo.CREATE_TIME = DateTime.Now;
                groupInfo.CREATE_USER = "system";

                DeptResponse response = WXQYClientServiceApi.Create().CreateDept(accessToken, groupInfo.NAME, wxParentId, groupInfo.WX_ORDER.Value);
                if (response != null && response.ErrorCode == 0)
                {
                    groupInfo.WX_GROUP_ID = response.ID;
                    groupInfo.WX_PARENT_ID = wxParentId;
                    groupId = (int)_groupRepo.Insert(groupInfo);

                    if (groupId > 0 && groupInfo.TYPE == 3)    //创建MDSearch
                    {
                        MDSearch storeInfo = new MDSearch();
                        storeInfo.Name = deptInfo.DeptName;
                        storeInfo.Code = deptInfo.DeptCode;
                        storeInfo.StoreType = deptInfo.StoreType;
                        storeInfo.IS_PICK_UP = deptInfo.IsPickUp;
                        storeInfo.Address = deptInfo.Address;
                        storeInfo.Phone = deptInfo.Telephone;
                        storeInfo.X = deptInfo.Latitude;
                        storeInfo.Y = deptInfo.Longitude;

                        storeInfo.BelongsAreaNo = deptInfo.BelongsAreaNo;
                        storeInfo.Province = deptInfo.Province;
                        storeInfo.City = deptInfo.City;
                        storeInfo.Area = deptInfo.Area;
                        storeInfo.PP = deptInfo.Brand;

                        storeInfo.GROUP_ID = groupId;

                        _groupRepo.Insert(storeInfo);
                    }
                    deptInfo.Type = groupInfo.TYPE;
                }
                else
                {
                    errMsg = response.ErrorMessage;
                }

            }

            return groupId;
        }


        public DeptInfo GetDept(int id, int type)
        {
            DeptInfo deptInfo = null;

            GROUP_INFO groupInfo = _groupRepo.Get(id);
            if (groupInfo != null)
            {
                deptInfo = new DeptInfo();
                deptInfo.ID = groupInfo.ID;
                deptInfo.DeptName = groupInfo.NAME;
                deptInfo.DeptCode = groupInfo.CODE;
                deptInfo.Type = groupInfo.TYPE;
                deptInfo.MagentoGroupID = groupInfo.MAGENTO_GROUP_ID;
                deptInfo.Order = groupInfo.WX_ORDER.HasValue ? groupInfo.WX_ORDER.Value : 0;
                deptInfo.ParentID = groupInfo.PARENT_ID;
                deptInfo.WXGroupID = groupInfo.WX_GROUP_ID;

                if (groupInfo.TYPE == 3)    //获取门店信息
                {
                    MDSearch storeInfo = _storeRepo.GetByGroupId(groupInfo.ID);
                    if (storeInfo != null)
                    {
                        deptInfo.StoreType = storeInfo.StoreType.HasValue ? storeInfo.StoreType.Value : 1;
                        deptInfo.IsPickUp = storeInfo.IS_PICK_UP.HasValue ? storeInfo.IS_PICK_UP.Value : false;
                        deptInfo.GroupID = storeInfo.GROUP_ID.HasValue ? storeInfo.GROUP_ID.Value : 0;
                        deptInfo.Address = storeInfo.Address;
                        deptInfo.Telephone = storeInfo.Phone;
                        deptInfo.Latitude = storeInfo.X;
                        deptInfo.Longitude = storeInfo.Y;

                        deptInfo.BelongsAreaNo = storeInfo.BelongsAreaNo;
                        deptInfo.Province = storeInfo.Province;
                        deptInfo.City = storeInfo.City;
                        deptInfo.Area = storeInfo.Area;
                        deptInfo.Brand = storeInfo.PP;

                    }
                }

            }

            return deptInfo;
        }

        public int ModifyDept(DeptInfo deptInfo, string accessToken, ref string errMsg)
        {
            int rows = 0, rootId = AppConfig.QYDeptRootID;
            GROUP_INFO existGroupInfo = _groupRepo.Get(deptInfo.ID);
            if (existGroupInfo != null)
            {
                int wxParentId = 0;
                if (existGroupInfo.PARENT_ID == 0)
                {
                    wxParentId = rootId;
                }
                else
                {
                    GROUP_INFO parentInfo = _groupRepo.Get(existGroupInfo.PARENT_ID);
                    if (parentInfo != null)
                    {
                        wxParentId = parentInfo.WX_GROUP_ID;
                    }
                }
                DeptResponse response = WXQYClientServiceApi.Create().UpdateDept(accessToken, existGroupInfo.WX_GROUP_ID, deptInfo.DeptName, wxParentId, deptInfo.Order);
                if (response != null && response.ErrorCode == 0)
                {
                    GROUP_INFO groupInfo = new GROUP_INFO();
                    groupInfo.ID = deptInfo.ID;
                    groupInfo.NAME = deptInfo.DeptName;
                    groupInfo.CODE = deptInfo.DeptCode;
                    groupInfo.MAGENTO_GROUP_ID = deptInfo.MagentoGroupID;
                    groupInfo.WX_ORDER = deptInfo.Order;
                    groupInfo.LAST_MODIFY_TIME = DateTime.Now;
                    groupInfo.LAST_MODIFY_USER = "system";
                    groupInfo.FullUpdate = false;

                    rows = (int)_groupRepo.Update(groupInfo);
                    if (rows > 0 && existGroupInfo.TYPE == 3)
                    {
                        MDSearch storeInfo = _storeRepo.GetByGroupId(deptInfo.ID);
                        if (storeInfo == null)
                        {
                            storeInfo = new MDSearch();
                        }
                        storeInfo.Name = deptInfo.DeptName;
                        storeInfo.Code = deptInfo.DeptCode;
                        storeInfo.StoreType = deptInfo.StoreType;
                        storeInfo.IS_PICK_UP = deptInfo.IsPickUp;
                        storeInfo.Address = deptInfo.Address;
                        storeInfo.Phone = deptInfo.Telephone;
                        storeInfo.X = deptInfo.Latitude;
                        storeInfo.Y = deptInfo.Longitude;
                        storeInfo.BelongsAreaNo = deptInfo.BelongsAreaNo;
                        storeInfo.Province = deptInfo.Province;
                        storeInfo.City = deptInfo.City;
                        storeInfo.Area = deptInfo.Area;
                        storeInfo.PP = deptInfo.Brand;

                        if (storeInfo.ID > 0)
                        {
                            storeInfo.FullUpdate = false;
                            _storeRepo.Update(storeInfo);
                        }
                        else
                        {
                            storeInfo.GROUP_ID = deptInfo.ID;

                            _groupRepo.Insert(storeInfo);
                        }
                    }
                }
                else
                {
                    errMsg = response.ErrorMessage;
                }
            }


            return rows;
        }

        public int DeleteDept(int id, string accessToken)
        {
            int rows = 0;
            GROUP_INFO groupInfo = _groupRepo.Get(id);
            if (groupInfo != null)
            {
                int childrenCount = _groupRepo.GetCountByParentID(id);
                if (childrenCount > 0)
                {
                    rows = -1;  //如果有子部门，不能删除
                }
                int empCount = _empRepo.GetCountByGroupID(id);
                if (empCount > 0)
                {
                    rows = -2;  //组织结构下有员工，不能删除
                }

                DeptResponse response = WXQYClientServiceApi.Create().DeleteDept(accessToken, groupInfo.WX_GROUP_ID);
                if (response != null && response.ErrorCode == 0)
                {
                    rows = _groupRepo.Delete(id);
                    if (groupInfo.TYPE == 3)
                    {
                        MDSearch storeInfo = _storeRepo.GetByGroupId(id);
                        if (storeInfo != null)
                        {
                            _storeRepo.Delete(storeInfo.ID);
                        }
                    }
                }

            }

            return rows;
        }


        public List<GROUP_INFO_EX> GetAllGroupByID(int groupId)
        {
            var list = _groupRepo.GetAllByParentID(groupId);
            GetAllGroupAsTree(list);

            return list;
        }

        public void GetAllGroupAsTree(List<GROUP_INFO_EX> list)
        {
            //list = _groupRepo.GetAllByParentID(groupId);
            if (list == null || list.Count == 0) return;

            foreach (var item in list)
            {
                var children = _groupRepo.GetAllByParentID(item.ID);
                item.Children = children;
                GetAllGroupAsTree(children);
            }
        }

        public MDSearch GetStoreInfo(int storeId)
        {
            return _storeRepo.Get(storeId);
        }

        public MDSearch GetCode(string code)
        {
            return _storeRepo.GetCode(code);
        }

        public List<MDSearch> GetAllStore(int groupId)
        {
            var groups = _groupRepo.GetRecursiveAllByParentID(groupId);
            int[] groupIds = groups.Select(m => m.ID).ToArray();
            if (groupIds.Length > 0)
            {
                List<MDSearch> list = _storeRepo.GetByGroupID(groupIds);
                list.ForEach(delegate (MDSearch t)
                {
                    t.ID = t.GROUP_ID.HasValue ? t.GROUP_ID.Value : 0;
                });

                return list;
            }
            return null;
        }


        public void InitDepartments(string accessToken)
        {

            //select a.ID,a.NAME,a.PARENT_ID,a.WX_GROUP_ID,a.wx_parent_id,
            //b.ID,b.NAME, b.WX_GROUP_ID,b.wx_parent_id,
            //'update GROUP_INFO set PARENT_ID = ' + str(b.ID) + ' where ID =' + str(a.ID)
            //from GROUP_INFO a
            //left
            //join GROUP_INFO b on a.wx_parent_id = b.WX_GROUP_ID
            //where b.id is not null

            //select *,
            //'update MDSearch set group_id='+STR(g.ID) +' where id = '+str(m.ID),
            //'update group_info set code =''' + m.Code+''' where id= ' +STR(g.id)
            //from GROUP_INFO g
            //inner join MDSearch m on m.Name = g.NAME

            int rootId = AppConfig.QYDeptRootID;
            DeptListResponse response = WXQYClientServiceApi.Create().QueryDept(accessToken, rootId);
            if (response != null && response.Departments != null)
            {
                List<GROUP_INFO> groupList = new List<GROUP_INFO>();
                List<Department> children = response.Departments.FindAll(m => m.ID != rootId);
                foreach (var item in children)
                {
                    GROUP_INFO g = new GROUP_INFO();
                    g.NAME = item.Name;
                    g.CODE = "";
                    g.PARENT_ID = 0;
                    if (item.ParentID == rootId)
                    {
                        g.TYPE = 1;
                    }
                    else
                    {
                        var cc = children.FindAll(m => m.ParentID == item.ID);
                        if (cc.Count > 0)
                        {
                            g.TYPE = 2;
                        }
                        else
                        {
                            g.TYPE = 3;
                        }
                    }
                    g.WX_GROUP_ID = item.ID;
                    g.WX_PARENT_ID = item.ParentID;
                    g.WX_ORDER = item.Order;
                    g.CREATE_TIME = DateTime.Now;
                    g.CREATE_USER = "system";
                    groupList.Add(g);
                }

                _storeRepo.BatchInsert(groupList);
            }
        }


        public void SyncUsersToLocal(string accessToken)
        {

            int rootId = AppConfig.QYDeptRootID;
            UserListResponse response = WXQYClientServiceApi.Create().QueryUser(accessToken, rootId);
            if (response != null
                && response.Users != null
                && response.Users.Count > 0)
            {
                _empRepo.DeleteAllRel();
                var empList = _empRepo.QueryAll();
                var groupList = _groupRepo.GetAll();
                foreach (var item in response.Users)
                {
                    EMPLOYEE emp = empList.FirstOrDefault(m => m.USERID == item.UserId);
                    bool existed = emp != null;
                    if (!existed)
                    {
                        emp = new EMPLOYEE();
                    }
                    emp.USERID = item.UserId;
                    emp.NAME = item.Name;
                    emp.MOBILE = item.Mobile;
                    emp.EMAIL = item.Email;
                    emp.WECHAT_ID = item.WeiXinId;
                    emp.EMP_NO = item.UserId;
                    emp.GENDER = item.Gender;
                    emp.POSITION = item.Position;
                    emp.AVATAR_URL = item.Avatar;
                    emp.STATUS = item.Status;
                    emp.CREATE_TIME = DateTime.Now;

                    if (existed)
                    {
                        emp.FullUpdate = false;
                        _empRepo.Update(emp);
                    }
                    else
                    {
                        emp.ID = (int)_empRepo.Insert(emp);
                    }

                    if (item.Department != null && item.Department.Length > 0)
                    {
                        var groupId = item.Department[0];

                        var groupInfo = groupList.FirstOrDefault(m => m.WX_GROUP_ID == groupId);
                        if (groupInfo != null)
                        {
                            REL_EMP_GROUP rel = new REL_EMP_GROUP();
                            rel.TYPE = groupInfo.TYPE;
                            rel.EMP_ID = emp.ID;
                            rel.GROUP_ID = groupInfo.ID;
                            rel.CREATE_TIME = DateTime.Now;
                            rel.CREATE_USER = "system";
                            _empRepo.Insert(rel);
                        }
                        else
                        {
                            LogService.Debug(groupId + ",");
                        }
                    }
                    else
                    {
                        LogService.Info(item.UserId + ",");
                    }

                }
            }
        }

        public List<GROUP_INFO> GetGroupInfoByCode(string code)
        {
            return _storeRepo.GetGroupInfoByCode(code);
        }

        public void SyncDeptToRemote(string accessToken)
        {
            List<GROUP_INFO> groups = this._groupRepo.GetAllByNames();
            foreach (var item in groups)
            {
                int order = item.WX_ORDER.HasValue ? item.WX_ORDER.Value : 1;
                DeptResponse response = WXQYClientServiceApi.Create().UpdateDept(accessToken, item.WX_GROUP_ID, item.NAME, item.WX_PARENT_ID.Value, order);
                if (response != null && response.ErrorCode == 0)
                {
                    LogService.Info(item.NAME + "-成功");
                }
                else
                {
                    LogService.Warn(item.NAME + "-失败");
                }
            }
        }
        /// <summary>
        /// 修改组织架构
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="deptId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public int UpdateDeptOrg(string accessToken, int deptId, int parentId, ref string errMsg)
        {
            int rows = 0, rootId = AppConfig.QYDeptRootID;
            GROUP_INFO existGroupInfo = _groupRepo.Get(deptId);
            if (existGroupInfo != null)
            {
                int wxParentId = 0;
                if (parentId == 0)
                {
                    wxParentId = rootId;
                }
                else
                {
                    GROUP_INFO parentInfo = _groupRepo.Get(parentId);
                    if (parentInfo != null)
                    {
                        wxParentId = parentInfo.WX_GROUP_ID;
                    }
                }
                DeptResponse response = WXQYClientServiceApi.Create().UpdateDept(accessToken, existGroupInfo.WX_GROUP_ID, existGroupInfo.NAME, wxParentId, existGroupInfo.WX_ORDER.Value);
                if (response != null && response.ErrorCode == 0)
                {
                    GROUP_INFO groupInfo = new GROUP_INFO();
                    groupInfo.ID = deptId;
                    groupInfo.PARENT_ID = parentId;
                    groupInfo.WX_PARENT_ID = wxParentId;
                    groupInfo.LAST_MODIFY_TIME = DateTime.Now;
                    groupInfo.LAST_MODIFY_USER = "system";
                    groupInfo.FullUpdate = false;

                    rows = (int)_groupRepo.Update(groupInfo);

                }
                else
                {
                    errMsg = response.ErrorMessage;
                }
            }


            return rows;
        }

    }
}
