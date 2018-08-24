using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Interface;
using System;
using System.Collections.Generic;

namespace Hmj.Business.ServiceImpl
{
    public class MySmallShopService : IMySmallShopService
    {
        private MySmallShopRepository _set;

        public MySmallShopService()
        {
            _set = new MySmallShopRepository();
        }

        public string GetNickImg(string FromUserName) 
        {
            return _set.GetNickImg(FromUserName);
        }
        public int GetGuanJiaCount(string FromUserName)
        {
            return _set.GetGuanJiaCount(FromUserName);
        }
        public EMPLOYEE GetGuanJiaName(string FromUserName)
        {
            return _set.GetGuanJiaName(FromUserName);
        }
        public EMPLOYEE GetGuanjiaById(int Id) 
        {
            return _set.GetGuanjiaById(Id);
        }
        public List<Region> GetRegion()
        {
            return _set.GetRegion();
        }
        public City GetCityByCode(int CityCode)
        {
            return _set.GetCityByCode(CityCode);
        }
        public Callback GetCallbackByFromUserName(string FromUserName)
        {
            return _set.GetCallbackByFromUserName(FromUserName);
        }
        public int DeleteEmp_Cust(int Id) 
        {
            return _set.DeleteEmp_Cust(Id);
        }
        public Emp_Cust GetEmp_CustByFromUserName(string FromUserName)
        {
            return _set.GetEmp_CustByFromUserName(FromUserName);
        }
        public List<City> GetCityByReg(string RegionCode)
        {
            return _set.GetCityByReg(RegionCode);
        }
        /// <summary>
        /// 根据商户ID获取门店列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WD_STORE> GetStoreByMid(int Mid, PageView view)
        {
            return _set.GetStoreByMid(Mid, view);
        }

        /// <summary>
        /// 获取价目表明细
        /// </summary>
        /// <param name="Mid"></param>
        /// <returns></returns>
        public List<ProDetail> GetPriceDetailListByPid(int Mid)
        {
            return _set.GetPriceDetailListByPid(Mid);
        }
        public List<WXCUST_MSG_HIS> GetWXCUST_MSG_HIS(string strwhere)
        {
            return _set.GetWXCUST_MSG_HIS(strwhere);
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        public List<MDSearch> GetCity(string Province, int Storetype)
        {
            return _set.GetCity(Province, Storetype);
        }
        public List<MDSearch> GetAllCity(string Province)
        {
            return _set.GetAllCity(Province);
        }
        public List<MDSearch> GetProvince(int storetype)
        {
            return _set.GetProvince(storetype);
        }
        public List<MDSearch> GetAllProvince()
        {
            return _set.GetAllProvince();
        }
        public List<MDSearch> GetAll()
        {
            return _set.GetAll();
        }
        public List<AreaManage> GetAllArea()
        {
            return _set.GetAllArea();
        }
        public List<AreaManage> GetAllAreaByName(string name)
        {
            return _set.GetAllAreaByName(name);
        }
        public AreaManage GetArea(int id)
        {
            return _set.GetArea(id);
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        public List<MDSearch> GetModelList(string Province)
        {
            return _set.GetModelList(Province);
        }
        public List<AreaManage> GetMoArea(string strWhere)
        {
            return _set.GetMoArea(strWhere);
        }
        /// <summary>
        /// 获取价目表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        //public PagedList<Pro> GetPriceListByMid(int Mid, PageView view)
        //{
        //    return _set.GetPriceListByMid(Mid, view);
        //}

        ///// <summary>
        ///// 获取价目表
        ///// </summary>
        ///// <param name="Mid"></param>
        ///// <returns></returns>
        //public List<Pro> GetPriceListByMid(int Mid)
        //{
        //    return _set.GetPriceListByMid(Mid);
        //}

        /// <summary>
        /// 获取价目表明细
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<ProDetail> GetPriceDetailListByPid(int Mid, PageView view)
        {
            return _set.GetPriceDetailListByPid(Mid, view);
        }


        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <returns></returns>
        public List<WD_STORE> GetStoreByMid(int Mid)
        {
            return _set.GetStoreByMid(Mid);
        }

        /// <summary>
        /// 获取门店列表，是否适用优惠券
        /// </summary>
        /// <param name="Mid">商户id</param>
        /// <param name="Tid">优惠券id</param>
        /// <returns></returns>
        public List<WD_STORE_EX> GetStoreByMid(int Mid, int Tid)
        {
            return _set.GetStoreByMid(Mid, Tid);
        }

        /// <summary>
        ///  根据商户ID获取预约列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WD_BOOKING> GetBookingListByMid(int Mid, PageView view)
        {
            return _set.GetBookingListByMid(Mid, view);
        }

        /// <summary>
        /// 保存门店
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int SaveStore(WD_STORE s)
        {
            return _set.SaveStore(s);
        }
        public int SaveCallback(Callback s)
        {
            return _set.SaveCallback(s);
        }
        public int SaveDelEmp_Cust(DelEmp_Cust s)
        {
            return _set.SaveDelEmp_Cust(s);
        }
        ///// <summary>
        ///// 保存价目表
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public int SavePrice(Pro p)
        //{
        //    if (p.Id == 0)
        //        return (int)_set.Insert(p);
        //    else
        //        return _set.Update(p);
        //}

        /// <summary>
        /// 保存价目表明细
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public int SavePriceDetail(ProDetail pd)
        {
            if (pd.Id == 0)
                return (int)_set.Insert(pd);
            else
                return _set.Update(pd);
        }

        /// <summary>
        /// 获取门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_STORE GetStore(int id)
        {
            return _set.GetStore(id);
        }

        /// <summary>
        /// 获取商户所有门店
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public List<WD_STORE> GetStoreList(string ToUserName)
        {
            return _set.GetStoreList(ToUserName);
        }


        /// <summary>
        /// 获取商户所有门店
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public List<WD_STORE> GetStoreList(string ToUserName, string city)
        {
            return _set.GetStoreList(ToUserName, city);
        }

        /// <summary>
        /// 获取门店城市
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public List<string> GetCityList(string ToUserName)
        {
            return _set.GetCityList(ToUserName);
        }

        /// <summary>
        /// 根据ID获取门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_STORE GetStoreByID(int id)
        {
            return _set.GetStoreByID(id);
        }

        /// <summary>
        /// 获取门店所有员工
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public List<WD_EMPLOYEE> GetEmployeeList(int ID)
        {
            return _set.GetEmployeeList(ID);
        }

        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_EMPLOYEE GetEmployeeByID(int id)
        {
            return _set.GetEmployeeByID(id);
        }

        /// <summary>
        /// 获取员工的作品集
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WD_WORKS> GetWorksList(int id)
        {
            return _set.GetWorksList(id);
        }

        /// <summary>
        /// 获取门店作品集
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WD_WORKS_EX> GetStoreWorksList(int id)
        {
            return _set.GetStoreWorksList(id);
        }

        /// <summary>
        /// 获取员工作品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_WORKS GetWorksByID(int id)
        {
            return _set.GetWorksByID(id);
        }

        /// <summary>
        /// 保存页面访问日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int SavePageLog(WX_PageLog log)
        {
            return (int)_set.Insert(log);
        }

        /// <summary>
        /// 查询门店适用优惠券
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public List<WXTicket> GetTicketBySID(int sid)
        {
            return _set.GetTicketBySID(sid);
        }

        /// <summary>
        /// 查询门店适用优惠券 及会员是否已领取
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public List<WXTicket_EX> GetTicketBySID(int sid, string OpenID)
        {
            return _set.GetTicketBySID(sid, OpenID);
        }

        /// <summary>
        /// 获取游客信息
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public WX_PageLog GetPerson(string OpenID)
        {
            return _set.GetPerson(OpenID);
        }

        /// <summary>
        /// 领券/核销券
        /// </summary>
        /// <param name="per"></param>
        /// <returns></returns>
        public int SaveTicketForPerson(WXTicketForPerson per)
        {
            return _set.SaveTicketForPerson(per);
        }

        /// <summary>
        /// 获取已领取的某张券
        /// </summary>
        /// <param name="OpenID"></param>
        /// <param name="id">券id</param>
        /// <returns></returns>
        public WXTicketForPerson GetTicketByOpenID(string OpenID, int id)
        {
            return _set.GetTicketByOpenID(OpenID, id);
        }

        /// <summary>
        /// 获取已领取的券列表
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public List<WXTicketForPerson> GetTicketLsit(string OpenID)
        {
            return _set.GetTicketLsit(OpenID);
        }

        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXTicket GetTicket(int id)
        {
            return _set.GetTicket(id);
        }

        /// <summary>
        /// 保存优惠券信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int UpdateTicket(WXTicket t)
        {
            return _set.UpdateTicket(t);
        }

        /// <summary>
        /// 获取已领取的优惠券列表
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public List<WXTicket_EX> GetMyTicketList(string OpenID)
        {
            return _set.GetMyTicketList(OpenID);
        }

        /// <summary>
        /// 判断用户编号与用户名是否已存在
        /// </summary>
        /// <param name="MERCHANT_NO"></param>
        /// <param name="USER_NAME"></param>
        /// <returns></returns>
        public ORG_INFO GetWDByUserNo(string MERCHANT_NO, string USER_NAME)
        {
            return _set.GetWDByUserNo(MERCHANT_NO, USER_NAME);
        }

        public int SaveMD(ORG_INFO m)
        {
            if (m.ID == 0)
                return (int)_set.Insert(m);
            else
                return _set.Update(m);
        }

        /// <summary>
        /// 保存授权信息
        /// </summary>
        /// <param name="oa"></param>
        /// <returns></returns>
        public int SaveOAtuh(OAauth_Log oa)
        {
            return _set.SaveOAtuh(oa);
        }

        /// <summary>
        /// 获取授权信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        public OAauth_Log GetOA(string FromUserName)
        {
            return _set.GetOA(FromUserName);
        }

        public int SaveLog(WXLOG log)
        {
            return (int)_set.Insert(log);
        }

        /// <summary>
        /// 获取商户信息  供pos接口
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public ORG_INFO GetWD(string ToUserName)
        {
            return _set.GetWD(ToUserName);
        }

        /// <summary>
        /// 获取商户信息  供pos接口
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public ORG_INFO GetWD(int mid)
        {
            return _set.GetWD(mid);
        }


        /// <summary>
        /// 获取商户信息  用于验证重复授权
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public ORG_INFO GetWD(string ToUserName, int id)
        {
            return _set.GetWD(ToUserName, id);
        }

        /// <summary>
        /// 保存短信发送记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int SaveDX(WXDXLog log)
        {
            return (int)_set.Insert(log);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int SaveOA(OAauth_Log log)
        {
            return (int)_set.Update(log);
        }

        /// <summary>
        /// 获取价目表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public Pro GetPrice(int id)
        //{
        //    return _set.GetPrice(id);
        //}

        /// <summary>
        /// 获取价目表明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProDetail GetProDetail(int id)
        {
            return _set.GetProDetail(id);
        }

        public int GetCountPage()
        {
            return _set.GetCountPage();
        }

        /// <summary>
        /// 根据优惠券ID获取适用门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WD_STORE> GetTicketStore(int id)
        {
            return _set.GetTicketStore(id);
        }

        /// <summary>
        /// 根据订单号获取
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public WD_Evaluation GetEvaluation(string OrderID, string FromUserName)
        {
            return _set.GetEvaluation(OrderID, FromUserName);
        }

        /// <summary>
        /// 保存评价
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public int SaveEvaluation(WD_Evaluation e)
        {
            return (int)_set.Insert(e);
        }

        /// <summary>
        /// 获取我的评价
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        public List<WD_Evaluation_EX> GetMyEvaluationList(string FromUserName)
        {
            return _set.GetMyEvaluationList(FromUserName);
        }

        /// <summary>
        /// 获取临时二维码信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MJ_Qrcode_ls GetLSQrcode(int id)
        {
            return _set.GetLSQrcode(id);
        }

        /// <summary>
        /// 保存临时二维码
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int SaveLSQrcode(MJ_Qrcode_ls ls)
        {
            if (ls.ID == 0)
                return (int)_set.Insert(ls);
            else
                return _set.Update(ls);
        }

        /// <summary>
        /// 保存二维码扫描记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int SaveQrCodeLog(MJ_Qrcode_Log log)
        {
            return _set.SaveQrCodeLog(log);
        }

        /// <summary>
        /// 获取二维码扫描记录
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        public MJ_Qrcode_Log GetQrcodeLog(int qid)
        {
            return _set.GetQrcodeLog(qid);
        }

        /// <summary>
        /// 获取门店配置列表
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WD_STORE_SET_EX> GetStoreSetList(int Sid, PageView view)
        {
            return _set.GetStoreSetList(Sid, view);
        }

        public List<WD_STORE_SET> GetStoreSetList(int sid)
        {
            return _set.GetStoreSetList(sid);
        }

        /// <summary>
        /// 保存预约
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int SaveBooking(WD_BOOKING book)
        {
            if (book.ID == 0)
                return (int)_set.Insert(book);
            else
                return _set.Update(book);
        }

        /// <summary>
        /// 根据id获取配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_STORE_SET GetStoreSet(int id)
        {
            return _set.GetStoreSet(id);
        }

        /// <summary>
        /// 根据openid和门店ID获取配置信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <param name="StoreID"></param>
        /// <returns></returns>
        public WD_STORE_SET GetStoreSet(string FromUserName, int StoreID)
        {
            return _set.GetStoreSet(FromUserName, StoreID);
        }

        /// <summary>
        /// 保存门店配置信息
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public int SaveStoreSet(WD_STORE_SET set)
        {
            return _set.SaveStoreSet(set);
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteStoreSet(int id)
        {
            return _set.DeleteStoreSet(id);
        }

        /// <summary>
        /// 根据openid 获取预约记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public List<book_ex> GetBookListByOpenid(string openid)
        {
            return _set.GetBookListByOpenid(openid);
        }

        /// <summary>
        /// 根据posid及pos门店id获取本地保存的预约记录
        /// </summary>
        /// <param name="posid"></param>
        /// <param name="posstoreid"></param>
        /// <returns></returns>
        public WD_BOOKING GetWDBOOK(string posid, string posstoreid)
        {
            return _set.GetWDBOOK(posid, posstoreid);
        }

        /// <summary>
        /// 获取自己绑定的绑定列表  美街
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        public List<WD_STORE> GetSTORELISTBYSET(string FromUserName)
        {
            return _set.GetSTORELISTBYSET(FromUserName);
        }

        /// <summary>
        /// 保存地理位置
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public int SaveLocation(WD_Location l)
        {
            return _set.SaveLocation(l);
        }

        /// <summary>
        /// 获取最近一条地理位置信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public WD_Location GetLocation(string openid)
        {
            return _set.GetLocation(openid);
        }

        /// <summary>
        /// 获取最近的门店信息
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public WD_STORE_EX GetStoreByLocation(string x, string y)
        {
            return _set.GetStoreByLocation(x, y);
        }

        /// <summary>
        /// 保存签到记录
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int SaveSign(WD_SIGN s)
        {
            return _set.SaveSign(s);
        }

        /// <summary>
        /// 获取评价列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WD_Evaluation_EX> GetEvaluationList(string tousername, PageView view)
        {
            return _set.GetEvaluationList(tousername, view);
        }

        /// <summary>
        /// 获取评价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_Evaluation_EX GetEvaluation(int id)
        {
            return _set.GetEvaluation(id);
        }

        /// <summary>
        /// 删除所有门店信息
        /// </summary>
        /// <returns></returns>
        public int deleteWD_STORE()
        {
            return _set.deleteWD_STORE();
        }

        /// <summary>
        /// 获取绑定会员
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public CUST_INFO GetCust(string openid)
        {
            return _set.GetCust(openid);
        }

        /// <summary>
        /// 更新会员(用于demo操作)
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int UpdateCustInfoS(CUST_INFO entity)
        {
            return _set.UpdateCustInfoS(entity);
        }
        public string GetNameByBrand(string code)
        {
            return _set.GetNameByBrand(code);
        }
        /// <summary>
        /// 保存分享记录
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int SaveShare(WD_Share s)
        {
            return (int)_set.Insert(s);
        }

        /// <summary>
        /// 保存模板消息发送记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int SaveTemplateMessagelog(WD_TemplateMessageLog log)
        {
            return (int)_set.SaveTemplateMessagelog(log);
        }

        /// <summary>
        /// 获取最后一条有效签到记录
        /// </summary>
        /// <param name="Openid"></param>
        /// <returns></returns>
        public WD_SIGN GetSignByOpenid(string Openid)
        {
            return _set.GetSignByOpenid(Openid);
        }

        /// <summary>
        /// 报存抽奖记录
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int SaveScratch(Scratch s)
        {
            return _set.SaveScratch(s);
        }

        /// <summary>
        /// 获取自己的抽奖记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public Scratch GetScratch(string openid)
        {
            return _set.GetScratch(openid);
        }

        /// <summary>
        /// 获取指定时间之后，指定奖品的中间情况
        /// </summary>
        /// <param name="jp"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Scratch> GetScratchList(string jp, DateTime dt)
        {
            return _set.GetScratchList(jp, dt);
        }

        /// <summary>
        /// 获取custinfo 是 会员 ,但是粉丝表没有的数据
        /// </summary>
        /// <returns></returns>
        public List<CUST_INFO> GetNullFans()
        {
            return _set.GetNullFans();
        }

        /// <summary>
        /// 获取优惠券二维码
        /// </summary>
        /// <returns></returns>
        public COUPON_INFO GetDiscountCode(string ido)
        {
            return _set.GetDiscountCode(ido);
        }

        /// <summary>
        /// 查询是否有授权记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int GetISOA(string openid)
        {
            return _set.GetISOA(openid);
        }

        /// <summary>
        /// 查询当天是否有授权记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int GetOAByDay(string openid)
        {
            return _set.GetOAByDay(openid);
        }
    }
}
