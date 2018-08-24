using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;

namespace Hmj.DataAccess.Repository
{
    public class CustMemberRepository : BaseRepository
    {
        /// <summary>
        /// 根据openid获取会员普通信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public CUST_MEMBER GetMemberByOpenId(string openid)
        {
            string sql = "SELECT * FROM dbo.CUST_MEMBER WHERE FANS_ID=(SELECT ID FROM dbo.WXCUST_FANS WHERE FROMUSERNAME=@FROMUSERNAME)";

            return base.Get<CUST_MEMBER>(sql, new { FROMUSERNAME = openid });
        }


        /// <summary>
        /// 根据手机号获取会员普通信息 华美家专用
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public CUST_MEMBER GetMemberByMobile(string mobile)
        {
            string sql = "SELECT * FROM dbo.CUST_MEMBER WHERE MOBILE=@MOBILE";

            return base.Get<CUST_MEMBER>(sql, new { MOBILE = mobile });
        }

        /// <summary>
        /// 根据手机号获取会员普通信息 原佰草集项目也在使用
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public CUST_MEMBER GetMemberByMobile(string mobile, int id)
        {
            string sql = "SELECT * FROM dbo.CUST_MEMBER WHERE MOBILE=@MOBILE OR FANS_ID=@FANS_ID";

            return base.Get<CUST_MEMBER>(sql, new { MOBILE = mobile, FANS_ID = id });
        }

        /// <summary>
        /// 获取粉丝信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public WXCUST_FANS GetFans(string openid)
        {
            string sql = "SELECT TOP 1 * FROM dbo.WXCUST_FANS WHERE FROMUSERNAME=@FROMUSERNAME";

            return base.Get<WXCUST_FANS>(sql, new { FROMUSERNAME = openid });
        }

        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="mebers">如果AVA_POINTS积分赋值-1则表示不更新</param>
        /// <returns></returns>
        public int UpdateMember(CUST_MEMBER mebers)
        {
            string sqlex = string.Empty;

            if (mebers.AVA_POINTS >= 0)
            {
                sqlex += ",AVA_POINTS=@AVA_POINTS";
            }

            if (!string.IsNullOrEmpty(mebers.LOGINPASSON))
            {
                sqlex += ",LOGINPASSON=@LOGINPASSON";
            }

            if (!string.IsNullOrEmpty(mebers.ZZAFLD000004))
            {
                sqlex += ",ZZAFLD000004=@ZZAFLD000004";
            }

            if (!string.IsNullOrEmpty(mebers.MOBILE))
            {
                sqlex += ",MOBILE=@MOBILE";
            }

            if (!string.IsNullOrEmpty(mebers.MEMBERNO))
            {
                sqlex += ",MEMBERNO=@MEMBERNO";
            }

            string sql = @"UPDATE CUST_MEMBER SET  BIRTHDAY=@BIRTHDAY, 
NAME = @NAME,STORE = @STORE, ADDRESS = @ADDRESS,GENDER = @GENDER,MEM_LEVEL = @MEM_LEVEL,NAME_LAST=@NAME_LAST,NAME_FIRST=@NAME_FIRST  "
+ sqlex + " WHERE ID = @ID";

            return base.Excute(sql, new
            {
                AVA_POINTS = mebers.AVA_POINTS,
                BIRTHDAY = mebers.BIRTHDAY,
                NAME = mebers.NAME,
                ADDRESS = mebers.ADDRESS,
                GENDER = mebers.GENDER,
                MEM_LEVEL = mebers.MEM_LEVEL,
                STORE = mebers.STORE,
                ID = mebers.ID,
                LOGINPASSON = mebers.LOGINPASSON,
                MOBILE = mebers.MOBILE,
                MEMBERNO = mebers.MEMBERNO,
                NAME_LAST = mebers.NAME_LAST,
                NAME_FIRST = mebers.NAME_FIRST,
                ZZAFLD000004 = mebers.ZZAFLD000004,
            });
        }

        /// <summary>
        /// 分页得到数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<SURREY_GROUP_EX> QueryGetGroups(GroupSearch search, PageView view)
        {
            return base.PageGet<SURREY_GROUP_EX>(view, "*", "SURREY_GROUP", "", "id", " order by ID desc");
        }

        public int DeleteMember(string fROMUSERNAME)
        {
            string sql = "DELETE CUST_MEMBER WHERE FANS_ID=(SELECT ID FROM dbo.WXCUST_FANS WHERE FROMUSERNAME=@FROMUSERNAME)";

            return base.Excute(sql, new { FROMUSERNAME = fROMUSERNAME });
        }

        /// <summary>
        /// 修改当前粉丝的状态
        /// </summary>
        /// <param name="iD"></param>
        /// <returns></returns>
        public string UpdateIsRegion(int iD)
        {
            string sql = "UPDATE dbo.WXCUST_FANS SET IS_REGISTER=1 WHERE ID=@ID";

            return base.Excute(sql, new { ID = iD }).ToString();
        }

        /// <summary>
        /// 得到bp号
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string GetOldMember(string openid)
        {
            string sql = @"SELECT B.BP FROM TB_Customer_Info A
LEFT JOIN TB_OLD B ON A.Unionid=B.Unionid
WHERE A.OpenId=@OpenId";

            return base.Get<string>(sql, new { OpenId = openid });
        }


        public WXCustScanRecordEx GetLastWXCustScanRecord(string openid)
        {
            string sql = @"SELECT top 1 a.Id,a.TOUSERNAME,a.FROMUSERNAME,a.MSGTYPE,a.WXEVENT,REPLACE(a.EVENTKEY,'qrscene_','') EVENTKEY,a.CREATE_DATE ,b.vgroup,b.[source]
                            FROM WXCustScanRecord a  with(nolock) 
                            inner join BCJ_STORES b  with(nolock)  on (a.EVENTKEY=b.POS_CODE or REPLACE(a.EVENTKEY,'qrscene_','')=b.POS_CODE)
                             where a.FROMUSERNAME=@FROMUSERNAME order by a.id desc ";
            return base.Get<WXCustScanRecordEx>(sql, new { FROMUSERNAME =openid});
        }
    }
}
