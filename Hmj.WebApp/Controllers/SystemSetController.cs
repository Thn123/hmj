using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Extension;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace WeChat.WebApp.Controllers
{
    public class SystemSetController : WXMyControllerBase
    {
        //这里需要修改
        //private IPaymentModeService _eservice;
        private ISystemService _service;
        public SystemSetController( ISystemService service)
        {
            //_eservice = eService;
            _service = service;
            ViewData["ORG_ID"] = base.CurrentUserOrgId;
        }
        //
        // GET: /SystemSet/

        //public SystemSetController(ISystemService service)
        //{
        //    _service = service;
        //}
        public ActionResult Index(string value)
        {
            ViewData["value"] = "证件类型";
            if (value != null)
                ViewData["value"] = value;
            return View();
        }

        /// <summary>
        /// 权限菜单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemMenu()
        {
            return View(new SYS_RIGHT_EX() { PARENT_ID = 0 });
        }

        ///// <summary>
        ///// 短信设置
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult SMSSettings()
        //{

        //    return View(_service.GetSMS_CONFIG_EX(base.CurrentUserOrgId));
        //}

        //[HttpPost]
        //public JsonResult SaveSMS()
        //{
        //    JsonSMsg mess = new JsonSMsg();
        //    //ContentResult content = new ContentResult();
        //    SMS_CONFIG_EX ex = _service.GetSMS_CONFIG_EX(base.CurrentUserOrgId);
        //    ex.Dx1.IS_BOOT = Request.Form["IS_BOOT1"].ToString() == "false" ? false : true;
        //    ex.Dx1.REMIND_TIME = int.Parse(Request.Form["REMIND_TIME1"].ToString() == "" ? "0" : Request.Form["REMIND_TIME1"].ToString());
        //    ex.Dx1.TEMPLET_CONTENT = Request.Form["TEMPLET_CONTENT1"].ToString();

        //    ex.Dx2.IS_BOOT = Request.Form["IS_BOOT2"].ToString() == "false" ? false : true;
        //    ex.Dx2.VALUE_CONDITION = int.Parse(Request.Form["VALUE_CONDITION2"].ToString() == "" ? "0" : Request.Form["VALUE_CONDITION2"].ToString());
        //    ex.Dx2.TEMPLET_CONTENT = Request.Form["TEMPLET_CONTENT2"].ToString();

        //    ex.Dx3.IS_BOOT = Request.Form["IS_BOOT3"].ToString() == "false" ? false : true;
        //    ex.Dx3.REMIND_TIME = int.Parse(Request.Form["REMIND_TIME3"].ToString() == "" ? "0" : Request.Form["REMIND_TIME3"].ToString());
        //    ex.Dx3.TEMPLET_CONTENT = Request.Form["TEMPLET_CONTENT3"].ToString();
        //    ex.Dx3.REMIND_INTERVAL = int.Parse(Request.Form["REMIND_INTERVAL3"].ToString() == "" ? "0" : Request.Form["REMIND_INTERVAL3"].ToString());

        //    ex.Dx4.IS_BOOT = Request.Form["IS_BOOT4"].ToString() == "false" ? false : true;
        //    ex.Dx4.VALUE_CONDITION = int.Parse(Request.Form["VALUE_CONDITION4"].ToString() == "" ? "0" : Request.Form["VALUE_CONDITION4"].ToString());
        //    ex.Dx4.TEMPLET_CONTENT = Request.Form["TEMPLET_CONTENT4"].ToString();
        //    ex.Dx4.REMIND_INTERVAL = int.Parse(Request.Form["REMIND_INTERVAL4"].ToString() == "" ? "0" : Request.Form["REMIND_INTERVAL4"].ToString());

        //    ex.Dx5.IS_BOOT = Request.Form["IS_BOOT5"].ToString() == "false" ? false : true;
        //   ex.Dx5.TEMPLET_CONTENT = Request.Form["TEMPLET_CONTENT5"].ToString();

        //    ex.Dx6.TEMPLET_CONTENT = Request.Form["TEMPLET_CONTENT6"].ToString();
        //    ex.Dx6.IS_BOOT = Request.Form["IS_BOOT6"].ToString() == "false" ? false : true ;
        //    if (_service.SaveSMS_CONFIG_EX(ex) > 0)
        //    {
        //        mess.Status = 0;
        //        mess.Message = "保存成功";
        //    }
        //    else
        //    {
        //        mess.Status = -1;
        //        mess.Message = "保存失败";
        //    }
        //    return Json(mess);
        //    //return content;
        //}

        public ActionResult SMSMessages()
        {
            return View();
        }

        //public JsonResult SMSMessageList(SMSMessageSearch search, FormCollection form)
        //{
        //    if (search == null)
        //    {
        //        search = new SMSMessageSearch();
        //    }
        //    PageView view = new PageView(form);

        //    string colkey = form["colkey"];
        //    string colsinfo = form["colsinfo"];
        //    PagedList<SMS_MESSAGE> pList = _service.QuerySMSMessageList(search, view);
        //    JsonQTable fdata = JsonQTable.ConvertFromPagedList<SMS_MESSAGE>(pList, colkey, colsinfo.Split(','));
        //    return Json(fdata);
        //}

        /// <summary>
        /// 查询左边的数据字典树值
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QueryLeft(FormCollection form)
        {
            List<JsonTreeNode> treelist = new List<JsonTreeNode>();
            JsonTreeNode nodeM = new JsonTreeNode();

            nodeM.id = "0";
            nodeM.text = "权限菜单";
            nodeM.value = "";
            nodeM.hasChildren = true;
            nodeM.isexpand = true;

            List<SYS_RIGHT> cuList = _service.GetAllRight();
            nodeM.isexpand = true;
            SetTreeChildree(cuList, ref nodeM, 0);

            treelist.Add(nodeM);
            return Json(treelist);

        }

        void SetTreeChildree(List<SYS_RIGHT> cuList, ref JsonTreeNode cuJsonTreeNode, int pid)
        {
            var childList = cuList.Where(c => c.PARENT_ID == pid);

            foreach (var p in childList)
            {
                JsonTreeNode node = new JsonTreeNode();
                node.hasChildren = (cuList.Count(r => r.PARENT_ID == p.RIGHT_ID) > 0);
                node.id = p.RIGHT_ID.ToString();
                node.text = p.RIGHT_NAME;
                node.value = p.MENU_CODE;
                node.isexpand = true;

                SetTreeChildree(cuList, ref node, p.RIGHT_ID);
                cuJsonTreeNode.ChildNodes.Add(node);
            }
        }
        /// <summary>
        /// 查询右边的数据字典列表值
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QueryRight(FormCollection form)
        {
            PageView view = new PageView(form);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];

            int id = form["id"] == null ? 0 : int.Parse(form["id"].ToString());

            PagedList<SYS_RIGHT> cList = _service.GetRightListByPid(id, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<SYS_RIGHT>(cList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        /// <summary>
        /// 获取数据字典编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int id)
        {
            SYS_RIGHT_EX cuobj = null;
            var jmsg = new JsonSMsg();
            try
            {
                cuobj = _service.GetRightByID(id);
                jmsg.Status = 0;
                jmsg.Data = cuobj;
            }
            catch (Exception ex)
            {
                jmsg.Status = -1;
                jmsg.Message = ex.Message;
            }
            return Json(jmsg);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(SYS_RIGHT_EX CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                //CuObj.IS_MENU = '1';
                //CuObj.IS_RIGHT = '1';
                // CuObj.ORG_ID = base.CurrentUserOrgId;
                try
                {
                    int.Parse(CuObj.RIGHT_DSC);
                }
                catch (Exception)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "排序字段应为数字";
                    return Json(rMsg);
                }
                if (CuObj.MENU_CODE == null || CuObj.MENU_CODE == "")
                {
                    rMsg.Status = -1;
                    rMsg.Message = "唯一编码不可为空";
                    return Json(rMsg);
                }
                if (CuObj.MENU_ICON == null || CuObj.MENU_ICON == "")
                {
                    CuObj.MENU_ICON = "home";
                }
                int ret = _service.SaveMenu(CuObj);
                if (ret >= 0)
                {
                    rMsg.Status = 0;
                    rMsg.Data = CuObj.RIGHT_ID;
                }
                else if (ret == -2)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "唯一编码重复，请重新填写";
                }
                //else if (ret == -3)
                //{
                //    rMsg.Status = -1;
                //    rMsg.Message = "数据字典名称值重复，请重新填写";
                //}
                else
                {
                    rMsg.Status = -1;
                    rMsg.Message = "意外错误，请稍后重试";
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                if (id == 0)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "参数错误！";
                    return Json(rMsg);
                }

                int ret = _service.DeleteMenu(id);
                if (ret > 0)
                {
                    rMsg.Status = 0;
                    rMsg.Data = ret;
                }
                else if (ret == -2)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "有子数据字典，不能删除。";
                }
                else if (ret == -3)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "有关联数据，不能删除。";
                }
                else if (ret == -4)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "删除失败，此异常请与管理员联系。";
                }
                else
                {
                    rMsg.Status = -1;
                    rMsg.Message = "意外错误，请稍后重试";
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
        #region 支付方式管理

        //public ActionResult PaymentMode()
        //{
        //    //ORG_ID 代表当前登录的用户ID
        //    return View(new PAYMENT_MODE());
        //}


        //[HttpPost]
        //public JsonResult PaymentList(FormCollection form)
        //{
        //    string pt = "";
        //    //if (form["POST_TYPES"] != null)
        //    //    int.TryParse(form["POST_TYPES"].ToString(), out pt);
        //    //var search = new PaymentModeSearch()
        //    //{
        //    //    TYPE = pt,
        //    //    //ORG_ID = base.CurrentUserOrgId
        //    //};

        //    if (form["POST_TYPES"] != null)
        //    {
        //        pt = form["POST_TYPES"].ToString();
        //    }
        //    var search = new PaymentModeSearch()
        //    {
        //        TYPE = pt
        //        //ORG_ID = base.CurrentUserOrgId
        //    };


        //    PageView view = new PageView(form);
        //    string colkey = form["colkey"];
        //    string colsinfo = form["colsinfo"];

        //    PagedList<PAYMENT_MODE_EX> pList = _eservice.QueryPostList(search, view);
        //    JsonQTable fdata = JsonQTable.ConvertFromPagedList<PAYMENT_MODE_EX>(pList, colkey, colsinfo.Split(','));
        //    return Json(fdata);
        //}


        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public ActionResult PostEdit(int? id)
        //{

        //    PAYMENT_MODE post = null;
        //    var jmsg = new JsonSMsg();
        //    try
        //    {
        //        if (id.HasValue)
        //        {
        //            post = _eservice.GetPost(id.Value);
        //        }
        //        else
        //        {
        //            post = new PAYMENT_MODE()
        //            {
        //                //ORG_ID = base.CurrentUserOrgId
        //            };
        //        }

        //        jmsg.Status = 0;
        //        jmsg.Data = post;
        //    }
        //    catch (Exception ex)
        //    {
        //        jmsg.Status = -1;
        //        jmsg.Message = ex.Message;
        //    }
        //    return Json(jmsg);
        //}

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        //public JsonResult PostSave(PAYMENT_MODE post, FormCollection collection)
        //{
        //    //JsonMsg类是用于操作之后的提示
        //    JsonSMsg rMsg = new JsonSMsg();
        //    try
        //    {
        //        string errmsg = "";
        //        //if (!ValidatePOS(post, out errmsg))
        //        //{
        //        //    rMsg.Status = -1;
        //        //    rMsg.Message = errmsg;
        //        //    return Json(rMsg);
        //        //}

        //        var IS_BUY_CARD = collection["YES_NO1"];
        //        var IS_BUY_SERVICE = collection["YES_NO2"];
        //        var IS_BUY_GOODS = collection["YES_NO3"];

        //        post.IS_BUY_CARD = (IS_BUY_CARD == "0" ? false : true);
        //        post.IS_BUY_SERVICE = (IS_BUY_SERVICE == "0" ? false : true);
        //        post.IS_BUY_GOODS = (IS_BUY_GOODS == "0" ? false : true);
        //        post.NAME = collection["PAY_NAME"];
        //        post.TYPE = collection["PAY_TYPE"];
        //        var c = collection["COMMISSIONRATE"];
        //        if (collection["DISCOUNTRATE"] != null && collection["DISCOUNTRATE"].ToString() != "")
        //        {
        //            post.DISCOUNT_RATE = Convert.ToDecimal(collection["DISCOUNTRATE"]);
        //        }
        //        if (collection["COMMISSIONRATE"] != null && collection["COMMISSIONRATE"].ToString() != "")
        //        {
        //            //if (float.Parse(collection["COMMISSIONRATE"].ToString()) > 0 && float.Parse(collection["COMMISSIONRATE"].ToString()) < 1)
        //            //{
        //                post.COMMISSION_RATE = Convert.ToDecimal(collection["COMMISSIONRATE"]);
        //                int ret = _eservice.SavePost(post);
        //                if (ret > 0)
        //                {
        //                    rMsg.Status = 0;
        //                    rMsg.Data = ret;
        //                }
        //                else if (ret == -2)
        //                {
        //                    rMsg.Status = -1;
        //                    rMsg.Message = "级别编号重复，请重新填写。";
        //                }
        //                else
        //                {
        //                    rMsg.Status = -1;
        //                    rMsg.Message = "意外错误，请稍后重试";
        //                }
        //            //}
        //            //else
        //            //{
        //            //    rMsg.Status = -1;
        //            //    rMsg.Message = "提成系数请输入0-1之间的数字";
        //            //}

        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        rMsg.Status = -1;
        //        rMsg.Message = ex.Message;
        //    }
        //    return Json(rMsg);
        //}

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="collection"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult PostDelete(int id)
        //{
        //    JsonSMsg rMsg = new JsonSMsg();
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            rMsg.Status = -1;
        //            rMsg.Message = "参数错误！";
        //            return Json(rMsg);
        //        }

        //        int ret = _eservice.DeletePost(id);
        //        if (ret > 0)
        //        {
        //            rMsg.Status = 0;
        //            rMsg.Data = ret;
        //        }
        //        //else if (ret == -2)
        //        //{
        //        //    rMsg.Status = -1;
        //        //    rMsg.Message = "此级别下有员工，不能删除。";
        //        //}
        //        else
        //        {
        //            rMsg.Status = -1;
        //            rMsg.Message = "意外错误，请稍后重试";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        rMsg.Status = -1;
        //        rMsg.Message = ex.Message;
        //    }
        //    return Json(rMsg);
        //}

        ///// <summary>
        ///// 验证
        ///// </summary>
        ///// <param name="post"></param>
        ///// <param name="errmsg"></param>
        ///// <returns></returns>
        //private static bool ValidatePOS(PAYMENT_MODE post, out string errmsg)
        //{
        //    errmsg = "";
        //    if (post == null)
        //    {
        //        errmsg = "参数错误！";
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(post.NAME))
        //    {
        //        errmsg = "支付名称不能为空！";
        //        return false;
        //    }
        //    return true;
        //}

        #endregion

        /// <summary>
        /// 商户
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        public ActionResult Merchants()
        {
            return View(new ORG_INFO() {  ID=0 });
        }
        [MyAuthorize]
        public ActionResult Merchant()
        {
            return View(new ORG_INFO() { ID = 0, MEM_PIC = "/assets/images/default.png" });
        }


        //列表
        [HttpPost]
        public JsonResult MerchantsList(RoleSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new RoleSearch();
            }
            search.ORG_ID = base.CurrentUserOrgId;
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<ORG_INFO> pList = _service.QueryMerchantsList(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<ORG_INFO>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        //修改       
        public ActionResult MerchantsEdit(int? id)
        {
            ORG_INFO cuobj = null;
            var jmsg = new JsonSMsg();
            try
            {
                if (id==0)
                    cuobj = new ORG_INFO() { ID = 0 };
                else
                {
                    cuobj = _service.GetMerchants(int.Parse(id.ToString()));
                }
                if (cuobj.MEM_PIC == null)
                {
                    cuobj.MEM_PIC = "/assets/images/default.png";
                }
                else
                {
                    cuobj.MEM_PIC = ConfigurationSettings.AppSettings["WebUrl"] + cuobj.MEM_PIC;
                }
                jmsg.Status = 0;
                jmsg.Data = cuobj;
            }
            catch (Exception ex)
            {
                jmsg.Status = -1;
                jmsg.Message = ex.Message;
            }
            return Json(jmsg);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveMerchants(ORG_INFO CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {

                int ret = _service.SaveMerchants(CuObj);
                if (ret >= 0)
                {
                    rMsg.Status = 0;
                    rMsg.Data = CuObj.ID;
                }
                else
                {
                    rMsg.Status = -1;
                    rMsg.Message = "意外错误，请稍后重试";
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
    }
}
