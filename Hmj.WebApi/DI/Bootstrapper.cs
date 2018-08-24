using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using Nancy.Bootstrapper;
using Nancy;
using Vulcan.Framework.DBConnectionManager;
using Nancy.Conventions;
//using ServiceStack.Text;

namespace Hmj.WebApi.DI
{
    public class Bootstrapper : StructureMapNancyBootstrapper
    {
        //protected override void ApplicationStartup(IContainer container, IPipelines pipelines)
        //{
        //    // No registrations should be performed in here, however you may
        //    // resolve things that are needed during application startup.
        //}

        protected override void ConfigureApplicationContainer(IContainer existingContainer)
        {
            DbConnectionFactory.Default = new SqlConnectionFactory();
            //JsConfig.ExcludeTypeInfo = true;

            existingContainer.Configure(c => c.AddConfigurationFromXmlFile("Configs/StructureMap/Service.config"));
            //existingContainer.Configure(c => c.AddConfigurationFromXmlFile("Configs/StructureMap/SAO.config"));
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Image", @"/Image")
            );

            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("logs", @"/logs")
            );

            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Test", @"/Test")
            );

            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("RedBagStyle", @"/RedBagStyle")
            );

            conventions.StaticContentsConventions.Add(
               StaticContentConventionBuilder.AddDirectory("QrCode", @"/QrCode")
           );
        }

        //protected override void RequestStartup(IContainer container, IPipelines pipelines, NancyContext context)
        //{
        //    // No registrations should be performed in here, however you may
        //    // resolve things that are needed during request startup.
        //}
    }
}