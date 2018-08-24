using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IStoreService
    {
        PagedList<ORG_STORE_EX> QueryAllStoreDate(StoreSearch search, PageView view);
     
        ORG_STORE_EX QueryStoreById(int id);

        ORG_STORE_EX QueryStoreById_Ex(int id);

        int SaveStore(ORG_STORE store);

        int DeleteStore(int id);

        List<ORG_STORE_EX> QueryStoreByOrgId(int orgId);

        List<ORG_DICT_EX> GetServicetype(string servicetype, int org_id);

        PagedList<ORG_STORE_EX> QueryAllStoreDateEx(StoreSearch search, PageView view);

        List<ORG_INFO_EX> GetORG_INFO_BY_ID(int ORG_ID);

        //获取服务类型
        List<ORG_DICT> GetServicetype(string servicetype);

        WXStore GetStore(int id);

        int UpdateStore(WXStore s);

        List<WXStore> GetStores(int merchants_ID);

        int AddStore(WXStore s);

        List<GROUP_INFO> GetAllGroup();

        List<GROUP_INFO_EX> GetAllGroupByID(int groupId);

        MDSearch GetStoreInfo(int storeId);

        MDSearch GetCode(string code);

        List<MDSearch> GetAllStore(int groupId);

        BaseDeptInfo GetGroupInfo(int groupId);

        int CreateDept(DeptInfo deptInfo, string accessToken, ref string errMsg);

        //int CreateStoreInfo(DeptInfo deptInfo, string accessToken);

        int ModifyDept(DeptInfo deptInfo, string accessToken, ref string errMsg);

        //int ModifyStoreInfo(DeptInfo deptInfo, string accessToken);

        DeptInfo GetDept(int id, int type);

        int DeleteDept(int id, string accessToken);

        void InitDepartments(string accessToken);

        void SyncUsersToLocal(string accessToken);

        List<GROUP_INFO> GetGroupInfoByCode(string code);

        void SyncDeptToRemote(string accessToken);
        /// <summary>
        /// 修改组织架构
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="deptId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        int UpdateDeptOrg(string accessToken, int deptId, int parentId, ref string errMsg);
    }
}
