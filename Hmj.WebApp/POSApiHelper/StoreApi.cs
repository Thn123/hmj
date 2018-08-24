using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.Interface;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Hmj.WebApp.POSApiHelper
{
    public class StoreApi : PosBasePage
    {
        private IStoreService _ss;
        public StoreApi()
        {
            _ss = new StoreService();
        }

        public Result BindStore(int posOrgID)
        {
            var result = new Result();
            var 微信门店s = _ss.GetStores(CurrentMerchants.ID);
            var 微信门店IDs = 微信门店s.Select(s => s.PosStoreID).ToList();

            var url = RequestUrlHelper.Store.List(posOrgID);
            var dtoResource = JsonConvert.DeserializeObject<DTOResource>(PostRequest(url));
            var POS门店s = dtoResource.data;
            var POS门店IDs = POS门店s.Select(t => t.ID).ToList();

            foreach (var 微信门店 in 微信门店s)
            {
                if (POS门店IDs.Contains(微信门店.PosStoreID))
                {
                    //更新微信门店
                    var POS门店 = POS门店s.FirstOrDefault(s => s.ID == 微信门店.PosStoreID);
                    微信门店.Merchants_ID = CurrentMerchants.ID;
                    微信门店.PosStoreID = POS门店.ID;
                    微信门店.Name = POS门店.NAME;
                    微信门店.Province = POS门店.PROVINCE;
                    微信门店.City = POS门店.CITY;
                    微信门店.Region = POS门店.REGION;
                    微信门店.Address = POS门店.ADDRESS;
                    微信门店.Manager = POS门店.MANAGER;
                    微信门店.Telephone = POS门店.TELEPHONE;
                    微信门店.RegionID = POS门店.REGION_ID;
                    微信门店.LastUpdateTime = DateTime.Now;
                    if (_ss.UpdateStore(微信门店) > 0)
                    {
                        result.updatecount++;
                    }
                }
                else
                {
                    //POS端该门店已删除或者关店 需删除微信门店
                    if (_ss.DeleteStore(微信门店.ID) > 0)
                    {
                        微信门店IDs.Remove(微信门店.PosStoreID);
                        result.deletecount++;
                    }
                }
            }

            foreach (var POS门店 in POS门店s)
            {
                if (!微信门店IDs.Contains(POS门店.ID))
                {
                    //POS新添加了一个门店 微信还未同步 创建
                    WXStore s = new WXStore();
                    s.Merchants_ID = CurrentMerchants.ID;
                    s.PosStoreID = POS门店.ID;
                    s.Name = POS门店.NAME;
                    s.Province = POS门店.PROVINCE;
                    s.City = POS门店.CITY;
                    s.Region = POS门店.REGION;
                    s.Address = POS门店.ADDRESS;
                    s.Manager = POS门店.MANAGER;
                    s.Telephone = POS门店.TELEPHONE;
                    s.RegionID = POS门店.REGION_ID;
                    if (_ss.AddStore(s) > 0)
                    {
                        result.addcount++;
                    }
                }
            }

            return result;
        }

        #region pos obj

        public class DTOResource
        {
            public DTOResourceData[] data;
            public int status;
            public string message;
        }

        public class DTOResourceData
        {
            public int ID;
            public string STORE_NO;
            public string NAME;
            public string PROVINCE;
            public string CITY;
            public string REGION;
            public string ADDRESS;
            public string MANAGER;
            public string TELEPHONE;
            public int REGION_ID;
        }

        public class Result
        {
            public Result()
            {
                updatecount = addcount = deletecount = 0;
            }

            public int updatecount { get; set; }
            public int addcount { get; set; }
            public int deletecount { get; set; }
        }

        #endregion
    }
}