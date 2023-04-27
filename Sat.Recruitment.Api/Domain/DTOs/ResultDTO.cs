using System.Collections.Generic;

namespace Sat.Recruitment.Api.Domain.DTOs
{
    public class ResultDTO<T>
    {
        public ResultDTO(bool isSuccess, string errors="", IList<T> data = null)
        {
            IsSuccess = isSuccess;
            Errors = errors;
            Data = data;
        }

        public bool IsSuccess { get; private set; }
        public string Errors { get; private set; }
        public IList<T> Data { get; private set;}
    }
}
