using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text;
using Mellon.Services.Infrastracture.Base;
using Mellon.Common.Services;

namespace Mellon.Services.Api
{
    public class ExceptionMiddleware
    {
    

        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="ExceptionMiddleware"/>.
        /// </summary>
        /// <param name="next">The next item in the middleware pipeline.</param>
        /// <param name="loggerFactory">Factory to create instances of <see cref="ILogger"/></param>
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Invokes the middleware performing global exception handling.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var logger = _loggerFactory.CreateLogger<ExceptionMiddleware>();

            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                string traceIdentifier = context.TraceIdentifier;
                var status = (int)HttpStatusCode.InternalServerError;
                var title = "An unhandled exception occurred.";
                var exceptionName = "Exception";
                var errorCode = ErrorCodes.GENERIC_UNKNOWN;
                IDictionary<string, object> extensions = null;

                if (exception is BaseException baseException)
                {
                    status = (int)baseException.StatusCode;
                    extensions = baseException.Extensions;
                    title = baseException.Title;
                    exceptionName = exception.GetType().Name;
                    errorCode = baseException.ErrorCode;
                }

                // https://datatracker.ietf.org/doc/html/rfc7807
                var problemDetails = new ProblemDetailsExtention
                {
                    Status = status,
                    Title = $"{title} ({(int)errorCode})",
                    Detail = exception.Message,
                    Instance = traceIdentifier,
                    Code = (int)errorCode
                };

                if (exception is RepositoryException repositoryException)
                {
                    problemDetails.Detail = GetCompleteMessage(repositoryException);
                }

                if (extensions != null)
                    foreach (var kvp in extensions)
                        problemDetails.Extensions.Add(kvp);

                logger.LogError($"Exception happened with trace identifier {traceIdentifier}" +
                        $"\n {GetCompleteMessage(exception)}" +
                        $"\n {exception.StackTrace}");

                var response = context.Response;
                response.StatusCode = problemDetails.Status.Value;
                response.Headers.Add("Error-Code", ((int)errorCode).ToString());
                response.ContentType = "application/problem+json";
                
                var result = JsonSerializer.Serialize(problemDetails);
                await response.WriteAsync(result);
            }
        }

        public class ProblemDetailsExtention : ProblemDetails
        {
            public int Code { get; set; }
        }

        private static string GetCompleteMessage(Exception error)
        {
            StringBuilder builder = new();
            Exception realerror = error;
            builder.AppendLine(error.Message);
            while (realerror.InnerException != null)
            {
                builder.AppendLine(realerror.InnerException.Message);
                realerror = realerror.InnerException;
            }
            return builder.ToString();
        }
    }
}
