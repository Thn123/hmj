namespace Hmj.Entity.Entities
{
    public class BdDistance
    {
        public int status { get; set; }
        public BdDistanceResult result { get; set; }


    }
    public class BdDistanceResult
    {
        public Bdlatlng location { get; set; }
        public int precise { get; set; }
        public int confidence { get; set; }
        public string level { get; set; }
    }

    public class Bdlatlng
    {
        public decimal lng { get; set; }
        public decimal lat { get; set; }
    }
    public class baidumap
    {
        public int status { get; set; }
        public zuobiao[] result { get; set; }
    }

    public class zuobiao
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
    }


}
