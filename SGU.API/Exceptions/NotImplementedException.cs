using System.Net;

namespace SGU.API.Exceptions
{
    public class NotImplementedException : CustomException
    {
        public NotImplementedException(string message) 
            : base(message, null, HttpStatusCode.NotImplemented)
        {
            
        }
    }
}
