using Kanban.Data.Contexts;
using Kanban.Data.Repositories;
using Kanban.Domain.Interfaces.Repositories;
using Kanban.Domain.Interfaces.Services;
using Kanban.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kanban.IoC
{
    public static class RegisterDependencies
    {
        public static IServiceCollection Services(this IServiceCollection services)
        {
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        public static IServiceCollection Repositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository, BaseRepository>();

            return services;
        }

        public static IServiceCollection Databases(this IServiceCollection services)
        {
            services.AddDbContext<KanbanContext>(options => options.UseInMemoryDatabase("KanbanDatabase"));
            services.AddScoped<ILogger, Logger<KanbanContext>>();

            return services;
        }
    }
}
