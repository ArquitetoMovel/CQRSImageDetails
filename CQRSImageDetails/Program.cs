using Autofac;
using MediatR;
using CQRSImageDetails.Queries;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ExeCutor;
using CQRSImageDetails.Commands;
using CQRSImageDetails.Events;

namespace CQRSImageDetails
{
    class Program
    {
        private static void UseServices(Type[] commands,
                                        Type[] events,
                                        Action<Executor> configureExecutor)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.RegisterType<Executor>();

            void registerHandler(Assembly assemblyHandler, Type typeHandler)
            {
                var registredHandler = builder
                                        .RegisterAssemblyTypes(assemblyHandler);
                registredHandler.AsClosedTypesOf(typeHandler);
                registredHandler.AsImplementedInterfaces();
            }

            foreach (var commandHandler in commands)
                registerHandler(commandHandler.GetTypeInfo().Assembly, typeof(IRequestHandler<,>));

            foreach (var eventHandler in events)
                registerHandler(eventHandler.GetTypeInfo().Assembly, typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });


            var container = builder.Build();
            container.Resolve<IMediator>();

            if (configureExecutor != null)
                configureExecutor.Invoke(container.Resolve<Executor>());

        }



        static async Task Main(string[] args)
        {
            Executor executorInstance = null;

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
                            (Executor instance) => { executorInstance = instance; }
                        );

            var crono = new Stopwatch();
            crono.Start();
            int count = 0;

            foreach (var item in new DirectoryInfo(@"C:\Users\alexa\OneDrive\Pictures\2016").GetFiles("*.*"))
            {
                count++;

                var result = await executorInstance.Send(new CreateNewImageCommand
                {
                    Name = $"[{count}] {item.Name}",
                    Path = item.FullName
                });

                await executorInstance.Publish(new ImageCreatedEvent { Name = $"[{count}] {item.Name}" });

                await executorInstance.Send(new RemoveImageCommand { Id = result.Id });
            }

            var q1 = new ImagesDetailsQueries();
            var totalDetails =  await q1.ImagesDetailsTotal();
            Console.WriteLine($"Total details => { totalDetails.Total }");


            //foreach (var item in await q1.GetImageDetails())
            //{
            //    Console.WriteLine(item.Name);
            //}

            crono.Stop();
            Console.WriteLine($" Total in seconds => {(crono.ElapsedMilliseconds / 1_000)}");
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
