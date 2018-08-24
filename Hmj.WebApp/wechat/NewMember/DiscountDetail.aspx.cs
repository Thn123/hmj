using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.Interface;
using Hmj.WebApp.DicountService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.NewMember
{
    public partial class discountDetail : WeiPage
    {
        MySmallShopService mss = new MySmallShopService();
        IStoreService sto = new StoreService();
        public string DiscountName;
        public string DiscountMs;
        public string DiscountStore;
        public string DiscountPrice;
        public string DiscountTime;
        public string DiscoutnNo;
        public string Qcode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["did"] != null)
                {
                    //获取优惠券列表
                    Z_CRM_LOY_WELFAREResponse Discount = SelectDiscount("", Request.QueryString["did"]);
                    if (Discount.ET_LOY_WELFARE.Length > 0)
                    {
                        DiscountName = Discount.ET_LOY_WELFARE[0].ZCPTYPET;
                        DiscountMs = Discount.ET_LOY_WELFARE[0].ZCPMS;
                        DiscountStore = GetStoreName(Discount.ET_LOY_WELFARE[0].ZCPSTORE);
                        DiscountPrice = Discount.ET_LOY_WELFARE[0].ZCPVALUE.ToString("0");
                        DiscountTime = Discount.ET_LOY_WELFARE[0].ZCPEND;
                        DiscoutnNo = Discount.ET_LOY_WELFARE[0].ZCPIDO;
                       COUPON_INFO info=   mss.GetDiscountCode(DiscoutnNo);
                       if (info!=null)
                       {
                           Qcode = info.QR_CODE;
                       }
                       
                    }
                  ;
                }
            }
        }

        //从MDsearch取code
        private string GetStoreName(string codelist)
        {
            string[] st = codelist.Split(',');
            string sb = "";
            string values = "";
            foreach (var item in st)
            {
                sb += "'" + item + "',";
            }
            List<GROUP_INFO> list = sto.GetGroupInfoByCode((sb.Length == 0 ? "0" : sb.Substring(0, sb.Length - 1)));

            foreach (var item in list)
            {
                values += item.NAME + ",";
            }
            return values.Length == 0 ? "" : values.Substring(0, values.Length - 1) + "等";
        }

        //从groupinfo取code
        private string GetStoreNameByGroupInfo(string codelist)
        {
            string[] st = codelist.Split(',');
            string sb = "";
            int i = 0;
            foreach (var item in st)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                if (i > 3)
                {
                    break;
                }
                sb += sto.GetGroupInfoByCode(item);
                i++;
            }
            return sb;
        }
    }
}