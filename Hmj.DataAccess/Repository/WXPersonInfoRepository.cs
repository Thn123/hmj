using Hmj.Entity;

namespace Hmj.DataAccess.Repository
{
    public class WXPersonInfoRepository : BaseRepository
    {
        public WXPersonInfo Get(int id)
        {
            string sql = @"SELECT TOP 1 * FROM [WXPersonInfo] WHERE [ID]=@ID";
            return base.Get<WXPersonInfo>(sql, new { ID = id });
        }

        public WXPersonInfo GetByFromUserName(string fromUserName, int orgID)
        {
            string sql = @"SELECT TOP 1 * FROM [WXPersonInfo] WHERE [FromUserName]=@FromUserName AND [ORG_ID]=@ORG_ID";
            return base.Get<WXPersonInfo>(sql, new { FromUserName = fromUserName, ORG_ID = orgID });
        }

        public WXPersonInfo GetByPhone(string phone, int orgID)
        {
            string sql = @"SELECT TOP 1 * FROM [WXPersonInfo] WHERE [Phone]=@Phone AND [ORG_ID]=@ORG_ID";
            return base.Get<WXPersonInfo>(sql, new { Phone = phone, ORG_ID = orgID });
        }
    }
}
