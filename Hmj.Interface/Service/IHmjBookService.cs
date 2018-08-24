using Hmj.Entity;
using Hmj.Entity.Entities;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IBcjBookService
    {
        /// <summary>
        /// 得到城市
        /// </summary>
        /// <returns></returns>
        List<BCJ_CITY> GetCity();

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <returns></returns>
        List<BCJ_CITY> GetAvalibleCity();

        /// <summary>
        /// 预约
        /// </summary>
        /// <param name="number"></param>
        /// <param name="member_id"></param>
        /// <param name="store_id"></param>
        /// <param name="book_date"></param>
        /// <param name="book_time"></param>
        /// <returns></returns>
        string ToBook(string number, string member_id, string store_id,
            string book_date, string book_time,string book_name,string book_phone);

        /// <summary>
        /// 得到预约显示列表
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        List<BOOK_TIMES> GetStoreData(string dates,string store_id);

        /// <summary>
        /// 加载预约列表
        /// </summary>
        /// <param name="store_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        List<HMJ_BOOK_EX> LoadBook(string store_id, string date);

        /// <summary>
        /// 得到预约历史
        /// </summary>
        /// <param name="member_Id"></param>
        /// <param name="flg">1：预约中 2：预约历史</param>
        /// <returns></returns>
        List<HMJ_BOOK_EX> GetBooks(string member_Id);

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        string SaveHis(string loadData,string MemberID,string GropuID);


        /// <summary>
        /// 改变预约状态
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        string ChageBookStatus(string bookid, string v,string whoid);

        /// <summary>
        /// 修改预约状态
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="bookDate"></param>
        /// <param name="bookTime"></param>
        /// <returns></returns>
        string UpdateBook(string bookid, string bookDate, string bookTime,string Store_Id);
    }
}
