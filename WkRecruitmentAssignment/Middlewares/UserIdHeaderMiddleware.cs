using Common.Domain.ActionContext;
using Common.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WebApi.Middlewares
{
    public class UserIdHeaderMiddleware
    {
        RequestDelegate _next;

        public UserIdHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx, IActionContextProvider actionContextProvider)
        {
            if (ctx.Request.Headers.TryGetValue("userId", out var val) && Guid.TryParse(val, out var userId))
                actionContextProvider.RegisterContext(new ActionContext(new UserId(userId)));
            await _next(ctx);
        }
    }
}
