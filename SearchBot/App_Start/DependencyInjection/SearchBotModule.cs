using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchBot.Service.Interfaces;
using SearchBot.Service.HRM;
using SearchBot.Service.DependencyInjection;
using SearchBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace SearchBot.DependencyInjection
{
    public class SearchBotModule : Module
    {
        public SearchBotModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            RegisterBotDependencies(builder);
        }

        private void RegisterBotDependencies(ContainerBuilder builder)
        {


            builder
              .RegisterType<EmployeeService>()
              .Keyed<IEmployeeService>(FiberModule.Key_DoNotSerialize)
               .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            

            builder
              .RegisterType<QueryManagerConversationInterface>()
              .Keyed<IQueryManagerConversationInterface>(FiberModule.Key_DoNotSerialize)
               .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<LUISDialog>()
                 .AsSelf()
                 .InstancePerDependency();

            //builder
            //      .RegisterType<CustomActivityMapper>()
            //      .AsImplementedInterfaces()
            //      .SingleInstance();

        }

    }
}