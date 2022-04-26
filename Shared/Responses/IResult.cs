using System.Collections.Generic;

namespace EmbPortal.Shared.Responses
{
    public interface IResult
    {
        List<string> Errors { get; set; }
        public string Message { get; set; }
        bool Succeeded { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}
