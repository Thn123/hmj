using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.DTO;
using Hmj.Entity;
using Hmj.Entity.PageSearch;
using Hmj.Interface;
using log4net;
using Nancy;
using Newtonsoft.Json;
using StructureMap.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using WeChatCRM.Common.Utils;

namespace Hmj.WebApi.Modules
{
    public class TestModule : BaseModule
    {
        [SetterProperty]
        public IHmjMemberService _hmjMember { get; set; }

        public TestModule()
            : base("/Test")
        {
            Get["/GetTest"] = GetTest;
            Post["/PostTest"] = PostTest;
          
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic PostTest(dynamic arg)
        {
            try
            {
                //得到请求参数
                MemberUpdateReqDTO request = base.BindObject<MemberUpdateReqDTO>();
                string id = "83";

                //根据memberid得到会员详情
                MemberResDTO res = _hmjMember.GetMemberModel(id);
                
                MemberSearch one = new MemberSearch();
                
                //得到会员集合
                List<MemberResDTO> list = _hmjMember.GetMemberList(one);

                return ResponseJson(true, "ok", res);
            }
            catch (Exception ex)
            {
                return ResponseJsonError(false, "系统错误：" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic GetTest(dynamic arg)
        {
            try
            {
                //购物车id
                string shop_id = base.GetValue<string>("shop_id");

                return ResponseJson(true, "成功");
            }
            catch (Exception ex)
            {
                return ResponseJsonError(false, "系统错误：" + ex.Message, ex);
            }
        }

        
        
    }
}