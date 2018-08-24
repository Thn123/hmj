using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Extension;
using Hmj.Interface;
using Hmj.WebApp.WechatApiHelper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace WeChat.WebApp.Controllers
{
    public class CustomMenuController : WXMyControllerBase
    {
        private ICustomMenuService _cms;
        private ISystemService _ss;
        public CustomMenuController(ICustomMenuService cms, ISystemService ss)
        {
            _cms = cms;
            _ss = ss;
        }

        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                var root = _cms.GetRootMenu(CurrentUserMerchantsID.Value);
                ViewBag.RootID = root.ID;
                return View(root);
            }
            else
                return View(new WXCustomMenu());
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult QueryCustomMenuList()
        {
            var menuList = _cms.GetCustomMenuList(CurrentUserMerchantsID.Value);
            return Json(menuList);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddChild(WXCustomMenu cm)
        {
            if (cm.ParentID.HasValue)
            {
                //JsonSMsg rMsg = new JsonSMsg();
                //rMsg.Status = -1;
                //rMsg.Message = "参数错误！";
                // var ccm = _cms.AddCustomMenu(cm.Name, cm.Type, cm.OrderNum, cm.Content, cm.Graphic_ID, cm.Url, CurrentUserMerchantsID.Value, cm.ParentID.Value);
                cm.Merchants_ID = CurrentMerchants.ID;
                var ccm = _cms.AddCustomMenu(cm);
                return Json(ccm);
            }
            return Json(null);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ShowMenuInfo(int id)
        {
            var cm = _cms.GetCustomMenu(id);
            JsonSMsg rMsg = new JsonSMsg();
            if (cm != null)
            {
                rMsg.Status = 0;
                rMsg.Message = "success";
                rMsg.Data = cm;
            }
            else
            {
                rMsg.Status = -1;
                rMsg.Message = "操作失败，未找到自定义菜单对象。";
            }
            return Json(rMsg);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ChangeMenuName(WXCustomMenu cm)
        {
            JsonSMsg rMsg = new JsonSMsg();
            if (!string.IsNullOrEmpty(cm.Name))
            {
                var b = _cms.SaveMenu(ref cm);
                if (b > 0)
                {

                    rMsg.Status = 0;
                    rMsg.Message = "success";
                    rMsg.Data = cm;
                }
                else
                {
                    rMsg.Status = 0;
                    rMsg.Message = "fail";
                }
            }
            return Json(rMsg);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveMenu(int id, int type, string typeArg,string OrderNum)
        {
            JsonSMsg rMsg = new JsonSMsg();
            var menu = _cms.GetCustomMenu(id);
            if (menu != null)
            {
                menu.Type = type;
                if (type == 3)
                    menu.Url = typeArg;
                else if (type == 4)
                    menu.Content = typeArg;
                else if (type == 0)
                    menu.Content = typeArg;
                else if (type == 1)
                    menu.Graphic_ID = int.Parse(typeArg);
                try
                {
                    menu.OrderNum = int.Parse(OrderNum);
                }
                catch { }

                var b = _cms.SaveMenu(ref menu);
                if (b > 0)
                {
                    rMsg.Status = 0;
                    rMsg.Message = "success";
                    rMsg.Data = menu;
                }
                else
                {
                    rMsg.Status = -1;
                    rMsg.Message = "保存失败";
                }
            }
            else
            {
                rMsg.Status = -1;
                rMsg.Message = "未找到对象";
            }
            return Json(rMsg);
        }

        //[HttpPost]
        //public JsonResult SaveMenu(CustomMenu cm)
        //{
        //    var b = _cms.SaveMenu(cm, CurrentUserMerchantsID.Value);
        //    JsonSMsg rMsg = new JsonSMsg();
        //    if (b != null)
        //    {
        //        rMsg.Status = 0;
        //        rMsg.Message = "success";
        //        rMsg.Data = b;
        //    }
        //    else
        //    {
        //        if (cm.Type == 7)
        //        {
        //            rMsg.Status = -1;
        //            rMsg.Message = "每个商户至多有一个多客服菜单，请检查。";
        //        }
        //        else
        //        {
        //            rMsg.Status = -1;
        //            rMsg.Message = "操作失败，未找到自定义菜单对象。";
        //        }
        //    }
        //    return Json(rMsg);
        //}

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteMenu(int id)
        {
            JsonSMsg rMsg = new JsonSMsg();
            var menu = _cms.GetCustomMenu(id);
            if (menu != null)
            {
                if (_cms.DeleteMenu(id))
                {
                    rMsg.Status = 0;
                    rMsg.Message = "success";
                }
                else
                {
                    rMsg.Status = -1;
                    rMsg.Message = "error";
                }
            }
            else
            {
                rMsg.Status = -1;
                rMsg.Message = "操作失败，未找到自定义菜单对象。";
            }
            return Json(rMsg);
        }

        public JsonResult PublishToWechat()
        {
            JsonSMsg rMsg = new JsonSMsg();
            CustomMenuApi cma = new CustomMenuApi();
            var result = cma.CreateMenu();
            rMsg.Status = result.errcode;
            rMsg.Message = result.errmsg;
            return Json(rMsg);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveMenuOrder(string menuJSON)
        {
            JsonSMsg rMsg = new JsonSMsg();
            var result = JsonConvert.DeserializeObject<SimpleMenuOne>(menuJSON);
            try
            {
                foreach (var one in result.menus)
                {
                    var i = 0;
                    foreach (var two in one.children)
                    {
                        var menu = _cms.GetCustomMenu(two.id);
                        menu.OrderNum = i;
                        if (_cms.SaveMenu(ref menu) > 0)
                        {
                            i++;
                        }
                    }
                }
                rMsg.Status = 0;
                rMsg.Message = "success";
            }
            catch
            {
                rMsg.Status = -1;
                rMsg.Message = "fail";
            }
            return Json(rMsg);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetGraphic(int id)
        {
            JsonSMsg rMsg = new JsonSMsg();
            var lgd = _ss.GetAllGraphicDetail(id);
            var lgdo = new List<GraphicDetailObj>();
            foreach (var gd in lgd)
            {
                var gdo = new GraphicDetailObj();
                gdo.title = gd.Title;
                gdo.pic = ConfigurationManager.AppSettings["WebUrl"] + gd.Pic;
                gdo.createdate = gd.CreateDate.ToString("MM月dd日");
                lgdo.Add(gdo);
            }
            if (lgdo.Count > 0)
            {
                rMsg.Status = 0;
                rMsg.Message = "success";
                rMsg.Data = lgdo.ToArray();
            }
            else
            {
                rMsg.Status = 0;
                rMsg.Message = "fail";
            }
            return Json(rMsg);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetGraphicList()
        {
            List<WXGraphicList> lgl = _ss.GetAllGraphicList(CurrentLoginUser.ORG_ID);
            List<GraphicObj> lgo = new List<GraphicObj>();
            foreach (WXGraphicList g in lgl)
            {
                GraphicObj go = new GraphicObj();
                go.id = g.ID;
                List<Graphic_Detail_EX> lgd = _ss.GetAllGraphicDetail(g.ID);
                var lgdo = new List<GraphicDetailObj>();
                foreach (var gd in lgd)
                {
                    var gdo = new GraphicDetailObj();
                    gdo.title = gd.Title;
                    gdo.pic = ConfigurationManager.AppSettings["WebUrl"] + gd.Pic;
                    gdo.createdate = gd.CreateDate.ToString("MM月dd日");
                    lgdo.Add(gdo);
                }
                go.details = lgdo.ToArray();
                lgo.Add(go);
            }
            JsonSMsg rMsg = new JsonSMsg();
            if (lgo.Count > 0)
            {
                rMsg.Status = 0;
                rMsg.Message = "success";
                rMsg.Data = lgo.ToArray();
            }
            else
            {
                rMsg.Status = 0;
                rMsg.Message = "fail";
            }
            return Json(rMsg);
        }

        #region obj

        public class SimpleMenuOne
        {
            public SimpleMenuTwo[] menus;
        }

        public class SimpleMenuTwo
        {
            public SimpleMenuThree[] children;
        }

        public class SimpleMenuThree
        {
            public int id;
            public string name;
        }

        public class GraphicObj
        {
            public int id { get; set; }
            public GraphicDetailObj[] details { get; set; }
        }

        public class GraphicDetailObj
        {
            public string title { get; set; }
            public string pic { get; set; }
            public string createdate { get; set; }
        }

        #endregion
        //public class ZTreeNode
        //{
        //    public ZTreeNode(CustomMenu cm)
        //    {
        //        id = cm.ID;
        //        if (cm.ParentID.HasValue)
        //            pId = cm.ParentID.Value;
        //        open = (cm.Depth == 2) ? false : true;
        //        name = cm.Name;
        //    }

        //    public int id { get; set; }
        //    public int pId { get; set; }
        //    public bool open { get; set; }
        //    public string name { get; set; }
        //}
    }
}
