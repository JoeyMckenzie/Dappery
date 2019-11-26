namespace Dappery.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class DapperyApiException : Exception
    {
        public DapperyApiException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
            ApiErrors = new List<DapperyApiError>();
        }

        public HttpStatusCode StatusCode { get; }

        public ICollection<DapperyApiError> ApiErrors { get; }
    }
}