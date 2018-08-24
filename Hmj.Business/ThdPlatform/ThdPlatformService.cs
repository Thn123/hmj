using Hmj.DataAccess.Repository;
using Hmj.Entity.Entities;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Business
{
    public class ThdPlatformService: IThdPlatformService
    {
        private ThdPlatformRepository _repo;

        public ThdPlatformService()
        {
            _repo = new ThdPlatformRepository();
        }

        //根据小程序openid 查找公众号openid
        public ThdPlatformCustInfo GetThdPlatformCustInfo(string OpenId, string BrandId)
        {
            return _repo.GetThdPlatformCustInfo(OpenId, BrandId);
        }

        //根据unionid 查找公众号openid
        public ThdPlatformCustInfo GetThdPlatformCustInfoByUnionid(string Unionid, string BrandId)
        {
            return _repo.GetThdPlatformCustInfoByUnionid(Unionid, BrandId);
        }
    }
}
