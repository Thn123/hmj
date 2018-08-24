using Hmj.Entity;
using Hmj.Entity.Entities;
using System;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class BcjBookRepository : BaseRepository
    {
        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <returns></returns>
        public List<BCJ_CITY> GetCity()
        {
            string sql = "SELECT * FROM BCJ_CITY";

            return base.Query<BCJ_CITY>(sql, new { });
        }

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <returns></returns>
        public List<BCJ_CITY> GetAvalibleCity()
        {
            string sql = "SELECT * FROM BCJ_CITY where ISNULL(LATITUDE,'无') !='无' ";

            return base.Query<BCJ_CITY>(sql, new { });
        }

        /// <summary>
        /// 改变预约状态
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public int ChageBookStatus(string bookid, string v,string whoid)
        {
            string sql = "UPDATE BCJ_BOOK SET STATUS=@STATUS,LAST_USER=@LAST_USER WHERE ID=@ID";

            return base.Excute(sql, new { STATUS = v, ID = bookid, LAST_USER= whoid });
        }

        /// <summary>
        /// 检查是否有人预约
        /// </summary>
        /// <param name="store_id"></param>
        /// <param name="book_date"></param>
        /// <param name="book_time"></param>
        /// <returns></returns>
        public int CheckBooking(string store_id, string book_date, string book_time)
        {
            string sql = "SELECT COUNT(0) FROM BCJ_BOOK WHERE BOOK_DATE=@BOOK_DATE AND BOOK_TIME=@BOOK_TIME AND STORE_ID=@STORE_ID AND STATUS != 2";

            return base.Get<int>(sql, new
            {
                BOOK_DATE = book_date,
                BOOK_TIME = book_time,
                STORE_ID = store_id
            });
        }

        /// <summary>
        /// 得到历史预约记录
        /// </summary>
        /// <param name="member_Id"></param>
        /// <returns></returns>
        public List<HMJ_BOOK_EX> GetBookHis(string member_Id)
        {
            string sql = @"SELECT A.*,C.NAME MEMBER_NAME,replace(B.NAME, ' ', '') STORE_NAME  FROM dbo.BCJ_BOOK A
LEFT JOIN dbo.BCJ_STORES B ON A.STORE_ID=B.ID
LEFT JOIN dbo.CUST_MEMBER C ON A.STORE_ID=C.ID
WHERE MEMBER_ID=@MEMBER_ID";

            return base.Query<HMJ_BOOK_EX>(sql, new { MEMBER_ID = member_Id, BOOK_DATE = DateTime.Now });
        }

        /// <summary>
        /// 得到详细信息
        /// </summary>
        /// <param name="gropuID"></param>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public int GetHisCount(string gropuID, string memberID)
        {
            string sql = "SELECT COUNT(0) FROM dbo.SURREY_HIS WHERE MEMBER_ID=@MEMBER_ID AND GRUOP_ID=@GRUOP_ID";

            return base.Get<int>(sql, new { MEMBER_ID = memberID, GRUOP_ID = gropuID });
        }

        /// <summary>
        /// 得到预约列表
        /// </summary>
        /// <param name="store_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<HMJ_BOOK_EX> LoadBook(string store_id, string date)
        {
            string sql = @"SELECT A.* FROM dbo.BCJ_BOOK A
WHERE STORE_ID = @STORE_ID AND BOOK_DATE = @BOOK_DATE";

            return base.Query<HMJ_BOOK_EX>(sql, new
            {
                STORE_ID = store_id,
                BOOK_DATE = date
            });
        }

        /// <summary>
        /// 得到预约日历
        /// </summary>
        /// <param name="dates"></param>
        /// <param name="store_id"></param>
        /// <returns></returns>
        public List<BOOK_TIMES> GetStoreData(string dates, string store_id)
        {
            string sql = @"SELECT BOOK_DATE,COUNT(0) NUMBER,BOOK_DATE BOOK_DATE_STR FROM dbo.BCJ_BOOK
WHERE STORE_ID=@STORE_ID
GROUP BY BOOK_DATE HAVING BOOK_DATE IN (" + dates + ") ORDER BY BOOK_DATE";

            return base.Query<BOOK_TIMES>(sql, new { STORE_ID = store_id });
        }

        /// <summary>
        /// 根据优惠券查询
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public int CheckBookingByVoucher(string number)
        {
            string sql = "SELECT COUNT(0) FROM BCJ_BOOK WHERE VOUCHER_NO=@VOUCHER_NO AND STATUS != 2";

            return base.Get<int>(sql, new
            {
                VOUCHER_NO = number
            });
        }

        /// <summary>
        /// 修改预约
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="bookDate"></param>
        /// <param name="bookTime"></param>
        /// <returns></returns>
        public string UpdateBook(string bookid, string bookDate, string bookTime)
        {
            string sql = "UPDATE BCJ_BOOK SET BOOK_DATE=@BOOK_DATE, BOOK_TIME=@BOOK_TIME WHERE ID=@ID";

            return base.Excute(sql, new { BOOK_DATE = bookDate, BOOK_TIME = bookTime, ID = bookid }).ToString();
        }

        /// <summary>
        /// 得到预约信息
        /// </summary>
        /// <param name="zCP_NUM"></param>
        /// <returns></returns>
        public HMJ_BOOK_EX GetBookByNo(string zCP_NUM)
        {
            string sql = "SELECT * FROM dbo.BCJ_BOOK WHERE VOUCHER_NO=@VOUCHER_NO AND STATUS != 2";

            return base.Get<HMJ_BOOK_EX>(sql,new { VOUCHER_NO = zCP_NUM });
        }
    }
}
