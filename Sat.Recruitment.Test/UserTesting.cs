using System;
using System.IO;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sat.Recruitment.Api.Application.Commands.UserCommands;
using Sat.Recruitment.Api.Application.Queries.UserQueries;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Domain.Interfaces;
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
            var servicesType = Type.GetType("Microsoft.Extensions.DependencyInjection.ServiceCollection," +
                 " Microsoft.Extensions.DependencyInjection.Abstractions");
            var services = (IServiceCollection)Activator.CreateInstance(servicesType);

            services.AddTransient<IFileService>(sp =>
            {
                string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Files\\{0}", "Users.txt");
                return new FileService(string.Format(path, "Read"), string.Format(path, "Write"));
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IValidator<UserCreateCommand>, UserCreateCommandValidator>();

            services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssembly(typeof(UserCreateCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(UserListQueryHandler).Assembly);
            });

            var serviceProvider = services.BuildServiceProvider();
            mediator = serviceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task CreateUser_Should_Return_Successful_Result()
        {
            //Arange
            var commandC = new UserCreateCommand()
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = Api.Domain.Enums.UserType.Normal,
                Money = 124
            };
            var controller = new UsersController(mediator);

            var commandM = new UserCreateCommand()
            {
                Name = "Mike M",
                Email = "mike_M@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354999",
                UserType = Api.Domain.Enums.UserType.Normal,
                Money = 132
            };

            //Act
            var resultController = await controller.CreateUser(commandC); //From Controller
            var resultMediator = await mediator.Send(commandM); //From Mediator

            //Assert
            Assert.Equal(true, resultController.IsSuccess);
            Assert.Equal("User Created", resultController.Errors);

            Assert.Equal(true, resultMediator.IsSuccess);
            Assert.Equal("User Created", resultMediator.Errors);
        }

        [Fact]
        public async Task CreateUser_Should_Return_Error_Result()
        {
            //Arange
            var commandC = new UserCreateCommand()
            {
                Name = "Mike S",
                Email = "mikeS---gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 11442454215",
                UserType = Api.Domain.Enums.UserType.Normal,
                Money = 124
            };
            var controller = new UsersController(mediator);

            var commandM = new UserCreateCommand()
            {
                Name = "Mike MS",
                Email = "mike_M..gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 11223549439",
                UserType = Api.Domain.Enums.UserType.Normal,
                Money = 132
            };

            //Act
            var resultController = await controller.CreateUser(commandC); //From controller
            var resultMediator = await mediator.Send(commandM); //From Mediator

            //Assert
            Assert.Equal(false, resultController.IsSuccess);
            Assert.Equal("Invalid email", resultController.Errors);

            Assert.Equal(false, resultMediator.IsSuccess);
            Assert.Equal("Invalid email", resultMediator.Errors);
        }

        [Fact]
        public async Task GetUsers_Should_Return_Successfull_Result()
        {
            //Arange
            var controller = new UsersController(mediator);
            //Action
            var resultController = await controller.ListUser(); // From Controller
            var resultMediator = await mediator.Send(new UserListQuery()); // From Mediator
            //Assert
            Assert.Equal(resultController.IsSuccess, true);
            Assert.Equal(resultController.Data.Count > 0, true);

            Assert.Equal(resultMediator.IsSuccess, true);
            Assert.Equal(resultMediator.Data.Count > 0, true);
        }
    }
}
