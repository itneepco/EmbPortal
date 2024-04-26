using System.Reflection;
using Application.Behaviors;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {            
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IMeasurementBookService, MeasurementBookService>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                options.AddOpenBehavior(typeof(PerformanceBehavior<,>));
                options.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
            });

            return services;
        }
    }
}
