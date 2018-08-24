using Hmj.Entity.Entities;

namespace Hmj.Extension
{
    /// <summary>
    /// Controller 基类，包装一些同样的方法和属性
    /// </summary>
    [Hmj.Extension.MyAuthorizeAttribute]
    public class MyControllerBase : BaseController
    {   

        /// <summary>
        /// 获取当前登录用户标识
        /// </summary>
        /// <value>The user ID.</value>
        protected virtual string UserId
        {
            get
            {
                return MyContext.Identity;
            }
        }

        ///// <summary>
        ///// 当前登录用户信息
        ///// </summary>
        ///// <value>The current user.</value>
        //protected virtual IdentityUser CurrentUser
        //{
        //    get
        //    {
        //        return MyContext.CurrentUser;
        //    }
        //}

        /// <summary>
        /// 判断用户是否拥有某个权限
        /// </summary>
        /// <param name="rightCode">权限标识</param>
        /// <returns>
        /// 	<c>true</c> 是否拥有指定权限<c>false</c>.
        /// </returns>
        //protected virtual bool HasRight(string rightCode)
        //{
        //    return MyContext.HasRight(MyContext.Identity, rightCode);
        //}

        /// <summary>
        /// 判断用户是否属于某个角色
        /// </summary>
        /// <param name="roleCode">角色标识</param>
        /// <returns>
        /// 	<c>true</c> 是否属于某个角色 <c>false</c>.
        /// </returns>
        ////protected virtual bool IsInRole(string roleCode)
        ////{
        ////    return MyContext.IsInRole(roleCode);
        ////}


        ///// <summary>
        ///// 获取当前登录用户ID
        ///// </summary>
        ///// <value>The LoginUserId ID.</value>
        //protected virtual int LoginUserId
        //{
        //    get
        //    {
        //        return CurrentLoginUser == null ? 0 : CurrentLoginUser.ID;
        //    }
        //}

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        /// <value>The current user.</value>
        protected virtual USER_INFO_EX CurrentLoginUser
        {
            get
            {
                return MyContext.CurrentLoginUser;
            }
        }

        /// <summary>
        /// 获取当前登录用户ID
        /// </summary>
        /// <value>The current user.</value>
        protected virtual int CurrentLoginUserId
        {
            get
            {
                return CurrentLoginUser == null ? 0 : MyContext.CurrentLoginUser.ID;
            }
        }

        /// <summary>
        /// 当前登录账户所属公司: -1没有登录用户
        /// </summary>
        /// <value>The current user.</value>
        protected virtual int CurrentUserOrgId
        {
            get
            {
                //return CurrentLoginUser == null || !CurrentLoginUser.ORG_ID.HasValue ? -1 : MyContext.CurrentLoginUser.ORG_ID.Value;
                return CurrentLoginUser == null || CurrentLoginUser.ORG_ID == 0 ? -1 : MyContext.CurrentLoginUser.ORG_ID;
            }
        }

        /// <summary>
        /// 当前登录账户所属公司名称
        /// </summary>
        /// <value>The current user.</value>
        protected virtual string CurrentUserOrgName
        {
            get
            {
                return CurrentLoginUser == null ? "" : CurrentLoginUser.ORG_NAME;
            }
        }
        /// <summary>
        /// 当前登录用户登录名
        /// </summary>
        /// <value>The current user.</value>
        protected virtual string CurrentUserNO
        {
            get
            {
                return CurrentLoginUser == null ? "" : CurrentLoginUser.USER_NO;
            }
        }
        /// <summary>
        /// 当前登录用户关联员工名称
        /// </summary>
        /// <value>The current user.</value>
        protected virtual string CurrentUserEmployeeName
        {
            get
            {
                return CurrentLoginUser == null ? "" : CurrentLoginUser.EMPLOYEE_NAME;
            }
        }

        /// <summary>
        /// 当前登录用户关联员工编号
        /// </summary>
        /// <value>The current user.</value>
        protected virtual int CurrentUserEmployeeId
        {
            get
            {
                return CurrentLoginUser == null ? 0 : CurrentLoginUser.EMPLOYEE_ID;
            }
        }

       

        /// <summary>
        /// 当前登录账户所属门店: -1没有登录用户(区域，集团，门店)
        /// </summary>
        /// <value>The current user.</value>
        protected virtual int CurrentUserStoreId
        {
            get
            {
                //return CurrentLoginUser == null || !CurrentLoginUser.STORE_ID.HasValue ? -1 : MyContext.CurrentLoginUser.STORE_ID;
                return CurrentLoginUser == null || CurrentLoginUser.STORE_ID == 0 ? -1 : MyContext.CurrentLoginUser.STORE_ID;
            }
            set
            {
                if (CurrentLoginUser != null)
                    CurrentLoginUser.STORE_ID = value;
            }
        }

        /// <summary>
        /// 当前登录账户数据权限(用,隔开的id)
        /// </summary>
        protected virtual string CurrentUserDataStores
        {
            get
            {
                return CurrentLoginUser == null ? "" : MyContext.CurrentLoginUser.USER_STORES_OBJ;
            }
        }
        /// <summary>
        /// 当前用户是否admin:true是公司admin角色，false是门店角色
        /// </summary>
        /// <value>The current user.</value>
        protected virtual bool CurrentIsOrgAdmin
        {
            get
            {
                return CurrentLoginUser == null || !CurrentLoginUser.ROLE_TYPE.HasValue ? false : (CurrentLoginUser.ROLE_TYPE.Value == 0 ? true : false);
            }
        }

        /// <summary>
        /// 当前登录账户类型（0集团用户，1区域用户，2门店用户）
        /// </summary>
        /// <value>The current user.</value>
        protected virtual string CurrentUserType
        {
            get
            {
                return CurrentLoginUser == null ? "" : CurrentLoginUser.USER_TYPE;
            }
        }

        /// <summary>
        /// 当前登录账户所属门店名称
        /// </summary>
        /// <value>The current user.</value>
        protected virtual string CurrentUserStoreName
        {
            get
            {
                return CurrentLoginUser == null ? "" : CurrentLoginUser.STORE_NAME;
            }
        }
        //当前登录公司卡级类型 单卡 多卡
        protected virtual int CurrentCardMode
        {
            get
            {
                return CurrentLoginUser == null ? 1 : CurrentLoginUser.CARD_MODE;
            }
        }

        //金额 四舍五入类型
        protected virtual int CurrentPointMode
        {
            get
            {
                return CurrentLoginUser == null ? 0 : CurrentLoginUser.POINT_MODE;
            }
        }

    }


}
