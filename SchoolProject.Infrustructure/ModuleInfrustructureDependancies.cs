using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.IRepositories;
using SchoolProject.Infrustructure.Repositories;

namespace SchoolProject.Infrustructure
{
    public static class ModuleInfrustructureDependancies
    {
        public static IServiceCollection AddInfrustructureDependancies(this IServiceCollection services)
        {
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IInstructorsRepository, InstructorsRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            return services;
        }
    }
}
