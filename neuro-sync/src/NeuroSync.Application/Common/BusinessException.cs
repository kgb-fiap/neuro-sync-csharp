using System;
using System.Net;

namespace NeuroSync.Application.Common
{
    public class BusinessException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public BusinessException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, Exception? inner = null)
            : base(message, inner)
        {
            StatusCode = statusCode;
        }
    }
}
