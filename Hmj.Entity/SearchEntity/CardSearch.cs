namespace Hmj.Entity.SearchEntity
{
    public class CardSearch
    {
        //名称
        public string NAME { get; set; }
        public int Type { get; set; }
        public int ORG_ID { get; set; }

        //根据PROD_TYPE和PROD_ID查找出所选门店
        public int PROD_TYPE { get; set; }
        public int PROD_ID { get; set; }
        public string  STATUS { get; set; }
    }

    public class CardItemSearch
    {
        public int CardID { get; set; }
        public string ItemName { get; set; }
        public int CategoryID { get; set; }
        public string BCATE { get; set; }
        public string SCATE { get; set; }
        public string PNAME { get; set; }
        public string P_Category { get; set; }
        public string PSCATE { get; set; }
    }    
}
