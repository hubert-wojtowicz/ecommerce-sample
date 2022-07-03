using Application.ShoppingCart.AddItem;
using Common.Domain.ActionContext;
using Domain.DiscountVoucher;
using Domain.Product;
using Domain.ShoppingCart;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MediatR;
using System.Linq;
using WebApi.Middlewares;
using System;
using WebApi.Controllers.Order;
using Domain.Order;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WkRecruitmentAssignment", Version = "v1" });
            });
            services.AddMediatR(typeof(AddProductCommandHandler));

            services.AddSingleton<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IDiscountVoucherRepository, DiscountVoucherRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();

            services.AddScoped<IActionContextProvider, ActionContextProvider>();

            AddMappers(services);
        }

        private void AddMappers(IServiceCollection services)
        {
            services.AddScoped<ICreateOrderRequsestMapper, CreateOrderRequsestMapper>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var validCorsOrigins = Configuration.GetSection("CorsOrigins").GetChildren().Select(x => x.Value).ToArray();
            app.UseCors(options => options
                .WithOrigins(validCorsOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WkRecruitmentAssignment v1"));
            }

            app.UseMiddleware<UserIdHeaderMiddleware>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
