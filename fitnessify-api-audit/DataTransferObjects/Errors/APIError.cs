﻿namespace fitnessify_api_audit.DataTransferObjects.Errors
{
    public class APIError
    {
        public APIError(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
