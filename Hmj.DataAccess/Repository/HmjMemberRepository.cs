using Hmj.Entity;
using Hmj.Entity.PageSearch;
using System.Collections.Generic;
using Hmj.Entity.Entities;
using System;

namespace Hmj.DataAccess.Repository
{
    public class HmjMemberRepository : BaseRepository
    {
        /// <summary>
        /// 得到一个实体类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CUST_MEMBER GetMemberModel(string id)
        {
            string sql = "SELECT * FROM  dbo.CUST_MEMBER WHERE ID=@ID";

            return base.Get<CUST_MEMBER>(sql, new { ID = id });
        }

        /// <summary>
        /// 得到集合
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<CUST_MEMBER> GetMemberList(MemberSearch search)
        {
            string sql = "SELECT * FROM  dbo.CUST_MEMBER";

            return base.Query<CUST_MEMBER>(sql, new { });
        }

        /// <summary>
        /// 得到会员详情
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public HmjMemberDetail GetMemberDetailByOpenId(string openid)
        {
            string sql = @"SELECT B.*,A.NAME NICK_NAME,A.IMAGE,C.IS_SEND FROM dbo.WXCUST_FANS A
LEFT JOIN dbo.CUST_MEMBER B ON A.ID = B.FANS_ID
LEFT JOIN MEMBER_MSG C ON B.ID=C.MEMBER_ID
WHERE A.FROMUSERNAME = @FROMUSERNAME AND B.ID IS NOT NULL";

            return base.Get<HmjMemberDetail>(sql, new { FROMUSERNAME = openid });
        }

        /// <summary>
        /// 更新粉丝头像信息
        /// </summary>
        /// <param name="valtr"></param>
        /// <param name="fans_id"></param>
        /// <returns></returns>
        public string UpdateImageUrl(string valtr, string openid)
        {
            string sql = @"UPDATE WXCUST_FANS SET IMAGE=@IMAGE
WHERE FROMUSERNAME = @FROMUSERNAME";

            return base.Excute(sql, new { IMAGE = valtr, FROMUSERNAME = openid }) > 0 ? "1" : "0";
        }

        /// <summary>
        /// 修改微信昵称
        /// </summary>
        /// <param name="oPENID"></param>
        /// <param name="nICK_NAME"></param>
        public void UpdateNick(string oPENID, string nICK_NAME)
        {
            string sql = "UPDATE dbo.WXCUST_FANS SET NAME=@Name WHERE FROMUSERNAME=@OPENID";

            base.Excute(sql, new { Name = nICK_NAME, OPENID = oPENID });
        }

        /// <summary>
        /// 得到汇率表
        /// </summary>
        /// <param name="vGROUP2"></param>
        /// <returns></returns>
        public EXCHANGE_RATE GetRateByBreadCode(string vGROUP2)
        {
            string sql = @"SELECT * FROM dbo.EXCHANGE_RATE WHERE BRAND_CODE=@BRAND_CODE";

            return base.Get<EXCHANGE_RATE>(sql, new { BRAND_CODE = vGROUP2 });
        }

        /// <summary>
        /// 得到发送信息
        /// </summary>
        /// <param name="pARTNER"></param>
        /// <returns></returns>
        public MEMBER_MSG GetMsgByBP(int id)
        {
            string sql = "SELECT * FROM dbo.MEMBER_MSG WHERE MEMBER_ID=@MEMBER_ID";

            return base.Get<MEMBER_MSG>(sql, new { MEMBER_ID = id });
        }

        /// <summary>
        /// 得到会员详细信息
        /// </summary>
        /// <param name="pARTNER"></param>
        /// <returns></returns>
        public CUST_MEMBER GetMemberByBP(string pARTNER)
        {
            string sql = "SELECT * FROM dbo.CUST_MEMBER WHERE PARTNER=@PARTNER";

            return base.Get<CUST_MEMBER>(sql, new { PARTNER = pARTNER });
        }

        /// <summary>
        /// 得到转化成华美家的等级
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public MEMBER_LVL GetHmjLvl(string lvl, string codes)
        {
            string sql = @"SELECT TOP 1 * FROM  MEMBER_LVL 
WHERE BRAND_CODE IN (" + lvl + ") AND BRAND_LVL IN(" + codes + @")
ORDER BY MEMBER_NUMBER DESC";
            return base.Get<MEMBER_LVL>(sql, new { BRAND_CODE = lvl, BRAND_LVL = codes });
        }

        /// <summary>
        /// 得到配置表
        /// </summary>
        /// <param name="template_Code"></param>
        /// <returns></returns>
        public List<WX_TMP_CONFIG> GetTmps(string template_Code)
        {
            string sql = "SELECT * FROM WX_TMP_CONFIG WHERE TMP_ID = @TMP_ID";

            return base.Query<WX_TMP_CONFIG>(sql, new { TMP_ID = template_Code });
        }

        /// <summary>
        /// 通过会员详细信息中的会员编码获取等级对应的中文
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public MEMBER_LVL GetHmjSelfLvl(string codes)
        {
            string sql = @"SELECT TOP 1 * FROM  MEMBER_LVL WHERE BRAND_LVL ='" + codes + @"' ORDER BY MEMBER_NUMBER DESC";
            return base.Get<MEMBER_LVL>(sql, new { BRAND_LVL = codes });
        }

        /// <summary>
        /// 查询模板发送记录
        /// </summary>
        /// <param name="IS_SEND"></param>
        /// <returns></returns>
        public List<WX_TMP_HIS> GetWxTmpHisByIsSend(int IS_SEND)
        {
            string sql = @"SELECT 
                            ID,
                            OPENID,
                            CONTACT_TYPE,
                            TMP_ID,
                            CAMPAIGN_CODE,
                            CAMPAIGN_NAME,
                            VGROUP,
                            DATA_SOURCE,
                            SOURCE_SYSTEM,
                            LOYALTY_BRAND,
                            ISREAL_TIME,
                            INVOKE_TIME,
                            SEND_TIME,
                            DETAIL,
                            IS_SEND,
                            IS_SELECT,
                            SEND_MSG 
                            FROM WX_TMP_HIS WHERE isnull(IS_SEND,0)=@IS_SEND";
            return base.Query<WX_TMP_HIS>(sql, new { IS_SEND = IS_SEND });
        }

        /// <summary>
        /// 修改模板消息发送记录
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IS_SEND"></param>
        /// <returns></returns>
        public int UpdateWxTmpHisIsSendByID(int ID,int IS_SEND,string result)
        {
            string sql = "UPDATE WX_TMP_HIS SET IS_SEND=@IS_SEND,SEND_MSG=@SEND_MSG WHERE ID=@ID";
            return base.Excute(sql, new { IS_SEND = IS_SEND, ID = ID , SEND_MSG = result });
        }

        public long insertWXCouponGiveInfo(WXCouponGiveInfo WXCouponGiveInfo)
        {
            return base.Insert(WXCouponGiveInfo);
        }

        public WXCouponGiveInfo GetWXCouponGiveInfoByOpenid(string Openid)
        {
            string sql = "select id,Openid,CouponNo,CardId,CreateDate,CouponGetDate,UseDate,UseStoreCode from WXCouponGiveInfo with(nolock) where Openid=@Openid";
            return base.Get<WXCouponGiveInfo>(sql, new { Openid = Openid });
        }


        public List<WXCouponNoInfo> QueryWXCouponNoInfo()
        {
            string sql = "select id,CouponNo from WXCouponNoInfo where isnull(IsImport,0)=0";
            return base.Query<WXCouponNoInfo>(sql, new { });
        }

        public int UpdateWXCouponNoInfoIsImport(long id)
        {
            string sql = "update WXCouponNoInfo set IsImport=1 ,ImportDate=getdate() where  id=@id ";
            return base.Excute(sql,new { id = id });
        }

        public WXCouponGiveInfo CanGetCoupon(string OpenId,string cardId)
        {
            string sql = "select Openid,CouponNo,[Status],CardId from WXCouponGiveInfo with(nolock) where Openid=@OpenId and isnull(Status,0)=0 and cardId=@cardId";
            return base.Get<WXCouponGiveInfo>(sql, new { OpenId= OpenId, cardId= cardId });
        }

        public int UpdateWXCouponGiveInfoIsHX(string CouponNo)
        {
            string sql = "update WXCouponGiveInfo set Status=2 ,UseDate=getdate() where  CouponNo=@CouponNo ";
            return base.Excute(sql, new { CouponNo = CouponNo });
        }

        public CardApiTicket GetModelCardApi()
        {
            string sql = "select  top 1 Id,Cardapi_Ticket,GetTicketTime from CardApiTicket Order by GetTicketTime desc";
            return base.Get<CardApiTicket>(sql, new { });
        }

        /// <summary>
        /// 查询用户某卡券获取资格
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public List<WXCouponGiveInfo> GetWXCouponGiveInfo(string openid,string cardid)
        {
            string sql = @"select Openid,CouponNo,Status,CardId,CreateDate,CouponGetDate,UseDate,UseStoreCode from WXCouponGiveInfo with(nolock)  where Openid=@openid and CardId=@cardid";

            return base.Query<WXCouponGiveInfo>(sql, new { openid = openid, cardid= cardid });
        }
    }
}
