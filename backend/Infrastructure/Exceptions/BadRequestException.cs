namespace Infrastructure.Exceptions;
using JetBrains.Annotations;

public class BadRequestException : Exception
{
    [StringFormatMethod("message")]
    public BadRequestException(string message)
        : base(message)
    {
    }

    [StringFormatMethod("message")]
    public BadRequestException(string message, object arg0)
        : base(string.Format(message, arg0))
    {
    }

    [StringFormatMethod("message")]
    public BadRequestException(string message, object arg0, object arg1)
        : base(string.Format(message, arg0, arg1))
    {
    }

    [StringFormatMethod("message")]
    public BadRequestException(string message, object arg0, object arg1, object arg2)
        : base(string.Format(message, arg0, arg1, arg2))
    {
    }

    [StringFormatMethod("message")]
    public BadRequestException(string message, params object[] args)
        : base(string.Format(message, args))
    {
    }
}