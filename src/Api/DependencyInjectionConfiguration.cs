using Microsoft.Extensions.DependencyInjection;

using ISTS.Application.Common;
using ISTS.Application.Rooms;
using ISTS.Application.Schedules;
using ISTS.Application.Studios;
using ISTS.Application.Users;
using ISTS.Domain.Common;
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
            services.AddSingleton<ISessionScheduleValidator, SessionScheduleValidator>();
            services.AddSingleton<IEmailValidator, EmailValidator>();
            services.AddSingleton<IUserValidator, UserValidator>();
            
            services.AddSingleton<IStudioRepository, StudioRepository>();
            services.AddSingleton<IRoomRepository, RoomRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            
            services.AddSingleton<IStudioService, StudioService>();
            services.AddSingleton<IRoomService, RoomService>();
            services.AddSingleton<IUserService, UserService>();
        }
    }
}