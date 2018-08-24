using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Extension;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WeChat.WebApp.Controllers
{
    public class RoleController : WXMyControllerBase
    {
        private IRoleService _cservice;
        public RoleController(IRoleService cService)
        {
            _cservice = cService;
        }

        public ActionResult Index()
        {
            return View(new SYS_ROLE_EX() { ORG_ID = base.CurrentUserOrgId, ROLE_ID = 0 });
        }

        //列表
        [HttpPost]
        public JsonResult List(RoleSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new RoleSearch();
            }
            search.ORG_ID = base.CurrentUserOrgId;
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<SYS_ROLE> pList = _cservice.QueryRoleList(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<SYS_ROLE>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        //修改       
        public ActionResult Edit(int? id)
        {
            SYS_ROLE_EX cuobj = null;
            var jmsg = new JsonSMsg();
            try
            {
                if (!id.HasValue)
                    cuobj = new SYS_ROLE_EX() { ORG_ID = base.CurrentUserOrgId, ROLE_ID = 0 };
                else
                {
                    cuobj = _cservice.GetRole(id.Value);
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

        #region 获取权限菜单树

        [HttpPost]
        public JsonResult GetTreeList(int orgId, int? roleId)
        {
            List<JsonTreeNode> treelist = new List<JsonTreeNode>();

            List<SYS_RIGHT_EX> cuList = _cservice.GetRIGHTSByOrgId(orgId, (roleId.HasValue ? roleId.Value : 0));
            var cuRootList = cuList.Where(r => r.PARENT_ID == 0);
            if (cuRootList != null && cuRootList.Count() > 0)
            {
                foreach (var p in cuRootList)
                {
                    JsonTreeNode node = new JsonTreeNode();
                    node.hasChildren = (cuList.Count(r => r.PARENT_ID == p.RIGHT_ID) > 0);
                    node.id = p.RIGHT_ID.ToString();
                    node.text = p.RIGHT_NAME;
                    node.value = p.RIGHT_ID.ToString();
                    node.showcheck = true;//(p.ROLE_RIGHT_ID > 0)
                    node.checkstate = ((byte)GetCheckStates(cuList, p));
                    node.complete = true;

                    var childList = cuList.Where(c => c.PARENT_ID == p.RIGHT_ID);
                    if (childList != null && childList.Count() > 0)
                    {
                        node.isexpand = false;
                        SetTreeChildree(childList.ToList(), cuList, ref node, ref treelist);
                    }
                    treelist.Add(node);
                }
            }

            return Json(treelist);

        }

        void SetTreeChildree(List<SYS_RIGHT_EX> cuList, List<SYS_RIGHT_EX> allList, ref JsonTreeNode cuJsonTreeNode, ref List<JsonTreeNode> treelist)
        {
            foreach (var p in cuList)
            {
                JsonTreeNode node = new JsonTreeNode();
                node.hasChildren = (allList.Count(r => r.PARENT_ID == p.RIGHT_ID) > 0);
                node.id = p.RIGHT_ID.ToString();
                node.text = p.RIGHT_NAME;
                node.value = p.RIGHT_ID.ToString();
                node.showcheck = true;
                node.checkstate = ((byte)GetCheckStates(cuList, p));
                node.complete = true;
                cuJsonTreeNode.ChildNodes.Add(node);
                var childList = allList.Where(c => c.PARENT_ID == p.RIGHT_ID);
                if (childList != null && childList.Count() > 0)
                {
                    //node.isexpand = false;
                    SetTreeChildree(childList.ToList(), allList, ref node, ref treelist);
                }
            }
        }

        int GetCheckStates(List<SYS_RIGHT_EX> cuList, SYS_RIGHT_EX cuObj)
        {
            var cList = cuList.Count(p => p.PARENT_ID == cuObj.RIGHT_ID);
            if (cList == 0)
            {
                if (cuObj.ROLE_RIGHT_ID > 0)
                {
                    return 1;
                }
                return 0;
            }
            else
            {
                var checkList = cuList.Count(p => p.PARENT_ID == cuObj.RIGHT_ID && p.ROLE_RIGHT_ID > 0);
                return checkList == 0 ? 0 : (cList == checkList ? 1 : 2);
            }
        }
        #endregion

        //保存
        [HttpPost]
        public JsonResult Save(SYS_ROLE_EX role, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                var menuIds = form["AddIDS"];
                role.MENU_IDS = menuIds;

                string errmsg = "";
                if (!ValidateCUS(role, out errmsg))
                {
                    rMsg.Status = -1;
                    rMsg.Message = errmsg;
                    return Json(rMsg);
                }

                if (role.ROLE_ID == 0)
                {
                    role.CREATE_USER = base.CurrentUserNO;
                }

                role.LAST_MODI_USER = base.CurrentUserNO;

                int ret = _cservice.SaveRole(role);
                if (ret > 0)
                {
                    rMsg.Status = 0;
                    rMsg.Data = ret;
                }
                else if (ret == -2)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "角色名重复";
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

        //删除
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                if (id <= 0)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "参数错误！";
                    return Json(rMsg);
                }

                int ret = _cservice.DeleteRole(id);
                if (ret > 0)
                {
                    rMsg.Status = 0;
                    rMsg.Data = ret;
                }
                else if (ret == -2)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "有关联用户，不能删除。";
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

        //验证
        private static bool ValidateCUS(SYS_ROLE role, out string errmsg)
        {
            errmsg = "";
            if (role == null)
            {
                errmsg = "参数错误！";
                return false;
            }
            if (string.IsNullOrEmpty(role.ROLE_NAME))
            {
                errmsg = "角色名称不能为空！";
                return false;
            }
            return true;
        }
    }
}
