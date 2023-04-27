    using MediatR;
using Sat.Recruitment.Api.Domain.DTOs;
using Sat.Recruitment.Api.Domain.Enums;
using Sat.Recruitment.Api.Domain.Models;

namespace Sat.Recruitment.Api.Application.Commands.UserCommands
{
    public class UserCreateCommand : IRequest<ResultDTO<User>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public UserType UserType { get; set; }
        public decimal Money { get; set; }
    }
}
