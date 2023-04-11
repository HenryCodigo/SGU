using System.Net;

namespace SGU.API.Exceptions
{
    public class UnauthorizedAccessException : CustomException
    {
        public UnauthorizedAccessException(string message)
            :base(message, null, HttpStatusCode.Unauthorized)
        {
        }
    }
}
