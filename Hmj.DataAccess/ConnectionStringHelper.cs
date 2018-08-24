namespace Hmj.DataAccess
{
    public class ConnectionStringHelper
    {

        public static string GetValueByKey(string dbkey)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[dbkey].ConnectionString;
        }
    }
}
