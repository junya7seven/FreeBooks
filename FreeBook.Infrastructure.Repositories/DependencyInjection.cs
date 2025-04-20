using FreeBook.Infrastructure.Repositories.Service;
using FreeBookAPI.Application.Interfaces;
using FreeBookAPI.Infrastructure.StorageAPI;
using FreeBookAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBook.Infrastructure.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton(configuration);
            services.AddHttpClient();
            services.AddScoped<IStorageService, StorageService>(); 
            services.AddScoped<IBookQuery, BookQuery>();
            services.AddScoped<IBookCommand, BookCommand>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }

}
