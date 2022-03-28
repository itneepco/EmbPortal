using System.Reflection;
using Application.Behaviors;
using Application.Interfaces;
using Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {            
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IMeasurementBookService, MeasurementBookService>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();
            services.AddScoped<IRABillService, RABillService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));

            return services;
        }
    }
}
