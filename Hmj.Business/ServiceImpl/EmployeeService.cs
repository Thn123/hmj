using Hmj.Common;
using Hmj.DataAccess;
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
    public class EmployeeService : IEmployeeService
    {

        private readonly EmployeeInfoRepository _empRepo;
        private readonly GroupInfoRepository _groupRepo;
        private readonly StoreInfoRepository _storeRepo;
        private readonly SystemSetRepository _systemRepo;


        public EmployeeService()
        {
            _empRepo = new EmployeeInfoRepository();
            _groupRepo = new GroupInfoRepository();
            _storeRepo = new StoreInfoRepository();
            _systemRepo = new SystemSetRepository();
        }
        public PagedList<EMPLOYEE> QueryEmpList(EmpInfoSearch search, PageView view)
        {
            int deptId = search.GroupID;
            List<int> deptIds = new List<int>();

            //获取所有大区、区域、门店
            if (deptId > 0)
            {
                deptIds.Add(deptId);
                var groups = _groupRepo.GetRecursiveAllByParentID(deptId);
                List<int> groupIds = groups.Select(m => m.ID).ToList();
                if (groupIds.Count > 0)
                {
                    deptIds.AddRange(groupIds);
                }
            }
            //else
            //{
            //    groupIds.Add(deptId);
            //}
            //var stores = _storeRepo.GetByGroupID(groupIds.ToArray());
            //if (stores.Count > 0)
            //{
            //    var storeIds = stores.Select(m => m.ID).ToList();
            //    deptIds.AddRange(storeIds);
            //}

            return _empRepo.QueryList(deptIds.ToArray(), search, view);
        }

        public List<EMPLOYEE_EX> QueryEmpList(int deptId)
        {
            List<EMPLOYEE_EX> empList = null;
            if (deptId == 0)    //查询所有员工
            {
                empList = _empRepo.QueryAllList();
            }
            else
            {
                List<int> deptIds = new List<int>();
                deptIds.Add(deptId);
                //获取所有大区、区域
                var groups = _groupRepo.GetRecursiveAllByParentID(deptId);
                List<int> groupIds = groups.Select(m => m.ID).ToList();
                if (groupIds.Count > 0)
                {
                    deptIds.AddRange(groupIds);
                }
                //else
                //{
                //    groupIds.Add(deptId);
                //}
                //var stores = _storeRepo.GetByGroupID(groupIds.ToArray());
                //if (stores.Count > 0)
                //{
                //    var storeIds = stores.Select(m => m.ID).ToList();
                //    deptIds.AddRange(storeIds);
                //}

                empList = _empRepo.QueryList(deptIds.ToArray());
            }
            if (empList != null)
            {
                List<GROUP_INFO> groupList = _groupRepo.GetAll();

                foreach (var item in empList)
                {
                    string name = "";
                    this.GetParentDeptNames(groupList, item.DeptID, ref name);
                    item.FullDeptName = "/" + AppConfig.Get("QYDeptRootName") + name;
                }
            }

            return empList;
        }

        private void GetParentDeptNames(List<GROUP_INFO> groupList, int deptId, ref string fullDepNames)
        {
            GROUP_INFO g = groupList.Find(m => m.ID == deptId);
            if (g != null)
            {
                fullDepNames = ("/" + g.NAME) + fullDepNames;
                GetParentDeptNames(groupList, g.PARENT_ID, ref fullDepNames);
            }
        }

        public int SaveEmployee(string accessToken, EMPLOYEE entity, DeptInfo dept, ref string errMsg)
        {
            using (TransScope scope = new TransScope())
            {
                int wxGroupId = 0, deptId = dept.ID;
                GROUP_INFO groupInfo = null;
                if (entity.ID > 0)
                {
                    var rel = _empRepo.GetGroupIdByEmpID(entity.ID);
                    deptId = rel;
                }
                groupInfo = _groupRepo.Get(deptId);
                if (groupInfo == null)
                {
                    return 0;
                }
                wxGroupId = groupInfo.WX_GROUP_ID;
                List<int> department = new List<int>() { wxGroupId };
                string gender = entity.GENDER.HasValue ? entity.GENDER.Value.ToString() : null;

                int rows = 0;
                if (entity.ID == 0)
                {
                    var response = WXQYClientServiceApi.Create().CreateUser(accessToken, entity.USERID, entity.NAME, department, entity.POSITION, entity.MOBILE, gender, entity.EMAIL, entity.WECHAT_ID);
                    if (response != null && response.ErrorCode == 0)
                    {
                        //同步头像
                        UserInfo user = WXQYClientServiceApi.Create().GetUser(accessToken, entity.USERID);
                        if (user != null)
                        {
                            entity.AVATAR_URL = user.Avatar;
                        }
                        entity.STATUS = 4;
                        entity.CREATE_TIME = DateTime.Now;
                        rows = (int)_empRepo.Insert(entity);
                        entity.ID = rows;
                        //修改关系表
                        if (rows > 0)
                        {
                            //删除所有员工关系
                            _empRepo.DeleteRelByEmpID(entity.ID);
                            //新增关系
                            REL_EMP_GROUP rel = new REL_EMP_GROUP();
                            rel.TYPE = groupInfo.TYPE;
                            rel.EMP_ID = entity.ID;
                            rel.GROUP_ID = deptId;
                            rel.CREATE_TIME = DateTime.Now;
                            rel.CREATE_USER = "system";
                            _empRepo.Insert(rel);
                        }
                    }
                    else
                    {
                        errMsg = response.ErrorMessage;
                    }
                }
                else
                {
                    var response = WXQYClientServiceApi.Create().UpdateUser(accessToken, entity.USERID, entity.NAME, department, entity.POSITION, entity.MOBILE, gender, entity.EMAIL, entity.WECHAT_ID);
                    if (response != null && response.ErrorCode == 0)
                    {
                        //同步头像
                        UserInfo user = WXQYClientServiceApi.Create().GetUser(accessToken, entity.USERID);
                        if (user != null)
                        {
                            entity.AVATAR_URL = user.Avatar;
                        }
                        entity.FullUpdate = false;
                        rows = _empRepo.Update(entity);
                    }
                    else
                    {
                        errMsg = response.ErrorMessage;
                    }
                }

                scope.Commit();

                return rows;
            }

        }
        public EMPLOYEE_EX GetEmp(int empId)
        {
            EMPLOYEE_EX emp = _empRepo.Get(empId);
            if (emp != null)
            {
                emp.FullDeptName = "/" + AppConfig.Get("QYDeptRootName") + _groupRepo.GetFullDeptName(emp.DeptID);
            }

            return emp;
        }

        public int DeleteEmployee(string accessToken, int empId)
        {
            EMPLOYEE emp = _empRepo.Get(empId);
            if (emp != null)
            {
                var response = WXQYClientServiceApi.Create().DeleteUser(accessToken, emp.USERID);
                if (response != null && response.ErrorCode == 0)
                {
                    using (TransScope scope = new TransScope())
                    {
                        int rows = _empRepo.Delete(empId);
                        _empRepo.DeleteRelByEmpID(empId);

                        scope.Commit();

                        return rows;
                    }
                }
            }

            return 0;
        }

        public List<EMPLOYEE> QueryAllEmpWithNoQrCode(string access_token)
        {
            var list = _empRepo.QueryAllNoQrCode();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    int ewmId = 0;
                    if (item.EwmId.HasValue)
                    {
                        ewmId = item.EwmId.Value;
                    }
                    else
                    {
                        ewmId = _systemRepo.GetEwmId() + 1;
                    }
                    item.EwmId = ewmId;
                    QRCodeResponse qrCodeResponse = WXMPClientServiceApi.Create().CreateQRCode(access_token, ewmId);
                    if (qrCodeResponse != null && qrCodeResponse.ErrorCode == 0)
                    {
                        string ticket = qrCodeResponse.Ticket;
                        item.EwmUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + qrCodeResponse.Ticket;
                    }
                    item.FullUpdate = false;

                    _empRepo.Update(item);
                }

            }

            return list;
        }

        public int ModifyEmpDept(string accessToken, int empId, int deptId, ref string errMsg)
        {
            EMPLOYEE_EX entity = _empRepo.Get(empId);
            if (entity != null)
            {
                GROUP_INFO_EX groupInfo = _groupRepo.Get(deptId);
                if (groupInfo != null)
                {
                    List<int> department = new List<int>() { groupInfo.WX_GROUP_ID };
                    string gender = entity.GENDER.HasValue ? entity.GENDER.Value.ToString() : null;
                    var response = WXQYClientServiceApi.Create().UpdateUser(accessToken, entity.USERID, entity.NAME, department, entity.POSITION, entity.MOBILE, gender, entity.EMAIL, entity.WECHAT_ID);
                    if (response != null && response.ErrorCode == 0)
                    {
                        REL_EMP_GROUP rel = _empRepo.GetRelByEmpID(empId);
                        rel.GROUP_ID = deptId;
                        rel.FullUpdate = false;

                        return _empRepo.Update(rel);
                    }
                    else
                    {
                        errMsg = response.ErrorMessage;
                    }

                }
            }

            return 0;
        }

        public int UpdateEmpStatus(string userId, byte status)
        {
            EMPLOYEE emp = _empRepo.GetByUserId(userId);
            if (emp != null)
            {
                emp.STATUS = status;
                emp.FullUpdate = false;

                return _empRepo.Update(emp);
            }

            return 0;
        }
    }
}
