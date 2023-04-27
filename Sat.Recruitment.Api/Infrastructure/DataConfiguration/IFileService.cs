using System.Collections.Generic;
using System.IO;

namespace Sat.Recruitment.Api.Infrastructure.DataConfiguration
{
    public interface IFileService
    {
        IEnumerable<T> ReadValuesFromFile<T>() where T : class, new();
        void WriteValuesToFile<T>(List<T> data) where T : class, new();
    }
}
