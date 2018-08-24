using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;
using System.Text;

namespace Hmj.DataAccess.Repository
{
    public class SystemSetRepository : BaseRepository
    {

        /// <summary>
        /// 获取所有权限菜单
        /// </summary>
        /// <returns></returns>
        public List<SYS_RIGHT> GetAllRight()
        {
            string sql = "SELECT * FROM dbo.SYS_RIGHT  ORDER BY CONVERT(int,RIGHT_DSC)";
            return base.Query<SYS_RIGHT>(sql, null);
        }
        public AreaManage GetAreaManage(int? id)
        {
            string sql = "select * from AreaManage where UserId=@id";
            return Get<AreaManage>(sql, new { id = id });
        }
        public int SaveAreaManage(AreaManage Area)
        {
            if (Area.Id == 0)
            {
                return (int)Insert(Area);
            }
            else
            {
                return Update(Area);
            }
        }
        public int SaveStore(Store_EX md)
        {
            if (md.ID == 0)
            {
                return (int)Insert(md);
            }
            else
            {
                return Update(md);
            }
        }
        public int SaveUser_Info(USER_INFO user)
        {
            if (user.ID == 0)
            {
                return (int)Insert(user);
            }
            else
            {
                return Update(user);
            }
        }


        /// <summary>
        /// 根据用户ID获取权限菜单
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<SYS_RIGHT> GetAllRight(int UserID)
        {
            string sql = string.Format(@"SELECT * FROM (SELECT * FROM (SELECT e.* FROM dbo.USER_INFO a LEFT JOIN dbo.ORG_EMPLOYEE b ON a.EMPLOYEE_ID=b.ID
                           LEFT JOIN SYS_ROLE C ON A.ROLE_ID=C.ROLE_ID
                           LEFT JOIN dbo.SYS_ROLE_RIGHT D ON c.ROLE_ID=d.ROLE_ID
                           LEFT JOIN dbo.SYS_RIGHT E ON d.RIGHT_ID=e.RIGHT_ID
                           WHERE e.IS_MENU=1 AND a.ID= {0} )T
                           UNION ALL
                           SELECT * FROM dbo.SYS_RIGHT WHERE RIGHT_ID IN (
                           SELECT PARENT_ID FROM dbo.USER_INFO a LEFT JOIN dbo.ORG_EMPLOYEE b ON a.EMPLOYEE_ID=b.ID
                           LEFT JOIN SYS_ROLE C ON A.ROLE_ID=C.ROLE_ID
                           LEFT JOIN dbo.SYS_ROLE_RIGHT D ON c.ROLE_ID=d.ROLE_ID
                           LEFT JOIN dbo.SYS_RIGHT E ON d.RIGHT_ID=e.RIGHT_ID
                           WHERE e.IS_MENU=1 AND a.ID= {0} 
                           )
                           UNION ALL
                           SELECT * FROM dbo.SYS_RIGHT WHERE RIGHT_ID IN (
                           SELECT PARENT_ID FROM dbo.SYS_RIGHT WHERE RIGHT_ID IN (
                           SELECT PARENT_ID FROM dbo.USER_INFO a LEFT JOIN dbo.ORG_EMPLOYEE b ON a.EMPLOYEE_ID=b.ID
                           LEFT JOIN SYS_ROLE C ON A.ROLE_ID=C.ROLE_ID
                           LEFT JOIN dbo.SYS_ROLE_RIGHT D ON c.ROLE_ID=d.ROLE_ID
                           LEFT JOIN dbo.SYS_RIGHT E ON d.RIGHT_ID=e.RIGHT_ID
                           WHERE e.IS_MENU=1 AND a.ID= {0} 
                           )) )B GROUP BY B.RIGHT_ID,B.RIGHT_NAME,B.RIGHT_DSC,B.IS_RIGHT,B.IS_MENU,B.MENU_CODE,
                           B.URL_LINK_TO,B.TARGET,B.PARENT_ID,B.MENU_ICON ORDER BY CONVERT(int,RIGHT_DSC),B.RIGHT_ID
                           ", UserID);
            return base.Query<SYS_RIGHT>(sql, null);
        }

        /// <summary>
        /// 判断唯一编码是否重复
        /// </summary>
        /// <param name="code"></param>
        /// <param name="rightid"></param>
        /// <returns></returns>
        public List<SYS_RIGHT> GetRightByCode(string code, int? rightid)
        {
            string sql = string.Format("SELECT * FROM dbo.SYS_RIGHT WHERE MENU_CODE='{0}' AND RIGHT_ID<>{1}", code, rightid);
            return Query<SYS_RIGHT>(sql, null);
        }

        public PagedList<SYS_RIGHT> GetRightListByPid(int pid, PageView view)
        {
            string cols = @" A.* ";
            return base.PageGet<SYS_RIGHT>(view, cols,
               "[SYS_RIGHT] A "
               , " AND A.PARENT_ID='" + pid + "'"
               , "CONVERT(int,a.RIGHT_DSC)", "");
        }

        /// <summary>
        /// 根据ID获取权限菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SYS_RIGHT_EX GetRightByID(int id)
        {
            string sql = "SELECT s.*,ISNULL(r.RIGHT_NAME,'权限菜单') PARENT_NAME FROM dbo.SYS_RIGHT s LEFT JOIN dbo.SYS_RIGHT r ON s.PARENT_ID=r.RIGHT_ID  WHERE s.RIGHT_ID=" + id;
            return base.Get<SYS_RIGHT_EX>(sql, null);
        }

        /// <summary>
        /// 删除指定权限菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteMenu(int id)
        {
            string sql = "DELETE dbo.SYS_ROLE_right WHERE RIGHT_ID=" + id;
            base.Excute(sql, null);
            sql = "delete SYS_RIGHT where RIGHT_ID=" + id;
            return base.Excute(sql, null);
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
            BegDate = BegDate.Substring(0, 4) + "-" + BegDate.Substring(4, 2) + "-" + BegDate.Substring(6, 2);
            EndDate = EndDate.Substring(0, 4) + "-" + EndDate.Substring(4, 2) + "-" + EndDate.Substring(6, 2);
            string sql = string.Format(@"SELECT SUM(YYRS) XZYY,SUM(XZHY) XZHY,SUM(YYSR) YYSR,SUM(HYKSR) HYKSR FROM  (
SELECT COUNT(1) YYRS,0 XZHY,0 YYSR,0 HYKSR FROM dbo.CUST_BOOKING
WHERE STORE_ID={0} AND CREATE_DATE>='{1}' AND CREATE_DATE<='{2}'-------预约人数
UNION ALL
SELECT 0,COUNT(1) [COUNT],0,0 FROM CUST_INFO 
WHERE STORE_ID={0} and CARD_OPEN_TIME>='{1}' AND CREATE_DATE<='{2}'  ----------新增会员
UNION ALL
SELECT 0,0,CONVERT(INT ,ISNULL(SUM(OP.PAY_AMT),0)) PAY_AMT,0 FROM ORDER_HEAD OH
LEFT JOIN ORDER_PAYMENT OP ON OH.ID=OP.ORDER_ID
WHERE OH.STORE_ID={0} AND OH.PAY_STATUS=1 AND OH.TRANS_DATE>='{1}' AND OH.TRANS_DATE<='{2}' --营业收入
UNION ALL
SELECT 0,0,0,CONVERT(INT ,iSNULL(SUM(OP.PAY_AMT),0)) PAY_AMT FROM ORDER_HEAD OH
LEFT JOIN ORDER_PAYMENT OP ON OH.ID=OP.ORDER_ID
WHERE OH.STORE_ID={0} AND OH.PAY_STATUS=1 AND OP.PROD_TYPE=1 AND OH.TRANS_DATE>='{1}' AND OH.TRANS_DATE<='{2}' --会员卡收入
)T", storeID, BegDate, EndDate);
            return base.Get<Index_Report_EX>(sql, null);
        }

        /// <summary>
        /// 获取仪表盘数据
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="BegDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public Index_Report_EX GetWXIndexReport(int Merchants_ID, string BegDate, string EndDate)
        {
            BegDate = BegDate.Substring(0, 4) + "-" + BegDate.Substring(4, 2) + "-" + BegDate.Substring(6, 2);
            EndDate = EndDate.Substring(0, 4) + "-" + EndDate.Substring(4, 2) + "-" + EndDate.Substring(6, 2);
            string sql = string.Format(@"select SUM(c1) c1,SUM(c2) c2,SUM(c3) c3,SUM(c4) c4 from (
select COUNT(1) c1,0 c2,0 c3,0 c4 from WXCUST_MSG_HIS c left join ORG_INFO s on c.TOUSERNAME=s.ToUserName where s.ID={0} and c.CREATE_DATE>='{1}' and c.CREATE_DATE<='{2}' and MSGTYPE='text'  --未读消息
union all
select 0 c1,COUNT(1) c2,0 c3,0 c4 from WXCUST_FANS c left join  ORG_INFO s on c.TOUSERNAME=s.ToUserName where s.ID={0} and c.CREATE_DATE>='{1}' and c.CREATE_DATE<='{2}' and c.STATUS=1 --新增粉丝
union all
select 0 c1,0 c2,COUNT(1) c3,0 c4 from WXCUST_FANS c left join  ORG_INFO s on c.TOUSERNAME=s.ToUserName where s.ID={0} and c.CANCEL_DATE>='{1}' and c.CANCEL_DATE<='{2}' and c.STATUS=0  --跑路粉丝
union all
select 0 c1,0 c2,0 c3,COUNT(1) c4 from WXCUST_FANS c left join  ORG_INFO s on c.TOUSERNAME=s.ToUserName where s.ID={0} and C.STATUS=1  --所有粉丝
)t", Merchants_ID, BegDate, EndDate);
            return base.Get<Index_Report_EX>(sql, null);
        }



        //全局检索会员信息
        public int SearchMemberInfo(string q, int orgid)
        {
            string sql = "SELECT ID FROM CUST_INFO WHERE (CARD_NO=@Q OR MOBILE=@Q) and org_id=@orgid";
            return base.Get<int>(sql, new { Q = q, orgid = orgid });
        }

        /// <summary>
        /// 根据电话号码查询会员信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public CUST_INFO SearchMemberInfoByPhone(string phone, int orgid)
        {
            string sql = @"SELECT c.ID,c.CUST_NO,c.NAME,c.CARD_NO,c.IMAGE_ID,c.GENDER,c.BIRTHDAY,c.MOBILE,
f.FILE_URL ADDRESS,c.EMAIL,c.QQ,c.FROM_USER_NAME,ISNULL(CONVERT(VARCHAR,d.BALANCE),'无卡') ZIPCODE FROM dbo.CUST_INFO c LEFT JOIN dbo.FILES f ON c.IMAGE_ID=f.ID
LEFT JOIN dbo.CUST_CARD d ON c.ID=d.CUST_ID  AND CARD_TYPE=1 WHERE 
  MOBILE=@Q and org_id=@orgid";
            return base.Get<CUST_INFO>(sql, new { Q = phone, orgid = orgid });
        }
        public int GetTopId()
        {
            string sql = @"select top 1 Id from AreaManage order by Id desc";
            return base.Get<int>(sql, null);
        }

        //未约进列表

        public List<CUST_INFO_EX> QueryBookingList(string storeId)
        {
            //            string sql = @"SELECT top 10 CI.ID,CI.NAME,ci.MOBILE,CONVERT(varchar(100),cb.CREATE_DATE,8)ONDATE 
            //                        FROM CUST_INFO CI JOIN CUST_BOOKING CB ON CI.ID=CB.CUST_ID
            //                        where ci.STORE_ID=@STORE_ID and cb.STATUS=7
            //                        order by CONVERT(varchar(100),cb.CREATE_DATE,8) desc";
            string sql = @" SELECT CB.ID,CB.CUST_NAME NAME,CB.CUST_MOBILE MOBILE
                        ,CONVERT(varchar(100),cb.CREATE_DATE,8)ONDATE 
                         FROM CUST_BOOKING CB 
                        WHERE
                        CB.STORE_ID=@STORE_ID 
                        AND CB.STATUS=0                        
                        AND
                        CB.IS_WX_CONFIRM=0 
                        AND CB.SOURCE=2
                        
                         order by CONVERT(varchar(100),cb.CREATE_DATE,8) desc";
            return base.Query<CUST_INFO_EX>(sql, new { STORE_ID = storeId });

        }

        //未约进 未付款订单 数
        public List<JsonSMsg> QueryOrderBookCount(string storeId)
        {
            string sql = @"  SELECT COUNT(CB.ID)DATA
                         FROM CUST_BOOKING CB 
                        WHERE
                        CB.STORE_ID=@STORE_ID 
                             AND CB.STATUS=0                        
                        AND
                        CB.IS_WX_CONFIRM=0
                        AND CB.SOURCE=2
 union all    
                        select COUNT(oh.ID) from ORDER_HEAD oh
                    left join CUST_INFO ci on oh.CUST_ID=ci.ID
                    where oh.PAY_STATUS=0 AND oh.ORDER_STATUS=0 and oh.STORE_ID=@STORE_ID
 and CONVERT(char(10),oh.TRANS_DATE,120)=CONVERT(char(10),GETDATE(),120) ";
            return base.Query<JsonSMsg>(sql, new { STORE_ID = storeId });
        }



        /// <summary>
        /// 获取微信消息
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="replyType"></param>
        /// <returns></returns>
        public Information_EX GetInformationModel(string Key, int replyType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,CreateTime,MsgType,Content,MsgId,Location_X,Location_Y,Scale,Label,PicUrl,replyType,fulltextUrl,Title,Description,KeyWords from wxinformation   ");
            strSql.Append(string.Format(" where  keywords LIKE '%{0}%' AND replytype={1}  ", Key, replyType));
            return base.Get<Information_EX>(strSql.ToString(), null);
        }
        public PagedList<EMPLOYEE> QueryGetEmp(EmpSearch search, PageView view)
        {
            string iszc = "";
            if (search.Name != null && search.Name != "")
                iszc += " and (w.name like '%" + search.Name + "%' or w.mobile like '%" + search.Name + "%')";

            return base.PageGet<EMPLOYEE>(view, @"w.*", "EMPLOYEE w", iszc, "w.id desc", "");
        }

        public List<EMPLOYEE> QueryAllEmp()
        {
            string sql = @"select * from EMPLOYEE order by id desc";
            return base.Query<EMPLOYEE>(sql, null);

        }
        public int GetMaxEwmId()
        {
            string sql = @"select top 1 EwmId from EMPLOYEE  where EwmId is not null order by id desc";
            return (int)base.Query<EMPLOYEE>(sql, null)[0].EwmId;
        }

        public List<EMPLOYEE> GetNullEwmIdList()
        {
            string sql = @"select * from EMPLOYEE where EwmId is null";
            return base.Query<EMPLOYEE>(sql, null);
        }
        public List<EMPLOYEE_EX> QueryEmplReport(EmpSearch search)
        {
            string condition = "";
            string timeLimit = "";
            if (search.StoreName != null && search.StoreName != "")
                condition += " and (e.storename like '%" + search.StoreName + "%') ";
            if (search.FirstTime != null && search.FirstTime != "" && search.EndTime != null && search.EndTime != "")
                timeLimit += " and (ec.createtime between '" + search.FirstTime + " 00:00:00.000' and '" + search.EndTime + " 23:59:59.999') ";
            string sql = @" select e.id,e.userid,e.name,e.mobile,e.ewmid,e.ewmurl,e.areaname,e.storename,count(ec.empid) FansNum from employee e
                        left join Emp_Cust ec on e.ewmid = ec.empid " + timeLimit +
                        "where 1=1 " + condition + @" 
                        GROUP BY e.id,e.userid,e.name,e.mobile,e.ewmid,e.ewmurl,e.areaname,e.storename
                        order by e.id desc";
            return base.Query<EMPLOYEE_EX>(sql, null);
        }
        /// <summary>
        /// 获取二维码列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<QMActivity_EX> QueryGetQm(string name, PageView view)
        {
            string iszc = "";
            if (!string.IsNullOrEmpty(name))
            {
                iszc = "  and q.MdName like '%" + name + "%'";
            }
            return base.PageGet<QMActivity_EX>(view, @"q.*, isnull(s.AddCount,0) as AddCount, isnull(x.DeleteCount,0) as DeleteCount",
                " QMActivity  q	left join	( 	select COUNT( DISTINCT FROMUSERNAME) as AddCount, b.EVENTKEY  from QMActivity a 	left join WXCUST_MSG_HIS b on a.MdSel=b.EVENTKEY  group by b.EVENTKEY 	) s on q.MdSel=s.EVENTKEY 	left join  	(    select COUNT( DISTINCT b.FROMUSERNAME) as DeleteCount,b.EVENTKEY  from QMActivity a 	left join WXCUST_MSG_HIS b on a.MdSel=b.EVENTKEY 	left join WXCUST_FANS c on b.FROMUSERNAME=c.FROMUSERNAME 	where c.STATUS=0 	  group by b.EVENTKEY 	  ) x on q.MdSel= x.EVENTKEY ",
                 iszc,
                " q.CreateTime desc", "");
        }
        public PagedList<AreaManage_EX> QueryGetArea(PageView view)
        {
            string iszc = "";
            return base.PageGet<AreaManage_EX>(view, @"a.*,u.USER_NO,u.USER_PASS", "AreaManage a left join USER_INFO u on u.ID = a.UserId", iszc, "a.id desc", "");
        }
        public PagedList<Store_EX> QueryGetStore(PageView view)
        {
            string iszc = "";
            return base.PageGet<Store_EX>(view, @"m.*,a.Name as BelongsAreaName", "MDSearch m left join AreaManage a on m.BelongsAreaNo = a.AreaNo", iszc, "a.id desc", "");
        }
        /// <summary>
        /// 获取微信消息集合
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public List<Information_EX> GetModelList(string sqlWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select i.*,d.Title,d.Describe Description,d.pic PicUrl,d.url FulltextUrl,
d.id did,d.IsURL from wxinformation i left join dbo.wxGraphicList l on i.Graphic_ID=l.id
left join dbo.wxGraphicDetail d on d.list_id=l.id
left join ORG_INFO m on i.Merchants_ID=m.ID ");
            if (strSql.ToString().Trim() != "")
            {
                strSql.Append(" where " + sqlWhere.ToString());
            }
            return base.Query<Information_EX>(strSql.ToString(), null);
        }

        /// <summary>
        /// 添加微信日志
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public int AddLog(WXLOG l)
        {
            return (int)base.Insert(l);
        }

        /// <summary>
        /// 添加微信消息记录
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int AddCUST_MSG_HIS(WXCUST_MSG_HIS c)
        {
            return (int)base.Insert(c);
        }
        public int AddEmp_Cust(Emp_Cust e)
        {
            return (int)base.Insert(e);
        }
        /// <summary>
        /// 获取商户列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<ORG_INFO> QueryMerchantsList(RoleSearch search, PageView view)
        {
            //string sql = "select * from Merchants";
            return base.PageGet<ORG_INFO>(view, "*", "ORG_INFO", " and id=" + search.ORG_ID, "id", "");
        }

        /// <summary>
        /// 获取商户列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public List<ORG_INFO> GetMerchantsList()
        {
            return base.Query<ORG_INFO>("select * from ORG_INFO", null);
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchants(int id)
        {
            string sql = "select * from ORG_INFO where id=" + id;
            return base.Get<ORG_INFO>(sql, null);
        }

        public ORG_INFO GetMerchants(string toUserName)
        {
            string sql = "select * from ORG_INFO where tousername=@ToUserName";
            return base.Get<ORG_INFO>(sql, new { ToUserName = toUserName });
        }
        public Emp_Cust GetEmp_Cust(string FromUserName)
        {
            return base.Get<Emp_Cust>("select * from Emp_Cust where fromusername= '" + FromUserName + "'", null);
        }

        public GROUP_EMPLOYER_EX GetStoreNameByFromUserName(string FromUserName)
        {
            string sql = @"select b.NAME,b.MOBILE,d.NAME as STORENAME from Emp_Cust a
left join EMPLOYEE b on b.MOBILE=a.Mobile
left join REL_EMP_GROUP c on c.EMP_ID=b.ID
left join GROUP_INFO d on d.ID=c.GROUP_ID
where a.FromUserName=@FromUserName";
            return base.Get<GROUP_EMPLOYER_EX>(sql, new { FromUserName = FromUserName });
        }

        public Emp_Cust GetEmp_CustById(int Id)
        {
            return base.Get<Emp_Cust>("select * from Emp_Cust where Id = " + Id, null);
        }
        /// <summary>
        /// 保存商户
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public int SaveMerchants(ORG_INFO sys)
        {
            if (sys.ID == 0)
                return (int)base.Insert(sys);
            else
                return base.Update(sys);
        }

        /// <summary>
        /// 保存图文列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int SaveGraphicList(WXGraphicList list)
        {
            if (list.ID == 0)
                return (int)base.Insert(list);
            else
                return base.Update(list);
        }

        /// <summary>
        /// 保存图文明细
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns></returns>
        public int SaveGraphicDetail(WXGraphicDetail Detail)
        {
            if (Detail.ID == 0)
                return (int)base.Insert(Detail);
            else
                return base.Update(Detail);
        }

        /// <summary>
        /// 获取图文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXGraphicList GetGraphicList(int id)
        {
            return Get<WXGraphicList>("select * from wxGraphicList where ID=" + id, null);
        }

        /// <summary>
        /// 获取图文明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Graphic_Detail_EX GetGraphicDetail(int id)
        {
            return Get<Graphic_Detail_EX>("select d.*,l.title Name from wxGraphicDetail d left join wxgraphiclist l on d.list_id=l.id where d.ID=" + id, null);
        }
        /// <summary>
        /// 保存二维码
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public int SaveQMActivity(QMActivity sys)
        {
            if (sys.Id == 0)
                return (int)base.Insert(sys);
            else
                return base.Update(sys);
        }
        public int SaveEMPLOYEE(EMPLOYEE sys)
        {

            if (sys.ID == 0)
                return (int)base.Insert(sys);
            else
                return base.Update(sys);
        }

        public int GetEwmId()
        {
            string sql = string.Format("select ISNULL(MAX(EwmId), 0) from EMPLOYEE");
            return base.Get<int>(sql, null);
        }
        /// <summary>
        /// 获取图文明细列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<Graphic_Detail_EX> QueryGetGraphicDetail(GraphicSearch search, PageView view)
        {
            return base.PageGet<Graphic_Detail_EX>(view, "d.*,l.Title Name", "wxGraphicDetail d left join wxGraphicList l on d.List_ID=l.ID", " and l.Merchants_ID=" + search.Merchants_ID, "d.id", "");
        }

        /// <summary>
        /// 获取粉丝信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        public WXCUST_FANS GetFansByFromUserName(string FromUserName)
        {
            return base.Get<WXCUST_FANS>("select * from WXCUST_FANS where fromusername='" + FromUserName + "'", null);
        }

        /// <summary>
        /// 根据商户微信号获取商户信息
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchantsByToUserName(string ToUserName)
        {
            string sql = "select  *  from ORG_INFO where ToUserName=@tousername";
            return base.Get<ORG_INFO>(sql, new { tousername = ToUserName });
        }

        /// <summary>
        /// 获取自动回复消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Information_EX GetInformation(int? id)
        {
            string sql = "select * from wxInformation where id=" + id;
            return base.Get<Information_EX>(sql, null);
        }

        /// <summary>
        /// 保存自动回复消息
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int SaveInformation(WXInformation i)
        {
            if (i.ID == 0)
                return (int)Insert(i);
            else
                return Update(i);
        }

        /// <summary>
        /// 获取自动回复消息列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<Information_EX> QueryInformationList(RoleSearch search, PageView view)
        {
            string sqlwhere = " and Merchants_ID=" + search.ORG_ID;
            if (search.replyType != 0)
                sqlwhere += " and replyType=" + search.replyType;
            return base.PageGet<Information_EX>(view, @"ID,CreateTime,case MsgType when 'text' then '文本' when 'image' then '图片' else '图文' end MsgType,Content,
case replyType when 1 then '关键字回复' when 2 then '被关注回复' else '自动回复' end XXType,
case replyType when 1 then ( case matchingType when 0 then '模糊匹配' else '全部匹配' end)  else '无' end PPType, case replyType when 1 then KeyWords else '无' end KeyWords", "wxInformation", sqlwhere, "id", "");
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
            string sql = string.Format("select COUNT(1) from wxinformation where replyType={1} and ID<>{0} and Merchants_ID={2}", id, replyType, Merchants_ID);
            return base.Get<int>(sql, null);
        }
        /// <summary>
        /// 获取微信菜单消息集合
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public List<CustomMenu_EX> GetCustomMenuModelList(string sqlWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select c.*,d.Title,d.Describe Description,d.pic PicUrl,d.url FulltextUrl,
d.id did,case c.type when 0 then 'text' when 1 then 'news' end MsgType,d.IsURL from wxCustomMenu c left join wxGraphicList l on c.Graphic_ID=l.ID
left join wxGraphicDetail d on l.ID=d.List_ID ");
            if (strSql.ToString().Trim() != "")
            {
                strSql.Append(" where " + sqlWhere.ToString());
            }
            return base.Query<CustomMenu_EX>(strSql.ToString(), null);
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<CUST_FANS_EX> QueryGetFansDetail(FansSearch search, PageView view)
        {
            string iszc = "";
            if (search.ISZC == "已注册")
                //iszc = " and p.name is not null";
                if (search.ISZC == "未注册")
                    //iszc = " and p.name is null";
                    if (search.State != null && search.State != -1)
                        iszc += " and w.STATUS=" + search.State;
            if (search.Name != null && search.Name != "")
                iszc += " and (w.name like '%" + search.Name + "%' or p.name like '%" + search.Name + "%' or p.phone like '%" + search.Name + "%')";
            return base.PageGet<CUST_FANS_EX>(view, @"w.*,case status when 0 then '已取消' else '未取消' end qx,case gender when 1 then '男' else '女' end xb", "wxCUST_FANS w ", " and w.ToUserName='" + search.ToUserName + "'  " + iszc, "w.LAST_CONN_DATE desc", "");
        }

        public PagedList<CUST_FANS_EX> QueryGetFansByEWM(FansSearch search, PageView view)
        {
            string iszc = " and w.FROMUSERNAME in( select DISTINCT FROMUSERNAME from WXCUST_MSG_HIS where EVENTKEY = '" + search.MdSel + "') and w.ID in (select MAX(ID) from WXCUST_FANS group by FROMUSERNAME )";
            return base.PageGet<CUST_FANS_EX>(view, @"w.*,p.Name xm,p.Phone,case status when 0 then '已取消' else '未取消' end qx,case gender when 1 then '男' else '女' end xb", "wxCUST_FANS w LEFT JOIN dbo.WXPersonInfo p ON w.FROMUSERNAME=p.FromUserName", " and w.ToUserName='" + search.ToUserName + "'  " + iszc, "w.LAST_CONN_DATE desc", "");
        }

        public PagedList<CUST_FANS_EX> QueryGetFansByEmp(FansSearch search, PageView view)
        {
            string iszc = " and ec.EmpId = " + search.MdSel;
            return base.PageGet<CUST_FANS_EX>(view, @"w.*,ec.Id as EcId,p.Name xm,p.Mobile as Phone,case status when 0 then '已取消' else '未取消' end qx,case gender when 1 then '男' else '女' end xb", "Emp_Cust ec left join wxCUST_FANS w  on w.FROMUSERNAME = ec.FromUserName left JOIN ( select distinct NAME,MOBILE,FROM_USER_NAME from Cust_Info ) p ON ec.FROMUSERNAME=p.From_User_Name", " and w.ToUserName='" + search.ToUserName + "'  " + iszc, "w.LAST_CONN_DATE desc", "");
        }
        public PagedList<CUST_FANS_EX> QueryGetFansByMobile(FansSearch search, PageView view)
        {
            string iszc = " and ec.Mobile = '" + search.Mobile + "' ";
            return base.PageGet<CUST_FANS_EX>(view, @"w.*,ec.Id as EcId,p.Name xm,p.Mobile as Phone,case status when 0 then '已取消' else '未取消' end qx,case gender when 1 then '男' else '女' end xb", "Emp_Cust ec left join wxCUST_FANS w  on w.FROMUSERNAME = ec.FromUserName left JOIN ( select distinct NAME,MOBILE,FROM_USER_NAME from Cust_Info ) p ON ec.FROMUSERNAME=p.From_User_Name", " and w.ToUserName='" + search.ToUserName + "'  " + iszc, "w.LAST_CONN_DATE desc", "");
        }
        /// <summary>
        /// 根据微信原始ID获取一位粉丝
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public WXCUST_FANS GetOneFans(string ToUserName)
        {
            return base.Get<WXCUST_FANS>("select top 1 * from wxCUST_FANS where ToUserName='" + ToUserName + "'", null);
        }
        /// <summary>
        /// 得到一个JsPai对象实体
        /// </summary>
        public ApiTicket GetModelJsApi()
        {
            string sql = "select  top 1 Id,JSapi_Ticket,GetTicketTime from ApiTicket Order by GetTicketTime desc";
            return base.Get<ApiTicket>(sql, null);
        }
        public int SaveApiTicket(ApiTicket m)
        {
            if (m.Id == 0)
                return (int)Insert(m);
            else
                return Update(m);
        }
        /// <summary>
        /// 根据粉丝ID获取粉丝详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CUST_FANS_EX GetFansByID(int? id)
        {
            return Get<CUST_FANS_EX>(@"select f.*,case status when 0 then '已取消关注' else '未取消关注' end qx,case gender when 1 then '男' else '女' end xb,
ISNULL(MEM_LEVEL,'未注册/绑定') MEM_LEVEL,p.name XM,p.phone,birthday,yzm from wxcust_fans f left join wxpersoninfo p on f.fromusername=p.fromusername
where f.ID=" + id, null);
        }
        public EMPLOYEE GetEmpByPhone(string phone)
        {
            return Get<EMPLOYEE>("select top 1 * from EMPLOYEE where Mobile = '" + phone + "'", null);
        }
        public EMPLOYEE GetEmpByUserID(string userId)
        {
            return Get<EMPLOYEE>("select top 1 * from EMPLOYEE where USERID = '" + userId + "'", null);
        }
        public EMPLOYEE GetEmpByEwmId(string EwmId)
        {
            return Get<EMPLOYEE>("select top 1 * from EMPLOYEE where EwmId = " + EwmId, null);
        }
        public string GetNameByEwmId(string EwmId)
        {
            return Get<string>("select top 1 name from EMPLOYEE where EwmId = " + EwmId, null);
        }
        /// <summary>
        /// 获取商户设置
        /// </summary>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        public MerchantsSeting_EX GetMerchantsSetingByMerchantsID(int? Merchants_ID)
        {
            return Get<MerchantsSeting_EX>("select * from wxMerchantsSeting where Merchants_ID=" + Merchants_ID, null);
        }

        /// <summary>
        /// 获取会员卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXMembershipCard GetMembershipCard(int? id)
        {
            return Get<WXMembershipCard>("select * from wxMembershipCard where id=" + id, null);
        }
        public AreaManage_EX GetArea(int? id)
        {
            return Get<AreaManage_EX>("select a.*,u.USER_NO,u.USER_PASS from AreaManage a left join USER_INFO u on u.ID = a.UserId where a.Id =" + id, null);
        }

        public int GetAllGraphicOne(string media_Id)
        {
            return Get<int>("SELECT * FROM dbo.WXGraphicList WHERE Media_ID=@Media_ID",
                new { Media_ID = media_Id });
        }

        public List<FILE_EX> GetFiilesImage()
        {
            //string sql = "SELECT * FROM dbo.FILES WHERE FANS_ID IS NOT NULL AND IS_GOOD=1";
            string sql = @"SELECT (SELECT COUNT(0) FROM FANS_FABULOUS WHERE FILES_ID=A.ID) COUNT_GOOD,* 
FROM dbo.FILES A WHERE FANS_ID IS NOT NULL AND IS_GOOD=1";

            return base.Query<FILE_EX>(sql, new { });
        }

        public Store_EX GetStore(int? id)
        {
            return Get<Store_EX>("select m.*,a.Name as BelongsAreaName from MDSearch m left join AreaManage a on m.BelongsAreaNo = a.AreaNo where m.Id =" + id, null);
        }
        /// <summary>
        /// 查询留言
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WXOpinion_EX> GetWXOpinionList(PageView view)
        {
            return base.PageGet<WXOpinion_EX>(view, @"o.*,isnull(NAME,'未知') NAME,isnull(IMAGE,'/getheadimg.jpg') IMAGE", "WXOpinion o left join WXCUST_FANS f  on o.FromUserName=f.FROMUSERNAME", "", "o.id desc", "");
        }

        /// <summary>
        /// 保存会员卡
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public int SaveMembershipCard(WXMembershipCard m)
        {
            if (m.ID == 0)
                return (int)Insert(m);
            else
                return Update(m);
        }

        /// <summary>
        /// 获取会员卡列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WXMembershipCard> QueryGetMembershipCard(MembershipCardSearch search, PageView view)
        {
            return base.PageGet<WXMembershipCard>(view, @"m.ID,m.Type,m.Title,s.NAME Explain,m.CreateDate,m.Merchants_ID,m.SID", "WXMembershipCard m left join ORG_STORE s on m.SID=s.ID", " and m.Merchants_ID=" + search.Merchants_ID, "m.id", "");
        }

        /// <summary>
        /// 获取所以图文
        /// </summary>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        public List<Graphic_Detail_EX> GetAllGraphicDetail(int? list_id)
        {
            return Query<Graphic_Detail_EX>(@"select d.*,g.title Name,g.CreateDate from dbo.wxGraphicList g left join dbo.wxGraphicDetail d on g.id=d.list_id
where d.list_id=" + list_id + " and d.id is not null order by g.id", null);
        }

        /// <summary>
        /// 获取所以图文
        /// </summary>
        /// <param name="Merchants_ID"></param>
        /// <returns></returns>
        public List<WXGraphicList> GetAllGraphicList(int? Merchants_ID)
        {
            return Query<WXGraphicList>
                (@"select * from dbo.wxGraphicList where merchants_id=" + Merchants_ID + " order by id desc", null);
        }

        /// <summary>
        /// 删除图文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteTW(int id)
        {
            string sql = string.Format(@"delete wxGraphicList where ID={0}
delete wxGraphicDetail where list_id={0}", id);
            return base.Excute(sql, null);
        }
        public int DeleteInformation(int id)
        {
            string sql = string.Format(@"delete WXInformation where ID = {0}", id);
            return base.Excute(sql, null);
        }
        public int DeleteStore(int id)
        {
            string sql = string.Format(@"delete MDSearch where ID={0}", id);
            return base.Excute(sql, null);
        }
        public int DeleteBD(int id)
        {
            string sql = string.Format(@"delete Emp_Cust where ID={0}", id);
            return base.Excute(sql, null);
        }
        public int DeleteEmp(int id)
        {
            string sql = string.Format(@"delete employee where ID={0}", id);
            return base.Excute(sql, null);
        }
        public int UpdateBD(int id, int? EwmId, string phone)
        {
            string sql = string.Format(@"Update Emp_Cust set EmpId = {0},Mobile = {1} where ID={2}", EwmId, phone, id);
            return base.Excute(sql, null);
        }
        public int UpdateUserInfo(int? userid, string userno, string userpass)
        {
            string sql = string.Format(@"update USER_INFO set user_no = '{0}',user_pass = '{1}' where ID = {2}", userno, userpass, userid);
            return base.Excute(sql, null);
        }
        public int UpdateAreaName(int id, string name, string title)
        {
            string sql = string.Format(@"update AreaManage set Name = '{0}',Title = '{1}' where Id = {2}", name, title, id);
            return base.Excute(sql, null);
        }
        public int UpdateShow(int id, string IsShow)
        {
            if (IsShow == "开启")
            {
                string sql = string.Format(@"update AreaManage set IsShow = '是' where Id = {0}", id);
                return base.Excute(sql, null);
            }
            else
            {
                string sql = string.Format(@"update AreaManage set IsShow = '否' where Id = {0}", id);
                return base.Excute(sql, null);
            }

        }
        /// <summary>
        /// 根据顺序获取图文
        /// </summary>
        /// <param name="list_id"></param>
        /// <param name="rowid"></param>
        /// <returns></returns>
        public Graphic_Detail_EX GetGraphic_DetailByRowID(int list_id, int rowid)
        {
            string sql = string.Format(@"select * from (select ROW_NUMBER()over(order by g.id) rid, d.*,g.title Name,g.CreateDate from dbo.wxGraphicList g left join dbo.wxGraphicDetail d on g.id=d.list_id
where d.list_id={0} and d.id is not null )t where rid={1}", list_id, rowid);
            return Get<Graphic_Detail_EX>(sql, null);
        }

        /// <summary>
        /// 根据顺序删除图文
        /// </summary>
        /// <param name="list_id"></param>
        /// <param name="rowid"></param>
        /// <returns></returns>
        public int DelGraphic_DetailByRowID(int list_id, int rowid)
        {
            string sql = string.Format(@"delete wxGraphicDetail where ID in(
select ID from (select ROW_NUMBER()over(order by g.id) rid, d.*,g.title Name,g.CreateDate from dbo.wxGraphicList g left join dbo.wxGraphicDetail d on g.id=d.list_id
where d.list_id={0} and d.id is not null )t where rid={1})", list_id, rowid);
            return base.Excute(sql, null);
        }

        /// <summary>
        /// 获取粉丝聊天记录 前20条
        /// </summary>
        /// <param name="fansid"></param>
        /// <returns></returns>
        public List<CUST_MSG_RECORD_EX> GetMsgList(int? fansid)
        {
            string sql = @"select TOP 20  m.ID,isnull(m.TOUSERNAME,s.tousername) tousername,isnull(m.FROMUSERNAME,f.fromusername) fromusername,m.MSGTYPE,case m.GraphicID when 0 then m.CONTENT else '[图文][<a href='''+case isurl when 1 then url else '/GraphicDisplay.aspx?id='+CONVERT(varchar, w.id) end +''' target=''_Blank''>'+w.title+'</a>]'+ISNULL(CONTENT,'') end CONTENT,m.GraphicID,m.ReturnID,m.IS_RETURN,m.IS_STAR,m.State,m.CREATE_DATE,case m.State when 0 then  f.NAME else s.ORG_NAME end name,case m.State when 0 then  f.IMAGE else '/assets/images/jqr.jpg' end image,f.NAME fname,F.ID FID from wxcust_fans f left join wxcust_msg_record m on f.fromusername=m.fromusername
left join ORG_INFO s on f.TOUSERNAME=s.TOUSERNAME
left join (select MIN(id) id from dbo.WXGraphicDetail group by list_id) d on m.graphicid=d.id
left join WXGraphicDetail w on d.id=w.id
where f.id=" + fansid + " order by m.CREATE_DATE desc";
            return Query<CUST_MSG_RECORD_EX>(sql, null);
        }

        /// <summary>
        /// 微信表情
        /// </summary>
        /// <returns></returns>
        public List<WXBiaoqing> GetBQList()
        {
            return Query<WXBiaoqing>("select * from wxbiaoqing", null);
        }

        /// <summary>
        /// 获取单条微信消息记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CUST_MSG_RECORD_EX GetMsgByID(int? id)
        {
            return Get<CUST_MSG_RECORD_EX>(@"select m.*,case m.State when 0 then  f.NAME else s.ORG_NAME end name,case m.State when 0 then  f.IMAGE else 'http://182.254.139.183/favicon.ico' end image,f.NAME fname from wxcust_fans f left join wxcust_msg_record m on f.fromusername=m.fromusername
left join ORG_INFO s on f.TOUSERNAME=s.TOUSERNAME
where m.id=" + id, null);
        }

        /// <summary>
        /// 保存微信消息记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int SaveMsg(WXCUST_MSG_RECORD msg)
        {
            if (msg.ID == 0)
                return (int)Insert(msg);
            else
                return Update(msg);
        }

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXTicket GetTicket(int? id)
        {
            return Get<WXTicket>("select * from wxticket where id=" + id, null);
        }

        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<WXTicket> QueryTicketList(RoleSearch search, PageView view)
        {
            return base.PageGet<WXTicket>(view, "*", "WXTicket", " and merchants_id=" + search.ORG_ID, "id", "");
        }

        /// <summary>
        /// 获取绑定列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<CUST_INFO> QueryCustList(RoleSearch search, PageView view)
        {
            string sqlwhere = string.IsNullOrEmpty(search.CName) ? "" : string.Format(" and  name LIKE '%{0}%' OR CardNo LIKE '%{0}%' OR MOBILE LIKE '%{0}%'", search.CName);
            sqlwhere = " and MOBILE IN('18510336239','18911883857','15611366550','13833445127')";
            return base.PageGet<CUST_INFO>(view, "*", "CUST_INFO", sqlwhere, "id", ""); ;
        }

        public List<ORG_STORE_EX> GetORG_STOREOrderBy(string sql)
        {
            return Query<ORG_STORE_EX>(sql, null);
        }

        //public PagedList<TEMPLATE_LOG> QueryTemplateLogList(RoleSearch search, PageView view)
        //{
        //    string sqlwhere = string.Format(" and  name LIKE '%{0}%' ", search.CName);
        //    return base.PageGet<TEMPLATE_LOG>(view, "*", "TEMPLATE_LOG", sqlwhere, "id", "");
        //}
        public List<WXCUST_FANS> QueryTemplateFansList(string groupid, string name)
        {
            return Query<WXCUST_FANS>(@"select a.NAME,a.FROMUSERNAME from WXCUST_FANS a     inner join Emp_Cust b on a.FROMUSERNAME=b.FromUserName       where b.Mobile in(	            select EMP.MOBILE                from EMPLOYEE EMP 	            inner join REL_EMP_GROUP REG on REG.EMP_ID =EMP.ID                 inner join GROUP_INFO GI on GI.ID = REG.GROUP_ID                where REG.GROUP_ID IN (" + groupid + ")            ) and a.NAME like '%" + name + "%'", null);
        }

        public PagedList<WXCUST_FANS> QueryTemplateFansListT(string groupid, string name, PageView view)
        {
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(groupid))
            {
                sqlwhere = " and REG.GROUP_ID IN (" + groupid + "))  and a.NAME like '%" + name + "%'";
            }
            else
            {
                sqlwhere = " and 1=1)";
            }
            return base.PageGet<WXCUST_FANS>(view, "a.NAME,a.FROMUSERNAME,a.[IMAGE]", "WXCUST_FANS a     inner join Emp_Cust b on a.FROMUSERNAME=b.FromUserName       where b.Mobile in(	            select EMP.MOBILE                from EMPLOYEE EMP 	            inner join REL_EMP_GROUP REG on REG.EMP_ID =EMP.ID                 inner join GROUP_INFO GI on GI.ID = REG.GROUP_ID        ", sqlwhere, "a.id", "");
        }

        public CUST_INFO GetCustInfoSms(string fromusername)
        {
            return Get<CUST_INFO>("select * from CUST_INFO where FROM_USER_NAME='" + fromusername + "'", null);
        }

        public int UpdateCustInfoBySms(string fromusername)
        {
            string sql = string.Format(@" update CUST_INFO set IsPushSms=0 where FROM_USER_NAME='{0}'", fromusername);
            return base.Excute(sql, null);
        }

        //public PagedList<TEMPLATE_INFO> QueryTemplateInfoList(PageView view)
        //{
        //    string sqlwhere = "";
        //    return base.PageGet<TEMPLATE_INFO>(view, "*", "TEMPLATE_INFO", sqlwhere, "id", "");
        //}

        //public TEMPLATE_INFO QueryTemplateInfo(int id)
        //{
        //    return Get<TEMPLATE_INFO>("select * from TEMPLATE_INFO where id=" + id, null);
        //}

        public List<FansList_Ex> QueryFansListByBuy()
        {
            string sql = @"select a.id,a.NAME, a.NOTICE_DATE,case when c.NAME is null then '' else c.NAME end  as Employee ,a.PROVINCE,
a.CITY,case a.GENDER when 1 then '男' when 0 then '女' end as Sex,case when d.CardNo is null then '0' else '1' end as CardNo,
case when  d.Mobile is null then '' else d.MOBILE end as Mobile ,case when d.CustLevel is null then '' else d.CustLevel end as Level ,
case when d.CustCity is null then '' else d.CustCity end as LName, a.IS_BUYER
from WXCUST_FANS a 
left join Emp_Cust b on a.FROMUSERNAME=b.FromUserName
left join EMPLOYEE c on b.Mobile=c.MOBILE
left join CUST_INFO d on d.FROM_USER_NAME=a.FROMUSERNAME 
where a.[STATUS]=1 ";
            return Query<FansList_Ex>(sql, null);

        }

        public List<Information_EX> GetModelListByKeywordList(int ReplyType, string Keyword, string ToUserName)
        {
            string sql = @"select i.*,d.Title,d.Describe Description,d.pic PicUrl,d.url FulltextUrl,
d.id did,d.IsURL from wxinformation i left join dbo.wxGraphicList l on i.Graphic_ID=l.id
left join dbo.wxGraphicDetail d on d.list_id=l.id
left join ORG_INFO m on i.Merchants_ID=m.ID  where i.replyType=@ReplyType  and m.ToUserName=@ToUserName ";
            if (!string.IsNullOrEmpty(Keyword))
            {
                sql += " and i.KeyWords=@Keyword";
            }
            return base.Query<Information_EX>(sql, new { ReplyType = ReplyType, Keyword = Keyword, ToUserName = ToUserName });
        }



        public List<CustomMenu_EX> GetCustomMenuModelList(string EventKey, string ToUserName)
        {
            string sql = @"select c.*,d.Title,d.Describe Description,d.pic PicUrl,d.url FulltextUrl,
d.id did,case c.type when 0 then 'text' when 1 then 'news' end MsgType,d.IsURL from wxCustomMenu c left join wxGraphicList l on c.Graphic_ID=l.ID
left join wxGraphicDetail d on l.ID=d.List_ID left join ORG_INFO m on c.Merchants_ID=m.ID  where  m.ToUserName=@ToUserName";

            if (!string.IsNullOrEmpty(EventKey))
            {
                sql += @" and c.ID=@EventKey ";
            }
            return base.Query<CustomMenu_EX>(sql, new { EventKey = EventKey, ToUserName = ToUserName });
        }

        public FILES GetFILES(int id)
        {
            return base.Get<FILES>("SELECT * FROM FILES WHERE ID=@ID", new { ID = id });
        }
        public AR_QR_FANS GetMaxArid()
        {
            string sql = string.Format("select TOP 1 * from AR_QR_FANS order by create_date DESC");
            return base.Get<AR_QR_FANS>(sql, null);
        }

        public AR_QR_FANS QueryArInfo(int id)
        {
            return base.Get<AR_QR_FANS>("SELECT * FROM AR_QR_FANS WHERE ID=@ID", new { ID = id });
        }

        public AR_QR_FANS QueryArInfoByArId(int arid)
        {
            return base.Get<AR_QR_FANS>("SELECT top 1 * FROM AR_QR_FANS WHERE AR_ID=@arid order by create_date desc", new { arid = arid });
        }

        public int DeleteAr(int id)
        {
            string sql = "DELETE dbo.AR_QR_FANS WHERE ID=" + id;
            return base.Excute(sql, null);
        }

        public List<TT_Detail> GetTTDetailList(int age, string ptype)
        {
            return Query<TT_Detail>("SELECT * FROM dbo.TT_Detail WHERE Age=@age AND PType=@ptype", new { age = age, ptype = ptype });
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<TT_Detail> QueryGoodsList(GraphicSearch search, PageView view)
        {
            return base.PageGet<TT_Detail>(view, "*", "TT_Detail", "", "id", " order by id desc"); 
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TT_Detail GetTT_Detail(int id)
        {
            return Get<TT_Detail>("select * from TT_Detail where id=@id", new { id = id });
        }

        /// <summary>
        /// 保存商品详情
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public int SaveTT_Detail(TT_Detail detail)
        {
            if (detail.ID == 0)
                return (int)Insert(detail);
            else
                return Update(detail);
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeteleTT_Detail(int id)
        {
            return Excute("delete TT_Detail where id=@id", new { id = id });
        }

        /// <summary>
        /// 添加微信扫码记录
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int AddWXCustScanRecord(WXCustScanRecord c)
        {
            return (int)base.Insert(c);
        }


        /// <summary>
        /// 标志卡券已经领取
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int UpdateWXCouponIsGet(string OpenId, string CouponNo, string CardId)
        {
            string sql = "update WXCouponGiveInfo set Status=1 ,CouponGetDate=getdate(),CouponNo=@CouponNo where  CardId=@CardId and OpenId=@OpenId and isnull(Status,0)=0";
            return base.Excute(sql, new
            {
                CouponNo = CouponNo,
                CardId = CardId,
                OpenId = OpenId
            });
        }

        /// <summary>
        /// 标志卡券已经核销
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int UpdateWXCouponIsHX(string OpenId, string CouponNo, string CardId)
        {
            string sql = "update WXCouponGiveInfo set Status=2 ,UseDate=getdate() where  CardId=@CardId and OpenId=@OpenId and CouponNo=@CouponNo and isnull(Status,0)=1";
            return base.Excute(sql, new
            {
                CouponNo = CouponNo,
                CardId = CardId,
                OpenId = OpenId
            });
        }
    }
}
