using Hmj.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Interface
{
    public interface IThdPlatformService
    {
        //根据小程序openid 查找公众号openid
        ThdPlatformCustInfo GetThdPlatformCustInfo(string OpenId, string BrandId);
        ThdPlatformCustInfo GetThdPlatformCustInfoByUnionid(string Unionid, string BrandId);
    }
}
