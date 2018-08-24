using Hmj.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DataAccess.Repository
{
    public class ThdPlatformRepository : ThdPlatformBaseRepository
    {
        //根据小程序openid 查找公众号openid   （TB_Customer_Info表的openid为服务号的openid）
        public ThdPlatformCustInfo GetThdPlatformCustInfo(string OpenId, string BrandId)
        {
            string sql = @"select 
                            Unionid,
                            NickName,
                            OpenId,
                            HeadImgUrl,
                            BrandId 
                           from TB_Customer_Info with(nolock)
                           where Unionid =(
                           select Unionid from TB_Customer_Info  with(nolock) where OpenId=@OpenId
                           ) and BrandId=@BrandId";
            return base.Get<ThdPlatformCustInfo>(sql, new { BrandId = BrandId, OpenId = OpenId });
        }

        //根据unionid查找用户
        public ThdPlatformCustInfo GetThdPlatformCustInfoByUnionid(string Unionid, string BrandId)
        {
            string sql = @"select 
                            Unionid,
                            NickName,
                            OpenId,
                            HeadImgUrl,
                            BrandId 
                           from TB_Customer_Info with(nolock)
                           where Unionid=@Unionid and BrandId=@BrandId";
            return base.Get<ThdPlatformCustInfo>(sql, new { BrandId = BrandId, Unionid = Unionid });
        }
    }
}
