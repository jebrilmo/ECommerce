using Domain.Operations;
using Infrastructure.Classes.DB;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;

namespace Api
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            TwilioClient.Init(Configuration["MobileProviderAccountId"], Configuration["MobileProviderToken"]);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<DataBaseContext>(context => context.UseMySQL(Configuration.GetConnectionString("ConnectionString")));
            services.AddScoped<OperationsUnitOfWork, OperationsUnitOfWork>();
            services.AddScoped<IRepositoryUnitOfWork, RepositoryUnitOfWork>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
