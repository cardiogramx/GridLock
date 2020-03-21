using System;

using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

using GridLock.AspNetCore.Mvc.Authorization;
using GridLock.Extensions.Storage.Distributed;


namespace GridLock.Extensions.DependencyInjection
{
    public static class GridLockServiceExtension
    {
        /// <summary>
        /// Adds GridLock services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGridLock(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddTransient<ISharedStorage, SharedStorage>();
            services.AddTransient<IGridLock, GridLock>();
            services.AddTransient<GridLockAuthorizationAttribute>();

            return services;
        }

        /// <summary>
        /// Adds GridLock services with <see cref="RedisCacheOptions"/> configurations to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddGridLock(this IServiceCollection services, Action<RedisCacheOptions> options)
        {
            services.AddStackExchangeRedisCache(options);
            services.AddTransient<ISharedStorage, SharedStorage>();
            services.AddTransient<IGridLock, GridLock>();
            services.AddTransient<GridLockAuthorizationAttribute>();

            return services;
        }
    }
}

