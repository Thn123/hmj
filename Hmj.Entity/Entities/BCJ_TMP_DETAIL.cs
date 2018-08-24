using System;

namespace Hmj.Entity.Entities
{
    public  class BCJ_TMP_DETAIL
    {
        /// <summary>
        /// 活动编号
        /// </summary>
        public string Campaign_Code { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Campaign_Name { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string STORE_NAME { get; set; }

        /// <summary>
        /// 销售组织
        /// </summary>
        public string Vgroup { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public string Data_source { get; set; }

        /// <summary>
        /// 忠诚度品牌
        /// </summary>
        public string Loyalty_Brand { get; set; }

        /// <summary>
        /// 是否实时 0：否；1：是
        /// </summary>
        public bool IsRealTime { get; set; }

        /// <summary>
        /// 调用时间
        /// </summary>
        public DateTime Invoke_Time { get; set; }

        /// <summary>
        /// 发送时间 请精确到小时
        /// </summary>
        public DateTime Send_Time { get; set; }

        /// <summary>
        /// 关联记录编号
        /// </summary>
        public string CamActivity_Code { get; set; }

        /// <summary>
        /// 消息模板编号
        /// </summary>
        public string Template_Code { get; set; }


        /// <summary>
        /// 联系方式 openID
        /// </summary>
        public string Contact_Information { get; set; }

        /// <summary>
        /// 消息模板示例内容
        /// </summary>
        public string Template_Content { get; set; }

        /// <summary>
        /// 模板消息要跳转的链接
        /// </summary>
        public string Redirect_Url { get; set; }

        /// <summary>
        /// 参数1
        /// </summary>
        public string P_1 { get; set; }

        /// <summary>
        /// 参数2
        /// </summary>
        public string P_2 { get; set; }

        /// <summary>
        /// 参数3
        /// </summary>
        public string P_3 { get; set; }

        /// <summary>
        /// 参数4
        /// </summary>
        public string P_4 { get; set; }

        /// <summary>
        /// 参数5
        /// </summary>
        public string P_5 { get; set; }

        /// <summary>
        /// 参数6
        /// </summary>
        public string P_6 { get; set; }

        /// <summary>
        /// 参数7
        /// </summary>
        public string P_7 { get; set; }
    }
}
