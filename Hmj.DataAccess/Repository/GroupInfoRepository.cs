using Hmj.Entity;
using Hmj.Entity.Entities;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class GroupInfoRepository : BaseRepository
    {
        public List<GROUP_INFO> GetAll()
        {
            string sql = @"select * from GROUP_INFO order by WX_ORDER";

            return base.Query<GROUP_INFO>(sql, null);
        }
        public List<GROUP_INFO_EX> GetRecursiveAllByParentID(int pId)
        {
            string sql = @"WITH CTE AS(
	                            SELECT ID,NAME,CODE,PARENT_ID,[TYPE],MAGENTO_GROUP_ID,WX_GROUP_ID,WX_ORDER 
	                            FROM GROUP_INFO 
	                            WHERE PARENT_ID=@ParentID
	                            UNION ALL
	                            SELECT A.ID,A.NAME,A.CODE,A.PARENT_ID,A.[TYPE],A.MAGENTO_GROUP_ID,A.WX_GROUP_ID,A.WX_ORDER 
	                            FROM GROUP_INFO AS A,CTE AS B 
	                            WHERE A.PARENT_ID = B.ID
                            )

                            SELECT * FROM CTE ORDER BY ID ASC";

            return base.Query<GROUP_INFO_EX>(sql, new { ParentID = pId });
        }
        public List<GROUP_INFO_EX> GetAllByParentID(int pId)
        {
            string sql = @"SELECT ID,NAME,CODE,PARENT_ID,[TYPE],MAGENTO_GROUP_ID,WX_GROUP_ID,WX_ORDER 
	                            FROM GROUP_INFO 
	                            WHERE PARENT_ID=@ParentID";

            return base.Query<GROUP_INFO_EX>(sql, new { ParentID = pId });
        }
        public GROUP_INFO_EX Get(int id)
        {
            string sql = @"SELECT ID,CODE,NAME,PARENT_ID,[TYPE],MAGENTO_GROUP_ID,WX_GROUP_ID,WX_ORDER
                        FROM GROUP_INFO
                        WHERE ID = @ID";

            return base.Get<GROUP_INFO_EX>(sql, new { ID = id });
        }

        public int Delete(int id)
        {
            string sql = @"DELETE FROM GROUP_INFO WHERE ID = @ID";

            return base.Excute(sql, new { ID = id });
        }

        public int GetCountByParentID(int parentID)
        {
            string sql = @"SELECT count(1)
                        FROM GROUP_INFO
                        WHERE PARENT_ID = @ParentID";

            return base.Get<int>(sql, new { ParentID = parentID });
        }
        public string GetFullDeptName(int deptId)
        {
            string sql = @"WITH CTE AS(
                            SELECT ID,NAME,PARENT_ID,[TYPE]
                            FROM GROUP_INFO 
                            WHERE ID=@DeptId
                            UNION ALL
                            SELECT A.ID,A.NAME,A.PARENT_ID,A.[TYPE]
                            FROM GROUP_INFO AS A,CTE AS B 
                            WHERE B.PARENT_ID = A.ID
                        )

                        SELECT '/'+STUFF((SELECT '/'+RTRIM(name) FROM (
		                        SELECT id,NAME,[TYPE] FROM CTE 
		                ) T1 order by [TYPE] asc, ID asc  FOR XML PATH('')),1,1,'') names  ";

            return base.Get<string>(sql, new { DeptId = deptId });
        }
        public List<GROUP_INFO> GetAllByNames()
        {
            string sql = @"select * from GROUP_INFO
where NAME  in (
'盛时表行合肥巢湖百大购物中心店',
'盛时表行合肥庐江安德利店',
'盛时表行哈尔滨哈西万达百货店',
'盛时表行哈尔滨新一百购物广场店',
'盛时表行哈尔滨香坊万达百货店',
'盛时表行福州东二环泰禾店',
'盛时表行福州东方百货店',
'盛时表行福州东百红星店',
'盛时表行福州东百元洪购物广场店',
'盛时表行泉州大洋百货店',
'盛时表行厦门中华城店',
'盛时表行合肥百盛元一时代广场店',
'盛时表行六安霍邱商之都店',
'盛时表行南京江宁万达百货店',
'盛时表行十堰五商步行街店',
'盛时表行海口望海国际店',
'盛时表行杭州水晶城购物中心',
'盛时表行杭州临安万华店',
'盛时表行杭州莱蒙商业中心临平店',
'盛时表行义乌世纪联华店',
'盛时表行北京亨得利翠微大成路店',
'盛时表行北京亨得利三河燕郊新世界店',
'盛时表行太原北美新天地天梭专卖店',
'盛时表行呼和浩特王府井百货店',
'盛时表行上海巴黎春天五角场店',
'盛时表行南京江宁金鹰购物中心',
'盛时表行萍乡百货大楼店',
'盛时表行南昌百货大楼店',
'盛时表行宜春青龙商厦店',
'盛时表行吉安莱斯百货店',
'盛时表行景德镇华达百货店',
'盛时表行九江联盛购物广场店',
'盛时表行九江太平洋广场店',
'盛时表行南昌维也纳百货大楼莲塘店',
'盛时表行南昌百货大楼店',
'盛时表行南昌胜利路时代广场店',
'盛时表行南昌中山路店',
'盛时表行上海仲盛世界商城店',
'南昌亨得利南昌胜利路店',
'盛时表行怀化市通程商业广场店',
'盛时表行株洲株洲百货大楼浪琴专柜店',
'盛时表行昆山商厦店',
'盛时表行上海久光百货浪琴专卖店',
'盛时表行上海新世界店',
'盛时表行苏州美罗商城店',
'盛时表行苏州久光百货欧米茄专卖店',
'盛时表行苏州泰华商城西楼欧米茄专卖店',
'盛时表行苏州美罗商城新区店',
'盛时表行苏州第一百货商店',
'盛时表行苏州世家名表店',
'盛时表行吴江万亚购物中心店',
'盛时表行镇江八佰伴店',
'盛时表行西安钟楼饭店店',
'盛时表行海宁银泰百货海宁店',
'盛时表行杭州百货大楼店',
'盛时表行杭州银泰百货临平店',
'盛时表行杭州银泰百货武林店',
'盛时表行杭州银泰百货西湖店',
'盛时表行杭州银泰百货城西店',
'盛时表行杭州银泰百货庆春店',
'盛时表行杭州银泰百货武林店',
'盛时表行合肥银泰城店',
'盛时表行湖州银泰百货东吴店',
'盛时表行金华银泰百货天地店',
'盛时表行台州锦江百货店',
'盛时表行乐清时代广场店',
'盛时表行乐清银泰百货店',
'盛时表行临海银泰百货店',
'盛时表行宁波银泰百货江东店',
'盛时表行宁波银泰百货天一店',
'盛时表行瑞安开太百货店',
'盛时表行瑞安时代广场店',
'盛时表行温岭银泰百货店',
'盛时表行杭州银泰百货武林店',
'盛时表行温州开太百货店',
'盛时表行温州世贸大厦店',
'盛时表行温州五马名表城',
'盛时表行杭州银泰下沙店',
'盛时表行杭州银泰百货银耀店',
'盛时表行芜湖银泰城店',
'盛时表行湖州银泰百货爱山店',
'盛时表行舟山银泰百货店',
'盛时表行诸暨雄风新天地购物中心',
'盛时表行诸暨第一百货店',
'盛时表行武汉新世界百货武昌店',
'盛时表行襄阳华洋堂店',
'盛时表行贵阳星力百货店',
'盛时表行昆明柏联百盛店',
'盛时表行昆明邦利百货店',
'盛时表行昆明顺城王府井店',
'盛时表行昆明新西南百盛店',
'盛时表行哈密金瑞惠美购物广场店',
'盛时表行乌鲁木齐欧乐星城店',
'盛时表行乌鲁木齐万达广场天梭汉米尔顿专卖店',
'盛时表行乌鲁木齐汇嘉时代北京路店',
'盛时表行乌鲁木齐西单商场店',
'盛时表行哈密金瑞惠美购物广场店',
'盛时表行库尔勒汇嘉时代二店',
'盛时表行五家渠汇嘉时代店',
'盛时表行乌鲁木齐福润德百货店',
'新宇三宝乌鲁木齐中山路店',
'盛时表行郑州丹尼斯郑花路店',
'盛时表行郑州丹尼斯百货人民路店',
'盛时表行郑州华联商厦店',
'盛时表行驻马店大商新玛特店',
'盛时表行重庆北城天街浪琴专卖店')";

            return base.Query<GROUP_INFO>(sql, null);
        }
    }
}
