using EDUZilla.Data.Repositories;

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

            return services;
        }
    }
}
