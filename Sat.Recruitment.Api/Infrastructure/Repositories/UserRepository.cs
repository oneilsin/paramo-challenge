using Sat.Recruitment.Api.Domain.Interfaces;
using Sat.Recruitment.Api.Domain.Models;
using Sat.Recruitment.Api.Infrastructure.DataConfiguration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IFileService _fileService;
        public UserRepository(IFileService fileService)
        {
            _fileService = fileService;
        }

        public Task CreateUser(List<User> userCreate)
        {
            _fileService.WriteValuesToFile<User>(userCreate);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return Task.FromResult(_fileService.ReadValuesFromFile<User>());
        }

        public async Task<bool> CheckDuplicateUser(string email, string phone, string name, string address)
        {
            var users = await GetUsers();
            var existingUser = users
                .FirstOrDefault(u => u.Email == email || u.Phone == phone || (u.Name == name && u.Address == address));

            return existingUser != null;
        }

    }
}
