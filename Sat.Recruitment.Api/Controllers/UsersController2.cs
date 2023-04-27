using System;
using System.IO;

namespace Sat.Recruitment.Api.Controllers
{
    public partial class UsersController
    {
        private StreamReader ReadUsersFromFile(string connectionPath)
        {
            var path = connectionPath;//Directory.GetCurrentDirectory() + "/Files/Users.txt";

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);
            return reader;
        }
    }
}
