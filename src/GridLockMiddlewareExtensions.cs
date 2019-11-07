using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GridLock
{
    public static class GridLockMiddlewareExtension
    {
        /// <summary>
        /// Uses GridLock middleware to lock down your application.
        /// </summary>
        /// <param name="app"></param>
        public static void UseGridLock(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var gridLock = context.RequestServices.GetService<IGridLock>();

                var key = context.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(key) || await gridLock.ValidateAsync<GridLockItem>(key) == false)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(string.Empty);
                }

                await next.Invoke();

            });


        }
    }
}
