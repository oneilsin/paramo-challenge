using FluentValidation;
using MediatR;
using Sat.Recruitment.Api.Domain.DTOs;
using Sat.Recruitment.Api.Domain.Interfaces;
using Sat.Recruitment.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Application.Commands.UserCommands
{
    public class UserCreateCommandHandler :
        IRequestHandler<UserCreateCommand, ResultDTO<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserCreateCommand> _validator;
        public UserCreateCommandHandler(IUserRepository userRepository, IValidator<UserCreateCommand> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<ResultDTO<User>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                string errors = string.Join(",", validationResult.Errors.ToList().Select(u => u.ToString()));
                return new ResultDTO<User>(
                    isSuccess: false,
                    errors: errors
                    );
                //throw new ValidationException(validationResult.Errors);
            }

            // Validate duplicate values.
            var users = await _userRepository.GetUsers();
            var existingUser = users
            .FirstOrDefault(u => u.Email == request.Email || u.Phone == request.Phone || (u.Name == request.Name && u.Address == request.Address));

            if (existingUser != null)
            {
                return new ResultDTO<User>(
                    isSuccess: false,
                    errors: "User is duplicated"
                );
            }

            // Set User Values
            var user = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Address = request.Address,
                Phone = request.Phone,
                UserType = request.UserType.ToString(),
                Money = request.Money
            };

            //Calculate gift by UserType
            var gifts = new Dictionary<Domain.Enums.UserType, Func<decimal, (decimal, decimal)>>
            {
                { Domain.Enums.UserType.Normal, money => money >= 100 ? CalculateGift(money, 0.12m) : money > 10 ? CalculateGift(money, 0.8m) : (0m, 0m) },
                { Domain.Enums.UserType.SuperUser, money => money > 100 ? CalculateGift(money, 0.20m) : (0m, 0m) },
                { Domain.Enums.UserType.Premium, money => money > 100 ? CalculateGift(money, 2m) : (0m, 0m) }
            };

            if (gifts.ContainsKey(request.UserType))
            {
                var (percentage, gift) = gifts[request.UserType](user.Money);
                user.Money += gift;
            }

            // Creare new User
            var userList = users.ToList();
            userList.Add(user);
            await _userRepository.CreateUser(userList);
            return new ResultDTO<User>(
                   isSuccess: true,
                   errors: "User Created");
        }

        private (decimal percentage, decimal gift) CalculateGift(decimal money, decimal percentage)
        {
            decimal gift = money * percentage;
            return (percentage, gift);
        }
    }
}
