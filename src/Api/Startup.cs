using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using AutoMapper;

using ISTS.Api.Filters;
using ISTS.Api.Helpers;
using ISTS.Infrastructure.Model;

namespace ISTS.Api
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
            services.AddCors();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HandleUnauthorizedAccessExceptionAttribute));
                options.Filters.Add(typeof(HandleApiExceptionAttribute));
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<IstsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IstsDb")));

            var appSettings = Configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettings>(appSettings);
            
            ConfigureAuthentication(services, appSettings["AuthenticationSecret"]);
            DependencyInjectionConfiguration.Configure(services);
        }

        private void ConfigureAuthentication(IServiceCollection services, string secret)
        {
            var key = System.Text.Encoding.Default.GetBytes(secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
