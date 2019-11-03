using System;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using GridLock.AspNetCore.Mvc.Authorization;
using GridLock.Extensions.Storage.Distributed;
using Microsoft.Extensions.Caching.Distributed;

namespace GridLock.Extensions.DependencyInjection
{
    public static class GridLockServiceExtension
    {
        public static IServiceCollection AddGridLock(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddTransient<ISharedStorage, SharedStorage>();
            services.AddTransient<IGridLock, GridLock>();
            services.AddTransient<GridLockAuthorizationAttribute>();

            return services;
        }

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

