using EDUZilla.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace EDUZilla.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectService(this IServiceCollection services)
        {
            services.AddScoped<StudentRepository>();
            services.AddScoped<StudentService>();

            services.AddScoped<ParentRepository>();
            services.AddScoped<ParentService>();

            services.AddScoped<ClassRepository>();
            services.AddScoped<ClassService>();

            services.AddScoped<TeacherRepository>();

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddViewLocalization().AddDataAnnotationsLocalization(options =>
            {
                var type = typeof(LanguageResource);
                var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                var factory = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                var localizer = factory.Create("LanguageResource", assemblyName.Name);
                options.DataAnnotationLocalizerProvider = (t, f) => localizer;
            });

            return services;
        }
    }
}
