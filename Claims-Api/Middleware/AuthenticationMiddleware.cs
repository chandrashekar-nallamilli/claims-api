using System;
using System.Text;
using System.Threading.Tasks;
using Claims_Api.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Claims_Api.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly string AUTHORIZATION_HEADER_PREFIX = "Bearer";

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var pathBase = context.Request.PathBase.Value;
                var path = context.Request.Path.Value;

                if (path.Equals("/") && pathBase.Equals(""))
                {
                    context.Response.StatusCode = 200;
                    await _next.Invoke(context);
                }else if(path.StartsWith("/index") || path.StartsWith("/docs") || path.StartsWith("/swagger") || path.StartsWith("/health"))
                {
                    context.Response.StatusCode = 200;
                    await _next.Invoke(context);
                }
                else
                {
                    string authHeader = context.Request.Headers["Authorization"];

                    if (authHeader == null)
                    {
                        // no authorization header
                        await SetContextAndMessage(context, "No Authorization Header");

                        return;
                    }

                    string[] header = authHeader.Split(' ');
                    switch (header[0].ToLower())
                    {
                        case "bearer":
                            await BearerAuthorization(context, authHeader);
                            break;
                        default:
                            await ThrowInvalidHeader(context, authHeader);
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                await SetContextAndErrorMessage(context, ex.Message);
            }           
        }

        private async Task SetContextAndMessage(HttpContext context, string message)
        {
            context.Response.StatusCode = 401; //Unauthorized
            context.Response.ContentType = "application/json";
            var errorResponse = new ErrorResponse
            {
                Message = message
            };
            //string jsonString = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse), Encoding.UTF8);
        }

        private async Task SetContextAndErrorMessage(HttpContext context, string message)
        {
            context.Response.StatusCode = 500; 
            context.Response.ContentType = "application/json";
            var errorResponse = new ErrorResponse
            {
                Message = message
            };
            //string jsonString = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse), Encoding.UTF8);
        }

        private async Task ThrowInvalidHeader(HttpContext context, string authHeader)
        {
            await SetContextAndMessage(context, $"Authorization header Invalid {authHeader}");
        }

        private async Task BearerAuthorization(HttpContext context, string authHeader)
        {
            var bearerToken = authHeader.Substring(AUTHORIZATION_HEADER_PREFIX.Length).Trim();

            if (string.IsNullOrEmpty(bearerToken))
            {
                await SetContextAndMessage(context, "Bearer token is null");
                
                return;
            }
            await _next.Invoke(context);
        }
    }
}
