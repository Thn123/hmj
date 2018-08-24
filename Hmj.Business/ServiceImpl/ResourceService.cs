using Hmj.Common.CacheManager;
using Hmj.DataAccess.Repository;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
namespace Hmj.Business.ServiceImpl
{
    public class ResourceService : IResourceService
    {
        private ResourceRepository dRepo;
        public ResourceService()
        {
            dRepo = new ResourceRepository();
        }

        public List<IResource> GetResourceByCode(string code)
        {
            return GetResourceByCode(code, false);
        }
        public List<IResource> GetResourceByCode(string code, bool hasAllOption)
        {
            return GetResourceByCode(code, "", hasAllOption);
        }
        public List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption)
        {
            object cache = CacheFactory.Instance.CreateCoreCacheInstance().Get("RESOURCE_" + code + "_" + parentCode);
            List<IResource> list = null;
            if (cache != null)
            {
                list = cache as List<IResource>;
            }
            if (list != null)
            {
                if (hasAllOption)
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择...", Value = null });
                return list;
            }
            switch (code)
            {
                case "AgeType":
                     list = new List<IResource>();

                     list.Add(new DefaultResource() { Code = "0", Name = "0-6个月", Value = "0" });
                     list.Add(new DefaultResource() { Code = "1", Name = "6-12个月", Value = "1" });
                     list.Add(new DefaultResource() { Code = "2", Name = "12-18个月", Value = "2" });
                     list.Add(new DefaultResource() { Code = "3", Name = "18个月以上", Value = "3" });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择适用年龄", Value = null });
                    break;
                case "PType": // 商品类别
                    list = new List<IResource>();

                    list.Add(new DefaultResource() { Code = "奶嘴", Name = "奶嘴", Value = "奶嘴" });
                    list.Add(new DefaultResource() { Code = "奶瓶", Name = "奶瓶", Value = "奶瓶" });
                    list.Add(new DefaultResource() { Code = "喂哺电器", Name = "喂哺电器", Value = "喂哺电器" });
                    list.Add(new DefaultResource() { Code = "储奶袋", Name = "储奶袋", Value = "储奶袋" });
                    list.Add(new DefaultResource() { Code = "安抚奶嘴", Name = "安抚奶嘴", Value = "安抚奶嘴" });
                    list.Add(new DefaultResource() { Code = "喂哺附件", Name = "喂哺附件", Value = "喂哺附件" });
                    list.Add(new DefaultResource() { Code = "学饮杯", Name = "学饮杯", Value = "学饮杯" });
                    list.Add(new DefaultResource() { Code = "餐具", Name = "餐具", Value = "餐具" });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择商品类型", Value = null });
                    break;
                case "StoreType": // 门店类别
                    list = new List<IResource>();

                    list.Add(new DefaultResource() { Code = "1", Name = "销售门店", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "维修门店", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "办公室", Value = "3" });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择门店类型", Value = null });
                    break;
                case "BelongsArea"://所属区域
                    list = new List<IResource>();

                    var balist = dRepo.GetSelArea();
                    list = balist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.AreaNo.ToString(), Name = x.Name, Value = x.AreaNo.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选门店所属区域", Value = null });
                    break;
                case "TemplateArea"://模板消息所属大区
                    list = new List<IResource>();

                    var templist = dRepo.GetTemplateArea(parentCode);
                    list = templist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    });
                    break;
                case "TemplateArea1"://模板消息所属区域
                    list = new List<IResource>();

                    var templist1 = dRepo.GetTemplateArea(parentCode);
                    list = templist1.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    });
                    break;
                case "TemplateArea2"://模板消息所属门店
                    list = new List<IResource>();

                    var templist2 = dRepo.GetTemplateArea(parentCode);
                    list = templist2.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    });
                    break;
                case "TemplateArea3"://模板消息所属标签
                    list = new List<IResource>();

                    var templist3 = dRepo.GetTemplateArea(parentCode);
                    list = templist3.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    });
                    break;
                //case "TemplateArea4"://模板消息所属标签
                //    list = new List<IResource>();

                //    var templist4 = dRepo.GetTemplateList(parentCode);
                //    list = templist4.ConvertAll<IResource>(x =>
                //    {
                //        return new DefaultResource() { Code = x.ID.ToString(), Name = x.TEMPLATE_NAME, Value = x.ID.ToString() };
                //    });
                //    list.Insert(0, new DefaultResource() { Code = "0", Name = "请选择模板", Value = "0" });
                //    break;
                case "SelectOrNot": // 有效 无效
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "必选", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "可选", Value = "0" });
                    break;

                case "Enabled_No": // 有效 无效
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "有效", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "无效", Value = "0" });
                    break;
                case "YES_NO": // 是否
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "是", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "否", Value = "0" });
                    break;
                case "CARD_STATUS": // 是否
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "启用", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "关闭", Value = "0" });

                    break;
                case "TicketType":  //优惠券类型
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "抵扣券", Name = "抵扣券", Value = "抵扣券" });
                    list.Add(new DefaultResource() { Code = "现金券", Name = "现金券", Value = "现金券" });
                    list.Add(new DefaultResource() { Code = "抵用券", Name = "抵用券", Value = "抵用券" });
                    list.Add(new DefaultResource() { Code = "兑换券", Name = "兑换券", Value = "兑换券" });
                    break;
                case "CARD_STATUS_DROP": // 是否
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "启用", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "关闭", Value = "0" });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择会员卡状态", Value = null });
                    break;
                case "CAN_SALE": // 可售不可售
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "可售", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "不可售", Value = "0" });

                    break;
                case "GENDER": // 男女
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "男", Name = "男", Value = "男" });
                    list.Add(new DefaultResource() { Code = "女", Name = "女", Value = "女" });
                    list.Insert(0, new DefaultResource() { Code = "0", Name = "请选择..", Value = "0" });
                    break;
                case "CGENDER": // 男女
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "男", Name = "男", Value = "男" });
                    list.Add(new DefaultResource() { Code = "女", Name = "女", Value = "女" });


                    break;
                case "IS_STAFF_GENDER": // 男女
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "男", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "女", Value = "0" });
                    list.Add(new DefaultResource() { Code = "2", Name = "无", Value = "2" });
                    break;
                case "EMP_GENDER": // 员工男女
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "男", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "女", Value = "2" });
                    break;
                case "STORE"://门店
                    list = new List<IResource>();

                    var slist = dRepo.GetAvailableStore(int.Parse(parentCode));
                    list = slist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "根据门店查询", Value = null });
                    break;

                case "PROD_STATUS":
                    list = new List<IResource>();

                    list.Add(new DefaultResource() { Code = "1", Name = "上架", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "下架", Value = "0" });

                    break;
                case "PROD_TYPE":
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = "0", Name = "请选择产品类型...", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "正装产品", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "赠品", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "耗材", Value = "3" });
                    break;
                case "CARD_TYPE":
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "身份证", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "军官证", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "护照", Value = "3" });
                    break;
                case "STORE_SERVICETYPE"://门店服务类型
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择服务类别...", Value = null });
                    list.Add(new DefaultResource() { Code = "1", Name = "美容", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "美发", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "SPA", Value = "3" });
                    list.Add(new DefaultResource() { Code = "4", Name = "足浴", Value = "4" });
                    break;
                case "Masseur":// 技师 

                    var elist = dRepo.GetEmployeeByStoreId(parentCode);//
                    list = elist.ConvertAll<IResource>(x => new DefaultResource() { Code = x.EMPLOYEE_NO, Name = x.NAME, Value = x.ID.ToString() });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择...", Value = null });
                    break;

                case "CUST_TYPE"://客户类型
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "会员", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "体验用户", Value = "2" });
                    break;
                case "PROD_SERVICEByCard"://服务项目
                    //list = new List<IResource>();
                    //var plistbycard = dRepo.GetProd_ServiceByStoreId("1001");
                    //list = plistbycard.ConvertAll<IResource>(x =>
                    //{
                    //    return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    //});
                    break;
                case "PROD_SERVICE"://服务项目
                    //list = new List<IResource>();
                    //var plist = dRepo.GetProd_ServiceByStoreId(parentCode);
                    //list = plist.ConvertAll<IResource>(x =>
                    //{
                    //    return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    //});
                    break;
                case "DATETIME"://时间
                    list = new List<IResource>();
                    for (int i = 10; i < 24; i++)
                    {
                        list.Add(new DefaultResource() { Code = i + ":" + "00", Name = i + ":" + "00", Value = i + ":" + "00" });
                        list.Add(new DefaultResource() { Code = i + ":" + "15", Name = i + ":" + "15", Value = i + ":" + "15" });
                        list.Add(new DefaultResource() { Code = i + ":" + "30", Name = i + ":" + "30", Value = i + ":" + "30" });
                        list.Add(new DefaultResource() { Code = i + ":" + "45", Name = i + ":" + "45", Value = i + ":" + "45" });
                    }
                    break;
                case "ISStaff"://是否指定护理师
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "所有护理师", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "指定护理师", Value = "2" });
                    break;
                case "EMP_TYPE"://员工类型
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择员工类型...", Value = null });
                    list.Add(new DefaultResource() { Code = "1", Name = "全职员工", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "兼职员工", Value = "2" });
                    break;
                case "File_Type":
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择附件类型", Value = null });
                    list.Add(new DefaultResource() { Code = "身份证照片", Name = "身份证照片", Value = "身份证照片" });
                    list.Add(new DefaultResource() { Code = "身份证扫描件", Name = "身份证扫描件", Value = "身份证扫描件" });
                    list.Add(new DefaultResource() { Code = "签名", Name = "签名", Value = "签名" });
                    list.Add(new DefaultResource() { Code = "照片", Name = "照片", Value = "照片" });
                    break;
                case "EMP_STATUS"://员工状态
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择员工状态...", Value = null });
                    list.Add(new DefaultResource() { Code = "1", Name = "在职", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "离职", Value = "0" });
                    break;
                case "EMP_TEAM"://员工团队
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择团队...", Value = null });
                    list.Add(new DefaultResource() { Code = "1", Name = "团队1", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "团队2", Value = "2" });
                    break;
                case "SER_TYPE"://服务项目类别-edit by liuliyuan(value改为GUID)
                    list = new List<IResource>();
                    var plist = dRepo.GetCateTypeByOrgId(parentCode, "2");
                    list = plist.ConvertAll<IResource>(x =>
                    {

                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.CATE_NAME, Value = x.ID.ToString() };

                    });

                    if (hasAllOption)
                        list.Insert(0, new DefaultResource() { Code = null, Name = "请选择服务项目类别..", Value = null });
                    break;
                case "SER_TYPE_EX"://服务项目类别
                    list = new List<IResource>();
                    var plistex = dRepo.GetCateTypeByOrgId(parentCode, "2");
                    list = plistex.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.CATE_NAME, Value = x.ID.ToString() };
                    });
                    //if (hasAllOption)
                    //    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择服务项目类别..", Value = null });
                    break;
                case "SER_BIGTYPE"://服务项目类别
                    list = new List<IResource>();
                    var biglist = dRepo.GetCateTypeByOrgId(parentCode, "2");
                    list = biglist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.CATE_NAME, Name = x.CATE_NAME, Value = x.CATE_NAME.ToString() };
                    });

                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择项目所属大类...", Value = null });

                    break;
                case "ORG"://所属公司
                    list = new List<IResource>();
                    var olist = dRepo.GetAllOrgInfo();
                    list = olist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.ORG_NAME, Value = x.ID.ToString() };
                    });
                    break;

                case "POSTCARD_TYPE"://现金/刷卡
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "选择现金/刷卡...", Value = null });
                    list.Add(new DefaultResource() { Code = "1", Name = "现金", Value = "1" });
                    //list.Add(new DefaultResource() { Code = "2", Name = "POS机", Value = "2" });
                    //list.Add(new DefaultResource() { Code = "3", Name = "微信支付", Value = "3" });
                    //list.Add(new DefaultResource() { Code = "4", Name = "支付宝支付", Value = "4" });
                    break;


                case "ORDER_STATUS"://订单状态
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "所有订单", Value = null });
                    list.Add(new DefaultResource() { Code = "0", Name = "未付款订单", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "已付款订单", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "已作废订单", Value = "2" });
                    list.Add(new DefaultResource() { Code = "9", Name = "已取消订单", Value = "9" });
                    //list.Add(new DefaultResource() { Code = "8", Name = "已退款订单", Value = "8" });
                    break;
                case "ORG_STORE":
                    list = new List<IResource>();
                    var oslist = dRepo.GetAvailableStoreByOrgId(parentCode);
                    list = oslist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    });
                    if (hasAllOption)
                    {
                        list.Insert(0, new DefaultResource() { Code = null, Name = "请选择门店...", Value = null });
                    }
                    break;
                case "PROD_CATEGORY": //产品类别
                    list = new List<IResource>();
                    var calist = dRepo.GetProdTypeByOrgId(parentCode, "0"); ;
                    list = calist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.CATE_NAME, Value = x.ID.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择产品类别...", Value = null });
                    break;
                case "PROD_BIGCATEGORY": //产品类别
                    list = new List<IResource>();
                    var cablist = dRepo.GetProdTypeByOrgId(parentCode, "0"); ;
                    list = cablist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.CATE_NAME.ToString(), Name = x.CATE_NAME, Value = x.CATE_NAME.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择产品类别...", Value = null });
                    break;
                case "ROLE_ID": //公司角色 
                    list = new List<IResource>();
                    var rlist = dRepo.GetSYS_ROLEByOrgId(parentCode);
                    list = rlist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ROLE_ID.ToString(), Name = x.ROLE_NAME, Value = x.ROLE_ID.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择角色...", Value = null });

                    break;
                case "OPEN_CLOSE"://正常关闭状态
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "正常", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "关闭", Value = "0" });
                    break;
                case "SET_ON_OFF"://已设置未设置
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "已设置", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "未设置", Value = "0" });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择设置状态...", Value = null });
                    break;
                case "ORG_XSQD"://销售渠道
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "微信", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "点评网", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "团购", Value = "3" });
                    list.Add(new DefaultResource() { Code = "4", Name = "杂志", Value = "4" });
                    list.Add(new DefaultResource() { Code = "5", Name = "路过", Value = "5" });
                    list.Add(new DefaultResource() { Code = "6", Name = "朋友介绍", Value = "6" });
                    list.Add(new DefaultResource() { Code = "7", Name = "微博", Value = "7" });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择销售渠道..", Value = null });
                    break;
                case "UP_DOWN"://活动上线下线状态
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "0", Name = "上线", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "下线", Value = "1" });
                    break;

                case "IS_ALLDAY"://是否全天
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "整天", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "自定义时间", Value = "0" });
                    break;
                case "IS_ALLPROD"://是否所有项目
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "所有项目", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "部分项目", Value = "0" });
                    break;
                case "PROMOTION_PATH"://活动渠道
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "大众点评网", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "丁丁网", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "腾讯微生活", Value = "3" });
                    list.Add(new DefaultResource() { Code = "4", Name = "微信", Value = "4" });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择活动渠道..", Value = null });
                    break;
                case "PROMISSION_PRO_TYPE"://产品类型
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择产品类型..", Value = null });
                    list.Add(new DefaultResource() { Code = "0", Name = "实物产品", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "卡", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "项目", Value = "2" });
                    break;
                case "PROMISSION_PRO_TYPE2"://产品类型（只包括护理项目和产品）
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择产品类型..", Value = null });
                    list.Add(new DefaultResource() { Code = "2", Name = "项目", Value = "2" });
                    list.Add(new DefaultResource() { Code = "0", Name = "实物产品", Value = "0" });
                    break;
                case "PROMISSION_PRO_TYPE3"://买赠 添加赠送抵用卷
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择产品类型..", Value = null });
                    list.Add(new DefaultResource() { Code = "0", Name = "实物产品", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "卡", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "项目", Value = "2" });
                    list.Add(new DefaultResource() { Code = "4", Name = "抵用卷", Value = "4" });
                    break;
                case "PROMISSION_SUB_TYPE"://团购类型
                    list = new List<IResource>();
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择团购类型..", Value = null });
                    list.Add(new DefaultResource() { Code = "1", Name = "折扣", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "代金券", Value = "2" });
                    break;
                case "WEEK"://周
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "星期一", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "星期二", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "星期三", Value = "3" });
                    list.Add(new DefaultResource() { Code = "4", Name = "星期四", Value = "4" });
                    list.Add(new DefaultResource() { Code = "5", Name = "星期五", Value = "5" });
                    list.Add(new DefaultResource() { Code = "6", Name = "星期六", Value = "6" });
                    list.Add(new DefaultResource() { Code = "0", Name = "星期天", Value = "0" });
                    break;


                case "CASH_TYPE"://支付方式类型
                    list = new List<IResource>();
                    // var cashlist = dRepo.GeDICTByPDicCode("cash_type", int.Parse(parentCode));
                    //  list = cashlist.ConvertAll<IResource>(x =>
                    //{
                    //    return new DefaultResource() { Code = x.DICT_CODE, Name = x.DICT_VALUE, Value = x.DICT_CODE };
                    //});
                    //list.Add(new DefaultResource() { Code = "0", Name = "现金", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "银联POS", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "应收账款", Value = "2" });
                    //list.Add(new DefaultResource() { Code = "3", Name = "代金券", Value = "3" });
                    //list.Add(new DefaultResource() { Code = "5", Name = "会员卡", Value = "5" });
                    //list.Add(new DefaultResource() { Code = "999", Name = "疗程卡", Value = "999" });

                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择支付方式类型...", Value = null });
                    break;

                case "CASH_TYPE_SEARCH"://支付方式类型查询使用
                    list = new List<IResource>();
                    // var cashlist = dRepo.GeDICTByPDicCode("cash_type", int.Parse(parentCode));
                    //  list = cashlist.ConvertAll<IResource>(x =>
                    //{
                    //    return new DefaultResource() { Code = x.DICT_CODE, Name = x.DICT_VALUE, Value = x.DICT_CODE };
                    //});
                    list.Add(new DefaultResource() { Code = "0", Name = "现金", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "银联POS", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "应收账款", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "代金券", Value = "3" });
                    list.Add(new DefaultResource() { Code = "5", Name = "会员卡", Value = "5" });
                    list.Add(new DefaultResource() { Code = "999", Name = "疗程卡", Value = "999" });

                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择支付方式类型...", Value = null });
                    break;

                case "Income_Range"://支付方式类型
                    list = new List<IResource>();
                    var incomelist = dRepo.GeDICTByPDicCode("income", int.Parse(parentCode));
                    list = incomelist.ConvertAll<IResource>(x =>
                   {
                       return new DefaultResource() { Code = x.DICT_CODE, Name = x.DICT_VALUE, Value = x.DICT_CODE };
                   });

                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择收入范围...", Value = null });
                    break;
                case "Cust_Source"://支付方式类型
                    list = new List<IResource>();
                    //var sourcelist = dRepo.GeDICTByPDicCode("custsource", int.Parse(parentCode));
                    //list = sourcelist.ConvertAll<IResource>(x =>
                    //{
                    //    return new DefaultResource() { Code = x.DICT_CODE, Name = x.DICT_VALUE, Value = x.DICT_CODE };
                    //});
                    //路过、会员或朋友介绍、团购（大众点评、美团、其它）、微信公众号、户外广告、媒体杂志、门店异业合作
                    list.Add(new DefaultResource() { Code = "1", Name = "合作商户", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "微信公众号", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "媒体杂志", Value = "3" });
                    list.Add(new DefaultResource() { Code = "4", Name = "路过", Value = "4" });
                    list.Add(new DefaultResource() { Code = "5", Name = "会员或朋友介绍", Value = "5" });
                    list.Add(new DefaultResource() { Code = "6", Name = "团购(大众点评,美团,其它)", Value = "6" });
                    list.Add(new DefaultResource() { Code = "7", Name = "户外广告", Value = "7" });
                    list.Add(new DefaultResource() { Code = "8", Name = "门店异业合作", Value = "8" });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择顾客来源...", Value = null });
                    break;

                case "CUST_GROUP_CONDITION"://顾客分组筛选条件
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "基本条件", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "消费状况", Value = "2" });
                    //list.Add(new DefaultResource() { Code = "3", Name = "活动记录", Value = "3" });
                    break;

                case "CUST_GROUP_PROPERTY"://顾客基本条件属性
                    list = new List<IResource>();
                    //list.Add(new DefaultResource() { Code = "NAME", Name = "名称", Value = "NAME" });
                    list.Add(new DefaultResource() { Code = "GENDER", Name = "性别", Value = "GENDER" });
                    list.Add(new DefaultResource() { Code = "BIRTHDAY", Name = "生日", Value = "BIRTHDAY" });
                    //list.Add(new DefaultResource() { Code = "IDTYPE", Name = "证件类型", Value = "IDTYPE" });
                    //list.Add(new DefaultResource() { Code = "IDCARD", Name = "证件号码", Value = "IDCARD" });
                    //list.Add(new DefaultResource() { Code = "MOBILE", Name = "手机", Value = "MOBILE" });
                    list.Add(new DefaultResource() { Code = "COUNTRY", Name = "国家", Value = "COUNTRY" });
                    list.Add(new DefaultResource() { Code = "PROVINCE", Name = "省份", Value = "PROVINCE" });
                    list.Add(new DefaultResource() { Code = "CITY", Name = "城市", Value = "CITY" });
                    list.Add(new DefaultResource() { Code = "REGION", Name = "地区", Value = "REGION" });
                    //list.Add(new DefaultResource() { Code = "ADDRESS", Name = "地址", Value = "ADDRESS" });
                    list.Add(new DefaultResource() { Code = "EDUCATION", Name = "教育水平", Value = "EDUCATION" });
                    //list.Add(new DefaultResource() { Code = "EMAIL", Name = "电子邮件", Value = "EMAIL" });
                    list.Add(new DefaultResource() { Code = "SOURCE", Name = "来源", Value = "SOURCE" });
                    //list.Add(new DefaultResource() { Code = "QQ", Name = "QQ", Value = "QQ" });
                    //list.Add(new DefaultResource() { Code = "WECHAT", Name = "微信", Value = "WECHAT" });
                    //list.Add(new DefaultResource() { Code = "FACEBOOK", Name = "FACEBOOK", Value = "FACEBOOK" });
                    //list.Add(new DefaultResource() { Code = "WEIBO", Name = "微博", Value = "WEIBO" });
                    list.Add(new DefaultResource() { Code = "INCOME", Name = "收入水平", Value = "INCOME" });
                    //list.Add(new DefaultResource() { Code = "POSITION", Name = "职位", Value = "POSITION" });
                    //list.Add(new DefaultResource() { Code = "WORK_STATION", Name = "工作环境", Value = "WORK_STATION" });

                    //var t = (new CUST_INFO()).GetType();
                    //var ts = t.GetProperties().Where(p => p.Name != "ID").Select(p => p.Name).ToList();
                    //list = new List<IResource>();
                    //list.AddRange(ts.Select(p => new DefaultResource() { Code = p, Name = p, Value = p }));
                    break;


                case "VOUCHER_STATES"://代金券状态
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "0", Name = "未使用", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "已使用", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "已过期", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "已作废", Value = "3" });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择代金券状态..", Value = null });
                    break;
                case "CUST_GROUP_TYPE"://分组类型
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "0", Name = "静态", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "动态", Value = "1" });
                    break;
                case "EMP_POST_TYPE"://员工级别岗位
                    list = new List<IResource>();
                    var objs = dRepo.GeDICTByPDicCode("emp_post_type", int.Parse(parentCode));
                    list = objs.ConvertAll<IResource>(x =>
                  {
                      return new DefaultResource() { Code = x.DICT_CODE, Name = x.DICT_VALUE, Value = x.DICT_CODE };
                  });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择岗位..", Value = null });
                    break;

                case "YEAR"://年
                    list = new List<IResource>();
                    for (int i = 0; i <= 20; i++)
                    {
                        list.Add(new DefaultResource() { Code = (2008 + i).ToString(), Name = (2008 + i).ToString(), Value = (2008 + i).ToString() });
                    }
                    break;
                case "MONTH"://月
                    list = new List<IResource>();
                    //for (int i = 1; i <= 12; i++)
                    //{
                    //    list.Add(new DefaultResource() { Code = i.ToString(), Name = i.ToString(), Value = i.ToString() });
                    //}
                    list.Add(new DefaultResource() { Code = "01", Name = "1", Value = "01" });
                    list.Add(new DefaultResource() { Code = "02", Name = "2", Value = "02" });
                    list.Add(new DefaultResource() { Code = "03", Name = "3", Value = "03" });
                    list.Add(new DefaultResource() { Code = "04", Name = "4", Value = "04" });
                    list.Add(new DefaultResource() { Code = "05", Name = "5", Value = "05" });
                    list.Add(new DefaultResource() { Code = "06", Name = "6", Value = "06" });
                    list.Add(new DefaultResource() { Code = "07", Name = "7", Value = "07" });
                    list.Add(new DefaultResource() { Code = "08", Name = "8", Value = "08" });
                    list.Add(new DefaultResource() { Code = "09", Name = "9", Value = "09" });
                    list.Add(new DefaultResource() { Code = "10", Name = "10", Value = "10" });
                    list.Add(new DefaultResource() { Code = "11", Name = "11", Value = "11" });
                    list.Add(new DefaultResource() { Code = "12", Name = "12", Value = "12" });
                    break;
                case "VOUCHER_TYPE"://代金券类型
                    list = new List<IResource>();
                    var objs2 = dRepo.GeDICTByPDicCode("voucher_type", int.Parse(parentCode));
                    list = objs2.ConvertAll<IResource>(x =>
                  {
                      return new DefaultResource() { Code = x.DICT_CODE, Name = x.DICT_VALUE, Value = x.DICT_CODE };
                  });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择类型..", Value = null });
                    break;
                case "REGION"://所属区域
                    list = new List<IResource>();
                    var objs3 = dRepo.GeREGION(int.Parse(parentCode));
                    list = objs3.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.ORG_NAME, Value = x.ID.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择区域..", Value = null });
                    break;
                case "USER_TYPE"://用户类别
                    list = new List<IResource>();
                    //当前用户类别
                    if (parentCode == "0")
                    {
                        list.Add(new DefaultResource() { Code = "0", Name = "集团用户", Value = "0" });
                        list.Add(new DefaultResource() { Code = "1", Name = "区域用户", Value = "1" });
                        list.Add(new DefaultResource() { Code = "2", Name = "门店用户", Value = "2" });
                    }
                    //区域用户
                    else if (parentCode == "1")
                    {
                        list.Add(new DefaultResource() { Code = "1", Name = "区域用户", Value = "1" });
                        list.Add(new DefaultResource() { Code = "2", Name = "门店用户", Value = "2" });
                    }
                    //门店用户
                    else if (parentCode == "2")
                    {
                        list.Add(new DefaultResource() { Code = "2", Name = "门店用户", Value = "2" });
                    }
                    //  var objs4 = dRepo.GeDICTByPDicCode("user_type", int.Parse(parentCode));
                    //  list = objs4.ConvertAll<IResource>(x =>
                    //{
                    //    return new DefaultResource() { Code = x.DICT_CODE, Name = x.DICT_VALUE, Value = x.DICT_CODE };
                    //});
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择类型..", Value = null });
                    break;
                case "ORG_LEVEL"://公司等级
                    list = new List<IResource>();
                    var objs5 = dRepo.GeDICTByPDicCode("org_level", int.Parse(parentCode));
                    objs5 = objs5.Where(p => p.DICT_CODE != "0").ToList();
                    list = objs5.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.DICT_CODE, Name = x.DICT_VALUE, Value = x.DICT_CODE };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择公司等级..", Value = null });
                    break;

                case "USE_STORE"://是否所有项目
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "所有", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "部分", Value = "0" });
                    break;
                case "MY_STORE_DATA"://当前用户权限内的数据权限
                    list = new List<IResource>();
                    if (!string.IsNullOrEmpty(parentCode))
                    {
                        var objs6 = dRepo.GeStoresByIds(parentCode);
                        list = objs6.ConvertAll<IResource>(x =>
                        {
                            return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                        });
                        list.Insert(0, new DefaultResource() { Code = null, Name = "请选择门店..", Value = null });
                    }
                    break;

                //case "PROD_CATEGORY"://会员商品
                //    list = new List<IResource>();
                //    var catelist = dRepo.getCATEGORY(parentCode);
                //    list = catelist.ConvertAll<IResource>(x =>
                //    {
                //        return new DefaultResource() { Code = x.ID.ToString(), Name = x.CATE_NAME, Value = x.ID.ToString() };
                //    });
                //    list.Insert(0, new DefaultResource() { Code = null, Name = "选择产品类别..", Value = null });
                //    break;
                //case "Present_Prod"://会员商品
                //    list = new List<IResource>();
                //    var catelist = dRepo.getPresent_Prod(parentCode);
                //    list = catelist.ConvertAll<IResource>(x =>
                //    {
                //        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                //    });
                //    list.Insert(0, new DefaultResource() { Code = null, Name = "选择产品类别..", Value = null });
                //    break;
                case "IS_ALLCUST"://适用顾客
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "0", Name = "所有顾客", Value = "0" });
                    list.Add(new DefaultResource() { Code = "1", Name = "部分顾客", Value = "1" });
                    break;

                //case "Merchants_ID": //商户编号 
                //    list = new List<IResource>();
                //    var rlist2 = dRepo.GetMerchants();
                //    list = rlist2.ConvertAll<IResource>(x =>
                //    {
                //        return new DefaultResource() { Code = x.ID.ToString(), Name = x.Name, Value = x.ID.ToString() };
                //    });
                //    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择商户...", Value = null });

                //    break;
                case "Graphic_ID": //图文资源 
                    list = new List<IResource>();
                    var rlist3 = dRepo.GetGraphicList("0");
                    list = rlist3.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.Title, Value = x.ID.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择图文资源...", Value = null });

                    break;
                case "MatchingType": //关键字匹配类型
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "完全匹配", Value = "1" });
                    list.Add(new DefaultResource() { Code = "0", Name = "模糊匹配", Value = "0" });
                    break;
                case "MsgType": //关键字回复类型
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "news", Name = "图文", Value = "news" });
                    list.Add(new DefaultResource() { Code = "text", Name = "文本", Value = "text" });
                    list.Add(new DefaultResource() { Code = "image", Name = "图片", Value = "image" });
                    break;
                case "replyType": //消息类型
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "关键字回复", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "被关注回复", Value = "2" });
                    list.Add(new DefaultResource() { Code = "3", Name = "自动回复", Value = "3" });
                    break;
                case "StoreType2": //门店类型
                    list = new List<IResource>();
                    list.Add(new DefaultResource() { Code = "1", Name = "店中店", Value = "1" });
                    list.Add(new DefaultResource() { Code = "2", Name = "自收银", Value = "2" });
                    break;

            }
            if (list != null)
            {
                CacheFactory.Instance.CreateCoreCacheInstance().Insert("RESOURCE_" + code + "_" + parentCode, list);
            }
            if (hasAllOption)
                list.Insert(0, new DefaultResource() { Code = null, Name = "请选择..", Value = null });
            return list;
        }

        public List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption, string Merchants_ID)
        {
            object cache = CacheFactory.Instance.CreateCoreCacheInstance().Get("RESOURCE_" + code + "_" + parentCode);
            List<IResource> list = null;
            if (cache != null)
            {
                list = cache as List<IResource>;
            }
            if (list != null)
            {
                if (hasAllOption)
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择...", Value = null });
                return list;
            }
            switch (code)
            {

                case "Graphic_ID": //图文资源 
                    list = new List<IResource>();
                    var rlist3 = dRepo.GetGraphicList(Merchants_ID);
                    list = rlist3.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.Title, Value = x.ID.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择图文资源...", Value = null });

                    break;
                case "SID": //所属门店 
                    list = new List<IResource>();
                    var rlist4 = dRepo.GetORG_STORE(Merchants_ID);
                    list = rlist4.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择门店...", Value = null });

                    break;

            }
            if (list != null)
            {
                CacheFactory.Instance.CreateCoreCacheInstance().Insert("RESOURCE_" + code + "_" + parentCode, list);
            }
            if (hasAllOption)
                list.Insert(0, new DefaultResource() { Code = null, Name = "请选择..", Value = null });
            return list;
        }

        public List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption, double LowerAmt, int type)
        {
            object cache = CacheFactory.Instance.CreateCoreCacheInstance().Get("RESOURCE_" + code + "_" + parentCode);
            List<IResource> list = null;
            if (cache != null)
            {
                list = cache as List<IResource>;
            }
            if (list != null)
            {
                if (hasAllOption)
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择...", Value = null });
                return list;
            }

            if (list != null)
            {
                CacheFactory.Instance.CreateCoreCacheInstance().Insert("RESOURCE_" + code + "_" + parentCode, list);
            }
            if (hasAllOption)
                list.Insert(0, new DefaultResource() { Code = null, Name = "请选择..", Value = null });
            return list;
        }

        public List<IResource> GetResourceByCode(string code, string parentCode, string regionid, string store, bool hasAllOption)
        {
            object cache = CacheFactory.Instance.CreateCoreCacheInstance().Get("RESOURCE_" + code + "_" + parentCode);
            List<IResource> list = null;
            if (cache != null)
            {
                list = cache as List<IResource>;
            }
            if (list != null)
            {
                if (hasAllOption)
                    list.Insert(0, new DefaultResource() { Code = null, Name = "请选择...", Value = null });
                return list;
            }
            switch (code)
            {
                case "LoginStore": // 有效 无效
                    list = new List<IResource>();
                    var slist = dRepo.GetAvailableStore(parentCode, "", int.Parse(store), int.Parse(regionid));
                    list = slist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    });
                    //list.Insert(0, new DefaultResource() { Code = null, Name = "请选择门店名称", Value = null });
                    break;
                case "REGIONSTORE"://地区
                    list = new List<IResource>();

                    var rslist = dRepo.GetAvailableRegion(int.Parse(parentCode), int.Parse(store), int.Parse(regionid));
                    list = rslist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.ORG_NAME, Value = x.ID.ToString() };
                    });
                    list.Insert(0, new DefaultResource() { Code = null, Name = "根据地区查询", Value = null });
                    break;



            }
            if (list != null)
            {
                CacheFactory.Instance.CreateCoreCacheInstance().Insert("RESOURCE_" + code + "_" + parentCode, list);
            }
            if (hasAllOption)
                list.Insert(0, new DefaultResource() { Code = null, Name = "请选择..", Value = null });
            return list;
        }

        //得到空闲技师信息 考虑预约冲突和排版的因素
        public List<IResource> GetResourceByCode(string code, string parentCode, int storeId, int orgid, DateTime beginTime, DateTime endTime, bool? gender, bool hasAllOption)
        {
            object cache = CacheFactory.Instance.CreateCoreCacheInstance().Get("RESOURCE_" + code + "_" + parentCode);
            List<IResource> list = null;
            if (cache != null)
            {
                list = cache as List<IResource>;
            }
            if (list != null)
            {
                if (hasAllOption)
                    list.Insert(0, new DefaultResource() { Code = null, Name = "随机", Value = null });
                return list;
            }
            switch (code)
            {
                case "FreeMasseur":
                    list = new List<IResource>();
                    var rslist = dRepo.GetFreeEmployee(storeId, orgid, beginTime, endTime, gender);
                    list = rslist.ConvertAll<IResource>(x =>
                    {
                        return new DefaultResource() { Code = x.ID.ToString(), Name = x.NAME, Value = x.ID.ToString() };
                    });
                    break;
            }
            if (list != null)
            {
                CacheFactory.Instance.CreateCoreCacheInstance().Insert("RESOURCE_" + code + "_" + parentCode, list);
            }
            if (hasAllOption)
                list.Insert(0, new DefaultResource() { Code = null, Name = "随机", Value = null });
            return list;
        }
    }

    [DataContract]
    public class DefaultResource : IResource
    {
        [DataMember(Name = "code")]
        public string Code
        {
            get;
            set;
        }
        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }
        [DataMember(Name = "value")]
        public string Value
        {
            get;
            set;
        }
    }
}
