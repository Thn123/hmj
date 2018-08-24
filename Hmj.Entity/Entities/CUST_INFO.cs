using System;
using System.Collections.Generic;

namespace Hmj.Entity
{
    //public partial class CUST_INFO1
    //{
    //    public string STORE_NAME { get; set; }
    //    //public string MemberId { get; set; }
    //    public string FullName { get; set; }
    //    public string MobilePhone { get; set; }
    //    public string UserCard { get; set; }
    //}



    public class CustomerPoints
    {
        public List<PointsDetail> PointsDetailList { get; set; }
    }

    public class GPoint
    {
        public string Member_No { get; set; }//会员编号
        public string Mem_Lvl { get; set; }//会员等级
        public int Ava_Points { get; set; }//可用积分
        public int Noaval_Points { get; set; }//过期积分
    }

    public class PointsDetail
    {
        public int Cust_Id { get; set; }
        public int Member_Id { get; set; }
        public DateTime Trans_Date { get; set; }
        public string Trans_Type { get; set; }
        public int Order_Id { get; set; }
        public int Redemp_Id { get; set; }
        public int Points { get; set; }
        public int Ava_Points { get; set; }
    }



    public class Order
    {
        public string MemberID { get; set; }
        public string ProjectName { get; set; }
        public int TotalCnt { get; set; }
        public int UsedCnt { get; set; }
        public int ValidCNT { get; set; }
        public decimal Price { get; set; }
        public string CardStatus { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }


    }
