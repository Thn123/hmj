using Hmj.Common;
using Hmj.Common.Exceptions;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using System;
using System.Web;
using System.Web.Security;

namespace Hmj.Extension
{
    public class WXMyContext
    {

        /// <summary>
        /// 当前登录账户的标识
        /// </summary>
        /// <value>The identity.</value>
        public static string Identity
        {
            get
            {

                string uid = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(uid))
                {
                    if (AppConfig.IsDebugMode)
                    {
                        return AppConfig.Get("DebuggerUserId");
                    }
                    else
                    {
                        throw new NoAuthorizeExecption();
                    }
                }
                return uid;
            }
        }
        public static bool IsIsAuthenticated
        {
            get
            {
                if (AppConfig.IsDebugMode)
                {
                    string userid = AppConfig.Get("DebuggerUserId");
                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        FormsAuthentication.SetAuthCookie(userid, false);
                    }
                    return true;
                }
                else
                {
                    return HttpContext.Current.User.Identity.IsAuthenticated;
                }
            }
        }
        ///// <summary>
        ///// 当前登录账户的完整信息
        ///// </summary>
        ///// <value>The current user.</value>
        //public static IdentityUser CurrentUser
        //{
        //    get
        //    {

        //        if (HttpContext.Current.Session["UseInfo"] != null)
        //        {
        //            return ObjectToStrongType<IdentityUser>(HttpContext.Current.Session["UseInfo"]);
        //        }
        //        else
        //        {
        //            IUser u = SystemService.GetUserInfo(Identity);
        //            IdentityUser user = u as IdentityUser;


        //            if (user == null)
        //            {
        //                throw new NoAuthorizeExecption();
        //            }
        //            var level_cookie = HttpContext.Current.Request.Cookies["USER_LEVEL"];
        //            if (level_cookie != null)
        //            {
        //                user.UserLevel = int.Parse(level_cookie.Value);
        //            }
        //            else
        //            {
        //                user.UserLevel = 2;
        //            }
        //            var branch_cookie = HttpContext.Current.Request.Cookies["SUB_BRANCHID"];
        //            if (branch_cookie != null)
        //            {
        //                user.StoreId = int.Parse(branch_cookie.Value);
        //            }
        //            //user.SubBranchId = 101;
        //            HttpContext.Current.Session["UseInfo"] = user;
        //            return user;
        //        }
        //    }
        //}
        //   if (user == null)
        //            {
        //                throw new NoAuthorizeExecption();
        //            }                   
        //            HttpContext.Current.Session["UseInfo"] = user;
        //            return user;
        //        }
        //    }
        //}

        /// <summary>
        /// 当前登录账户的完整信息
        /// </summary>
        /// <value>The current user.</value>
        public static USER_INFO_EX CurrentLoginUser
        {
            get
            {
                if (HttpContext.Current.Session["LoginUseInfo"] != null)
                {
                    return ObjectToStrongType<USER_INFO_EX>(HttpContext.Current.Session["LoginUseInfo"]);
                }
                else
                {
                    if (IsIsAuthenticated && !string.IsNullOrEmpty(Identity))
                    {
                        var cuo = CLoginService.WXGetUserById(Int32.Parse(Identity));
                        if (cuo != null)
                        {
                            return cuo;
                        }
                    }
                    return null;

                }
            }
        }

        public static void SaveMerchants(ORG_INFO m)
        {
            CLoginService.UpdateMerchants(m);
            HttpContext.Current.Session["CurrentMerchants"] = m;
        }


        /// <summary>
        /// 获取用户的IP
        /// </summary>
        /// <value>The user IP.</value>
        public static string UserIP
        {
            get
            {
                string usip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (String.IsNullOrEmpty(usip))
                    usip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                return usip;
            }
        }
        ///// <summary>
        ///// 判断当前用户是否在某个角色内
        ///// </summary>
        ///// <param name="roleCode">角色代码</param>
        ///// <returns>
        ///// 	<c>true</c> if [is in role] [the specified role code]; otherwise, <c>false</c>.
        ///// </returns>
        //public static bool IsInRole(string roleCode)
        //{
        //    if (AppConfig.IsDebugMode)
        //    {
        //        return true;
        //    }
        //    if (roleCode == Common.Constant.CCUSER_ROLECODE)
        //    {
        //        return CurrentUser.UserLevel != 2;
        //    }
        //    return false;
        //}
        /// <summary>
        /// 判断是否有权限
        /// </summary>
        /// <param name="rightCode">权限代码</param>
        /// <returns>
        /// 	<c>true</c> if the specified right code has right; otherwise, <c>false</c>.
        /// </returns>
        //public static bool HasRight(string userId, string rightCode)
        //{
        //    return SystemService.CheckUserPrivilege(userId, rightCode);
        //}

        //private static ISystemService _ISystemService;
        //private static ISystemService SystemService
        //{
        //    get
        //    {
        //        if (_ISystemService == null)
        //        {
        //            _ISystemService = ServiceFactory.GetInstance<ISystemService>();
        //        }
        //        return _ISystemService;
        //    }
        //}
        private static ILoginService _ILoginService;
        private static ILoginService CLoginService
        {
            get
            {
                if (_ILoginService == null)
                {
                    _ILoginService = ServiceFactory.GetInstance<ILoginService>();
                }
                return _ILoginService;
            }
        }

        /// <summary>
        /// 将object类型转换成指定类型，吞掉转换异常等情况
        /// </summary>
        /// <typeparam name="T">需要转换成的类型</typeparam>
        /// <param name="o">需要转换的对象</param>
        /// <returns></returns>
        private static T ObjectToStrongType<T>(object o) where T : class, new()
        {
            T t = null;
            if (o != null)
            {
                t = o as T;
            }
            return t;
        }

        /// <summary>
        /// 当前登录账户所属商户的完整信息
        /// </summary>
        /// <value>The current user.</value>
        public static ORG_INFO CurrentMerchants
        {
            get
            {
                if (HttpContext.Current.Session["CurrentMerchants"] != null)
                {
                    return ObjectToStrongType<ORG_INFO>(HttpContext.Current.Session["CurrentMerchants"]);
                }
                else
                {
                    //throw new Exception("登录超时"+Identity);
                    if (CurrentLoginUser != null)
                    {
                        var cuo = CLoginService.GetMerchants(Convert.ToInt32(CurrentLoginUser.ORG_ID));
                        if (cuo != null)
                        {
                            HttpContext.Current.Session["CurrentMerchants"] = cuo;
                            return cuo;
                        }
                    }
                    return null;

                }
            }
        }
    }
}
