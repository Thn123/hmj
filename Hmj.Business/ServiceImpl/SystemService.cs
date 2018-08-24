using Hmj.Common;
using Hmj.Common.CacheManager;
using Hmj.Common.Exceptions;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hmj.Business.ServiceImpl
{
    public class SystemService : ISystemService
    {
        private IUserService _uservice;
        private ICache _sessioncache;
        private SystemSetRepository _set;
        private readonly GroupInfoRepository _groupRepo;
        public SystemService(IUserService uservice)
        {
            _uservice = uservice;
            _sessioncache = CacheFactory.Instance.CreateSessionCacheInstance();
            _groupRepo = new GroupInfoRepository();
            _set = new SystemSetRepository();
        }

        public SystemService()
        {
            _sessioncache = CacheFactory.Instance.CreateSessionCacheInstance();
            _set = new SystemSetRepository();
        }
        #region

        public List<IPrivilege> GetUserMenus(string userid)
        {
            List<IPrivilege> retlist = null;
            object plistobject = _sessioncache.Get("USER_PRIVILEGE");
            if (plistobject != null)
            {
                retlist = plistobject as List<IPrivilege>;

            }
            else
            {
                retlist = _uservice.GetUserPrivilegeList(userid);
                _sessioncache.Insert("USER_PRIVILEGE", retlist);
            }
            if (retlist != null)
            {
                return retlist.FindAll(x => x.PrivilegeType == 0);
            }
            return new List<IPrivilege>();
        }

        public bool CheckUserPrivilege(string userId, string rightCode)
        {
            object hasright = _sessioncache.Get("RIGHT_" + rightCode);
            if (hasright != null)
            {
                return hasright.ToString() == "1";
            }
            else
            {
                List<IPrivilege> retlist = null;
                object plistobject = _sessioncache.Get("USER_PRIVILEGE");
                if (plistobject != null)
                {
                    retlist = plistobject as List<IPrivilege>;

                }
                else
                {
                    retlist = _uservice.GetUserPrivilegeList(userId);
                    _sessioncache.Insert("USER_PRIVILEGE", retlist);
                }
                bool ret = false;
                if (retlist != null)
                {
                    ret = retlist.Exists(x => x.PrivilegeCode == rightCode);
                    _sessioncache.Insert("RIGHT_" + rightCode, ret ? "1" : "0");
                }
                return ret;
            }

        }
        public AreaManage GetAreaManage(int? id)
        {
            return _set.GetAreaManage(id);
        }
        public IUser GetUserInfo(string userId)
        {
            IUser rspuser = _uservice.GetUserInfo(userId);
            if (rspuser != null)
            {
                IdentityUser user = new IdentityUser();
                user.UserId = rspuser.UserId;
                user.FullName = rspuser.FullName;
                user.StoreId = rspuser.StoreId;
                user.EmployID = rspuser.EmployID;
                user.OrgId = rspuser.OrgId;
                user.OrgName = rspuser.OrgName;
                return user;
            }
            else
            {
                throw new BOException("获取用户信息发生异常");
            }
        }
        public int SaveAreaManage(AreaManage area)
        {
            return _set.SaveAreaManage(area);
        }
        public int SaveStore(Store_EX se)
        {
            return _set.SaveStore(se);
        }
        public int SaveUser_Info(USER_INFO user)
        {
            return _set.SaveUser_Info(user);
        }

        /// <summary>
        /// 获取所以权限菜单
        /// </summary>
        /// <returns></returns>
        public List<SYS_RIGHT> GetAllRight()
        {
            return _set.GetAllRight();
        }

        /// <summary>
        /// 根据用户ID获取权限菜单
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<SYS_RIGHT> GetAllRight(int UserID)
        {
            return _set.GetAllRight(UserID);
        }

        /// <summary>
        /// 根据父级ID获取权限菜单
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<SYS_RIGHT> GetRightListByPid(int pid, PageView view)
        {
            return _set.GetRightListByPid(pid, view);
        }

        /// <summary>
        /// 获取指定权限菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SYS_RIGHT_EX GetRightByID(int id)
        {
            return _set.GetRightByID(id);
        }

        /// <summary>
        /// 保存权限菜单
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public int SaveMenu(SYS_RIGHT sys)
        {
            if (_set.GetRightByCode(sys.MENU_CODE, sys.RIGHT_ID).Count > 0)
                return -2;
            if (sys.RIGHT_ID == 0)
                return (int)_set.Insert(sys);
            else
                return _set.Update(sys);
        }

        /// <summary>
        /// 删除权限菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteMenu(int id)
        {
            return _set.DeleteMenu(id);
        }



        /// <summary>
        /// 获取仪表盘数据
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="BegDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public Index_Report_EX GetIndexReport(int storeID, string BegDate, string EndDate)
        {
            return _set.GetIndexReport(storeID, BegDate, EndDate);
        }

        /// <summary>
        /// 获取仪表盘数据
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="BegDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public Index_Report_EX GetWXIndexReport(int storeID, string BegDate, string EndDate)
        {
            return _set.GetWXIndexReport(storeID, BegDate, EndDate);
        }



        //全局检索会员信息
        public int SearchMemberInfo(string q, int orgid)
        {
            return _set.SearchMemberInfo(q, orgid);
        }

        /// <summary>
        /// 根据电话号码查询会员信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public CUST_INFO SearchMemberInfoByPhone(string phone, int orgid)
        {
            return _set.SearchMemberInfoByPhone(phone, orgid);
        }

        public int GetTopId()
        {
            return _set.GetTopId();
        }

        //未约进列表
        public List<CUST_INFO_EX> QueryBookingList(string storeId)
        {
            return _set.QueryBookingList(storeId);
        }

        //未约进 未付款订单 数
        public List<JsonSMsg> QueryOrderBookCount(string storeId)
        {
            return _set.QueryOrderBookCount(storeId);
        }

        #endregion


        /// <summary>
        /// 获取微信消息
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="replyType"></param>
        /// <returns></returns>
        public Information_EX GetInformationModel(string Key, int replyType)
        {
            return _set.GetInformationModel(Key, replyType);
        }
        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<QMActivity_EX> QueryGetQm(string name, PageView view)
        {
            return _set.QueryGetQm(name, view);
        }
        public PagedList<EMPLOYEE> QueryGetEmp(EmpSearch search, PageView view)
        {
            return _set.QueryGetEmp(search, view);
        }
        public List<EMPLOYEE> QueryAllEmp()
        {
            return _set.QueryAllEmp();
        }
        public int GetMaxEwmId()
        {
            return _set.GetMaxEwmId();
        }

        public List<EMPLOYEE> GetNullEwmIdList()
        {
            return _set.GetNullEwmIdList();
        }
        public List<EMPLOYEE_EX> QueryEmplReport(EmpSearch search)
        {
            return _set.QueryEmplReport(search);
        }
        public PagedList<AreaManage_EX> QueryGetArea(PageView view)
        {
            return _set.QueryGetArea(view);
        }
        public PagedList<Store_EX> QueryGetStore(PageView view)
        {
            return _set.QueryGetStore(view);
        }
        /// <summary>
        /// 获取微信消息集合
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public List<Information_EX> GetModelList(string sqlWhere)
        {
            return _set.GetModelList(sqlWhere);
        }

        /// <summary>
        /// 添加微信日志
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public int AddLog(WXLOG l)
        {
            return _set.AddLog(l);
        }

        /// <summary>
        /// 添加微信消息记录
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int AddCUST_MSG_HIS(WXCUST_MSG_HIS c)
        {
            return _set.AddCUST_MSG_HIS(c);
        }
        public int AddEmp_Cust(Emp_Cust e)
        {
            return _set.AddEmp_Cust(e);
        }
        /// <summary>
        /// 获取商户列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<ORG_INFO> QueryMerchantsList(RoleSearch search, PageView view)
        {
            return _set.QueryMerchantsList(search, view);
        }

        /// <summary>
        /// 获取商户列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public List<ORG_INFO> GetMerchantsList()
        {
            return _set.GetMerchantsList();
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchants(int id)
        {
            return _set.GetMerchants(id);
        }

        public ORG_INFO GetMerchants(string toUserName)
        {
            return _set.GetMerchants(toUserName);
        }
        public Emp_Cust GetEmp_Cust(string FromUserName)
        {
            return _set.GetEmp_Cust(FromUserName);
        }

        public GROUP_EMPLOYER_EX GetStoreNameByFromUserName(string FromUserName)
        {
            return _set.GetStoreNameByFromUserName(FromUserName);
        }

        public Emp_Cust GetEmp_CustById(int Id)
        {
            return _set.GetEmp_CustById(Id);
        }
        /// <summary>
        /// 保存商户
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public int SaveMerchants(ORG_INFO sys)
        {
            return _set.SaveMerchants(sys);
        }

        /// <summary>
        /// 保存图文列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int SaveGraphicList(WXGraphicList list)
        {
            return _set.SaveGraphicList(list);
        }

        /// <summary>
        /// 保存图文明细
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns></returns>
        public int SaveGraphicDetail(WXGraphicDetail Detail)
        {
            return _set.SaveGraphicDetail(Detail);
        }

        /// <summary>
        /// 获取图文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXGraphicList GetGraphicList(int id)
        {
            return _set.GetGraphicList(id);
        }

        /// <summary>
        /// 获取图文明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Graphic_Detail_EX GetGraphicDetail(int id)
        {
            return _set.GetGraphicDetail(id); ;
        }

        /// <summary>
        /// 获取图文明细列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<Graphic_Detail_EX> QueryGetGraphicDetail(GraphicSearch search, PageView view)
        {
            return _set.QueryGetGraphicDetail(search, view);
        }
        /// <summary>
        /// 保存二维码
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public int SaveQMActivity(QMActivity sys)
        {
            return _set.SaveQMActivity(sys);
        }
        public int SaveEMPLOYEE(EMPLOYEE sys)
        {
            return _set.SaveEMPLOYEE(sys);
        }
        public int GetEwmId()
        {
            int ewmId = _set.GetEwmId();
            if (ewmId < 10000)
            {
                ewmId = 10000;
            }
            return ewmId;
        }
        /// <summary>
        ///插入粉丝信息
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public int InsertFans(WXCUST_FANS f)
        {
            return (int)_set.Insert(f);
        }

        /// <summary>
        /// 修改粉丝信息
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public int UpdateFans(WXCUST_FANS f)
        {
            return _set.Update(f);
        }
        public int UpdateUserInfo(int? userid, string userno, string userpass)
        {
            return _set.UpdateUserInfo(userid, userno, userpass);
        }
        public int UpdateAreaName(int id, string name, string title)
        {
            return _set.UpdateAreaName(id, name, title);
        }
        public int UpdateShow(int id, string IsShow)
        {
            return _set.UpdateShow(id, IsShow);
        }
        /// <summary>
        /// 获取粉丝信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        public WXCUST_FANS GetFansByFromUserName(string FromUserName)
        {
            return _set.GetFansByFromUserName(FromUserName);
        }

        /// <summary>
        /// 根据商户微信号获取商户信息
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchantsByToUserName(string ToUserName)
        {
            return _set.GetMerchantsByToUserName(ToUserName);
        }

        /// <summary>
        /// 获取自动回复消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Information_EX GetInformation(int? id)
        {
            return _set.GetInformation(id);
        }

        /// <summary>
        /// 保存自动回复消息
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int SaveInformation(WXInformation i)
        {
            return _set.SaveInformation(i);
        }

        /// <summary>
        /// 获取自动回复消息列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<Information_EX> QueryInformationList(RoleSearch search, PageView view)
        {
            return _set.QueryInformationList(search, view);
        }

        /// <summary>
        /// 根据消息类型获取数据条数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="replyType"></param>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        public int GetCountByreplyType(int? id, int? replyType, int? Merchants_ID)
        {
            return _set.GetCountByreplyType(id, replyType, Merchants_ID);
        }

        /// <summary>
        /// 获取微信菜单消息集合
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public List<CustomMenu_EX> GetCustomMenuModelList(string sqlWhere)
        {
            return _set.GetCustomMenuModelList(sqlWhere);
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<CUST_FANS_EX> QueryGetFansDetail(FansSearch search, PageView view)
        {
            return _set.QueryGetFansDetail(search, view);
        }
        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<CUST_FANS_EX> QueryGetFansByEWM(FansSearch search, PageView view)
        {
            return _set.QueryGetFansByEWM(search, view);
        }
        public PagedList<CUST_FANS_EX> QueryGetFansByEmp(FansSearch search, PageView view)
        {
            return _set.QueryGetFansByEmp(search, view);
        }
        public PagedList<CUST_FANS_EX> QueryGetFansByMobile(FansSearch search, PageView view)
        {
            return _set.QueryGetFansByMobile(search, view);
        }
        /// <summary>
        /// 根据微信原始ID获取一位粉丝
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public WXCUST_FANS GetOneFans(string ToUserName)
        {
            return _set.GetOneFans(ToUserName);
        }
        /// <summary>
        /// 得到一个JsPai对象实体
        /// </summary>
        public ApiTicket GetModelJsApi()
        {
            return _set.GetModelJsApi();
        }
        public int SaveApiTicket(ApiTicket m)
        {
            return _set.SaveApiTicket(m);
        }
        /// <summary>
        /// 根据粉丝ID获取粉丝详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CUST_FANS_EX GetFansByID(int? id)
        {
            return _set.GetFansByID(id);
        }
        public EMPLOYEE GetEmpByPhone(string phone)
        {
            return _set.GetEmpByPhone(phone);
        }
        public EMPLOYEE GetEmpByUserID(string userId)
        {
            return _set.GetEmpByUserID(userId);
        }
        public string GetNameByEwmId(string EwmId)
        {
            return _set.GetNameByEwmId(EwmId);
        }
        public EMPLOYEE GetEmpByEwmId(string EwmId)
        {
            return _set.GetEmpByEwmId(EwmId);
        }
        /// <summary>
        /// 获取商户设置
        /// </summary>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        public MerchantsSeting_EX GetMerchantsSetingByMerchantsID(int? Merchants_ID)
        {
            return _set.GetMerchantsSetingByMerchantsID(Merchants_ID);
        }

        /// <summary>
        /// 保存商户设置
        /// </summary>
        /// <param name="seting"></param>
        /// <returns></returns>
        public int SaveMerchantsSeting(WXMerchantsSeting seting)
        {
            if (seting.ID == 0)
                return (int)_set.Insert(seting);
            else
                return _set.Update(seting);
        }

        /// <summary>
        /// 获取会员卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXMembershipCard GetMembershipCard(int? id)
        {
            return _set.GetMembershipCard(id);
        }
        public AreaManage_EX GetArea(int? id)
        {
            return _set.GetArea(id);
        }
        public Store_EX GetStore(int? id)
        {
            return _set.GetStore(id);
        }
        /// <summary>
        /// 查询留言
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WXOpinion_EX> GetWXOpinionList(PageView view)
        {
            return _set.GetWXOpinionList(view);
        }

        /// <summary>
        /// 保存会员卡
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public int SaveMembershipCard(WXMembershipCard m)
        {
            return _set.SaveMembershipCard(m);
        }

        /// <summary>
        /// 获取会员卡列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WXMembershipCard> QueryGetMembershipCard(MembershipCardSearch search, PageView view)
        {
            return _set.QueryGetMembershipCard(search, view);
        }

        /// <summary>
        /// 获取所以图文
        /// </summary>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        public List<Graphic_Detail_EX> GetAllGraphicDetail(int? list_id)
        {
            return _set.GetAllGraphicDetail(list_id);
        }

        /// <summary>
        /// 获取所以图文
        /// </summary>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        public List<WXGraphicList> GetAllGraphicList(int? Merchants_ID)
        {
            return _set.GetAllGraphicList(Merchants_ID); ;
        }

        /// <summary>
        /// 删除图文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteTW(int id)
        {
            return _set.DeleteTW(id);
        }
        public int DeleteInformation(int id)
        {
            return _set.DeleteInformation(id);
        }
        public int DeleteStore(int id)
        {
            return _set.DeleteStore(id);
        }
        public int DeleteBD(int id)
        {
            return _set.DeleteBD(id);
        }
        public int DeleteEmp(int id)
        {
            return _set.DeleteEmp(id);
        }
        public int UpdateBD(int id, int? EwmId, string phone)
        {
            return _set.UpdateBD(id, EwmId, phone);
        }
        /// <summary>
        /// 根据顺序获取图文
        /// </summary>
        /// <param name="list_id"></param>
        /// <param name="rowid"></param>
        /// <returns></returns>
        public Graphic_Detail_EX GetGraphic_DetailByRowID(int list_id, int rowid)
        {
            return _set.GetGraphic_DetailByRowID(list_id, rowid);
        }

        /// <summary>
        /// 根据顺序删除图文
        /// </summary>
        /// <param name="list_id"></param>
        /// <param name="rowid"></param>
        /// <returns></returns>
        public int DelGraphic_DetailByRowID(int list_id, int rowid)
        {
            return _set.DelGraphic_DetailByRowID(list_id, rowid);
        }

        /// <summary>
        /// 保存微信聊天记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int SaveCUST_MSG_RECORD(WXCUST_MSG_RECORD msg)
        {
            if (msg.ID == 0)
                return (int)_set.Insert(msg);
            else
                return _set.Update(msg);
        }

        /// <summary>
        /// 获取粉丝聊天记录
        /// </summary>
        /// <param name="fansid"></param>
        /// <returns></returns>
        public List<CUST_MSG_RECORD_EX> GetMsgList(int? fansid)
        {
            return _set.GetMsgList(fansid);
        }

        /// <summary>
        /// 微信表情
        /// </summary>
        /// <returns></returns>
        public List<WXBiaoqing> GetBQList()
        {
            return _set.GetBQList(); ;
        }

        /// <summary>
        /// 获取单条微信消息记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CUST_MSG_RECORD_EX GetMsgByID(int? id)
        {
            return _set.GetMsgByID(id);
        }

        /// <summary>
        /// 保存微信消息记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int SaveMsg(WXCUST_MSG_RECORD msg)
        {
            return _set.SaveMsg(msg);
        }

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXTicket GetTicket(int? id)
        {
            return _set.GetTicket(id);
        }

        /// <summary>
        /// 保存二维码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int SaveQrCode(Qrcode code)
        {
            if (code.ID == 0)
                return (int)_set.Insert(code);
            else
                return _set.Update(code);
        }

        /// <summary>
        /// 保存优惠券
        /// </summary>
        /// <param name="wx"></param>
        /// <returns></returns>
        public int SaveTicket(WXTicket wx)
        {
            if (wx.ID == 0)
                return (int)_set.Insert(wx);
            else
                return _set.Update(wx);
        }

        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WXTicket> QueryTicketList(RoleSearch search, PageView view)
        {
            return _set.QueryTicketList(search, view);
        }

        public int SaveCustInfo(CUST_INFO c)
        {
            return (int)_set.Insert(c);
        }

        /// <summary>
        /// 获取绑定列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<CUST_INFO> QueryCustList(RoleSearch search, PageView view)
        {
            return _set.QueryCustList(search, view);
        }

        public List<ORG_STORE_EX> GetORG_STOREOrderBy(string sql)
        {
            return _set.GetORG_STOREOrderBy(sql);
        }


        //public int SaveTemplateLog(TEMPLATE_LOG entity)
        //{
        //    return (int)_set.Insert(entity);
        //}

        //public PagedList<TEMPLATE_LOG> QueryTemplateLogList(RoleSearch search, PageView view)
        //{
        //    return _set.QueryTemplateLogList(search, view);
        //}

        public List<WXCUST_FANS> QueryTemplateFansList(string groupid, string name)
        {
            return _set.QueryTemplateFansList(groupid, name);
        }

        public PagedList<WXCUST_FANS> QueryTemplateFansListT(string groupid, string name, PageView view)
        {
            //获取所有大区、区域、门店
            if (!string.IsNullOrEmpty(groupid))
            {
                string[] grouplist = groupid.Split(',');
                foreach (var item in grouplist)
                {
                    var groups = _groupRepo.GetRecursiveAllByParentID(int.Parse(item));
                    List<int> groupIds = groups.Select(m => m.ID).ToList();
                    foreach (var item1 in groupIds)
                    {
                        groupid = groupid + "," + item1.ToString();
                    }
                }

            }
            return _set.QueryTemplateFansListT(groupid, name, view);
        }
        public CUST_INFO GetCustInfoSms(string fromusername)
        {
            return _set.GetCustInfoSms(fromusername);
        }

        public int UpdateCustInfoBySms(string fromusername)
        {
            return _set.UpdateCustInfoBySms(fromusername);
        }

        //public PagedList<TEMPLATE_INFO> QueryTemplateInfoList(PageView view)
        //{
        //    return _set.QueryTemplateInfoList(view);
        //}

        //public TEMPLATE_INFO QueryTemplateInfo(int id)
        //{
        //    return _set.QueryTemplateInfo(id);
        //}

        public List<FansList_Ex> QueryFansListByBuy()
        {
            return _set.QueryFansListByBuy();
        }

        public FILES UploadFile(string ext, string uploadurl, string contentType, byte[] data, string url, string remark)
        {
            FILES fileEntity = null;
            try
            {
                string newName = Guid.NewGuid().ToString("d") + ext;

                string filePath = string.Concat(uploadurl, DateTime.Today.ToString("yyyyMMdd"), "\\");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fullName = filePath + newName;
                using (FileStream steam = new FileStream(fullName, FileMode.CreateNew, FileAccess.Write))
                {
                    steam.Write(data, 0, data.Length);
                }

                fileEntity = new FILES();
                fileEntity.CONTENT_TYPE = contentType;
                fileEntity.FILE_NAME = fullName;
                fileEntity.FILE_SIZE = data.Length;
                fileEntity.FILE_URL = "<!--只有图片存放到外部时才有用哦--!>";

                if (remark != "")
                    fileEntity.REMARK = remark;
                else
                    fileEntity.REMARK = fullName.Split('\\')[fullName.Split('\\').Length - 1];
                fileEntity.ID = (int)_set.Insert(fileEntity);

                fileEntity.FILE_URL = url + "/" + fileEntity.ID;
                _set.Update(fileEntity);

            }
            catch (Exception ex)
            {
                throw new BOException("上传图片发生意外错误，请稍后重试，或联系管理员", ex);
            }
            return fileEntity;
        }

        public FILES GetUploadFile(int id)
        {
            return _set.GetFILES(id);
        }


        public int SaveCustFans(WXCUST_FANS entity)
        {
            if (entity.ID == 0)
            {
                return (int)_set.Insert(entity);
            }
            else
            {
                return _set.Update(entity);
            }
        }


        public int SaveLocation(WD_Location entity)
        {
            if (entity.ID == 0)
            {
                return (int)_set.Insert(entity);
            }
            else
            {
                return _set.Update(entity);
            }
        }


        public List<Information_EX> GetModelListByKeywordList(int ReplyType, string Keyword, string ToUserName)
        {
            return _set.GetModelListByKeywordList(ReplyType, Keyword, ToUserName);
        }


        public List<CustomMenu_EX> GetCustomMenuModelList(string EventKey, string ToUserName)
        {
            return _set.GetCustomMenuModelList(EventKey, ToUserName);
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<TT_Detail> QueryGoodsList(GraphicSearch search, PageView view)
        {
            return _set.QueryGoodsList(search, view);
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TT_Detail GetTT_Detail(int id)
        {
            return _set.GetTT_Detail(id);
        }

        /// <summary>
        /// 保存商品详情
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public int SaveTT_Detail(TT_Detail detail)
        {
            return _set.SaveTT_Detail(detail);
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeteleTT_Detail(int id)
        {
            return _set.DeteleTT_Detail(id);
        }

        public List<TT_Detail> GetTTDetailList(int age, string ptype)
        {
            return _set.GetTTDetailList(age, ptype);
        }

        /// <summary>
        /// 增加绑定关系并且送积分
        /// </summary>
        /// <param name="fansid"></param>
        /// <param name="openid"></param>
        public void GoToSendPoint(int fansid, string openid)
        {
            //IBcjStoreService api = ObjectFactory.GetInstance<IBcjStoreService>();
            //api.StartSendTmp();
            //CommonHelp.SendPoint(fansid, openid);
            CommonHelp.SendTmps(new List<WX_TMP_HIS>(), "");
        }


        /// <summary>
        /// 添加微信扫码记录
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int AddWXCustScanRecord(WXCustScanRecord c)
        {
            return _set.AddWXCustScanRecord(c);
        }

        /// <summary>
        /// 标志卡券已经领取
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int UpdateWXCouponIsGet(string OpenId, string CouponNo, string CardId)
        {
            return _set.UpdateWXCouponIsGet(OpenId, CouponNo, CardId);
        }


        /// <summary>
        /// 标志卡券已经核销
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int UpdateWXCouponIsHX(string OpenId, string CouponNo, string CardId)
        {
            return _set.UpdateWXCouponIsHX(OpenId, CouponNo, CardId);
        }
        #region 盛时ar
        public int SaveArInfo(AR_QR_FANS entity)
        {
            if (entity.ID > 0)
            {
                return _set.Update(entity);
            }
            else
            {
                return (int)_set.Insert(entity);
            }
        }
        public int GetMaxArid()
        {
            int ewmId = 0;
            AR_QR_FANS ar = _set.GetMaxArid();
            ewmId = ar.AR_ID.Value;
            if (ewmId > 9999)
            {
                ewmId = 0;
            }
            return ewmId;
        }

        public AR_QR_FANS QueryArInfo(int id)
        {
            return _set.QueryArInfo(id);
        }

        public AR_QR_FANS QueryArInfoByArId(int arid)
        {
            return _set.QueryArInfoByArId(arid);
        }

        public int DeleteAr(int id)
        {
            return _set.DeleteAr(id);
        }

        public bool GetAllGraphicOne(string media_Id)
        {
            int count = _set.GetAllGraphicOne(media_Id);

            return count > 0;
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <returns></returns>
        public List<FILE_EX> GetFiilesImage()
        {
            return _set.GetFiilesImage();
        }


        #endregion

    }
}
