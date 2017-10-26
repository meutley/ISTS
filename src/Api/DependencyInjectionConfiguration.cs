using Microsoft.Extensions.DependencyInjection;

using ISTS.Application.Common;
using ISTS.Application.PostalCodes;
using ISTS.Application.Rooms;
using ISTS.Application.Schedules;
using ISTS.Application.Studios;
using ISTS.Application.Users;

using ISTS.Domain.Common;
using ISTS.Domain.PostalCodes;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;
using ISTS.Domain.Users;
using ISTS.Infrastructure.Repository;

namespace ISTS.Api
{
    public static class DependencyInjectionConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IEmailValidator, EmailValidator>();
            services.AddScoped<IPostalCodeValidator, PostalCodeValidator>();
            services.AddScoped<ISessionScheduleValidator, SessionScheduleValidator>();
            services.AddScoped<IStudioValidator, StudioValidator>();
            services.AddScoped<IUserValidator, UserValidator>();
            
            services.AddScoped<IPostalCodeRepository, PostalCodeRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IStudioRepository, StudioRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<IStudioService, StudioService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserPasswordService, UserPasswordService>();
        }
    }
}