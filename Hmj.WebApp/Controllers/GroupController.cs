using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.ExtendAPI.CenSH;
using Hmj.ExtendAPI.WeiXin;
using Hmj.ExtendAPI.WeiXin.Models;
using Hmj.Extension;
using Hmj.Interface;
using Hmj.WebApp.Common.Pages;
using Hmj.WebApp.Models;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class GroupController : WXMyControllerBase
    {
        [SetterProperty]
        public IStoreService StoreService { get; set; }
        [SetterProperty]
        public IEmployeeService EmployeeService { get; set; }
        [SetterProperty]
        public ISystemService SystemService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult QueryGroupTree(string pid, int? storeId)
        {
            pid = System.Web.HttpUtility.UrlDecode(pid);
            int parentId = int.Parse(pid);
            List<JSTreeNode> treelist = new List<JSTreeNode>();
            var parentNode = GenerateParentNode(parentId);
            treelist.Add(parentNode);

            var list = StoreService.GetAllGroup();
            parentNode.hasChildren = list != null && list.Count > 0;
            List<int> excludeIDs = new List<int>();
            if (storeId.HasValue)
            {
                excludeIDs.Add(storeId.Value);
            }
            SetTreeChildren(list, ref parentNode, parentId, excludeIDs);

            return Json(treelist);
        }

        private JSTreeNode GenerateParentNode(int id)
        {
            JSTreeNode nodeM = new JSTreeNode();
            nodeM.hasChildren = true;
            nodeM.id = id.ToString();
            nodeM.text = AppConfig.Get("QYDeptRootName");
            nodeM.type = "root";
            //nodeM.icon = "root";
            //nodeM.value = Guid.Empty.ToString();
            nodeM.data = new Dictionary<string, string>() { { "type", "0" }, { "isexpand", "true" } };
            //nodeM.isexpand = true;
            //nodeM.complete = true;

            return nodeM;
        }

        private void SetTreeChildren(List<GROUP_INFO> cuAllList, ref JSTreeNode pNode, int pid, List<int> excludeIDs)
        {
            if (!pNode.hasChildren)
            {
                return;
            }
            var childList = cuAllList.FindAll(c => c.PARENT_ID == pid);

            foreach (var item in childList)
            {

                if (excludeIDs.Contains(item.ID))
                {
                    continue;
                }
                JSTreeNode node = new JSTreeNode();
                node.id = item.ID.ToString();
                node.text = item.NAME;
                //node.value = item.ID.ToString();
                //node.isexpand = false;
                node.data = new Dictionary<string, string>() { { "type", item.TYPE.ToString() } };
                //node.complete = true;
                pNode.children.Add(node);
                if (item.TYPE != 3) //大区or区域
                {
                    node.type = "folder";
                    node.hasChildren = (cuAllList.Count(r => r.PARENT_ID == item.ID) > 0);
                    SetTreeChildren(cuAllList, ref node, item.ID, excludeIDs);
                }
                else
                {
                    node.type = "file";
                    node.hasChildren = false;
                }
            }
        }

        [HttpPost]
        public ActionResult SaveGroup(DeptInfo deptInfo, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string accessToken = HmjClientServiceApi.Create().GetAccessToken();
            string errMsg = "";
            deptInfo.IsPickUp = form["IsPickUp"] == "1";
            if (deptInfo.ID == 0)    //新增
            {
                int groupId = StoreService.CreateDept(deptInfo, accessToken, ref errMsg);
                deptInfo.ID = groupId;
                rMsg.Status = groupId;
            }
            else
            {
                rMsg.Status = StoreService.ModifyDept(deptInfo, accessToken, ref errMsg);
            }
            rMsg.Message = errMsg;
            rMsg.Data = deptInfo;


            return Json(rMsg);
        }

        //[HttpPost]
        //public ActionResult SaveStore(DeptInfo deptInfo, FormCollection form)
        //{
        //    JsonSMsg rMsg = new JsonSMsg();

        //    string accessToken = AccessTokenGenerator.Instance.GetAccessToken();
        //    string sId = form["ID"];
        //    if (string.IsNullOrEmpty(sId))    //新增
        //    {
        //        int storeId = StoreService.CreateStoreInfo(deptInfo, accessToken);
        //        rMsg.Status = deptInfo.ID = storeId;
        //    }
        //    else
        //    {
        //        rMsg.Status = StoreService.ModifyStoreInfo(deptInfo, accessToken);
        //    }
        //    rMsg.Data = deptInfo;


        //    return Json(rMsg);
        //}

        [HttpPost]
        public ActionResult GetDept(int type, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            rMsg.Status = 1;

            string sId = form["id"];
            DeptInfo deptInfo = StoreService.GetDept(int.Parse(sId), type);
            rMsg.Data = deptInfo;


            return Json(rMsg);
        }

        [HttpPost]
        public ActionResult DeleteDept(FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string accessToken = HmjClientServiceApi.Create().GetAccessToken();
            string sId = form["id"];
            int rows = StoreService.DeleteDept(int.Parse(sId), accessToken);
            if (rows > 0)
            {
                rMsg.Status = rows;
            }
            else if (rows == -1 || rows == -2)
            {
                rMsg.Message = "该部门下存在子部门或成员，请先清空后再删除";
            }

            return Json(rMsg);
        }
        [HttpPost]
        public ActionResult QueryEmpList(FormCollection form)
        {
            EmpInfoSearch search = new EmpInfoSearch();
            search.EmpName = form["EMP_NAME"];
            string sGroupId = form["SelectGroupId"];
            if (string.IsNullOrEmpty(sGroupId))
            {
                search.GroupID = 0;
            }
            else
            {
                search.GroupID = int.Parse(sGroupId);
            }
            PageView view = new PageView(form);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            //员工列表           
            PagedList<EMPLOYEE> pList = EmployeeService.QueryEmpList(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<EMPLOYEE>(pList, colkey, colsinfo.Split(','));
            //var list = EmployeeService.QueryEmpList(search.GroupID);
            //JsonQTable fdata = JsonQTable.ConvertFromList<EMPLOYEE_EX>(list, colkey, colsinfo.Split(','), CheckState<EMPLOYEE_EX>);
            return Json(fdata);
        }
        private static int CheckState<T>(T item)
        {
            return 1;
        }

        [HttpPost]
        public ActionResult SaveEmployee(EMPLOYEE_MODEL model, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string accessToken = HmjClientServiceApi.Create().GetAccessToken();
            if (model.ID == 0)
            {
                WeiPage wp = new WeiPage();
                string mpToken = wp.Token(AppConfig.FWHOriginalID);
                int ewmId = SystemService.GetEwmId() + 1;
                model.EwmId = ewmId;
                QRCodeResponse qrCodeResponse = WXMPClientServiceApi.Create().CreateQRCode(mpToken, ewmId);
                if (qrCodeResponse != null && qrCodeResponse.ErrorCode == 0)
                {
                    model.EwmUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + qrCodeResponse.Ticket;
                }
                else
                {
                    rMsg.Message = "获取服务号access_token失败，请重试。";
                    return Json(rMsg);
                }
            }
            DeptInfo dept = new DeptInfo();
            dept.ID = model.EmpGroupId;
            string errMsg = null;
            int rows = EmployeeService.SaveEmployee(accessToken, model, dept, ref errMsg);
            if (rows > 0)
            {
                rMsg.Status = 1;
            }
            else
            {
                if (string.IsNullOrEmpty(errMsg))
                {
                    errMsg = "保存失败。";
                }
                rMsg.Message = errMsg;
            }

            return Json(rMsg);
        }

        [HttpPost]
        public ActionResult GetEmp(int id, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            rMsg.Status = 1;

            EMPLOYEE empInfo = EmployeeService.GetEmp(id);
            rMsg.Data = empInfo;


            return Json(rMsg);
        }

        [HttpPost]
        public ActionResult DeleteEmp(int id, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string accessToken = HmjClientServiceApi.Create().GetAccessToken();
            rMsg.Status = EmployeeService.DeleteEmployee(accessToken, id);

            return Json(rMsg);
        }
        [HttpPost]
        public ActionResult QueryBindList(FansSearch search, FormCollection form)
        {
            if (search == null)
            {
                search = new FansSearch();
            }
            string mdsel = search.Mobile;
            search.ToUserName = base.CurrentMerchants.ToUserName == null ? "" : CurrentMerchants.ToUserName;
            PageView view = new PageView(form);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<CUST_FANS_EX> pList = SystemService.QueryGetFansByMobile(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<CUST_FANS_EX>(pList, colkey, colsinfo.Split(','));

            return Json(fdata);
        }
        [HttpPost]
        public ActionResult ModifyEmpStore(int id, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();

            string sDeptId = form["deptId"];
            string accessToken = HmjClientServiceApi.Create().GetAccessToken();
            string errMsg = null;
            int rows = EmployeeService.ModifyEmpDept(accessToken, id, int.Parse(sDeptId), ref errMsg);

            rMsg.Status = rows;

            return Json(rMsg);
        }
        [HttpPost]
        public ActionResult UpdateDeptOrg(int id, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();

            int parentId = int.Parse(form["parentId"]);

            string errMsg = "";
            string accessToken = AccessTokenGenerator.Instance.GetQYAccessToken();
            int rows = StoreService.UpdateDeptOrg(accessToken, id, parentId, ref errMsg);
            if (rows > 0)
            {
                rMsg.Message = "修改成功。";
            }
            else
            {
                rMsg.Message = errMsg;
            }
            rMsg.Status = rows;

            return Json(rMsg);
        }
        [HttpGet]
        public ActionResult ExportEmp(int id, string deptName)
        {
            var OrderList = EmployeeService.QueryEmpList(id);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("数据");

            int cellIndex = 0;
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(cellIndex++).SetCellValue("姓名");
            row1.CreateCell(cellIndex++).SetCellValue("账号");
            row1.CreateCell(cellIndex++).SetCellValue("手机号");
            row1.CreateCell(cellIndex++).SetCellValue("微信号");
            row1.CreateCell(cellIndex++).SetCellValue("邮箱");
            row1.CreateCell(cellIndex++).SetCellValue("工号");
            row1.CreateCell(cellIndex++).SetCellValue("性别");
            row1.CreateCell(cellIndex++).SetCellValue("职位");
            row1.CreateCell(cellIndex++).SetCellValue("品牌         ");
            row1.CreateCell(cellIndex++).SetCellValue("个人简介         ");
            row1.CreateCell(cellIndex++).SetCellValue("所在部门         ");
            //row1.CreateCell(cellIndex++).SetCellValue("头像");

            //将数据逐步写入sheet1各个行
            for (int j = 0; j < OrderList.Count; j++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(j + 1);

                int cellIdx = 0;
                var emp = OrderList[j];
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.NAME);
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.USERID);
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.MOBILE);
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.WECHAT_ID);
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.EMAIL);
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.EMP_NO);
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.GENDER.HasValue ? (emp.GENDER.Value == 1 ? "男" : "女") : "");
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.POSITION);
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.BRAND);
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.INTRO);
                rowtemp.CreateCell(cellIdx++).SetCellValue(emp.FullDeptName);
                //rowtemp.CreateCell(cellIdx++).SetCellValue("<img src='" + emp.AVATAR_URL + "' />");

            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            return File(ms, "application/vnd.ms-excel", deptName + DateTime.Now.ToString("yyMMddHHmm") + ".xls");


        }
    }
}
