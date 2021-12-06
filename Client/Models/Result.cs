using System.Collections.Generic;

namespace Client.Models
{
    public class Result : IResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }

    public class Result<T> : Result, IResult<T>
    {
        public T Data { get; set; }
    }
}
