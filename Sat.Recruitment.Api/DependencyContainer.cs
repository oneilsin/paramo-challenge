using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sat.Recruitment.Api.Application.Commands.UserCommands;
using Sat.Recruitment.Api.Application.Queries.UserQueries;
using Sat.Recruitment.Api.Domain.Interfaces;
using Sat.Recruitment.Api.Infrastructure.DataConfiguration;
using Sat.Recruitment.Api.Infrastructure.Repositories;

namespace Sat.Recruitment.Api
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddSatServices(
            this IServiceCollection services,
            string readFileConnection,
            string writeFileConnection)
        {
            services.AddScoped<IFileService, FileService>(
               (provider) =>
               {
                   return new FileService(readFileConnection, writeFileConnection);
               });
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssembly(typeof(UserCreateCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(UserListQueryHandler).Assembly);
            });
            services.AddScoped<IValidator<UserCreateCommand>, UserCreateCommandValidator>();

            return services;
        }
    }
}
