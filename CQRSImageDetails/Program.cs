using Autofac;
using MediatR;
using CQRSImageDetails.Commands;
using CQRSImageDetails.Queries;
using CQRSImageDetails.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CQRSImageDetails.Infra;
using System.Runtime.CompilerServices;
using System.CodeDom;
using CQRSImageDetails.Events;

namespace CQRSImageDetails
{
    class Program
    {


        private static void UseServices(Type[] commands,
                                        Type[] events,
                                        Action<CommandManager> configureCommandManager,
                                        Action<EventManager> configureEventManager)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.RegisterType<CommandManager>();
            builder.RegisterType<EventManager>();

            void registerType(Assembly assbl, Type t)
            {
                var registredHandler = builder
                                        .RegisterAssemblyTypes(assbl);
                registredHandler.AsClosedTypesOf(t);
                registredHandler.AsImplementedInterfaces();
            }

            foreach (var commandHandler in commands)
                registerType(commandHandler.GetTypeInfo().Assembly, typeof(IRequestHandler<,>));


            foreach (var eventHandler in events)
                registerType(eventHandler.GetTypeInfo().Assembly, typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();
            container.Resolve<IMediator>();

            if (configureCommandManager != null)
                configureCommandManager.Invoke(container.Resolve<CommandManager>());

            if (configureEventManager != null)
                configureEventManager.Invoke(container.Resolve<EventManager>());

        }



        static async Task Main(string[] args)
        {
            CommandManager managerCommandInstance = null;
            EventManager managerEventInstance = null;

            UseServices(
                        new Type[]
                        {
                            typeof(CreateNewImageCommand),
                            typeof(RemoveImageCommand)
                        },
                        new Type[]
                        {
                            typeof(ImageCreatedEvent)
                        },
                            (CommandManager instance) => { managerCommandInstance = instance; },
                            (EventManager instance) => { managerEventInstance = instance; }
                        );

            var crono = new Stopwatch();
            crono.Start();
            int count = 0;
            await managerEventInstance.Publish(new ImageCreatedEvent { Name = "X" });
            foreach (var item in new DirectoryInfo(@"C:\Users\alexa\OneDrive\Pictures").GetFiles("*.*"))
            {
                count++;
                var result = await managerCommandInstance.Send(new CreateNewImageCommand { Name = $"[{count}] {item.Name}", Path = item.FullName });

                await managerEventInstance.Publish(new ImageCreatedEvent { Name = $"[{count}] {item.Name}" });

                await managerCommandInstance.Send(new RemoveImageCommand { Id = result.Id });
            }

            var q1 = new ImagesDetailsQueries();
            var totalDetails = await q1.ImagesDetailsTotal();
            Console.WriteLine($"Total details => { totalDetails.Total }");


            foreach (var item in await q1.GetImageDetails())
            {
                Console.WriteLine(item.Name);
                // await commandEngine.Send(new RemoveImageCommand { Id = item.Id }); 
            }

            crono.Stop();
            Console.WriteLine(crono.ElapsedMilliseconds);
            Console.ReadKey();

        }


        // teste sem mediatr
        //static void Main(string[] args)
        //{
        //    var repo = new RepositoryPostgres();
        //    var crono = new Stopwatch();
        //    crono.Start();
        //    foreach (var item in new DirectoryInfo(@"D:\Fotos Pai").GetFiles("*.jpg"))
        //    {
        //        repo.InsertImageDetails(new Commands.CreateNewImageCommand { Name = $"{item.Name} - {DateTime.Now}", Path = item.FullName });
        //    }

        //    for (int i = 0; i < 20; i++)
        //    {
        //        repo.DeleteImageDetails(i);
        //    }

        //    crono.Stop();
        //    Console.WriteLine(crono.ElapsedMilliseconds);
        //    Console.ReadKey();
        //}
    }
}
