using MediatR;
using Sat.Recruitment.Api.Domain.DTOs;
using Sat.Recruitment.Api.Domain.Models;

namespace Sat.Recruitment.Api.Application.Queries.UserQueries
{
    public class UserListQuery : IRequest<ResultDTO<User>>
    {
    }
}
