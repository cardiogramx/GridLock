using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace GridLock.AspNetCore.Mvc.Authorization
{
    /// <summary>
    /// This class is not to be used directly in your code. Use <see cref="GridLockAuthorizationAttribute"/> instead.
    /// </summary>
    public class GridLockAuthorizationAttribute : ActionFilterAttribute
    {
        public int[] Levels { get; set; } = default;

        private readonly IGridLock gridLock;

        public GridLockAuthorizationAttribute(IGridLock gridLock)
        {
            this.gridLock = gridLock;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];

            if (!string.IsNullOrWhiteSpace(id) && gridLock.List().Any(c => c.Id == id))
            {
                if (Levels != null)
                {
                    if (Levels.Any())
                    {
                        if (!Levels.Contains(gridLock.List().Where(c => c.Id == id).Select(c => c.Level).SingleOrDefault()))
                        {
                            context.Result = new UnauthorizedObjectResult(HttpStatusCode.Forbidden);
                        }
                    }
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult(HttpStatusCode.Forbidden);
            }
        }
    }

    public class LockAttribute : Attribute, IFilterFactory
    {
        public int[] AllowedLevels { get; set; }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<GridLockAuthorizationAttribute>();

            if (AllowedLevels.Any())
            {
                filter.Levels = AllowedLevels;
            }

            return filter;
        }
    }
}