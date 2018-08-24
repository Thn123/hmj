using Hmj.Business.ServiceImpl;
using Hmj.Common;
using Hmj.Entity;
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
    public class EWMController : WXMyControllerBase
    {
        //
        // GET: /Index/
        private ISystemService _service;
        private DeptTree deptTree;      //部门列表，调用微信接口获得
        private JArray deptJArray;
        private void setDeptJArray()
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
            deptJArray = (JArray)jsonData["department"];
        }
        private void setDeptTree()
        {
            if (deptJArray == null)
            {
                setDeptJArray();
            }
            DeptNode rootNode = null;
            //查找部门树的根节点
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

        public EWMController(ISystemService service)
        {
            _service = service;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Employee()
        {
            return View();
        }
        public ActionResult EWMEdit()
        {
            return View();
        }
        public ActionResult EmpEdit()
        {
            return View();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveEWM(QMActivity CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                CuObj.MdName = CuObj.MdName.ToString();
                //double x = 0;
                //double y = 0;
                //bd_decrypt(CuObj.Lat.Value, CuObj.Lng.Value, ref y, ref x);
                //CuObj.Lat = y;
                //CuObj.Lng = x;
                //CuObj.MIMAGE_ID = form["MIMAGE_ID"];
                int num = _service.SaveQMActivity(CuObj);
                WeiPage wp = new WeiPage();
                string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + wp.Token(AppConfig.FWHOriginalID);
                //这里需要修改
                string d = @"{ ""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"": {0}}}
        
        }";
                d = d.Replace("{0}", num.ToString());
                string mes = WeiPage.HttpXmlPostRequest(url, d, Encoding.UTF8);
                //Response.Write(mes);
                string[] b = mes.Split('\"');
                string ticket = Server.UrlEncode(b[3]);
                CuObj.Id = num;
                CuObj.Ticket = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket;
                CuObj.MdSel = "qrscene_" + num.ToString();
                num = _service.SaveQMActivity(CuObj);
                if (num > 0)
                {

                    rMsg.Status = 0;
                    rMsg.Message = "保存成功";
                }
                else
                {

                    rMsg.Status = -1;
                    rMsg.Message = "保存失败";

                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        public bool WriteTxt(string str)
        {
            ISystemService sbo = new SystemService();
            try
            {
                WXLOG l = new WXLOG();
                l.CON = str;
                l.TIME = DateTime.Now;
                sbo.AddLog(l);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }
        [HttpPost]
        public ActionResult SaveEmp(EMPLOYEE CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                //CuObj.NAME = CuObj.NAME.ToString();
                //CuObj.MOBILE = CuObj.MOBILE.ToString();
                CuObj.USERID = CuObj.MOBILE.ToString();
                CuObj.EwmId = _service.GetEwmId() + 1;
                string deptId = form["DeptId"];
                string parentDeptId = form["ParentDeptId"];
                //设置StoreName(所属部门)，AreaName(部门所属区域) 的值
                if (deptJArray == null)
                {
                    setDeptJArray();
                }
                foreach (JObject dept in deptJArray)
                {
                    if (dept["id"].ToString() == deptId)
                        CuObj.StoreName = dept["name"].ToString();
                    if (dept["id"].ToString() == parentDeptId)
                        CuObj.AreaName = dept["name"].ToString();

                }

                WeiPage wp = new WeiPage();
                string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + wp.Token(AppConfig.FWHOriginalID);
                //这里需要修改
                string d = @"{ ""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"": {0}}}
        
        }";
                d = d.Replace("{0}", CuObj.EwmId.ToString());
                string mes = WeiPage.HttpXmlPostRequest(url, d, Encoding.UTF8);
                //Response.Write(mes);
                string[] b = mes.Split('\"');
                string ticket = Server.UrlEncode(b[3]);
                //CuObj.Id = num;
                CuObj.EwmUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket;
                //CuObj.MdSel = "qrscene_" + num.ToString();
                int num = _service.SaveEMPLOYEE(CuObj);
                if (num > 0)
                {

                    rMsg.Status = 0;
                    rMsg.Message = "保存成功";
                }
                else
                {

                    rMsg.Status = -1;
                    rMsg.Message = "保存失败";

                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
        [HttpPost]
        public JsonResult DeleteBD(int id)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {


                _service.DeleteBD(id);
                rMsg.Status = 0;
                rMsg.Message = "删除成功";
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
        [HttpPost]
        public JsonResult DeleteEmp(int id)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                _service.DeleteEmp(id);
                rMsg.Status = 0;
                rMsg.Message = "删除成功";
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
        [HttpPost]
        public JsonResult UpdateBD(int id, string phone)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                EMPLOYEE e = _service.GetEmpByUserID(phone);    //根据账号获取员工信息
                if (e == null)
                {
                    rMsg.Status = 1;
                    rMsg.Message = "请输入正确的员工账号。";
                }
                else
                {
                    int ret = _service.UpdateBD(id, e.EwmId, phone);
                    if (ret > 0)
                    {

                        rMsg.Status = 0;
                        rMsg.Message = "转绑成功";
                    }
                    else
                    {
                        rMsg.Status = 1;
                        rMsg.Message = "";
                    }

                }

            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        public JsonResult List(string name, FormCollection form)
        {
            PageView view = new PageView(form);
            PagedList<QMActivity_EX> pList = _service.QueryGetQm(name, view);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<QMActivity_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }
        public JsonResult ListEmp(EmpSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new EmpSearch();
            }
            PagedList<EMPLOYEE> pList = _service.QueryGetEmp(search, view);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<EMPLOYEE>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }
        public JsonResult ListXQ(FansSearch search, FormCollection form)
        {
            if (search == null)
            {
                search = new FansSearch();
            }
            string mdsel = search.MdSel;
            search.ToUserName = base.CurrentMerchants.ToUserName == null ? "" : CurrentMerchants.ToUserName;
            PageView view = new PageView(form);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<CUST_FANS_EX> pList = _service.QueryGetFansByEWM(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<CUST_FANS_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        public JsonResult ListBD(FansSearch search, FormCollection form)
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
            PagedList<CUST_FANS_EX> pList = _service.QueryGetFansByMobile(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<CUST_FANS_EX>(pList, colkey, colsinfo.Split(','));
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
            else
            {
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
    }
}
