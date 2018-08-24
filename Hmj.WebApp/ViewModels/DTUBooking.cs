using System;

namespace Hmj.WebApp.ViewModels
{
    /// <summary>
    /// 预约信息数据
    /// </summary>
    public class DTUBooking
    {
        /// <summary>
        /// 标识
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public string Idx {get;set;} //标识
        /// <summary>
        ///  横轴标识 //横轴为技师 则为技师标识  横轴为房间 则为房间标识
        /// </summary>
        /// <value>
        /// The index of the col.
        /// </value>
        public string colIndex{get;set;}
        /// <summary>
        /// 横轴名称 //横轴为技师 则为技师名  横轴为房间 则为房间名称
        /// </summary>
        /// <value>
        /// The name of the col.
        /// </value>
        public string colName{get;set;}

        public int SubBranchId { get; set; }
        /// <summary>
        /// 房间编号
        /// </summary>
        /// <value>
        /// The room no.
        /// </value>
        public string BedId{get;set;}
        /// <summary>
        /// 房间名称
        /// </summary>
        /// <value>
        /// The name of the room.
        /// </value>
        public string BedName{get;set;}

        public string MemberId { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        /// <value>
        /// The user card no.
        /// </value>
        public string UserCard{get;set;} ///: "2121312", // 会员卡号
        /// <summary>
        ///会员卡类型 1 会员卡  2 临时卡
        /// </summary>
        /// <value>
        /// The type of the user card.
        /// </value>
        public byte UserCardType{get;set;} //会员卡类型 1 会员卡  2 临时卡
        /// <summary>
        /// 客户姓名
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserFullName { get; set; }// 客户姓名

        public string IdCard { get; set; }
        /// <summary>
        /// 客户手机号
        /// </summary>
        /// <value>
        /// The mobile phone.
        /// </value>
        public string MobilePhone{get;set;}// 客户手机号

        //public int PersonCount { get; set; }
        /// <summary>
        /// 预约项目编号
        /// </summary>
        /// <value>
        /// The type of the item.
        /// </value>
        public string ItemId{get;set;}
        /// <summary>
        /// 预约项目
        /// </summary>
        /// <value>
        /// The name of the item type.
        /// </value>
        public string ItemName{get;set;}
        /// <summary>
        /// 预约开始时间
        /// </summary>
        /// <value>
        /// The order start time.
        /// </value>
        public DateTime OrderStartTime{get;set;}
        /// <summary>
        /// 预约结束时间
        /// </summary>
        /// <value>
        /// The order end time.
        /// </value>
        public DateTime OrderEndTime{get;set;}
        /// <summary>
        /// 技师编号
        /// </summary>
        /// <value>
        /// The masseur no.
        /// </value>
        public string MasseurId{get;set;}
        /// <summary>
        /// 技师名称
        /// </summary>
        /// <value>
        /// The name of the masseur.
        /// </value>
        public string MasseurName{get;set;}
        /// <summary>
        /// 主题
        /// </summary>
        /// <value>
        /// The theme.
        /// </value>
        public int Theme{get;set;}
        /// <summary>
        /// 状态 1 新预约 2 已取消 3 已开单
        /// </summary>
        /// <value>
        ///  1 新预约 2 已取消 3 已开单
        /// </value>
        public byte Status{get;set;}

        public string Source { get; set; }

        public string OrderDate
        {
            get
            {
                return OrderStartTime.ToString("yyyy-MM-dd");
            }
        }
        public string OST
        {
            get
            {
                return OrderStartTime.ToString("HH:mm");
            }
        }
        public string OET
        {
            get
            {
                return OrderEndTime.ToString("HH:mm");
            }
        }

        public string Remark { get; set; }

        public bool CusReqMasseur { get; set; }

        public bool Is_Confirm { get; set; }

        public int CustId { get; set; }
    }
}