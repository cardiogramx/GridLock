using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GridLock
{
    public static class GridLockMiddlewareExtension
    {
        public static void UseGridLock(this IApplicationBuilder app, IGridLock gridLock)
        {
            app.Use(async (context, next) =>
            {
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
