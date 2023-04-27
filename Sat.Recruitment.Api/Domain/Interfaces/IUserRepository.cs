using Sat.Recruitment.Api.Domain.DTOs;
using Sat.Recruitment.Api.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task CreateUser(List<User> userCreate);
    }
}
