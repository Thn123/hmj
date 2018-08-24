using Newtonsoft.Json;
using Hmj.Business.ServiceImpl;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Hmj.Entity.Entities;
using WeChatCRM.Common.Utils;
using Hmj.Entity;
using Hmj.Common;

namespace Hmj.WebApp.WechatApiHelper
{
    public class CustomMenuApi : BasePage //: WechatApi
    {
        private ICustomMenuService _cms;
        private ISystemService _system;

        public CustomMenuApi()
        {
            _system = new SystemService();
            _cms = new CustomMenuService();
        }

        public Result CreateMenu()
        {
            var str = string.Empty;
            if (GetCreateJson(ref str))
            {
                var accessToken = base.Token();
                var postUrl = RequestUrlHelper.CustomMenu.Create(accessToken);


                LogService log = new LogService();
                log.Debug(str);


                string str2 = str;
                string str3 = str;

                string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + accessToken;
                var resMessage = HttpXmlPostRequest(url, str, Encoding.UTF8);


                str2 = str2.Substring(0, str2.LastIndexOf("}"));
                //log.Debug("我是删除后" + str2);
                str2 += @",""matchrule"":{""tag_id"": ""101""}}";
                string url2 = "https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token=" + accessToken;
                var resMessage2 = HttpXmlPostRequest(url2, str2, Encoding.UTF8);
                log.Debug(resMessage2);
                log.Debug("我是菜单2" + str2);


                str3 = str3.Replace("http://hmjwechattest.jahwa.com.cn/assets/hmjweixin/html/hytq.html", "http://hmjwechattest.jahwa.com.cn/assets/hmjweixin/html/hytqwithnoreg.html");
                str3 = str3.Substring(0, str3.LastIndexOf("}"));
                //log.Debug("我是删除后" + str3);
                str3 += @",""matchrule"":{""tag_id"": ""100""}}";
                string url3 = "https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token=" + accessToken;
                var resMessage3 = HttpXmlPostRequest(url3, str3, Encoding.UTF8);
                log.Debug(resMessage3);
                log.Debug("我是菜单3" + str3);


                //return JsonConvert.DeserializeObject<Result>(base.HttpXmlPostRequest(postUrl,
                //    str, Encoding.UTF8));

                var result = new Result() { errcode = 0, errmsg = str };
                return result;
            }
            else
            {
                var result = new Result() { errcode = -1, errmsg = str };
                return result;
            }


            //return null;
        }

        public bool GetCreateJson(ref string str)
        {
            var 所有菜单 = _cms.GetCustomMenuList(CurrentMerchants.ID);
            var 微信零级菜单 = new Wechat_Level0Menu();
            var 微信一级菜单列表 = new List<Wechat_Level1Menu>();
            //var i = 1;
            //var path = "D:/menulog.txt";
            //File.AppendAllText(path, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " ");

            foreach (var 一级菜单 in 所有菜单.Where(cm => cm.Depth == 1).OrderBy(cm => cm.OrderNum))
            {
                var 二级菜单集合 = 所有菜单.Where(cm => cm.ParentID == 一级菜单.ID).OrderBy(cm => cm.OrderNum);
                if (二级菜单集合.Count() > 0)
                {
                    var 微信一级菜单 = new Wechat_Level1Menu_hasChild();
                    微信一级菜单.name = 一级菜单.Name;
                    var 微信二级菜单集合 = new List<Wechat_Level2Menu>();

                    foreach (var 二级菜单 in 二级菜单集合)
                    {
                        if (!二级菜单.Type.HasValue)
                        {
                            str = string.Format("{0}-{1}没有配置响应动作。", 一级菜单.Name, 二级菜单.Name);
                            return false;//
                        }
                        else if (二级菜单.Type == 4 && !string.IsNullOrEmpty(二级菜单.Content))
                        {
                            var wechat_Level2Menu = new Wechat_Level2Menu_miniprogram();
                            string[] arrTemp = 二级菜单.Content.Split(',');
                            wechat_Level2Menu.name = 二级菜单.Name;
                            wechat_Level2Menu.appid = arrTemp[0];
                            wechat_Level2Menu.pagepath = arrTemp[1];
                            wechat_Level2Menu.url = arrTemp[2];
                            微信二级菜单集合.Add(wechat_Level2Menu);
                        }
                        else if (二级菜单.Type == 7 && string.IsNullOrEmpty(二级菜单.Media_ID)) //多客服
                        {
                            var wechat_Level2Menu = new Wechat_Level2Menu_click();
                            wechat_Level2Menu.name = 二级菜单.Name;
                            wechat_Level2Menu.key = "dkf";
                            微信二级菜单集合.Add(wechat_Level2Menu);
                        }
                        else if (二级菜单.Type != 3 && string.IsNullOrEmpty(二级菜单.Media_ID)) //不是外链
                        {
                            var wechat_Level2Menu = new Wechat_Level2Menu_click();
                            wechat_Level2Menu.name = 二级菜单.Name;
                            wechat_Level2Menu.key = 二级菜单.ID.ToString();
                            微信二级菜单集合.Add(wechat_Level2Menu);
                        }
                        else if (二级菜单.Type == 1 && !string.IsNullOrEmpty(二级菜单.Media_ID))
                        {
                            var wechat_Level2Menu = new Wechat_Level2Menu_click();
                            wechat_Level2Menu.name = 二级菜单.Name;
                            wechat_Level2Menu.type = "media_id";
                            wechat_Level2Menu.media_id = 二级菜单.Media_ID;
                            微信二级菜单集合.Add(wechat_Level2Menu);
                        }
                        else
                        {
                            var wechat_Level2Menu = new Wechat_Level2Menu_view();
                            wechat_Level2Menu.name = 二级菜单.Name;
                            wechat_Level2Menu.type = "view";
                            wechat_Level2Menu.url = 二级菜单.Url;

                            if (二级菜单.Url.Contains(ConfigurationManager.AppSettings["ServerIP"]))
                            {
                                if (二级菜单.Url.Contains("?"))
                                    二级菜单.Url = 二级菜单.Url + "&ToUserName=" + CurrentMerchants.ToUserName;
                                else
                                    二级菜单.Url = 二级菜单.Url + "?ToUserName=" + CurrentMerchants.ToUserName;

                                wechat_Level2Menu.url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect",
                                    GetAppid(), 二级菜单.Url);
                                ;
                            }
                            微信二级菜单集合.Add(wechat_Level2Menu);

                        }
                        //File.AppendAllText(path, i++.ToString() + ",");
                    }
                    微信一级菜单.sub_button = 微信二级菜单集合.ToArray();
                    微信一级菜单列表.Add(微信一级菜单);
                    //File.AppendAllText(
                }
                else
                {
                    if (!一级菜单.Type.HasValue)
                    {
                        str = string.Format("{0}没有配置响应动作。", 一级菜单.Name);
                        return false;
                    }
                    else if (一级菜单.Type == 4 && string.IsNullOrEmpty(一级菜单.Content)) //小程序
                    {
                        var 微信一级菜单 = new Wechat_Level1Menu_noChild_miniprogram();
                        string[] arrTemp = 一级菜单.Content.Split(',');
                        微信一级菜单.name = 一级菜单.Name;
                        微信一级菜单.type = "miniprogram";
                        微信一级菜单.appid = arrTemp[0];
                        微信一级菜单.pagepath = arrTemp[1];
                        微信一级菜单.url = arrTemp[2];
                        微信一级菜单列表.Add(微信一级菜单);
                    }
                    else if (一级菜单.Type == 1 && !string.IsNullOrEmpty(一级菜单.Media_ID))
                    {
                        var 微信一级菜单 = new Wechat_Level1Menu_noChild_view();
                        微信一级菜单.name = 一级菜单.Name;
                        微信一级菜单.type = "media_id";
                        微信一级菜单.media_id = 一级菜单.Media_ID;
                        微信一级菜单列表.Add(微信一级菜单);
                    }

                    else if (一级菜单.Type == 7 && string.IsNullOrEmpty(一级菜单.Media_ID)) //多客服
                    {
                        var 微信一级菜单 = new Wechat_Level1Menu_noChild_click();
                        微信一级菜单.name = 一级菜单.Name;
                        微信一级菜单.key = "dkf";
                        微信一级菜单列表.Add(微信一级菜单);
                    }
                    else if (一级菜单.Type != 3 && string.IsNullOrEmpty(一级菜单.Media_ID)) //不是外链
                    {
                        var 微信一级菜单 = new Wechat_Level1Menu_noChild_click();
                        微信一级菜单.name = 一级菜单.Name;
                        微信一级菜单.key = 一级菜单.ID.ToString();
                        微信一级菜单列表.Add(微信一级菜单);
                    }
                    else
                    {
                        var 微信一级菜单 = new Wechat_Level1Menu_noChild_view();
                        微信一级菜单.name = 一级菜单.Name;
                        微信一级菜单.type = "view";
                        微信一级菜单.url = 一级菜单.Url;
                        微信一级菜单列表.Add(微信一级菜单);
                    }
                }
            }
            微信零级菜单.button = 微信一级菜单列表.ToArray();
            str = JsonConvert.SerializeObject(微信零级菜单);
            return true;
        }

        #region wechat obj

        public class Wechat_Level0Menu
        {
            public Wechat_Level1Menu[] button;
        }

        public class Wechat_Level1Menu
        {
            public string name;
        }

        public class Wechat_Level1Menu_noChild : Wechat_Level1Menu
        {

        }

        public class Wechat_Level1Menu_noChild_click : Wechat_Level1Menu_noChild
        {
            public string type = "click";
            public string key;
        }
        public class Wechat_Level1Menu_noChild_miniprogram : Wechat_Level1Menu_noChild
        {
            public string type = "miniprogram";
            public string appid;
            public string pagepath;
            public string url;
        }

        public class Wechat_Level1Menu_noChild_view : Wechat_Level1Menu_noChild
        {
            public string type = "view";
            public string url;
            public string media_id;
        }

        public class Wechat_Level1Menu_hasChild : Wechat_Level1Menu
        {
            public Wechat_Level2Menu[] sub_button;
        }

        public class Wechat_Level2Menu
        {
            public string name;
        }

        public class Wechat_Level2Menu_click : Wechat_Level2Menu
        {
            public string type = "click";
            public string key;
            public string media_id;
        }

        public class Wechat_Level2Menu_miniprogram : Wechat_Level2Menu
        {
            public string type = "miniprogram";
            public string appid;
            public string pagepath;
            public string url;
        }

        public class Wechat_Level2Menu_view : Wechat_Level2Menu
        {
            public string type = "view";
            public string url;
        }

        public class Result
        {
            public int errcode;
            public string errmsg;
        }

        /// <summary>
        /// 加载微信素材
        /// </summary>
        /// <returns></returns>
        internal string DownLoad()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + base.Token();

            bool isok = true;

            FileRes Totle = new FileRes() { Item = new List<FileItemRes>() };

            int conut = 0;

            while (isok)
            {
                FilesReq req = new FilesReq();
                req.Count = 20;
                req.Offset = conut;
                req.Type = "news";

                string datas = JsonConvert.SerializeObject(req);

                string res = NetHelper.HttpRequest(url, datas, "POST", 6000, Encoding.UTF8, "application/json");

                //得到素材
                FileRes ones = JsonConvert.DeserializeObject<FileRes>(res);

                if (ones.Total_Count > conut)
                {
                    //添加到数据中
                    Totle.Item.AddRange(ones.Item);
                    conut += ones.Item_Count;

                    if (conut == ones.Total_Count)
                    {
                        isok = false;
                    }
                }
                else
                {
                    Totle.Total_Count = ones.Total_Count;
                    Totle.Item_Count = conut;
                    isok = false;
                }
            }

            foreach (FileItemRes item in Totle.Item)
            {
                bool bo = _system.GetAllGraphicOne(item.Media_Id);

                if (!bo)
                {
                    WXGraphicList list = new Hmj.Entity.WXGraphicList();
                    list.CreateDate = DateTime.Now;
                    list.Merchants_ID = CurrentLoginUser.ORG_ID;
                    list.Title = "";// Request["Name"];
                    list.Media_ID = item.Media_Id;
                    int lid = 0;
                    lid = _system.SaveGraphicList(list);

                    foreach (FileItemNewsRes filse in item.Content.News_Item)
                    {
                        WXGraphicDetail CuObj = new Hmj.Entity.WXGraphicDetail();
                        // CuObj.Body = Request["ckeditor"];Media_ID
                        CuObj.Describe = "微信同步";
                        CuObj.URL = filse.Url;
                        CuObj.IsURL = true;
                        CuObj.List_ID = lid;
                        CuObj.Sorting = 0;// int.Parse(Request["Sorting"]);
                        CuObj.Title = filse.Title;
                        _system.SaveGraphicDetail(CuObj);
                    }
                }
            }


            return "";
        }

        #endregion
    }
}