using SchoolApp.Application.Interfaces;
using SchoolApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        return services;
    }
}
