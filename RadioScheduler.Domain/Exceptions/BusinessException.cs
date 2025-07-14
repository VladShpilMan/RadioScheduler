using System.Net;

namespace RadioScheduler.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public BusinessException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
