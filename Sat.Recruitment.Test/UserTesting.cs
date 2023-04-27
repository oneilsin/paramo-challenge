using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sat.Recruitment.Api.Application.Commands.UserCommands;
using Sat.Recruitment.Api.Application.Queries.UserQueries;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Domain.DTOs;
using Sat.Recruitment.Api.Domain.Models;
using Sat.Recruitment.Api.Infrastructure.DataConfiguration;
using Sat.Recruitment.Api.Infrastructure.Repositories;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserTesting
    {
        private readonly IMediator mediator;
        public UserTesting()
        {
            string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Files\\{0}", "Users.txt");
            var files = new FileService(string.Format(path, "Read"), string.Format(path, "Write"));
            var repository = new UserRepository(files);
            var validator = new UserCreateCommandValidator();

            var servicesType = Type.GetType("Microsoft.Extensions.DependencyInjection.ServiceCollection," +
                " Microsoft.Extensions.DependencyInjection.Abstractions");
            var services = (IServiceCollection)Activator.CreateInstance(servicesType);

            services.AddTransient<IRequestHandler<UserListQuery, ResultDTO<User>>, UserListQueryHandler>(sp =>
                new UserListQueryHandler(repository));

            services.AddTransient<IRequestHandler<UserCreateCommand, ResultDTO<User>>, UserCreateCommandHandler>(sp =>
                new UserCreateCommandHandler(repository, validator));

            mediator = new Mediator(services.BuildServiceProvider());
        }

        [Fact]
        public async Task CreateUser_Should_Return_Successful_Result()
        {
            //Arange
            var command = new UserCreateCommand()
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = Api.Domain.Enums.UserType.Normal,
                Money = 124
            };
            var controller = new UsersController(mediator);

            //Act
            var result = await controller.CreateUser(command);

            //Assert
            Assert.Equal(true, result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }

        [Fact]
        public async Task GetUsers_Should_Return_Successfull_Result()
        {
            //Arange
            var controller = new UsersController(mediator);
            //Action
            var result = await controller.ListUser();
            //Assert
            Assert.Equal(result.IsSuccess, true);
            Assert.Equal(result.Data.Count > 0, true);
        }
    }
}
