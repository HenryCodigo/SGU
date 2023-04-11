using System.Net;

namespace SGU.API.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message)
            :base(message, null, HttpStatusCode.BadRequest)
        {
            
        }
    }
}
