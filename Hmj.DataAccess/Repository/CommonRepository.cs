using Hmj.Entity;
using Hmj.Entity.Entities;
using System;

namespace Hmj.DataAccess.Repository
{
    public class CommonRepository : BaseRepository
    {
        public Entity.FILES GetFILES(int id)
        {
            return base.Get<Entity.FILES>("SELECT * FROM FILES WHERE ID=@ID", new { ID = id });
        }


        public ORG_STORE getStoreInfo(int storeId)
        {
            string sql = "SELECT NAME,TELEPHONE,ADDRESS FROM ORG_STORE WHERE ID=@STORE_ID";
            return base.Get<ORG_STORE>(sql, new { STORE_ID = storeId });
        }

        //会员卡级信息
        public CUST_INFO_EX GetCustCard(int cid)
        {
            string sql = @"select CI.NAME,CI.CARD_NO,CI.MOBILE,CC.BALANCE,MI.AVA_POINTS from CUST_INFO CI 
            left join CUST_CARD CC ON CI.ID=CC.CUST_ID
            LEFT JOIN MEMBER_INFO MI ON CI.ID=MI.CUST_ID
            where  CI.id=@CID";
            return base.Get<CUST_INFO_EX>(sql, new { CID = cid });
        }

        public string InsertGood(string fansID, string fileID)
        {
            string sql = @"INSERT INTO dbo.FANS_FABULOUS
        (FANS_ID, FILES_ID, CREATE_TIME)
VALUES(@FANS_ID,@FILES_ID ,@CREATE_TIME)";

            return base.Excute(sql, new
            {
                FANS_ID = fansID,
                FILES_ID = fileID,
                CREATE_TIME = DateTime.Now
            }).ToString();
        }

        public int CheckStore(string zMD_ID)
        {
            string sql = "SELECT * FROM dbo.BCJ_STORES WHERE POS_CODE=@POS_CODE";

            BCJ_STORES model = base.Get<BCJ_STORES>(sql, new { POS_CODE = zMD_ID });

            return model == null ? 0 : model.ID;
        }
    }
}
