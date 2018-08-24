using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class CustomerRepository : BaseRepository
    {

        public PagedList<CUST_INFO_EX> QueryCustomerList(CustomerSearch search, PageView view, int org_id, int region_id)
        {
            string cols = @"A.[ID],A.[STORE_ID],B.[NAME] as STORE_NAME,A.[CUST_NO]
                          ,A.[NAME],A.[IMAGE_ID],A.[GENDER],A.[BIRTHDAY],A.[IDTYPE],A.[IDCARD],A.[MOBILE],A.[COUNTRY]
	                      ,A.[PROVINCE],A.[CITY],A.[REGION],A.[ADDRESS],A.[ZIPCODE],A.[EDUCATION],A.[EMAIL]
                          ,A.[SOURCE],A.[QQ],A.[WECHAT],A.[FACEBOOK],A.[WEIBO],A.[OPTIN_SMS]
                          ,A.[INCOME],A.[POSITION],A.[WORK_STATION],A.[CREATE_DATE],A.[CREATE_USER]
                          ,A.[LAST_MODI_DATE],A.[LAST_MODI_USER],A.[CARD_NO],A.[PASSWORD],A.[TYPE],C.balance BALANCE,d.name CARD_NAME";
            string sqlWhere = " AND A.NAME!='散客'";

            if (!string.IsNullOrEmpty(search.NAME))
            {
                sqlWhere += " AND A.[NAME] LIKE '%" + search.NAME + "%'";
            }
            else
            {
                if (search.STORE_ID > 0)
                {
                    sqlWhere += " AND (A.[STORE_ID]='" + search.STORE_ID + "'  or  A.[STORE_ID] is  null)";
                }
                //if (org_id > 0 && region_id == 0)
                //{
                //    sqlWhere += " AND (A.[STORE_ID] in (select B.ID from ORG_STORE where B.ORG_ID=" + org_id + ")  or  A.[STORE_ID] is  null)";
                //}

                if (region_id > 0)
                {
                    sqlWhere += " AND (A.[STORE_ID] in (select C.ID  from ORG_STORE C  left join ORG_INFO D on  C.REGION_ID =D.ID where D.ID=" + region_id + " and C.ORG_ID=" + org_id + ")  or  A.[STORE_ID] is  null)";
                }
            }
            if (!string.IsNullOrEmpty(search.IDCARD))
            {
                sqlWhere += " AND A.[IDCARD] LIKE '%" + search.IDCARD + "%'";
            }
            if (!string.IsNullOrEmpty(search.MOBILE))
            {
                sqlWhere += " AND A.[MOBILE] LIKE '%" + search.MOBILE + "%'";
            }


            if (!string.IsNullOrEmpty(search.CARD_NO))
            {
                sqlWhere += " AND A.[CARD_NO]='" + search.CARD_NO + "'";
            }

            sqlWhere += " AND A.ORG_ID =" + search.ORG_ID;
            return base.PageGet<CUST_INFO_EX>(view, cols,
                "[CUST_INFO] A  LEFT JOIN [ORG_STORE] B ON A.[STORE_ID]=B.ID left join   CUST_CARD c on a.id=c.Cust_id and c.CARD_TYPE=1 and c.STATUS=1  left join PROD_CARD d on c.CARD_ID=d.ID " //table
                , sqlWhere
                , "A.ID DESC ",
                "");
            //ORDER BY A.[LAST_MODI_DATE] DESC

        }

        public CUST_INFO_EX GetCustomerDetail(int id)
        {
            string s = AppConfig.ImageUrl;
            string sql = @"  SELECT  a.*,Replace(Replace(b.file_url,'/helper','"+s+"'),'/customer','"+s+"') IMAGE_URL  from cust_info a  left join FILES b on a.IMAGE_ID=b.id WHERE a.[ID]=@ID";
            return base.Get<CUST_INFO_EX>(sql, new { ID = id });
        }

        public CUST_INFO_EX GetCustomer(int id)
        {
            //            string sql = @"SELECT CI.*
            //                             ,PC.NAME CARD_NAME ,CC.BALANCE,CI.[STATUS]
            //                          FROM [CUST_INFO] CI
            //                          LEFT JOIN CUST_CARD CC ON CI.ID=CC.CUST_ID  
            //                          LEFT JOIN PROD_CARD PC ON CC.CARD_ID=PC.ID
            //                           WHERE CI.[ID]=@ID";
            string sql = @"      SELECT CI.*
                             ,B.CARD_NAME CARD_NAME ,B.BALANCE,CI.[STATUS]
                          FROM [CUST_INFO] CI   
                          LEFT JOIN(
                           select cc.CUST_ID,cc.BALANCE,cc.ARREARS,pc.NAME as CARD_NAME from CUST_CARD cc left join PROD_CARD pc on cc.CARD_ID=pc.ID
            where cc.CARD_TYPE=1 and cc.STATUS=1
                          )B ON CI.ID=B.CUST_ID
                          
                           WHERE CI.[ID]=@ID";
            return base.Get<CUST_INFO_EX>(sql, new { ID = id });
        }
        //根据姓名检索 散客
        public CUST_INFO_EX GetCustomerEx(string custname,int storeid,int orgid)
        {
            string sql = @"      SELECT CI.*
                             ,B.CARD_NAME CARD_NAME ,B.BALANCE,CI.[STATUS]
                          FROM [CUST_INFO] CI   
                          LEFT JOIN(
                           select cc.CUST_ID,cc.BALANCE,cc.ARREARS,pc.NAME as CARD_NAME from CUST_CARD cc left join PROD_CARD pc on cc.CARD_ID=pc.ID
            where cc.CARD_TYPE=1 
                          )B ON CI.ID=B.CUST_ID
                          
                           WHERE CI.[NAME]=@NAME AND CI.[ORG_ID]=@ORG_ID AND CI.[STORE_ID]=@STORE_ID";
            return base.Get<CUST_INFO_EX>(sql, new { NAME = custname,STORE_ID=storeid,ORG_ID=orgid });
        }
        public CUST_INFO GetCustomer(string custname)
        {
            string sql = @"SELECT [ID],[STORE_ID],[CUST_NO],[NAME],[IMAGE_ID],[GENDER],[BIRTHDAY],[MERCHANT]
                              ,[IDTYPE],[IDCARD],[MOBILE],[COUNTRY],[PROVINCE],[CITY]
                              ,[REGION],[ADDRESS],[ZIPCODE],[EDUCATION],[EMAIL]
                              ,[SOURCE],[QQ],[WECHAT],[FACEBOOK],[WEIBO],[OPTIN_SMS]
                              ,[INCOME],[POSITION],[WORK_STATION],[CREATE_DATE]
                              ,[CREATE_USER],[LAST_MODI_DATE],[LAST_MODI_USER],[CARD_NO],[PASSWORD],[STATUS],LINK_PERSON_NAME2,LINK_PERSON_NAME3,LINK_PERSON_NAME1,LINK_PERSON_PHONE1,LINK_PERSON_PHONE2,LINK_PERSON_PHONE3
                          FROM [CUST_INFO] WHERE [NAME]=@NAME";
            return base.Get<CUST_INFO>(sql, new { NAME = custname });
        }
        public CUST_INFO GetCustomerByFromUserName(string fromUserName)
        {
            string sql = @"SELECT [ID],[STORE_ID],[CUST_NO],[NAME],[IMAGE_ID],[GENDER],[BIRTHDAY],[MERCHANT]
                              ,[IDTYPE],[IDCARD],[MOBILE],[COUNTRY],[PROVINCE],[CITY]
                              ,[REGION],[ADDRESS],[ZIPCODE],[EDUCATION],[EMAIL]
                              ,[SOURCE],[QQ],[WECHAT],[FACEBOOK],[WEIBO],[OPTIN_SMS]
                              ,[INCOME],[POSITION],[WORK_STATION],[CREATE_DATE]
                              ,[CREATE_USER],[LAST_MODI_DATE],[LAST_MODI_USER],[CARD_NO],[PASSWORD],[STATUS],LINK_PERSON_NAME2,LINK_PERSON_NAME3,LINK_PERSON_NAME1,LINK_PERSON_PHONE1,LINK_PERSON_PHONE2,LINK_PERSON_PHONE3,FROM_USER_NAME
                          FROM [CUST_INFO] WHERE [FROM_USER_NAME]=@FROM_USER_NAME";
            return base.Get<CUST_INFO>(sql, new { FROM_USER_NAME = fromUserName });
        }
        public CUST_INFO GetCustomerByMobile(string mobile)
        {
            string sql = @"SELECT [ID],[STORE_ID],[CUST_NO],[NAME],[IMAGE_ID],[GENDER],[BIRTHDAY],[MERCHANT]
                              ,[IDTYPE],[IDCARD],[MOBILE],[COUNTRY],[PROVINCE],[CITY]
                              ,[REGION],[ADDRESS],[ZIPCODE],[EDUCATION],[EMAIL]
                              ,[SOURCE],[QQ],[WECHAT],[FACEBOOK],[WEIBO],[OPTIN_SMS]
                              ,[INCOME],[POSITION],[WORK_STATION],[CREATE_DATE]
                              ,[CREATE_USER],[LAST_MODI_DATE],[LAST_MODI_USER],[CARD_NO],[PASSWORD],[STATUS],LINK_PERSON_NAME2,LINK_PERSON_NAME3,LINK_PERSON_NAME1,LINK_PERSON_PHONE1,LINK_PERSON_PHONE2,LINK_PERSON_PHONE3,FROM_USER_NAME,APP_PWD
                          FROM [CUST_INFO] WHERE [MOBILE]=@MOBILE";
            //            string sql = @"SELECT TOP 1
            //                          FROM [CUST_INFO] WHERE [MOBILE]=@MOBILE";
            return base.Get<CUST_INFO>(sql, new { MOBILE = mobile });
        }

        public CUST_INFO GetCustomerByMobile(string mobile, string org_id)
        {
            string sql = @"SELECT a.*
                          FROM [CUST_INFO] a left join ORG_STORE b on a.STORE_ID=b.ID
                          where a.ORG_ID=@ORG_ID AND [MOBILE]=@MOBILE";
            //            string sql = @"SELECT TOP 1
            //                          FROM [CUST_INFO] WHERE [MOBILE]=@MOBILE";
            return base.Get<CUST_INFO>(sql, new { MOBILE = mobile, ORG_ID = org_id });
        }

        public CUST_INFO GetCustomerByMobile(string mobile, int org_id)
        {
            string sql = @"SELECT *
                          FROM [CUST_INFO] a left join ORG_STORE b on a.STORE_ID=b.ID
                          where a.ORG_ID=@ORG_ID AND [MOBILE]=@MOBILE";
            //            string sql = @"SELECT TOP 1
            //                          FROM [CUST_INFO] WHERE [MOBILE]=@MOBILE";
            return base.Get<CUST_INFO>(sql, new { MOBILE = mobile, ORG_ID = org_id });
        }

        public CUST_INFO LoginByMobile(string mobile, string pwd)
        {
            string sql = @"select id from cust_info where  [MOBILE]=@MOBILE and app_pwd=@APPPWD ";
            //            string sql = @"SELECT TOP 1
            //                          FROM [CUST_INFO] WHERE [MOBILE]=@MOBILE";
            return base.Get<CUST_INFO>(sql, new { MOBILE = mobile, APPPWD = pwd });
        }
        public int DeleteCustomer(int id)
        {
            string sql = "DELETE FROM [CUST_INFO] WHERE [ID]=@ID";
            return base.Excute(sql, new { ID = id });
        }
        /// <summary>
        /// 根据条件检索会员信息 2014-05-04 by Levin
        /// </summary>
        /// <param name="qText"></param>
        /// <returns></returns>
        public List<CUST_INFO> QueryCustomer(string qText,int storeid, int org_id)
        {
            string sql = @"SELECT TOP 10 [ID],[CUST_NO],[NAME],[MOBILE],[IDCARD],[CARD_NO] FROM [CUST_INFO] WHERE ORG_ID=" + org_id + "  AND (";
            sql += "  [NAME] Like '%" + qText + "%'";
            sql += " OR [MOBILE] Like '" + qText + "%'";
            sql += " OR [CARD_NO] = '" + qText + "') ";
            if (qText.Contains("散客"))
            {
                sql += " AND [STORE_ID] = "+ storeid + " ";
            }
            return base.Query<CUST_INFO>(sql, null);
        }

        /// <summary>
        /// 获取客户完整信息
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public CUST_INFO_EX GetCustomerFullInfoById(string cid,int orgid)
        {
            //            string sql = @"SELECT ci.[ID],ci.[STORE_ID],[CUST_NO],[CARD_NO],ci.[NAME],ci.[IMAGE_ID],[GENDER],[BIRTHDAY],[IDTYPE],[IDCARD],[MOBILE]
            //                          ,[COUNTRY],ci.[PROVINCE],ci.[CITY],ci.[REGION],ci.[ADDRESS],[ZIPCODE],[EDUCATION],[EMAIL],[SOURCE],[QQ],[WECHAT]
            //                          ,[FACEBOOK],[WEIBO],[OPTIN_SMS],[INCOME],[POSITION],[WORK_STATION],ci.[CREATE_DATE],ci.[CREATE_USER]
            //                          ,ci.[LAST_MODI_DATE],ci.[LAST_MODI_USER],ci.[STATUS] ,cc.BALANCE,cc.ARREARS,s.NAME STORE_NAME
            //                         FROM [CUST_INFO] ci left join CUST_CARD cc on ci.ID=cc.CUST_ID
            //                         left join ORG_STORE s on ci.STORE_ID=s.ID WHERE ci.[ID] = @CUST_ID";

            string sql = @"SELECT ci.[ID],ci.[STORE_ID],[CUST_NO],[CARD_NO],ci.[NAME],ci.[IMAGE_ID],[GENDER],[BIRTHDAY],[IDTYPE],[IDCARD],[MOBILE]
                          ,[COUNTRY],ci.[PROVINCE],ci.[CITY],ci.[REGION],ci.[ADDRESS],[ZIPCODE],[EDUCATION],[EMAIL],[SOURCE],[QQ],[WECHAT]
                          ,[FACEBOOK],[WEIBO],[OPTIN_SMS],[INCOME],[POSITION],[WORK_STATION],ci.[CREATE_DATE],ci.[CREATE_USER]
                          ,ci.[LAST_MODI_DATE],ci.[LAST_MODI_USER],ci.[STATUS]
                        ,(select sum(BALANCE) BALANCE from CUST_CARD cc where ci.ID=cc.CUST_ID and cc.CARD_TYPE=1 and cc.STATUS=1 group by cc.CUST_ID)BALANCE,
                          (select sum(ARREARS) ARREARS from CUST_CARD cc where ci.ID=cc.CUST_ID and cc.CARD_TYPE=1 and cc.STATUS=1 group by cc.CUST_ID)ARREARS,
                           s.NAME STORE_NAME,s2.NAME REG_STORE_NAME
                       
                        FROM [CUST_INFO] ci 
                         left join ORG_STORE s on ci.STORE_ID=s.ID  left join ORG_STORE s2 on ci.REG_STORE_ID=s2.ID WHERE ci.[ID]= @CUST_ID AND ci.ORG_ID=@ORG_ID";
            return base.Get<CUST_INFO_EX>(sql, new { CUST_ID = cid,ORG_ID=orgid });
        }

       


        /// <summary>
        /// 顾客360页面tab，获取顾客动态信息
        /// </summary>
        /// <param name="view"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<VIEW_CUST_HIS> GetCustDynamicList(string cid)
        {
            string sql = "SELECT * FROM VIEW_CUST_HIS WHERE CUST_ID=@CUST_ID ORDER BY SERVICE_TIME DESC";
            return base.Query<VIEW_CUST_HIS>(sql, new { CUST_ID = cid });

        }

       


        public Entity.FILES GetFILES(int id)
        {
            return base.Get<Entity.FILES>("SELECT * FROM FILES WHERE ID=@ID", new { ID = id });
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteFile(int id)
        {
            return base.Excute(string.Format(@"DELETE dbo.FILES WHERE ID IN(SELECT FILES_ID FROM dbo.REL_CUST_FILES WHERE ID={0})
DELETE dbo.REL_CUST_FILES WHERE ID={0}", id), null);
        }

        #region 顾客分组 ---maya 2014-06-27

        


        #endregion

        //会员卡信息
        public CUST_INFO_EX getCard(string cardNo, string pwd)
        {
            //string sql = "SELECT * FROM CUST_CARD WHERE CARD_NO=@CARD_NO ";
            string where = "";
            if (!string.IsNullOrEmpty(pwd))
            {
                where += " AND CI.PASSWORD=@PASSWORD";
            }

            //string sql = string.Format(@"SELECT CI.ID,CI.NAME,CI.CUST_NO ,CI.CARD_NO,SUM(CC.PAR_AMT)PAR_AMT, SUM(CC.BALANCE) AS BALANCE FROM CUST_INFO CI
            string sql = string.Format(@"SELECT CI.ID,CI.NAME,CI.CUST_NO ,CI.MOBILE,CI.CARD_NO,SUM(CC.PAR_AMT)PAR_AMT, SUM(CC.BALANCE) AS BALANCE FROM CUST_INFO CI
                            LEFT JOIN CUST_CARD CC ON CI.ID=CC.CUST_ID
                            WHERE CI.CARD_NO=@CARD_NO {0} and cc.CARD_TYPE=1
                            GROUP BY CI.ID,CI.NAME,CI.CUST_NO,CI.CARD_NO,CI.MOBILE", where);
            return base.Get<CUST_INFO_EX>(sql, new { CARD_NO = cardNo, PASSWORD = pwd });
        }

        //会员转账操作
        public int CardTransferSave(int outcid, int incid, double TransferAmt)
        {
            string sql = @"UPDATE CUST_CARD SET BALANCE=BALANCE-@TransferAmt WHERE ID=@OUTCID
                           UPDATE CUST_CARD SET BALANCE=BALANCE+@TransferAmt WHERE ID=@INCID ";
            return base.Excute(sql, new { OUTCID = outcid, INCID = incid, TransferAmt = TransferAmt });
        }

        //修改会员卡状态 //挂失 解冻 
        public int UpdateCardStatus(int? cid, int status)
        {
            string sql = "UPDATE CUST_INFO SET STATUS=@STATUS WHERE ID=@ID ";
            return base.Excute(sql, new { STATUS = status, ID = cid });
        }

        //修改会员卡状态 //挂失 解冻 
        public int UpdateCustCardStatus(int? cid, int status)
        {
            string sql = "UPDATE CUST_CARD SET STATUS=@STATUS WHERE CUST_ID=@CID AND STATUS!=3";
            return base.Excute(sql, new { STATUS = status, CID = cid });
        }

        //验证旧密码
        public int CheckCardPwd(int? cid, string oldpwd)
        {
            string sql = "SELECT COUNT(1) FROM CUST_INFO WHERE ID=@ID AND ISNULL(PASSWORD,'')=@PASSWORD";
            return base.Get<int>(sql, new { ID = cid, PASSWORD = oldpwd });
        }
        //验证新卡号是否有重复
        public int CheckNewCardNo(string cardNo, int org_Id)
        {
            string sql = "SELECT COUNT(1) FROM CUST_INFO WHERE CARD_NO=@CARD_NO AND ORG_ID=@ORG_ID";
            return base.Get<int>(sql, new { CARD_NO = cardNo, ORG_ID = org_Id });
        }

        //更新新的卡号和密码
        public int UpdateNewCardNo(int? cid, string cardNo, string pwd)
        {
            string sql = "UPDATE CUST_INFO SET CARD_NO=@CARD_NO,PASSWORD=@PASSWORD,[STATUS]=1 WHERE ID=@ID";
            return base.Excute(sql, new { ID = cid, CARD_NO = cardNo, PASSWORD = pwd });
        }

        //日结统计
        public List<DailyCheck_EX> GetDailyIncome(int? storeId, string sDate, string eDate,int? orgid)
        {
            #region
            string sql = string.Format(@"select '会员卡收入' PAYMENT_TYPE,null as PAY_AMT
UNION ALL
SELECT NAME,SUM(PAY_AMT) FROM (
SELECT NAME,0 PAY_AMT FROM PAYMENT_MODE WHERE ORG_ID=7 OR ORG_ID=0
UNION ALL
SELECT ISNULL(PM.NAME,'其他')NAME, SUM(OP.PAY_AMT)PAY_AMT FROM ORDER_PAYMENT OP
LEFT JOIN PAYMENT_MODE PM ON OP.PAYMENT_TYPE=PM.CODE
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
WHERE OP.PROD_TYPE=1 AND OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME!='会员卡'
GROUP BY OP.PAYMENT_TYPE,PM.NAME)t GROUP BY t.NAME

UNION ALL
select '未付款',isnull(SUM(isnull(ARREARS,0)),0) from CUST_CARD cc 
where cc.CARD_TYPE=1 and cc.STATUS=1 and STORE_ID={0} and cc.LAST_MODI_DATE>='{1}'
and cc.LAST_MODI_DATE<='{2}'

union all
SELECT '优惠金额', isnull(SUM(PROD_PRICE-PROD_AMT),0) FROM PAYMENT_MODE PM
LEFT JOIN  ORDER_PAYMENT OP ON OP.PAYMENT_TYPE=PM.CODE and OP.PROD_TYPE=1
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
LEFT JOIN dbo.ORDER_DETAIL OD ON oh.ID=od.ORDER_ID
WHERE   OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME!='会员卡'

UNION ALL
select '项目/商品收入' PAYMENT_TYPE,null as PAY_AMT
UNION ALL

SELECT NAME,SUM(PAY_AMT) FROM (
SELECT NAME,0 PAY_AMT FROM PAYMENT_MODE WHERE ORG_ID=7 OR ORG_ID=0
UNION ALL
SELECT ISNULL(PM.NAME,'其他')+'扣款' as NAME, SUM(OP.PAY_AMT)PAY_AMT FROM ORDER_PAYMENT OP
LEFT JOIN PAYMENT_MODE PM ON OP.PAYMENT_TYPE=PM.CODE
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
WHERE OP.PROD_TYPE=1 AND OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME='会员卡'
GROUP BY OP.PAYMENT_TYPE,PM.NAME)t GROUP BY t.NAME

union all
SELECT ISNULL(PM.NAME,'其他')NAME, SUM(OP.PAY_AMT)PAY_AMT FROM ORDER_PAYMENT OP
LEFT JOIN PAYMENT_MODE PM ON OP.PAYMENT_TYPE=PM.CODE
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
WHERE OP.PROD_TYPE is null AND OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME!='会员卡'
GROUP BY OP.PAYMENT_TYPE,PM.NAME
UNION ALL

SELECT '优惠金额', isnull(SUM(PROD_PRICE-PROD_AMT),0) FROM PAYMENT_MODE PM
LEFT JOIN  ORDER_PAYMENT OP ON OP.PAYMENT_TYPE=PM.CODE and OP.PROD_TYPE=1
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
LEFT JOIN dbo.ORDER_DETAIL OD ON oh.ID=od.ORDER_ID
WHERE   OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME='会员卡'

union all
select '总计' PAYMENT_TYPE,null as PAY_AMT
UNION ALL

SELECT NAME,SUM(PAY_AMT) PAY_AMT FROM (
SELECT NAME,0 PAY_AMT FROM PAYMENT_MODE WHERE ORG_ID=7 OR ORG_ID=0
UNION ALL
SELECT ISNULL(PM.NAME,'其他')NAME, SUM(OP.PAY_AMT)PAY_AMT FROM ORDER_PAYMENT OP
LEFT JOIN PAYMENT_MODE PM ON OP.PAYMENT_TYPE=PM.CODE
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
WHERE OP.PROD_TYPE=1 AND OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME!='会员卡'
GROUP BY OP.PAYMENT_TYPE,PM.NAME

UNION ALL
select '未付款',isnull(SUM(isnull(ARREARS,0)),0) from CUST_CARD cc 
where cc.CARD_TYPE=1 and cc.STATUS=1 and STORE_ID={0} and cc.LAST_MODI_DATE>='{1}'
and cc.LAST_MODI_DATE<='{2}'

union all
SELECT '优惠金额', isnull(SUM(PROD_PRICE-PROD_AMT),0) FROM PAYMENT_MODE PM
LEFT JOIN  ORDER_PAYMENT OP ON OP.PAYMENT_TYPE=PM.CODE and OP.PROD_TYPE=1
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
LEFT JOIN dbo.ORDER_DETAIL OD ON oh.ID=od.ORDER_ID
WHERE   OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME!='会员卡'

UNION ALL

SELECT NAME,SUM(PAY_AMT) FROM (
SELECT NAME,0 PAY_AMT FROM PAYMENT_MODE WHERE ORG_ID=7 OR ORG_ID=0
UNION ALL
SELECT ISNULL(PM.NAME,'其他')+'扣款' as NAME, SUM(OP.PAY_AMT)PAY_AMT FROM ORDER_PAYMENT OP
LEFT JOIN PAYMENT_MODE PM ON OP.PAYMENT_TYPE=PM.CODE
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
WHERE OP.PROD_TYPE=1 AND OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME='会员卡'
GROUP BY OP.PAYMENT_TYPE,PM.NAME)t GROUP BY t.NAME

union all
SELECT ISNULL(PM.NAME,'其他')NAME, SUM(OP.PAY_AMT)PAY_AMT FROM ORDER_PAYMENT OP
LEFT JOIN PAYMENT_MODE PM ON OP.PAYMENT_TYPE=PM.CODE
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
WHERE OP.PROD_TYPE is null AND OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME!='会员卡'
GROUP BY OP.PAYMENT_TYPE,PM.NAME
UNION ALL

SELECT '优惠金额', isnull(SUM(PROD_PRICE-PROD_AMT),0) FROM PAYMENT_MODE PM
LEFT JOIN  ORDER_PAYMENT OP ON OP.PAYMENT_TYPE=PM.CODE and OP.PROD_TYPE=1
LEFT JOIN ORDER_HEAD OH ON OP.ORDER_ID=OH.ID
LEFT JOIN dbo.ORDER_DETAIL OD ON oh.ID=od.ORDER_ID
WHERE   OH.STORE_ID={0} AND OP.CREATE_DATE>='{1}' AND OP.CREATE_DATE<='{2}'
and PM.NAME='会员卡')t GROUP BY t.NAME
", storeId, sDate, eDate);
            #endregion

            return base.Query<DailyCheck_EX>(sql, null);
        }
        //验证未付款订单
        public int InvalidDailyCheck(int storeId)
        {
            string sql = @"SELECT COUNT(1) FROM ORDER_HEAD WHERE STORE_ID=@STORE_ID AND  ORDER_STATUS=0 AND PAY_STATUS=0 AND TRANS_DATE>=(CONVERT(char(10),GETDATE(),120)+' 5:00')";
            return base.Get<int>(sql, new { STORE_ID = storeId });
        }

        //日结保存
        public int SaveDailyCheck(string userid, int storeId)
        {
            string sql = @"UPDATE ORDER_HEAD SET IS_DAY_CHECK=@IS_DAY_CHECK,DAY_CHECK_TIME=@DAY_CHECK_TIME,DAY_CHECK_USER=@DAY_CHECK_USER
                            WHERE STORE_ID=@STORE_ID AND  ORDER_STATUS=1 AND PAY_STATUS=1 AND TRANS_DATE>=(CONVERT(char(10),GETDATE(),120)+' 5:00')";
            return base.Excute(sql, new { STORE_ID = storeId, IS_DAY_CHECK = true, DAY_CHECK_TIME = DateTime.Now, DAY_CHECK_USER = userid });
        }
        public int DeleteNote(int id)
        {
            string sql = "DELETE FROM [CUST_NOTES_HIS] WHERE [ID]=@ID";
            return base.Excute(sql, new { ID = id });
        }

        //积分兑换
        public int SavePoints(CUST_INFO_EX custInfo)
        {
            string sql = "DELETE FROM [CUST_NOTES_HIS] WHERE [ID]=@ID";
            return base.Excute(sql, new { ID = 1 });
        }

        public CUST_INFO GetCustomerByUserCard(string cardno)
        {
            string sql = @"SELECT [ID],[STORE_ID],[CUST_NO],[NAME],[IMAGE_ID],[GENDER],[BIRTHDAY]
                              ,[IDTYPE],[IDCARD],[MOBILE],[COUNTRY],[PROVINCE],[CITY]
                              ,[REGION],[ADDRESS],[ZIPCODE],[EDUCATION],[EMAIL]
                              ,[SOURCE],[QQ],[WECHAT],[FACEBOOK],[WEIBO],[OPTIN_SMS]
                              ,[INCOME],[POSITION],[WORK_STATION],[CREATE_DATE]
                              ,[CREATE_USER],[LAST_MODI_DATE],[LAST_MODI_USER],[CARD_NO],[PASSWORD],[STATUS],LINK_PERSON_NAME2,LINK_PERSON_NAME3,LINK_PERSON_NAME1,LINK_PERSON_PHONE1,LINK_PERSON_PHONE2,LINK_PERSON_PHONE3
                          FROM [CUST_INFO] WHERE [CARD_NO]=@CARD_NO";
            return base.Get<CUST_INFO>(sql, new { CARD_NO = cardno });
        }


        //是否是会员
        public int CheckIsMember(string phone)
        {
            string sql = "SELECT COUNT(1) FROM CUST_INFO WHERE MOBILE=@MOBILE ";
            return base.Get<int>(sql, new { MOBILE = phone });
        }


        public int UpdateMemberInfo(int cid, int? point)
        {
            string sql = "UPDATE MEMBER_INFO SET USED_POINTS=USED_POINTS-@USED_POINTS,AVA_POINTS=AVA_POINTS-@USED_POINTS WHERE CUST_ID=@CUST_ID";
            return base.Excute(sql, new { USED_POINTS = point, CUST_ID = cid });
        }



        /// <summary>
        /// 根据客户分组id删除CUST_GROUP_CONDITION
        /// </summary>
        /// <param name="Group_ID"></param>
        /// <returns></returns>
        public int DeleteCustGroupCondition(int Group_ID)
        {
            string sql = @"delete CUST_GROUP_CONDITION where GROUP_ID=@GROUP_ID";
            return base.Excute(sql, new { GROUP_ID = Group_ID });
        }

        /// <summary>
        /// 根据客户分组id删除CUST_GROUP_DETAIL
        /// </summary>
        /// <param name="Group_ID"></param>
        /// <returns></returns>
        public int DeleteCustGroupDetail(int Group_ID)
        {
            string sql = @"delete CUST_GROUP_DETAIL where GROUP_ID=@GROUP_ID";
            return base.Excute(sql, new { GROUP_ID = Group_ID });
        }

        /// <summary>
        /// 删除客户分组
        /// </summary>
        /// <param name="Group_ID"></param>
        /// <returns></returns>
        public int DeleteCustGroup(int ID)
        {
            string sql = @"delete CUST_GROUP_Condition where GROUP_ID=@ID   delete CUST_GROUP_DETAIL where GROUP_ID=@ID   delete CUST_GROUP where ID=@ID";
            return base.Excute(sql, new { ID = ID });
        }

        


        #region 会员扩展项
       

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="cuObj"></param>
        /// <returns></returns>
        public int DeleteCUST_EXINFO_CONFIG(Guid id)
        {
            string sql = @"SELECT COUNT(1) FROM [CUST_EXINFO_CONFIG] where PARENT_ID=@PARENT_ID";
            var re = base.Get<int>(sql, new { PARENT_ID = id });
            if (re > 0)
                return -2;
            var dValue = base.Get<string>("SELECT EX_CODE FROM [CUST_EXINFO_CONFIG] where ID=@ID", new { ID = id });
            sql = "DELETE FROM CUST_EXINFO_CONFIG WHERE ID=@ID";


            if (re > 0)
                return -3;
            else
                return base.Excute(sql, new { ID = id });

        }

        

        //


       

        public int DeleteEXINFO_DATA(int custid)
        {
            string cols = @"Delete from CUST_EXINFO_DATA where CUST_ID=@CUST_ID";
            return base.Excute(cols, new { CUST_ID = custid });
        }

        #endregion



        public ORG_INFO GetOrgInfo()
        {
            string sql = @"SELECT * from org_info where  parent_id=0 and org_level=0 ";
            return base.Get<ORG_INFO>(sql, new { });
        }


        /// <summary>
        /// 判断是否是散客
        /// </summary>
        /// <param name="cust_id"></param>
        /// <returns>true散客 false会员</returns>
        public bool IS_VISITOR(int CUST_ID)
        {
            string sql = "SELECT [TYPE] FROM CUST_INFO WHERE ID=@CUST_ID";
            int TYPE = base.Get<int>(sql, new { CUST_ID = CUST_ID });
            if (TYPE == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

