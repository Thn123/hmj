using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface ISystemService
    {
        AreaManage GetAreaManage(int? id);
        /// <summary>
        /// 判断用户是否拥有权限
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <param name="rightCode">权限标识</param>
        /// <returns></returns>
        bool CheckUserPrivilege(string userId, string rightCode);
        int SaveAreaManage(AreaManage area);
        int SaveStore(Store_EX area);
        int SaveUser_Info(USER_INFO user);
        int GetTopId();
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        IUser GetUserInfo(string userId);

        /// <summary>
        /// 获取用户的菜单
        /// </summary>
        /// <param name="userid">用户标识</param>
        /// <returns></returns>
        List<IPrivilege> GetUserMenus(string userid);


        /// <summary>
        /// 获取所以权限菜单
        /// </summary>
        /// <returns></returns>
        List<SYS_RIGHT> GetAllRight();

        /// <summary>
        /// 根据用户ID获取所以权限菜单
        /// </summary>
        /// <returns></returns>
        List<SYS_RIGHT> GetAllRight(int UserID);
        List<FILE_EX> GetFiilesImage();

        /// <summary>
        /// 根据父菜单查询权限列表
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<SYS_RIGHT> GetRightListByPid(int pid, PageView view);

        /// <summary>
        /// 获取指定权限菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SYS_RIGHT_EX GetRightByID(int id);

        /// <summary>
        /// 保存权限菜单
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        int SaveMenu(SYS_RIGHT sys);

        /// <summary>
        /// 删除权限菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteMenu(int id);


        /// <summary>
        /// 获取仪表盘数据
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="BegDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        Index_Report_EX GetIndexReport(int storeID, string BegDate, string EndDate);

        /// <summary>
        /// 获取仪表盘数据
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="BegDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        Index_Report_EX GetWXIndexReport(int storeID, string BegDate, string EndDate);


        int SearchMemberInfo(string q, int orgid);

        /// <summary>
        /// 根据电话号码查询会员信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        CUST_INFO SearchMemberInfoByPhone(string phone, int orgid);


        //未约进列表
        List<CUST_INFO_EX> QueryBookingList(string storeId);



        /// <summary>
        /// 获取微信消息
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="replyType"></param>
        /// <returns></returns>
        Information_EX GetInformationModel(string Key, int replyType);

        /// <summary>
        /// 获取微信消息集合
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        List<Information_EX> GetModelList(string sqlWhere);

        /// <summary>
        /// 添加微信日志
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        int AddLog(WXLOG l);

        /// <summary>
        /// 添加微信消息记录
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        int AddCUST_MSG_HIS(WXCUST_MSG_HIS c);

        //添加绑定关系
        int AddEmp_Cust(Emp_Cust e);

        /// <summary>
        /// 获取商户列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<ORG_INFO> QueryMerchantsList(RoleSearch search, PageView view);

        /// <summary>
        /// 获取商户列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        List<ORG_INFO> GetMerchantsList();

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ORG_INFO GetMerchants(int id);
        ORG_INFO GetMerchants(string toUserName);
        Emp_Cust GetEmp_Cust(string FromUserName);
        /// <summary>
        /// 根据fromusername获取员工门店信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        GROUP_EMPLOYER_EX GetStoreNameByFromUserName(string FromUserName);

        Emp_Cust GetEmp_CustById(int Id);
        /// <summary>
        /// 保存商户
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        int SaveMerchants(ORG_INFO sys);
        /// 保存二维码
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        int SaveQMActivity(QMActivity sys);
        int GetEwmId();
        int SaveEMPLOYEE(EMPLOYEE sys);
        /// <summary>
        /// 保存图文列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        int SaveGraphicList(WXGraphicList list);

        /// <summary>
        /// 保存图文明细
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns></returns>
        int SaveGraphicDetail(WXGraphicDetail Detail);

        /// <summary>
        /// 获取图文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WXGraphicList GetGraphicList(int id);

        /// <summary>
        /// 获取图文明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Graphic_Detail_EX GetGraphicDetail(int id);



        /// <summary>
        /// 获取图文明细列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<Graphic_Detail_EX> QueryGetGraphicDetail(GraphicSearch search, PageView view);

        /// <summary>
        ///插入粉丝信息
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        int InsertFans(WXCUST_FANS f);

        /// <summary>
        /// 修改粉丝信息
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        int UpdateFans(WXCUST_FANS f);
        int UpdateAreaName(int id, string name, string title);
        int UpdateUserInfo(int? userid, string userno, string userpass);
        int UpdateShow(int id, string IsShow);
        /// <summary>
        /// 获取粉丝信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        WXCUST_FANS GetFansByFromUserName(string FromUserName);

        /// <summary>
        /// 根据商户微信号获取商户信息
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        ORG_INFO GetMerchantsByToUserName(string ToUserName);

        /// <summary>
        /// 获取自动回复消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Information_EX GetInformation(int? id);

        /// <summary>
        /// 保存自动回复消息
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        int SaveInformation(WXInformation i);

        /// <summary>
        /// 获取自动回复消息列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<Information_EX> QueryInformationList(RoleSearch search, PageView view);

        /// <summary>
        /// 根据消息类型获取数据条数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="replyType"></param>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        int GetCountByreplyType(int? id, int? replyType, int? Merchants_ID);

        /// <summary>
        /// 获取微信菜单消息集合
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        List<CustomMenu_EX> GetCustomMenuModelList(string sqlWhere);


        bool GetAllGraphicOne(string media_Id);


        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<CUST_FANS_EX> QueryGetFansDetail(FansSearch search, PageView view);

        //二维码查看详情
        PagedList<CUST_FANS_EX> QueryGetFansByEWM(FansSearch search, PageView view);
        PagedList<CUST_FANS_EX> QueryGetFansByEmp(FansSearch search, PageView view);
        PagedList<CUST_FANS_EX> QueryGetFansByMobile(FansSearch search, PageView view);
        /// <summary>
        /// 根据微信原始ID获取一位粉丝
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        WXCUST_FANS GetOneFans(string ToUserName);
        /// <summary>
        /// 得到一个JsPai对象实体
        /// </summary>
        ApiTicket GetModelJsApi();
        int SaveApiTicket(ApiTicket m);
        /// <summary>
        /// 根据粉丝ID获取粉丝详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CUST_FANS_EX GetFansByID(int? id);
        EMPLOYEE GetEmpByPhone(string phone);

        EMPLOYEE GetEmpByUserID(string userId);

        EMPLOYEE GetEmpByEwmId(string EwmId);
        string GetNameByEwmId(string EwmId);
        /// <summary>
        /// 获取二维码列表列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<QMActivity_EX> QueryGetQm(string name, PageView view);
        PagedList<EMPLOYEE> QueryGetEmp(EmpSearch serarch, PageView view);
        /// <summary>
        /// 获取emp数据
        /// </summary>
        /// <returns></returns>
        List<EMPLOYEE> QueryAllEmp();

        /// <summary>
        /// 获取emp最大二维码id
        /// </summary>
        /// <returns></returns>
        int GetMaxEwmId();

        List<EMPLOYEE> GetNullEwmIdList();
        List<EMPLOYEE_EX> QueryEmplReport(EmpSearch serarch);
        PagedList<AreaManage_EX> QueryGetArea(PageView view);

        PagedList<Store_EX> QueryGetStore(PageView view);
        /// <summary>
        /// 获取商户设置
        /// </summary>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        MerchantsSeting_EX GetMerchantsSetingByMerchantsID(int? Merchants_ID);

        /// <summary>
        /// 保存商户设置
        /// </summary>
        /// <param name="seting"></param>
        /// <returns></returns>
        int SaveMerchantsSeting(WXMerchantsSeting seting);


        /// <summary>
        /// 获取会员卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WXMembershipCard GetMembershipCard(int? id);

        AreaManage_EX GetArea(int? id);

        Store_EX GetStore(int? id);
        /// <summary>
        /// 保存会员卡
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        int SaveMembershipCard(WXMembershipCard m);

        /// <summary>
        /// 获取会员卡列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<WXMembershipCard> QueryGetMembershipCard(MembershipCardSearch search, PageView view);

        /// <summary>
        /// 查询留言
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<WXOpinion_EX> GetWXOpinionList(PageView view);

        /// <summary>
        /// 获取所以图文明细
        /// </summary>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        List<Graphic_Detail_EX> GetAllGraphicDetail(int? list_ID);

        /// <summary>
        /// 获取所以图文
        /// </summary>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        List<WXGraphicList> GetAllGraphicList(int? Merchants_ID);

        /// <summary>
        /// 删除图文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteTW(int id);
        int DeleteInformation(int id);
        int DeleteStore(int id);
        int DeleteBD(int id);
        int DeleteEmp(int id);
        int UpdateBD(int id, int? EwmId, string phone);
        /// <summary>
        /// 根据顺序获取图文
        /// </summary>
        /// <param name="list_id"></param>
        /// <param name="rowid"></param>
        /// <returns></returns>
        Graphic_Detail_EX GetGraphic_DetailByRowID(int list_id, int rowid);

        /// <summary>
        /// 根据顺序删除图文
        /// </summary>
        /// <param name="list_id"></param>
        /// <param name="rowid"></param>
        /// <returns></returns>
        int DelGraphic_DetailByRowID(int list_id, int rowid);

        /// <summary>
        /// 保存微信聊天记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        int SaveCUST_MSG_RECORD(WXCUST_MSG_RECORD msg);

        /// <summary>
        /// 获取粉丝聊天记录列表
        /// </summary>
        /// <param name="fansid"></param>
        /// <returns></returns>
        List<CUST_MSG_RECORD_EX> GetMsgList(int? fansid);

        /// <summary>
        /// 微信表情
        /// </summary>
        /// <returns></returns>
        List<WXBiaoqing> GetBQList();

        /// <summary>
        /// 获取单条微信消息记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CUST_MSG_RECORD_EX GetMsgByID(int? id);

        /// <summary>
        /// 保存微信消息记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        int SaveMsg(WXCUST_MSG_RECORD msg);

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WXTicket GetTicket(int? id);

        /// <summary>
        /// 保存二维码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        int SaveQrCode(Qrcode code);

        /// <summary>
        /// 保存优惠券
        /// </summary>
        /// <param name="wx"></param>
        /// <returns></returns>
        int SaveTicket(WXTicket wx);

        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<WXTicket> QueryTicketList(RoleSearch search, PageView view);

        /// <summary>
        /// 获取绑定列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<CUST_INFO> QueryCustList(RoleSearch search, PageView view);

        List<ORG_STORE_EX> GetORG_STOREOrderBy(string sql);

        int SaveCustInfo(CUST_INFO c);

        /// <summary>
        /// 更加fromusername获取用户信息(sms)
        /// </summary>
        /// <param name="fromusername"></param>
        /// <returns></returns>
        CUST_INFO GetCustInfoSms(string fromusername);

        /// <summary>
        /// 更新状态(sms)
        /// </summary>
        /// <param name="fromusername"></param>
        /// <returns></returns>
        int UpdateCustInfoBySms(string fromusername);

        #region by moon crete_time 2016-12-6

        /// <summary>
        /// 保存模板群发消息日志
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //int SaveTemplateLog(TEMPLATE_LOG entity);


        /// <summary>
        /// 获取模板群发消息日志列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        //PagedList<TEMPLATE_LOG> QueryTemplateLogList(RoleSearch search, PageView view);

        /// <summary>
        /// 获取需要发送模板消息的粉丝
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        List<WXCUST_FANS> QueryTemplateFansList(string groupid,string name);

        /// <summary>
        /// 获取需要发送模板消息的粉丝
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<WXCUST_FANS> QueryTemplateFansListT(string groupid,string name, PageView view);


        /// <summary>
        /// 获取模板详细列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        //PagedList<TEMPLATE_INFO> QueryTemplateInfoList(PageView view);

        ///// <summary>
        ///// 获取模板信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //TEMPLATE_INFO QueryTemplateInfo(int id);


        /// <summary>
        /// 获取已经关注的粉丝信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<FansList_Ex> QueryFansListByBuy();

        /// <summary>
        /// 获取微信消息集合 (根据关键字获取文本或者图文)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        List<Information_EX> GetModelListByKeywordList(int ReplyType, string Keyword, string ToUserName);

        /// <summary>
        /// 获取微信菜单消息集合
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        List<CustomMenu_EX> GetCustomMenuModelList(string EventKey, string ToUserName);

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FILES GetUploadFile(int id);

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="contentType"></param>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        FILES UploadFile(string ext, string uploadurl, string contentType, byte[] data, string url, string remark);


        /// <summary>
        /// 保存粉丝信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int SaveCustFans(WXCUST_FANS entity);

        /// <summary>
        /// 保存微信用户地理位置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int SaveLocation(WD_Location entity);
        #endregion


        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<TT_Detail> QueryGoodsList(GraphicSearch search, PageView view);

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TT_Detail GetTT_Detail(int id);

        /// <summary>
        /// 保存商品详情
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        int SaveTT_Detail(TT_Detail detail);

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeteleTT_Detail(int id);

        #region 盛时ar
        /// <summary>
        /// 保存ar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int SaveArInfo(AR_QR_FANS entity);

        /// <summary>
        /// 获取ar最大二维码id
        /// </summary>
        /// <returns></returns>
        int GetMaxArid();

        /// <summary>
        /// 根据图片id获取openid
        /// </summary>
        /// <param name="mediaid"></param>
        /// <returns></returns>
        AR_QR_FANS QueryArInfo(int id);

        /// <summary>
        /// 根据ar_id获取记录
        /// </summary>
        /// <param name="arid"></param>
        /// <returns></returns>
        AR_QR_FANS QueryArInfoByArId(int arid);

        /// <summary>
        /// 删除ar记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteAr(int id);
        void GoToSendPoint(int fansid, string openid);

        #endregion

        /// <summary>
        /// 添加微信扫码记录
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        int AddWXCustScanRecord(WXCustScanRecord c);

        /// <summary>
        /// 标志卡券已经领取
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        int UpdateWXCouponIsGet(string OpenId, string CouponNo, string CardId);

        /// <summary>
        /// 标志卡券已经核销
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        int UpdateWXCouponIsHX(string OpenId, string CouponNo, string CardId);
    }
}


