using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Consul;
using MedPark.Catalog.Domain;
using MedPark.Catalog.Dto;
using MedPark.Catalog.Handlers.Catalog;
using MedPark.Catalog.Infrastructure;
using MedPark.Catalog.Queries;
using MedPark.Common;
using MedPark.Common.Consul;
using MedPark.Common.Handlers;
using MedPark.Common.Mongo;
using MedPark.Common.Mvc;
using MedPark.Common.RabbitMq;
using MedPark.Common.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Category = MedPark.Catalog.Domain.Category;

namespace MedPark.Catalog
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddRedis(Configuration);
            services.AddInitializers(typeof(IMongoDbInitializer));
            services.AddHealthChecks();
            services.AddConsul();

            services.AddMvc(mvc => mvc.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsImplementedInterfaces();
            builder.AddDispatchers();
            builder.AddRabbitMq();
            builder.AddMongo();
            builder.AddMongoRepository<Product>("product");
            builder.AddMongoRepository<Category>("category");
            builder.AddMongoRepository<ProductCatalog>("product-catalog");
            builder.AddMongoSeeder<MongoCatalogSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IStartupInitializer startupInitializer, IHostApplicationLifetime lifetime, IConsulClient consulClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                startupInitializer.InitializeAsync();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();
            app.UseEndpoints(endpoit =>
            {
                endpoit.MapHealthChecks("/health");
            });
            app.UseHttpsRedirection();

            app.UseRabbitMq();

            var serviceID = app.UseConsul();
            lifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceID);
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
