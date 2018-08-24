using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using System;
using System.Collections.Generic;

namespace Hmj.Business.ServiceImpl
{
    public class BcjBookService : IBcjBookService
    {
        BcjBookRepository _bcjstor;
        public BcjBookService()
        {
            _bcjstor = new BcjBookRepository();
        }

        /// <summary>
        /// 改变预约状态
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public string ChageBookStatus(string bookid, string v,string whoid="")
        {
            return _bcjstor.ChageBookStatus(bookid, v, whoid).ToString();
        }

        /// <summary>
        /// 得到预约列表
        /// </summary>
        /// <param name="member_Id"></param>
        /// <returns></returns>
        public List<HMJ_BOOK_EX> GetBooks(string member_Id)
        {
            List<HMJ_BOOK_EX> books = _bcjstor.GetBookHis(member_Id);

            return books;
        }

        /// <summary>
        /// 得到城市列表
        /// </summary>
        /// <returns></returns>
        public List<BCJ_CITY> GetCity()
        {
            return _bcjstor.GetCity();
        }

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <returns></returns>
        public List<BCJ_CITY> GetAvalibleCity()
        {
            return _bcjstor.GetAvalibleCity();
        }

        /// <summary>
        /// 佰草集预约日历
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public List<BOOK_TIMES> GetStoreData(string dates, string store_id)
        {
            return _bcjstor.GetStoreData(dates, store_id);
        }

        /// <summary>
        /// 加载预约列表
        /// </summary>
        /// <param name="store_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<HMJ_BOOK_EX> LoadBook(string store_id, string date)
        {
            return _bcjstor.LoadBook(store_id, date);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        public string SaveHis(string loadData, string MemberID, string GropuID)
        {
            int count = _bcjstor.GetHisCount(GropuID, MemberID);

            if (count > 0)
            {
                return "-1";
            }

            string[] strArry = loadData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<SURREY_HIS> hisArry = new List<SURREY_HIS>();

            foreach (string item in strArry)
            {
                string[] an = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                string SUB_ID = an[0];

                string[] ANSWER_IDS = an[1].Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string ANSWER_ID in ANSWER_IDS)
                {
                    SURREY_HIS his = new SURREY_HIS();
                    his.CREATE_TIME = DateTime.Now;
                    his.GRUOP_ID = int.Parse(GropuID);
                    his.SUB_ID = int.Parse(SUB_ID);
                    his.ANSWER_ID = int.Parse(ANSWER_ID);
                    his.MEMBER_ID = int.Parse(MemberID);

                    hisArry.Add(his);
                }
            }

            return _bcjstor.BatchInsert(hisArry).ToString();
        }

        /// <summary>
        /// 预约开始
        /// </summary>
        /// <param name="number"></param>
        /// <param name="member_id"></param>
        /// <param name="store_id"></param>
        /// <param name="book_date"></param>
        /// <param name="book_time"></param>
        /// <returns></returns>
        public string ToBook(string number, string member_id,
            string store_id, string book_date, string book_time, string book_name, string book_phone)
        {
            //第一步判断该日期是否被别人预约
            int count = _bcjstor.CheckBooking(store_id, book_date, book_time);
            
            if (count >= 1)
            {
                return "-1";
            }

            //判断优惠券是不是已经约过
            int voconunt = _bcjstor.CheckBookingByVoucher(number);

            if (voconunt >= 1)
            {
                return "-3";
            }

            //插入预约数据
            BCJ_BOOK bookModel = new BCJ_BOOK()
            {
                BOOK_DATE = DateTime.Parse(book_date),
                BOOK_TIME = int.Parse(book_time),
                MEMBER_ID = int.Parse(member_id),
                STORE_ID = int.Parse(store_id),
                VOUCHER_NO = number,
                STATUS = 0,
                MEMBER_NAME = book_name,
                MOBILE = book_phone
            };

            long newid = 0L;// _bcjstor.Insert(bookModel);

            //预约成功
            if (newid <= 0)
            {
                return "-2";
            }

            //到这里表示插入预约成功了，此时还要调用接口去核销优惠券（没写）。

            return "1";

        }

        /// <summary>
        /// 修改预约
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="bookDate"></param>
        /// <param name="bookTime"></param>
        /// <returns></returns>
        public string UpdateBook(string bookid, string bookDate, string bookTime, string Store_Id)
        {
            //第一步判断该日期是否被别人预约
            int count = _bcjstor.CheckBooking(Store_Id, bookDate, bookTime);

            if (count >= 1)
            {
                return "-1";
            }

            return _bcjstor.UpdateBook(bookid, bookDate, bookTime);
        }
    }
}
