using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedCacheLinkage.Objects;
using DistributedCacheLinkage.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleWeb.Models.Data;
using SampleWeb.Models.Repositories;

namespace SampleWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddControllers();


            services.AddScoped<IEntityRepository<Member, int>, MemberRepository>();
            services.AddScoped<IEntityRepository<Corp, int>, CorpRepository>();

            services.AddScoped<RepositoryProxy<Member, int>>();
            services.AddScoped<RepositoryProxy<Corp, int>>();

            services.AddScoped<IEntityOne<List<Setting>>, SettingModel>();

            services.AddScoped<OneObjectProxy<List<Setting>>>();

            services.AddScoped<IEntityMany<Icon, int>, IconsModel>();

            services.AddScoped<ManyObjectProxy<Icon, int>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
