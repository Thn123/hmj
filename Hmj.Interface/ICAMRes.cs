namespace Hmj.Interface
{
    /// <summary>
    /// 静态资源服务和文件服务请求返回数据实体接口
    /// </summary>
    public interface ICAMRes
    {
        int Status { get; set; }
        string Message { get; set; }
        string MemberId { get; set; }
    }

    public interface ICredit
    {
        string UserCard { get; set; }
        decimal RemainedGPoints { get; set; }
        string UserLevel { get; set; }
        decimal ThenExpiredGPoints { get; set; }
    }
    /*
    ZBPEXT	CHAR(20)	BU_BPEXT	外部系统中的业务合作伙伴编号	会员卡号
    CREATED_TIME	DATS(8)	CREATE_DATE	积分明细日期	积分明细日期
    ORDER_TYPE	CHAR(40)	CRMT_DESCRIPTION	描述	类型
    SAPORDER_ID	CHAR(10)	CRMT_OBJECT_ID_DB	处理标识	对应SAP订单号码
    SAPITEM_ID	CHAR(10)	LOY_MA_ACT_ID	                                                            	积分单据号
    EXORDER_ID	CHAR(35)	CRMT_PO_NUMBER_UC	售达方的外部参考号 (大写字母)	对应线上单据号
    ITEM_ID	NUMC(10)	CRMT_ITEM_NO	凭证中的项目编号	对应项目号
    POINTS	DEC(10,2)	LOY_PT_POINT	                                                            	积分数
    PRODUCT_ID	CHAR(40)	LOY_MA_PRODUCT_ID	                                                            	产品ID
    PRODUCT_DESC	CHAR(40)	COMT_PRSHTEXTX	产品描述	产品描述
    CATEGORY	CHAR(30)	LOY_MA_CAT_ID	                                                            	积分类型标志
    QUANTITY	QUAN(13,3)	MENGE_D	数量	产品数量
    QTY_UNIT	UNIT(3)	LOY_MA_PROCESS_QTY_UNIT	                                                            	产品数量单位
    EXPIRY_DATE	DATS(8)	LOY_FRW_CREATED_ON		积分到期日期
    */
    public interface ICreditDetail
    {
        string UserCard { get; set; }
        string CreateTime { get; set; }
        string OrderType { get; set; }
        string SAPOderId { get; set; }
        string SAPItemId { get; set; }
        string CRMOrder_Id { get; set; }
        string ItemId { get; set; }
        decimal Points { get; set; }
        string ProductId { get; set; }
        string ProductDesc { get; set; }
        string Category { get; set; }
        decimal Quantity { get; set; }
        string QTYUnit { get; set; }
        string ExpiryDate {get;set;}    
    }

}
