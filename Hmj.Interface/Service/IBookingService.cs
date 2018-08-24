using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System;
using System.Collections.Generic;

namespace Hmj.Interface
{
    /// <summary>
    /// 预约服务接口
    /// by Levin 
    /// 2014-05-06
    /// </summary>
    public interface IBookingService
    {


        /// <summary>+根据门店和技师得到客户预约信息
        /// </summary>
        /// <param name="qStart"></param>
        /// <param name="qEnd"></param>
        /// <param name="masseurId"></param>
        /// <param name="subId"></param>
        /// <returns></returns>
        List<CUST_BOOKING_EX> QueryListByDate(DateTime qStart, DateTime qEnd, string masseurId, int subId);

        /// <summary>+根据ID 得到预约信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CUST_BOOKING_EX GetCust_Booking(int id);

        //顾客预约信息
        List<CUST_BOOKING_EX> getBookingByCid(int cid);

        /// <summary>+保存预约
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        int SaveBooking(CUST_BOOKING_EX booking);

        /// <summary>+未约进列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<CUST_BOOKING_EX> QueryFailList(BookingSearch search, PageView view);

        

        /// <summary>+微信列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<CUST_BOOKING_EX> QueryWxList(BookingSearch search, PageView view);

        //修改状态
        int UpdateBookingStatus(int? id, CUST_BOOKING_EX book);
        int UpdateWXBookingStatus(int? id, CUST_BOOKING_EX book);


        //某个技师预约的情况
        //List<CUST_BOOKING> QueryBookingTime(string mid, DateTime qstart, DateTime qend);

        PagedList<CUST_BOOKING_EX> QueryListByCustId(string cust_id, DateTime start, DateTime end, PageView view);
        PagedList<CUST_BOOKING_EX> QueryListByCustId(string cust_id, PageView view);

        //验证是否为会员
        int CheckIsMember(string phone);
        //验证是否男技师
        int checkMale(string masseurId);
        int SaveBookingReq(CUST_BOOKING_EX item, bool force, out string errmsg);

        //修改预约状态 取消 确认预约
        int UpdateBookStatus(int? id, int? status);



        /// <summary>
        /// 是否通过预约冲突检测
        /// </summary>
        /// <param name="cust_id"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>true通过冲突检测，无冲突；false，未通过冲突检测，存在冲突。</returns>
        bool CheckBookingConflict(int cust_id, DateTime startTime, DateTime endTime);


        PROD_CATEGORY GetMainPROD_CATEGORY(string categoryID);

        //ipad 获取门店预约列表  根据日期
        List<CUST_BOOKING_EX> getBookingByStoreId(int storeid, string bookingdate);


    }
}
