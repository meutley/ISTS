using Microsoft.Extensions.DependencyInjection;

using ISTS.Application.Rooms;
using ISTS.Application.Schedules;
using ISTS.Application.Studios;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;
using ISTS.Infrastructure.Repository;

namespace ISTS.Api
{
    public static class DependencyInjectionConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<ISessionScheduleValidator, SessionScheduleValidator>();
            
            services.AddSingleton<IStudioRepository, StudioRepository>();
            services.AddSingleton<IRoomRepository, RoomRepository>();
            
            services.AddSingleton<IStudioService, StudioService>();
            services.AddSingleton<IRoomService, RoomService>();
        }
    }
}