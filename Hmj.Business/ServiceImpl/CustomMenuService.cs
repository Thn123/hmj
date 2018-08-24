using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using System;
using System.Collections.Generic;

namespace Hmj.Business.ServiceImpl
{
    public class CustomMenuService : ICustomMenuService
    {
        private CustomMenuRepository _cmr;
        public CustomMenuService()
        {
            _cmr = new CustomMenuRepository();
        }

        public WXCustomMenu AddCustomMenu(WXCustomMenu cm)
        {
            WXCustomMenu parent = null;
            if (cm.ParentID.HasValue)
                parent = _cmr.GetCustomMenu(cm.ParentID.Value);
            if (parent != null)
                cm.Depth = parent.Depth + 1;
            else
                cm.Depth = 0;
            cm.CeateTime = cm.LastUpdateTime = DateTime.Now;
            var id = _cmr.Insert(cm);
            if (id > 0)
            {
                return this.GetCustomMenu((int)id);
            }
            return null;
        }

        private WXCustomMenu AddRootMenu(int merchants_ID)
        {
            var cm = new WXCustomMenu();
            cm.Merchants_ID = merchants_ID;
            cm.Name = "菜单管理";
            return this.AddCustomMenu(cm);
        }

        public bool DeleteMenu(int id)
        {
            if (_cmr.GetChildrenCount(id) > 0)  //
            {
                var a = _cmr.DeleteMenuByParentID(id);
                if (a < 0)
                {
                    return false;
                }
            }
            var b = _cmr.DeleteMenu(id);
            if (b > 0)
            {
                return true;
            }
            return false;
        }

        public int SaveMenu(ref WXCustomMenu cm)
        {
            cm.LastUpdateTime = DateTime.Now;
            var id = _cmr.Update(cm);
            if (id > 0)
                cm = this.GetCustomMenu(cm.ID);
            else
                cm = null;
            return id;
        }

        //public int SaveMenu(CustomMenu cm)
        //{
        //    cm.LastUpdateTime = DateTime.Now;
        //    return _cmr.Update(cm);
        //}

        public WXCustomMenu GetCustomMenu(int id)
        {
            return _cmr.GetCustomMenu(id);
        }

        public WXCustomMenu GetRootMenu(int merchants_ID)
        {
            var rm = _cmr.GetRootMenu(merchants_ID);
            if (rm != null)
                return rm;
            else
            {
                var newrum = this.AddRootMenu(merchants_ID);
                if (newrum != null)
                    return newrum;
            }
            return null;
        }

        public List<WXCustomMenu_EX> GetCustomMenuList(int merchants_ID)
        {
            return _cmr.GetCustomMenuList(merchants_ID);
        }

        //private CustomMenu AddRootMenu(int merchants_ID)
        //{
        //    return this.AddCustomMenu("菜单管理", null, null, null, null, null, merchants_ID, null);
        //}

        //public CustomMenu AddCustomMenu(string name, int? type, int? orderNum, string content, int? graphic_ID, string url, int merchants_ID, int parentID)
        //{
        //    return this.AddCustomMenu(name, type, orderNum, content, graphic_ID, url, merchants_ID, _cmr.GetCustomMenu(parentID));
        //}

        //private CustomMenu AddCustomMenu(string name, int? type, int? orderNum, string content, int? graphic_ID, string url, int merchants_ID, CustomMenu parentCustomMenu)
        //{
        //    var cm = new CustomMenu();
        //    cm.Name = name;
        //    cm.OrderNum = orderNum;
        //    if (parentCustomMenu != null)
        //    {
        //        cm.ParentID = parentCustomMenu.ID;
        //        cm.Depth = parentCustomMenu.Depth + 1;

        //        if (type.HasValue)
        //            cm.Type = type.Value;
        //        else
        //            cm.Type = 0; //默认文本

        //        if (cm.Type == 0)
        //        {
        //            if (!string.IsNullOrEmpty(content))
        //                cm.Content = content;
        //            cm.Graphic_ID = null;
        //            cm.Url = null;
        //        }
        //        else if (cm.Type == 1) //如果是图文
        //        {
        //            cm.Graphic_ID = graphic_ID;
        //            cm.Content = cm.Url = null;
        //        }
        //        else if (cm.Type == 3)  //外链
        //        {
        //            if (!string.IsNullOrEmpty(url))
        //                cm.Url = url;
        //            cm.Content = null;
        //            cm.Graphic_ID = null;
        //        }
        //        else if (cm.Type == 7)  //多客服
        //        {
        //            cm.Url = null;
        //            cm.Content = null;
        //            cm.Graphic_ID = null;
        //        }
        //    }
        //    else
        //    {
        //        cm.Depth = 0;  //一级菜单
        //    }

        //    cm.Merchants_ID = merchants_ID;
        //    var id = _cmr.Insert(cm);
        //    if (id > 0)
        //    {
        //        return this.GetCustomMenu((int)id);
        //    }
        //    return null;
        //}

        //public CustomMenu SaveMenu(CustomMenu cm, int merchants_ID)
        //{
        //    cm.LastUpdateTime = DateTime.Now;
        //    if (cm.Depth == 0)
        //        cm.Type = null;

        //    if (cm.Type == 0)
        //    {
        //        cm.Graphic_ID = null;
        //        cm.Url = null;
        //    }
        //    else if (cm.Type == 1) //如果是图文
        //    {
        //        cm.Content = null;
        //        cm.Url = null;
        //    }
        //    else if (cm.Type == 3)  //外链
        //    {
        //        cm.Content = null;
        //        cm.Graphic_ID = null;
        //    }
        //    else if (cm.Type == 7)  //多客服
        //    {
        //        cm.Content = null;
        //        cm.Graphic_ID = null;
        //        cm.Url = null;

        //        if (_cmr.ExistType7Menu(merchants_ID) != null)
        //        {
        //            return null;
        //        }
        //    }

        //    if (_cmr.Update(cm) > 0)
        //    {
        //        return cm;
        //    }
        //    return null;
        //}
    }
}
