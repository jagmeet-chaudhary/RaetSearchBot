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
using SearchBot.Service.RVM;

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
          .RegisterType<RVM_UserService>()
          .Keyed<IRVM_UserService>(FiberModule.Key_DoNotSerialize)
           .AsImplementedInterfaces()
            .InstancePerLifetimeScope();


            builder
              .RegisterType<QueryManagerConversationInterface>()
              .Keyed<IQueryManagerConversationInterface>(FiberModule.Key_DoNotSerialize)
               .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
              .RegisterType<GreetingsConversationalInterface>()
              .Keyed<IGreetingsConversationalInterface>(FiberModule.Key_DoNotSerialize)
               .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
           
            builder.RegisterType<LUISDialog>()
                 .AsSelf()
                 .InstancePerDependency();

            builder
                .RegisterType<RootDialog>()
                .AsSelf()
                .InstancePerDependency();

            //builder.RegisterType<RootDialog<object>>()
            //     .AsSelf()
            //     .InstancePerDependency();

            //builder
            //      .RegisterType<CustomActivityMapper>()
            //      .AsImplementedInterfaces()
            //      .SingleInstance();

        }

    }
}