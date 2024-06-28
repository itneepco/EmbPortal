using System;
using System.Collections.Generic;

namespace Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException() : base()
    {
    }

    public BadRequestException(string message) : base(message)
    {
    }

    public IEnumerable<string> Errors { get; set; } = new List<string>();
}