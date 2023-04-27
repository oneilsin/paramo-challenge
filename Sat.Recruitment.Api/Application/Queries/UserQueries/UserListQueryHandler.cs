using MediatR;
using Sat.Recruitment.Api.Domain.DTOs;
using Sat.Recruitment.Api.Domain.Interfaces;
using Sat.Recruitment.Api.Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Application.Queries.UserQueries
{
    public class UserListQueryHandler :
        IRequestHandler<UserListQuery, ResultDTO<User>>
    {
        private readonly IUserRepository _userRepository;
        public UserListQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResultDTO<User>> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetUsers();
            if (result == null)
            {
                return new ResultDTO<User>(
                   isSuccess: false,
                   errors: "Users not found"
                   );
            }

            return new ResultDTO<User>(
                   isSuccess: true,
                   data: result.ToList()
                   );
        }
    }
}
