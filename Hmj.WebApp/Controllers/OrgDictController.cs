using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Extension;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WeChat.WebApp.Controllers
{
    public class OrgDictController : WXMyControllerBase
    {
        private IOrgDictService _service; // ICustomerService cService
        public OrgDictController(IOrgDictService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View(new ORG_DICT_EX() { ORG_ID = base.CurrentUserOrgId, ID = Guid.Empty, PARENT_ID = Guid.Empty, PARENT_NAME = "数据字典" });
        }

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

            nodeM.id = Guid.Empty.ToString();
            nodeM.text = "数据字典";
            nodeM.value = "";
            nodeM.hasChildren = true;
            nodeM.isexpand = true;
            nodeM.pid = Guid.Empty.ToString();
            List<ORG_DICT> cuList = _service.GetAllORG_DICT(base.CurrentUserOrgId);
            SetTreeChildree(cuList, ref nodeM, Guid.Empty);

            treelist.Add(nodeM);
            return Json(treelist);
        }

        void SetTreeChildree(List<ORG_DICT> cuList, ref JsonTreeNode cuJsonTreeNode, Guid pid)
        {
            var childList = cuList.Where(c => c.PARENT_ID == pid);

            foreach (var p in childList)
            {
                JsonTreeNode node = new JsonTreeNode();
                node.hasChildren = (cuList.Count(r => r.PARENT_ID == p.ID) > 0);
                node.id = p.ID.ToString();
                node.text = p.DICT_VALUE;
                node.value = p.DICT_CODE;
                node.pid = pid.ToString();
                node.isexpand = true;
                SetTreeChildree(cuList, ref node, p.ID);
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

            Guid id = form["id"] == null ? Guid.Empty : Guid.Parse(form["id"].ToString());

            PagedList<ORG_DICT> cList = _service.GetORG_DICTByParentId(id, view,CurrentLoginUser.ORG_ID);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<ORG_DICT>(cList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        /// <summary>
        /// 获取数据字典编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Guid id)
        {
            ORG_DICT_EX cuobj = null;
            var jmsg = new JsonSMsg();
            try
            {
                cuobj = _service.GetORG_DICTById(id);
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
        public ActionResult Save(ORG_DICT_EX CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                CuObj.ORG_ID = base.CurrentUserOrgId;
                int ret = _service.SaveORG_DICT(CuObj);
                if (ret >= 0)
                {
                    rMsg.Status = 0;
                    rMsg.Data = CuObj.ID;
                }
                else if (ret == -2)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "数据字典名称重复，请重新填写";
                }
                else if (ret == -3)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "数据字典名称值重复，请重新填写";
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                if (id == Guid.Empty)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "参数错误！";
                    return Json(rMsg);
                }

                int ret = _service.DeleteORG_DICT(id);
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

    }
}