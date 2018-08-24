using StructureMap;


namespace Hmj.WebApp
{
    public class Bootstrapper
    {

        private static bool _hasStarted;

        public void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x =>
            {
                // x.UseDefaultStructureMapConfigFile = true;
                // x.AddConfigurationFromXmlFile("Configs/StructureMap/Repository.config");
                x.AddConfigurationFromXmlFile("Configs/StructureMap/Service.config");
                //x.AddConfigurationFromXmlFile("Configs/StructureMap/ExtendApi.config");          
                //x.AddConfigurationFromXmlFile("Configs/StructureMap/SAO.config");
                
            });

            //DependencyResolver.SetResolver(new StructureMapResolver(ObjectFactory.Container));
            
        }

        public static void Restart()
        {
            if (_hasStarted)
            {
                ObjectFactory.ResetDefaults();
            }
            else
            {
                Bootstrap();
                _hasStarted = true;
            }
        }

        public static void Bootstrap()
        {
            new Bootstrapper().BootstrapStructureMap();
        }
    }
}