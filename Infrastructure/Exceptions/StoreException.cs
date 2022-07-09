using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class StoreException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public StoreException(string? message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
