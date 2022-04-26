using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbPortal.Shared.Responses
{
    public class Result : IResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; }
        public bool Succeeded { get; set; }

        public static IResult Success(string message)
        {
            return new Result { Succeeded = true, Message = message };
        }

        public static IResult Fail(string message)
        {
            return new Result { Succeeded = false, Message = message };
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public T Data { get; set; }

        public static IResult<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T> { Succeeded = false, Errors = errors.ToList() };
        }
        public static async Task<IResult<T>> FailureAsync(IEnumerable<string> errors)
        {
            return await Task.FromResult(Failure(errors));
        }
        public static IResult<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data };
        }
        public static async Task<IResult<T>> SuccessAsync(T data)
        {
            return await Task.FromResult(Success(data));
        }
    }
}
