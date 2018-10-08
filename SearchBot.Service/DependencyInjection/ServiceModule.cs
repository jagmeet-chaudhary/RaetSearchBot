using Autofac;
using SearchBot.Common;
using SearchBot.Connectors;
using SearchBot.Connectors.HRM;
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
            //base.Load(builder);
            //builder.RegisterType<MockHrmConnector>()
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();

            base.Load(builder);
            builder.RegisterType<HrmApiConnector>()
                .As<IHrmConnector>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestHelper>()
                .As<IRequestHelper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenProvider>()
                .As<ITokenProvider>()
                .InstancePerLifetimeScope();

        }
    }
}
