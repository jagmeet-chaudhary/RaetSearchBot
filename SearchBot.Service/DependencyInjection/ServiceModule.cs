using Autofac;
using SearchBot.Connectors.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Service.DependencyInjection
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<MockHrmConnector>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
         
        }
    }
}
