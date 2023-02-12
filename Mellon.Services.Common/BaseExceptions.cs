using System;
using System.Collections.Generic;
using System.Net;

namespace Mellon.Common.Services
{
    public abstract class BaseException : Exception
    {

        public IDictionary<string, object> Extensions { get; }
        public HttpStatusCode StatusCode { get; }
        public string Title { get; }
        public ErrorCodes ErrorCode { get; }

        public BaseException(HttpStatusCode statusCode, string title, string message, ErrorCodes? errorCode = null, Exception inner = null) : base(message, inner)
        {
            ErrorCode = errorCode ?? ErrorCodes.GENERIC_UNKNOWN;
            StatusCode = statusCode;
            Title = title;
            Extensions = new Dictionary<string, object>();
        }
    }

    public class BadRequestException : BaseException
    {
        public const string TITLE = "The request is not valid.";
        public BadRequestException(string message, ErrorCodes? errorCode = null, Exception inner = null)
            : base(HttpStatusCode.BadRequest, TITLE, message, errorCode ?? ErrorCodes.GENERIC_BAD_REQUEST, inner)
        {
        }
    }

    public class ForbiddenException : BaseException
    {
        public const string TITLE = "The request is valid, but the server is refusing action.";
        public ForbiddenException(string message, ErrorCodes? errorCode = null, Exception inner = null)
            : base(HttpStatusCode.Forbidden, TITLE, message, errorCode ?? ErrorCodes.GENERIC_FORBIDDEN, inner)
        {
        }
    }

    public class ServiceUnavailableException : BaseException
    {
        public const string TITLE = "A problem has occurred.";
        public ServiceUnavailableException(string message, ErrorCodes? errorCode = null, Exception inner = null)
            : base(HttpStatusCode.ServiceUnavailable, TITLE, message, errorCode ?? ErrorCodes.GENERIC_SERVICE_UNAVAILABLE, inner)
        {
        }
    }

    public class NotFoundException : BaseException
    {
        public const string TITLE = "The resource is not found.";
        public NotFoundException(string resourceName, object identifier, ErrorCodes? errorCode = null, Exception inner = null)
            : base(HttpStatusCode.NotFound, TITLE, GetMessage(resourceName, identifier), errorCode ?? ErrorCodes.GENERIC_NOT_FOUND, inner)
        {
        }

        private static string GetMessage(string resourceName, object identifier)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentException($"'{nameof(resourceName)}' cannot be null or empty.", nameof(resourceName));
            return $"Resource {resourceName} with identifier {identifier} was not found.";
        }
    }

    public class ApprovalStatusException : BaseException
    {
        public const string TITLE = "Approval is on another status";
        public ApprovalStatusException(string resourceName, object identifier, ErrorCodes? errorCode = null, Exception inner = null)
            : base(HttpStatusCode.BadRequest, TITLE, GetMessage(resourceName, identifier), errorCode ?? ErrorCodes.APPROVAL_WRONG_STATUS
                  , inner)
        {
        }

        private static string GetMessage(string resourceName, object identifier)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentException($"'{nameof(resourceName)}' cannot be null or empty.", nameof(resourceName));
            return $"Resource {resourceName} with identifier {identifier} was not found.";
        }
    }

    public class ERPDecisionException : BaseException
    {
        public const string TITLE = "ERP Problem has been accured";
        public ERPDecisionException(string message, ErrorCodes? errorCode = null, Exception inner = null)
            : base(HttpStatusCode.InternalServerError, TITLE, message, errorCode ?? ErrorCodes.ERP_ERROR
                  , inner)
        {
        }

        
    }

    public sealed class GuardException : BadRequestException
    {
        public GuardException(string message, ErrorCodes? errorCode = null)
            : base(message, errorCode ?? ErrorCodes.GENERIC_GUARD)
        {
        }
    }
}
