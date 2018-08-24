using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.Extension;
using Hmj.Interface;
using System.Collections.Generic;
using System.Linq;

namespace WeChat.WebApp.Controllers
{
    public  class SystemMenuController : WXMyControllerBase
    {
        //
        // GET: /SystemMenu/
        private static ISystemService _sservice;
        
       
        //public  SystemMenuController(ISystemService sservice)
        //{
        //    _sservice = sservice;
        //}

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public static string GetMenu()
        {
            if(_sservice==null)
            _sservice = new SystemService();
            string menu = "";
            if (MyContext.CurrentLoginUser != null)
            {
                int user = MyContext.CurrentLoginUser.ID;
                List<SYS_RIGHT> list = _sservice.GetAllRight(user);
                var menu1 = list.Where(c => c.PARENT_ID == 0);
                foreach (var s in menu1)
                {
                    if ((list.Count(r => r.PARENT_ID == s.RIGHT_ID) > 0))    //有子节点
                    {
                        menu += string.Format("<li><a href=\"javascript:;\"><i class=\"fa fa-{0}\"></i><span>{1}</span></a><ul class=\"acc-menu\">", s.MENU_ICON, s.RIGHT_NAME);
                        menu += GetParentMenu(list, s.RIGHT_ID);
                        menu += "</ul></li>";
                    }
                    else  //无子节点
                    {
                        menu += string.Format("<li><a href=\"{0}\"><i class=\"fa fa-{1}\"></i><span>{2}</span></a></li>", s.URL_LINK_TO == null ? "#" : s.URL_LINK_TO, s.MENU_ICON, s.RIGHT_NAME);
                    }
                }
            }
            return  @menu;
        }

        /// <summary>
        /// 根据父节点获取菜单
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string GetParentMenu(List<SYS_RIGHT> list, int? pid)
        {
            string menu = "";
            var menu1 = list.Where(c => c.PARENT_ID == pid);
            foreach (var s in menu1)
            {
                if ((list.Count(r => r.PARENT_ID == s.RIGHT_ID) > 0))    //有子节点
                {
                    menu += string.Format("<li><a href=\"javascript:;\">{0}</a><ul class=\"acc-menu\">", s.RIGHT_NAME);
                    menu += GetParentMenu(list, s.RIGHT_ID);
                    menu += "</ul></li>";
                }
                else  //无子节点
                {
                    menu += string.Format("<li><a href=\"{0}\">{1}</a></li>", s.URL_LINK_TO==null?"#":s.URL_LINK_TO, s.RIGHT_NAME);
                }
            }

            return menu;
        }

    }
}
