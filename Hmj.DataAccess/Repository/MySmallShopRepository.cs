using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using System;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class MySmallShopRepository : BaseRepository
    {

        /// <summary>
        /// 根据商户ID获取门店列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WD_STORE> GetStoreByMid(int Mid, PageView view)
        {
            return base.PageGet<WD_STORE>(view, @"*", "WD_STORE", " and MERCHANT_ID=" + Mid, "id", "");
        }
        public string GetNickImg(string FromUserName)
        {
            string sql = "select top 1 Image from WXCUST_FANS where FromUserName = '" + FromUserName + "' order by ID desc";
            return Get<string>(sql, null);
        }
        public int GetGuanJiaCount(string FromUserName)
        {
            string sql = "select count(1) from Emp_Cust where Mobile in( select Mobile from Emp_Cust where FromUserName = '" + FromUserName + "')";
            return Get<int>(sql, null);
        }
        public int DeleteEmp_Cust(int Id)
        {
            string sql = "Delete Emp_Cust where Id = " + Id;
            return base.Excute(sql, null);
        }
        public EMPLOYEE GetGuanJiaName(string FromUserName)
        {
            string sql = "select top 1 *  from EMPLOYEE where USERID in ( select top 1 Mobile from Emp_Cust where FromUserName = '" + FromUserName + "')";
            return Get<EMPLOYEE>(sql, null);
        }
        public EMPLOYEE GetGuanjiaById(int Id)
        {
            string sql = "select  *  from EMPLOYEE where Id = " + Id;
            return Get<EMPLOYEE>(sql, null);
        }
        /// <summary>
        /// 获取价目表明细
        /// </summary>
        /// <param name="Mid"></param>
        /// <returns></returns>
        public List<ProDetail> GetPriceDetailListByPid(int Mid)
        {
            string sql = "select * from ProDetail where ProId=@mid";
            return Query<ProDetail>(sql, new { mid = Mid });
        }
        public List<WXCUST_MSG_HIS> GetWXCUST_MSG_HIS(string strwhere)
        {
            string sql = "select * from WXCUST_MSG_HIS where " + strwhere;
            return Query<WXCUST_MSG_HIS>(sql, null);
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        public List<MDSearch> GetCity(string Province, int Storetype)
        {
            string sql = "select City from MDSearch ";
            if (Province != "")
                sql += " where Province= @province";
            sql += " and X<>'0' and Name not like '%已撤柜%' and Storetype = '" + Storetype + "' and phone<>'' group by City";
            return Query<MDSearch>(sql, new { province = Province });
        }
        public List<MDSearch> GetAllCity(string Province)
        {
            string sql = "select City from MDSearch ";
            if (Province != "")
                sql += " where Province= @province group by City";
            return Query<MDSearch>(sql, new { province = Province });
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns></returns>
        public List<MDSearch> GetProvince(int StoreType)
        {
            string sql = "select Province from MDSearch where Phone<>'' and X<>'0' and Name not like '%已撤柜%' and StoreType=@storetype group by Province order by Province";
            return Query<MDSearch>(sql, new { storetype = StoreType });
        }
        //用于筛选区域活动的  不分门店
        public List<MDSearch> GetAllProvince()
        {
            string sql = "select Province from MDSearch  group by Province order by Province";
            return Query<MDSearch>(sql, null);
        }
        public List<Region> GetRegion()
        {
            string sql = "select * from Region";
            return Query<Region>(sql, null);
        }
        public List<City> GetCityByReg(string RegionCode)
        {
            string sql = "select * from City where RegionCode = " + RegionCode;
            return Query<City>(sql, null);
        }
        public City GetCityByCode(int CityCode)
        {
            string sql = "SELECT * FROM City WHERE CityCode=@CityCode";
            return Get<City>(sql, new { CityCode = CityCode });
        }
        public Callback GetCallbackByFromUserName(string FromUserName)
        {
            string sql = "select top 1 * from Callback where FromUserName=@FromUserName order by Id desc";
            return Get<Callback>(sql, new { FromUserName = FromUserName });
        }
        public Emp_Cust GetEmp_CustByFromUserName(string FromUserName)
        {
            string sql = "select top 1 * from Emp_Cust where FromUserName=@FromUserName order by Id desc";
            return Get<Emp_Cust>(sql, new { FromUserName = FromUserName });
        }
        public List<MDSearch> GetAll()
        {
            string sql = "select * from MDSearch";
            return Query<MDSearch>(sql, null);
        }
        public List<AreaManage> GetAllArea()
        {
            string sql = "select * from AreaManage where IsShow = '是'";
            return Query<AreaManage>(sql, null);
        }
        public List<AreaManage> GetAllAreaByName(string name)
        {
            string sql = "select * from AreaManage where name like '%" + name + "%' and IsShow = '是'";
            return Query<AreaManage>(sql, null);
        }
        public AreaManage GetArea(int id)
        {
            string sql = "SELECT * FROM AreaManage WHERE Id=@id";
            return Get<AreaManage>(sql, new { id = id });
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<MDSearch> GetModelList(string strWhere)
        {
            string sql = "select * from MDSearch ";
            if (strWhere.Trim() != "")
            {
                sql += " where " + strWhere;
            }
            return Query<MDSearch>(sql, null);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<AreaManage> GetMoArea(string strWhere)
        {
            string sql = "select * from AreaManage where areano in (select Belongsareano from mdsearch ";

            if (strWhere.Trim() != "")
            {
                sql += " where " + strWhere;
            }
            sql += ")";
            return Query<AreaManage>(sql, null);
        }
        /// <summary>
        /// 获取价目表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        //public PagedList<Pro> GetPriceListByMid(int Mid, PageView view)
        //{
        //    return base.PageGet<Pro>(view, @"*", "Pro", " and MID=" + Mid, "id", "");
        //}

        /// <summary>
        /// 获取价目表
        /// </summary>
        /// <param name="Mid"></param>
        /// <returns></returns>
        //public List<Pro> GetPriceListByMid(int Mid)
        //{
        //    string sql = "select * from Pro where mid=@mid";
        //    return Query<Pro>(sql, new { mid = Mid });
        //}

        /// <summary>
        /// 获取价目表明细
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<ProDetail> GetPriceDetailListByPid(int Mid, PageView view)
        {
            return base.PageGet<ProDetail>(view, @"*", "ProDetail", " and ProId=" + Mid, "id", "");
        }

        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <returns></returns>
        public List<WD_STORE> GetStoreByMid(int Mid)
        {
            return base.Query<WD_STORE>("select * from WD_STORE where MERCHANT_ID=@mid", new { mid = Mid });
        }

        /// <summary>
        /// 获取门店列表，是否适用优惠券
        /// </summary>
        /// <param name="Mid">商户id</param>
        /// <param name="Tid">优惠券id</param>
        /// <returns></returns>
        public List<WD_STORE_EX> GetStoreByMid(int Mid, int Tid)
        {
            string sql = "SELECT s.*,CASE WHEN t.ID IS not NULL THEN 1 ELSE 0 END IsTicket FROM dbo.WD_STORE s LEFT JOIN dbo.WX_Ticket_Store t ON s.ID=t.WD_StoreID AND t.WXTicketID=@tid WHERE s.MERCHANT_ID=@mid";
            return Query<WD_STORE_EX>(sql, new { tid = Tid, mid = Mid });
        }

        /// <summary>
        ///  根据商户ID获取预约列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WD_BOOKING> GetBookingListByMid(int Mid, PageView view)
        {
            return base.PageGet<WD_BOOKING>(view, @"b.ID,b.FromUserName,b.CUST_NAME,b.CUST_MOBILE,s.STORE_NAME,b.ARRIVE_TIME,o.headimgurl create_user,b.ServerType,o.Nickname remark", "WD_BOOKING b left join WD_STORE s on b.STORE_NAME=s.STORE_NO left join OAauth_Log o on b.FromUserName=o.FromUserName", " and s.MERCHANT_ID=" + Mid, "b.id desc", "");
        }


        /// <summary>
        /// 保存门店
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int SaveStore(WD_STORE s)
        {
            if (s.ID == 0)
                return (int)base.Insert(s);
            else
                return base.Update(s);
        }
        public int SaveCallback(Callback s)
        {
            if (s.Id == 0)
                return (int)base.Insert(s);
            else
                return base.Update(s);
        }
        public int SaveDelEmp_Cust(DelEmp_Cust s)
        {
            if (s.Id == 0)
                return (int)base.Insert(s);
            else
                return base.Update(s);
        }
        /// <summary>
        /// 获取门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_STORE GetStore(int id)
        {
            string sql = "SELECT * FROM dbo.WD_STORE WHERE ID=@id";
            return Get<WD_STORE>(sql, new { id = id });
        }

        /// <summary>
        /// 获取商户所有门店
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public List<WD_STORE> GetStoreList(string ToUserName)
        {
            string sql = "SELECT s.* FROM dbo.WD_STORE s LEFT JOIN dbo.ORG_INFO m ON s.MERCHANT_ID=m.ID WHERE ToUserName=@ToUserName";
            return base.Query<WD_STORE>(sql, new { ToUserName = ToUserName });
        }


        /// <summary>
        /// 获取商户所有门店
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public List<WD_STORE> GetStoreList(string ToUserName, string city)
        {
            string sql = "SELECT s.* FROM dbo.WD_STORE s LEFT JOIN dbo.ORG_INFO m ON s.MERCHANT_ID=m.ID WHERE ToUserName=@ToUserName and s.city=@city";
            return base.Query<WD_STORE>(sql, new { ToUserName = ToUserName, city = city });
        }

        /// <summary>
        /// 获取门店城市
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public List<string> GetCityList(string ToUserName)
        {
            string sql = "SELECT s.CITY FROM dbo.WD_STORE s LEFT JOIN dbo.ORG_INFO m ON s.MERCHANT_ID=m.ID WHERE ToUserName=@ToUserName GROUP BY s.CITY";
            return base.Query<string>(sql, new { ToUserName = ToUserName });
        }

        /// <summary>
        /// 根据ID获取门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_STORE GetStoreByID(int id)
        {
            string sql = "select * from WD_STORE where id=@id";
            return Get<WD_STORE>(sql, new { id = id });
        }

        /// <summary>
        /// 获取门店所有员工
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public List<WD_EMPLOYEE> GetEmployeeList(int ID)
        {
            string sql = "SELECT s.* FROM dbo.WD_EMPLOYEE s LEFT JOIN dbo.WD_STORE m ON s.STORE_ID=m.ID WHERE m.ID=@ID";
            return base.Query<WD_EMPLOYEE>(sql, new { ID = ID });
        }

        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_EMPLOYEE GetEmployeeByID(int id)
        {
            string sql = "select * from WD_EMPLOYEE where id=@id";
            return Get<WD_EMPLOYEE>(sql, new { id = id });
        }

        /// <summary>
        /// 获取员工的作品集
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WD_WORKS> GetWorksList(int id)
        {
            string sql = "SELECT * FROM dbo.WD_WORKS WHERE EMPLOYEE_ID=@id";
            return Query<WD_WORKS>(sql, new { id = id });
        }

        /// <summary>
        /// 获取门店作品集
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WD_WORKS_EX> GetStoreWorksList(int id)
        {
            string sql = "SELECT w.*,e.FILE_URL,e.NAME,e.POST_NAME FROM dbo.WD_WORKS w LEFT JOIN dbo.WD_EMPLOYEE e ON w.EMPLOYEE_ID=e.ID WHERE e.STORE_ID=@id";
            return Query<WD_WORKS_EX>(sql, new { id = id });
        }

        /// <summary>
        /// 获取员工作品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_WORKS GetWorksByID(int id)
        {
            string sql = "select * from WD_WORKS where id=@id";
            return Get<WD_WORKS>(sql, new { id = id });
        }

        /// <summary>
        /// 查询门店适用优惠券
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public List<WXTicket> GetTicketBySID(int sid)
        {
            string sql = "SELECT t.* FROM dbo.WXTicket t LEFT JOIN dbo.WX_Ticket_Store s ON t.ID=s.WXTicketID WHERE  s.WD_StoreID=@sid  AND EndDate>GETDATE() AND IsShow=1";
            return Query<WXTicket>(sql, new { sid = sid });
        }

        /// <summary>
        /// 查询门店适用优惠券 及会员是否已领取
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public List<WXTicket_EX> GetTicketBySID(int sid, string OpenID)
        {
            string sql = @"SELECT t.*,CASE WHEN p.ID IS NULL THEN 0 ELSE 1 END IsHav FROM dbo.WXTicket t LEFT JOIN dbo.WX_Ticket_Store s ON t.ID=s.WXTicketID
LEFT JOIN dbo.WXTicketForPerson p ON p.TicketID=t.ID AND FromUserName=@openid
 WHERE  s.WD_StoreID=@sid  AND EndDate>GETDATE() AND IsShow=1";
            return Query<WXTicket_EX>(sql, new { sid = sid, openid = OpenID });
        }

        /// <summary>
        /// 获取游客信息
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public WX_PageLog GetPerson(string OpenID)
        {
            string sql = "SELECT top 1 * FROM dbo.WX_PageLog WHERE Nickname<>'' AND FromUserName=@openid";
            return Get<WX_PageLog>(sql, new { openid = OpenID });
        }

        /// <summary>
        /// 领券/核销券
        /// </summary>
        /// <param name="per"></param>
        /// <returns></returns>
        public int SaveTicketForPerson(WXTicketForPerson per)
        {
            if (per.ID == 0)
                return (int)Insert(per);
            else
                return Update(per);
        }

        /// <summary>
        /// 获取已领取的某张券
        /// </summary>
        /// <param name="OpenID"></param>
        /// <param name="id">券id</param>
        /// <returns></returns>
        public WXTicketForPerson GetTicketByOpenID(string OpenID, int id)
        {
            string sql = "SELECT * FROM dbo.WXTicketForPerson WHERE FromUserName=@openid AND TicketID=@id";
            return Get<WXTicketForPerson>(sql, new { openid = OpenID, id = id });
        }

        /// <summary>
        /// 获取已领取的券列表
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public List<WXTicketForPerson> GetTicketLsit(string OpenID)
        {
            string sql = "SELECT * FROM dbo.WXTicketForPerson WHERE FromUserName=@openid";
            return Query<WXTicketForPerson>(sql, new { openid = OpenID });
        }

        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXTicket GetTicket(int id)
        {
            string sql = "select * from WXTicket where id=@id";
            return Get<WXTicket>(sql, new { id = id });
        }

        /// <summary>
        /// 保存优惠券信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int UpdateTicket(WXTicket t)
        {
            return Update(t);
        }

        /// <summary>
        /// 获取已领取的优惠券列表
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public List<WXTicket_EX> GetMyTicketList(string OpenID)
        {
            string sql = "SELECT t.*,'' MERCHANT_NAME FROM dbo.WXTicketForPerson p LEFT JOIN dbo.WXTicket t ON p.TicketID=t.ID LEFT JOIN dbo.ORG_INFO m ON m.ID=t.Merchants_ID WHERE p.FromUserName=@openid  AND m.ToUserName IS NOT null   AND t.EndDate>GETDATE() AND p.UseTime IS NULL";
            return Query<WXTicket_EX>(sql, new { openid = OpenID });
        }

        /// <summary>
        /// 判断用户编号与用户名是否已存在
        /// </summary>
        /// <param name="MERCHANT_NO"></param>
        /// <param name="USER_NAME"></param>
        /// <returns></returns>
        public ORG_INFO GetWDByUserNo(string MERCHANT_NO, string USER_NAME)
        {
            string sql = "SELECT * FROM dbo.ORG_INFO WHERE USER_NAME=@USER_NAME and MERCHANT_NO=@MERCHANT_NO";
            return Get<ORG_INFO>(sql, new { USER_NAME = USER_NAME, MERCHANT_NO = MERCHANT_NO });
        }

        /// <summary>
        /// 保存授权信息
        /// </summary>
        /// <param name="oa"></param>
        /// <returns></returns>
        public int SaveOAtuh(OAauth_Log oa)
        {
            return (int)Insert(oa);
        }

        /// <summary>
        /// 获取授权信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        public OAauth_Log GetOA(string FromUserName)
        {
            string sql = "SELECT * FROM dbo.OAauth_Log WHERE FromUserName=@openid";
            return Get<OAauth_Log>(sql, new { openid = FromUserName });
        }

        /// <summary>
        /// 获取商户信息  供pos接口
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public ORG_INFO GetWD(string ToUserName)
        {
            string sql = "select * from ORG_INFO where tousername=@tousername";
            return Get<ORG_INFO>(sql, new { tousername = ToUserName });
        }

        /// <summary>
        /// 获取商户信息  供pos接口
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public ORG_INFO GetWD(int mid)
        {
            string sql = "select * from ORG_INFO where  id=@tousername";
            return Get<ORG_INFO>(sql, new { tousername = mid });
        }

        /// <summary>
        /// 获取商户信息  用于验证重复授权
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public ORG_INFO GetWD(string ToUserName, int id)
        {
            string sql = "select * from ORG_INFO where tousername=@tousername and id<>@id";
            return Get<ORG_INFO>(sql, new { tousername = ToUserName, id = id });
        }


        /// <summary>
        /// 获取价目表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public Pro GetPrice(int id)
        //{
        //    string sql = "select * from pro where id=@id";
        //    return Get<Pro>(sql, new { id = id });
        //}

        /// <summary>
        /// 获取价目表明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProDetail GetProDetail(int id)
        {
            string sql = "select * from ProDetail where id=@id";
            return Get<ProDetail>(sql, new { id = id });
        }

        public int GetCountPage()
        {
            string sql = "SELECT COUNT(*) FROM dbo.WX_PageLog WHERE Page LIKE 'http://www.meijiewd.com/wechat/mysmallshop/test.aspx%'";
            return Get<int>(sql, null);
        }

        /// <summary>
        /// 根据优惠券ID获取适用门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WD_STORE> GetTicketStore(int id)
        {
            string sql = @"SELECT w.* FROM dbo.WXTicket t LEFT JOIN dbo.WX_Ticket_Store s ON t.id=s.WXTicketID
LEFT JOIN dbo.WD_STORE w ON s.WD_StoreID=w.ID 
WHERE t.ID=@id";
            return Query<WD_STORE>(sql, new { id = id });
        }

        /// <summary>
        /// 根据订单号获取
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public WD_Evaluation GetEvaluation(string OrderID, string FromUserName)
        {
            string sql = "select * from WD_Evaluation where orderid=@orderid and FromUserName=@openid";
            return Get<WD_Evaluation>(sql, new { orderid = OrderID, openid = FromUserName });
        }

        /// <summary>
        /// 获取我的评价
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        public List<WD_Evaluation_EX> GetMyEvaluationList(string FromUserName)
        {
            string sql = "SELECT e.*,isnull(s.STORE_NAME,m.nick_name) wdname FROM dbo.WD_Evaluation e LEFT JOIN dbo.ORG_INFO m ON e.ToUserName=m.ToUserName left join WD_STORE s on s.STORE_NO=e.storeno WHERE FromUserName=@openid";
            return Query<WD_Evaluation_EX>(sql, new { openid = FromUserName });
        }

        /// <summary>
        /// 获取临时二维码信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MJ_Qrcode_ls GetLSQrcode(int id)
        {
            string sql = "select * from MJ_Qrcode_ls where id=@id";
            return Get<MJ_Qrcode_ls>(sql, new { id = id });
        }

        /// <summary>
        /// 保存二维码扫描记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int SaveQrCodeLog(MJ_Qrcode_Log log)
        {
            return (int)Insert(log);
        }

        /// <summary>
        /// 获取二维码扫描记录
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        public MJ_Qrcode_Log GetQrcodeLog(int qid)
        {
            string sql = "SELECT  TOP 1 * FROM dbo.MJ_Qrcode_Log WHERE Qrcode_ls_id=@id ORDER BY id DESC";
            return Get<MJ_Qrcode_Log>(sql, new { id = qid });
        }

        /// <summary>
        /// 获取门店配置列表
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WD_STORE_SET_EX> GetStoreSetList(int Sid, PageView view)
        {
            return base.PageGet<WD_STORE_SET_EX>(view, @"s.*,f.NAME nc,f.IMAGE tx", "dbo.WD_STORE_SET s LEFT JOIN dbo.WXCUST_FANS f ON s.FromUserName=f.FROMUSERNAME", " and s.storeid=" + Sid, "s.id", "");
        }

        public List<WD_STORE_SET> GetStoreSetList(int sid)
        {
            return Query<WD_STORE_SET>("select * from WD_STORE_SET where storeid=@id", new { id = sid });
        }

        /// <summary>
        /// 根据id获取配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_STORE_SET GetStoreSet(int id)
        {
            string sql = "select * from WD_STORE_SET where id=@id";
            return Get<WD_STORE_SET>(sql, new { id = id });
        }

        /// <summary>
        /// 根据openid和门店ID获取配置信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <param name="StoreID"></param>
        /// <returns></returns>
        public WD_STORE_SET GetStoreSet(string FromUserName, int StoreID)
        {
            string sql = "select * from WD_STORE_SET where FromUserName=@openid and StoreID=@sid";
            return Get<WD_STORE_SET>(sql, new { openid = FromUserName, sid = StoreID });
        }

        /// <summary>
        /// 保存门店配置信息
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public int SaveStoreSet(WD_STORE_SET set)
        {
            if (set.ID == 0)
                return (int)Insert(set);
            else
                return Update(set);
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteStoreSet(int id)
        {
            string sql = "delete WD_STORE_SET where id=@id";
            return base.Excute(sql, new { id = id });
        }

        /// <summary>
        /// 根据openid 获取预约记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public List<book_ex> GetBookListByOpenid(string openid)
        {
            string sql = @"SELECT CUST_NAME cust_name,CUST_MOBILE cust_mobile,CONVERT(VARCHAR,ARRIVE_TIME,21) begin_time,CONVERT(VARCHAR,CREATE_DATE,21) end_time,
ServerType service_item,CONVERT(VARCHAR,BOOKING_NUM) booking_num,CONVERT(VARCHAR,STORE_ID) store_id,s.STORE_NAME store_name,s.TELEPHONE store_tel,
s.ADDRESS store_addr,b.STATUS state FROM dbo.WD_BOOKING b LEFT JOIN dbo.WD_STORE s ON b.STORE_ID=s.ID WHERE FromUserName=@openid";
            return Query<book_ex>(sql, new { openid = openid });
        }

        /// <summary>
        /// 根据posid及pos门店id获取本地保存的预约记录
        /// </summary>
        /// <param name="posid"></param>
        /// <param name="posstoreid"></param>
        /// <returns></returns>
        public WD_BOOKING GetWDBOOK(string posid, string posstoreid)
        {
            string sql = "SELECT * FROM dbo.WD_BOOKING WHERE PosID=@posid AND STORE_NAME=@sid";
            return Get<WD_BOOKING>(sql, new { posid = posid, sid = posstoreid });
        }

        /// <summary>
        /// 获取自己绑定的绑定列表  美街
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        public List<WD_STORE> GetSTORELISTBYSET(string FromUserName)
        {
            string sql = "SELECT w.* FROM dbo.WD_STORE_SET s LEFT JOIN dbo.WD_STORE w ON s.StoreID=w.ID WHERE FromUserName=@openid";
            return Query<WD_STORE>(sql, new { openid = FromUserName });
        }

        /// <summary>
        /// 保存地理位置
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public int SaveLocation(WD_Location l)
        {
            return (int)Insert(l);
        }

        /// <summary>
        /// 获取最近一条地理位置信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public WD_Location GetLocation(string openid)
        {
            string sql = "select top 1 * from wd_location where fromusername=@openid order by createdate desc";
            return Get<WD_Location>(sql, new { openid = openid });
        }

        /// <summary>
        /// 获取最近的门店信息
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public WD_STORE_EX GetStoreByLocation(string x, string y)
        {
            string sql = "select top 1 dbo.jl(lat,lng,@y,@x) jl,* from wd_store  order by dbo.jl(lat,lng,@y,@x) ";
            return Get<WD_STORE_EX>(sql, new { x = x, y = y });
        }

        /// <summary>
        /// 保存签到记录
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int SaveSign(WD_SIGN s)
        {
            return (int)Insert(s);
        }

        /// <summary>
        /// 获取评价列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WD_Evaluation_EX> GetEvaluationList(string tousername, PageView view)
        {
            return base.PageGet<WD_Evaluation_EX>(view, @"dbo.WD_Evaluation.*,l.headimgurl,l.Nickname,isnull(s.STORE_NAME,'美丽田园') WDName", "WD_Evaluation  LEFT JOIN dbo.OAauth_Log l ON dbo.WD_Evaluation.FromUserName = l.FromUserName  left join WD_STORE s on s.STORE_NO=WD_Evaluation.storeno", " ", "WD_Evaluation.id", "");
        }

        /// <summary>
        /// 获取评价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WD_Evaluation_EX GetEvaluation(int id)
        {
            string sql = "SELECT dbo.WD_Evaluation.*,l.headimgurl,l.Nickname,isnull(s.STORE_NAME,'美丽田园') WDName FROM dbo.WD_Evaluation  LEFT JOIN dbo.OAauth_Log l ON dbo.WD_Evaluation.FromUserName = l.FromUserName left join WD_STORE s on s.STORE_NO=WD_Evaluation.storeno WHERE WD_Evaluation.ID=@id";
            return Get<WD_Evaluation_EX>(sql, new { id = id });
        }

        /// <summary>
        /// 删除所有门店信息
        /// </summary>
        /// <returns></returns>
        public int deleteWD_STORE()
        {
            string sql = "delete WD_STORE";
            return base.Excute(sql, null);
        }

        /// <summary>
        /// 获取绑定会员
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public CUST_INFO GetCust(string openid)
        {
            string sql = "select * from CUST_INFO where FROM_USER_NAME=@openid";
            return Get<CUST_INFO>(sql, new { openid = openid });
        }

        public int UpdateCustInfoS(CUST_INFO entity)
        {
            string sql = "update CUST_INFO set CustLevel='"+entity.CustLevel+"',CustCity='"+entity.CustCity+"' where ID="+entity.ID;
            return base.Excute(sql, null);
        }

        public string GetNameByBrand(string code)
        {
            string sql = "select name from Brand where code = @code";
            string ret = Get<string>(sql, new { code = code });
            if (ret == null)
            {
                ret = code;
            }
            return ret;
        }

        /// <summary>
        /// 保存模板消息发送记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int SaveTemplateMessagelog(WD_TemplateMessageLog log)
        {
            return (int)Insert(log);
        }

        /// <summary>
        /// 获取最后一条有效签到记录
        /// </summary>
        /// <param name="Openid"></param>
        /// <returns></returns>
        public WD_SIGN GetSignByOpenid(string Openid)
        {
            string sql = "select  top 1 * from WD_SIGN where FromUserName=@openid and JL<=800 order by ID desc";
            return Get<WD_SIGN>(sql, new { openid = Openid });
        }

        /// <summary>
        /// 报存抽奖记录
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int SaveScratch(Scratch s)
        {
            return (int)Insert(s);
        }

        /// <summary>
        /// 获取自己的抽奖记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public Scratch GetScratch(string openid)
        {
            string sql = "SELECT * FROM dbo.Scratch WHERE FromUserName=@openid";
            return Get<Scratch>(sql, new { openid = openid });
        }

        /// <summary>
        /// 获取指定时间之后，指定奖品的中间情况
        /// </summary>
        /// <param name="jp"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Scratch> GetScratchList(string jp, DateTime dt)
        {
            string sql = "SELECT * FROM dbo.Scratch WHERE JP=@jp AND CreateDate>@dt";
            return Query<Scratch>(sql, new { jp = jp, dt = dt.ToString("yyyy-MM-dd") });
        }

        public List<CUST_INFO> GetNullFans()
        {
            string sql =@"
select * from CUST_INFO a 
left join WXCUST_FANS b on a.FROM_USER_NAME=b.FROMUSERNAME 
where b.ID is null ";
            return Query<CUST_INFO>(sql, null);
        }

        public COUPON_INFO GetDiscountCode(string ido)
        {
            string sql = "SELECT QR_CODE FROM COUPON_INFO WHERE COUPON_NO=@COUPON_NO";
            return Get<COUPON_INFO>(sql, new { COUPON_NO = ido });
        }

        /// <summary>
        /// 查询是否有授权记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int GetISOA(string openid)
        {
            return Get<int>("SELECT COUNT(1) FROM dbo.OAauth_Log WHERE FromUserName=@openid", new { openid = openid });
        }

        /// <summary>
        /// 查询当天是否有授权记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int GetOAByDay(string openid)
        {
            return Get<int>("SELECT COUNT(1) FROM dbo.OAauth_Log WHERE FromUserName=@openid and DATEDIFF(dd,GETDATE(),CreateDate)=0", new { openid = openid });
        }


    }
}
