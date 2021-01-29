using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDb.Api.Data.Configuration.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using CosmosDb.Api.Interface;
using CosmosDb.Api.Services;
using Cosmonaut;


namespace CosmosDb.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            ConfigureComosStoreSetting(services);

            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IFamilyService, FamilyService>();
            services.AddScoped<IFamilyServiceCosmonault, FamilyServiceCosmonaut>();

            services.AddControllers();

            AddSwagger(services);


        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("default");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseReDoc();

        }

        private void AddCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("default",
                    builder => builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddOpenApiDocument(d =>
            {
                d.Title = Configuration.GetSection("Application").Value;
            });
        }

        private void ConfigureComosStoreSetting(IServiceCollection services)
        {
            Database database = null;
            var cosmosStoreConfig = Configuration.GetSection("CosmosStoreConnection").Get<CosmosStoreConfigs>();

           var cosmosClient = new CosmosClient(cosmosStoreConfig.AccountEndPoint, cosmosStoreConfig.AccountKey);

            var cosmosStore = new CosmosStore
            {
                ClientStore = cosmosClient,
                DatabaseCosmos = database,
                DatabaseName = cosmosStoreConfig.Database
            };

            services.AddSingleton(cosmosStore);

            //using Cosmonault DI

            var cosmonaultStore = new CosmosStoreSettings(cosmosStoreConfig.Database,
                                                          cosmosStoreConfig.AccountEndPoint,
                                                          cosmosStoreConfig.AccountKey);
            services.AddSingleton(cosmonaultStore);



        }
    }
}
