using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Extension;
using Hmj.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using WeChatCRM.Common.Utils;

namespace Hmj.WebApp.Controllers
{
    public class EmplReportController : WXMyControllerBase
    {
        private ISystemService _service;
        private DeptTree deptTree;      //部门列表，调用微信接口获得
        private void setDeptTree()
        {
            //获取access_token
            String access_token = getServiceToken();
            //调用微信接口,获取部门列表,json字符串
            String url = "https://qyapi.weixin.qq.com/cgi-bin/department/list";
            String data = "access_token=" + access_token;
            String method = "GET";
            int timeout = 6000;
            String contentType = "text/xml";
            String resp = NetHelper.HttpRequest(url, data, method, timeout, Encoding.UTF8, contentType);
            JObject jsonData = JObject.Parse(resp);
            JArray deptJArray = (JArray)jsonData["department"];
            DeptNode rootNode = null;
            foreach (JObject jNode in deptJArray)
            {
                if ("1" == jNode["id"].ToString())
                {
                    rootNode = new DeptNode(jNode);
                    break;
                }
            }
            deptTree = new DeptTree(deptJArray, rootNode);
        }

        //获取微信access_token
        private String getServiceToken()
        {
            //获取微信access_token所需参数
            String sCorpID = "wx42c6025ff9bb2576";
            String corpSecret = "L80AwfTPBxPdm9CQrkvHEPk-HYVIHREG7ukNWZ1-ZrMX_vPQXyJpriwl0IwhrMu3";
            //HttpRequest请求所需参数
            String url = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";
            String data = "corpid=" + sCorpID + "&corpsecret=" + corpSecret;
            String method = "GET";
            int timeout = 6000;
            String contentType = "text/xml";
            String resp = NetHelper.HttpRequest(url, data, method, timeout, Encoding.UTF8, contentType);
            JObject jsonData = JObject.Parse(resp);
            String access_token = (String)jsonData["access_token"];
            return access_token;
        }

        public EmplReportController(ISystemService service)
        {
            _service = service;
        }
        //
        // GET: /Demo/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List(EmpSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new EmpSearch();
            }
            search.FirstTime = form["FirstTime"];
            search.EndTime = form["EndTime"];
            search.DeptId = form["DeptId"];
            search.StoreName = form["StoreName"];
            List<string> deptIdList = new List<string>();
            if (deptTree == null)
                setDeptTree();
            List<EMPLOYEE_EX> pList = _service.QueryEmplReport(search);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            JsonQTable fdata = JsonQTable.ConvertFromList<EMPLOYEE_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        /// <summary>
        /// 查询左边的数据树值
        /// 调用微信的通讯录接口获取部门列表,将部门数据封装成JsonTreeNode形式返回
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QueryLeft(FormCollection form)
        {
            if (deptTree == null)
            {
                setDeptTree();
            }
            List<JsonTreeNode> jTreeList = new List<JsonTreeNode>();
            jTreeList.Add(getJTreeNode(deptTree));
            return Json(jTreeList);
        }

        private JsonTreeNode getJTreeNode(DeptTree deptTree)
        {
            JsonTreeNode jTreeNode = new JsonTreeNode();
            jTreeNode.id = deptTree.deptNode.id;
            jTreeNode.text = deptTree.deptNode.name;
            jTreeNode.value = deptTree.deptNode.id;
            jTreeNode.pid = deptTree.deptNode.parentId;
            jTreeNode.hasChildren = (deptTree.childTreeList.Count > 0 ? true : false);
            jTreeNode.data = new Dictionary<string, string>() { { "order", deptTree.deptNode.order } };
            if ("1" == deptTree.deptNode.id)
            {
                jTreeNode.isexpand = true;
            }
            else {
                jTreeNode.isexpand = false;
            }
            if (jTreeNode.hasChildren)
            {
                foreach (DeptTree childDeptTree in deptTree.childTreeList)
                {
                    jTreeNode.ChildNodes.Add(getJTreeNode(childDeptTree));
                }
            }
            jTreeNode.complete = true;
            return jTreeNode;
        }

        //导出员工列表
        [HttpGet]
        public FileResult ExportEmployeeList(string StoreName,string FirstTime, string EndTime)
        {
            //如果没有输条件，则导出该用户权限下所有员工数据

            JsonSMsg rMsg = new JsonSMsg();
            EmpSearch search = new EmpSearch();
            search.StoreName = StoreName;
            search.FirstTime = FirstTime;
            search.EndTime = EndTime;

            List<EMPLOYEE_EX> pList = _service.QueryEmplReport(search);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("id");
            row1.CreateCell(1).SetCellValue("userid");
            row1.CreateCell(2).SetCellValue("姓名");
            row1.CreateCell(3).SetCellValue("手机");
            row1.CreateCell(4).SetCellValue("ewmid");
            row1.CreateCell(5).SetCellValue("ewmurl");
            row1.CreateCell(6).SetCellValue("区域");
            row1.CreateCell(7).SetCellValue("门店");
            row1.CreateCell(8).SetCellValue("绑定人数");
            //将数据逐步写入sheet1各个行
            for (int j = 0; j < pList.Count; j++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(j + 1);
                rowtemp.CreateCell(0).SetCellValue(pList[j].ID);
                rowtemp.CreateCell(1).SetCellValue(pList[j].USERID);
                rowtemp.CreateCell(2).SetCellValue(pList[j].NAME);
                rowtemp.CreateCell(3).SetCellValue(pList[j].MOBILE);
                rowtemp.CreateCell(4).SetCellValue(pList[j].EwmId.ToString());
                rowtemp.CreateCell(5).SetCellValue(pList[j].EwmUrl);
                rowtemp.CreateCell(6).SetCellValue(pList[j].AreaName);
                rowtemp.CreateCell(7).SetCellValue(pList[j].StoreName);
                rowtemp.CreateCell(8).SetCellValue(pList[j].FansNum);
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, 0);

            return File(ms, "application/vnd.ms-excel", "员工列表.xls");

        }
    }
}
