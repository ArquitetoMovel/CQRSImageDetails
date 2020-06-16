﻿using Autofac;
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

namespace CQRSImageDetails
{
    class Program
    {
        private static CommandEngine UseCommandEngine(Type[] commands)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            builder.RegisterType<CommandEngine>();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                foreach (var command in commands)
                {
                    builder
                        .RegisterAssemblyTypes(command.GetTypeInfo().Assembly)
                        .AsClosedTypesOf(mediatrOpenType)
                        // when having a single class implementing several handler types
                        // this call will cause a handler to be called twice
                        // in general you should try to avoid having a class implementing for instance `IRequestHandler<,>` and `INotificationHandler<>`
                        // the other option would be to remove this call
                        // see also https://github.com/jbogard/MediatR/issues/462
                        .AsImplementedInterfaces();
                }
            }

            //builder.RegisterInstance(writer).As<TextWriter>();

            // It appears Autofac returns the last registered types first
            //builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(GenericRequestPreProcessor<>)).As(typeof(IRequestPreProcessor<>));
            //builder.RegisterGeneric(typeof(GenericRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));
            //builder.RegisterGeneric(typeof(GenericPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(ConstrainedRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));
            //builder.RegisterGeneric(typeof(ConstrainedPingedHandler<>)).As(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();

            // The below returns:
            //  - RequestPreProcessorBehavior
            //  - RequestPostProcessorBehavior
            //  - GenericPipelineBehavior
            //  - RequestExceptionActionProcessorBehavior
            //  - RequestExceptionProcessorBehavior

            //var behaviors = container
            //    .Resolve<IEnumerable<IPipelineBehavior<Ping, Pong>>>()
            //    .ToList();

            var mediator = container.Resolve<IMediator>();
            var engine = container.Resolve<CommandEngine>();

            return engine;
        }

        static async Task Main(string[] args)
        {

            var commandEngine = UseCommandEngine(new Type[]
            { typeof(CreateNewImageCommand),
                typeof(RemoveImageCommand)
            });


            var crono = new Stopwatch();
            crono.Start();
            foreach (var item in new DirectoryInfo(@"C:\oppl\a0\O908880\Pictures").GetFiles("*.*"))
            {
                await commandEngine.Send(new CreateNewImageCommand { Name =item.Name, Path = item.FullName}); 
            }
            
            var q1 = new ImagesDetailsQueries();
            var totalDetails = await q1.ImagesDetailsTotal();
            Console.WriteLine($"Total details => { totalDetails.Total }");


            foreach (var item in  await q1.GetImageDetails())
            {
                Console.WriteLine(item.Name);
                await commandEngine.Send(new RemoveImageCommand { Id = item.Id }); 
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