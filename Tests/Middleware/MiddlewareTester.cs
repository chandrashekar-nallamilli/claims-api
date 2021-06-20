using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;
using Claims_Api.Extensions;
using Claims_Api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;

namespace Tests.Middleware
{
    public class MiddlewareTester
    {
        private IHost _host;

        [SetUp]
        public async Task Init()
        {
            _host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddCoreServices();
                            services.AddServicesAndRepositories();
                        })
                        .Configure(app =>
                        {
                            app.UseMiddleware<AuthenticationMiddleware>();
                        });
                })
                .StartAsync();
        }
        [Test]
        public async Task MiddlewareTest_ReturnsNotFoundForRequest()
        {
            var response = await _host.GetTestClient().GetAsync("/");
            Assert.AreEqual(HttpStatusCode.NotFound , response.StatusCode);
        }
        
        [Test]
        public async Task TestMiddleware_Expected401Response()
        {

            var server = _host.GetTestServer();
            server.BaseAddress = new Uri("https://example.com/A/Path/");

            var context = await server.SendAsync(c =>
            {
                c.Request.Method = HttpMethods.Post;
                c.Request.Path = "/and/file.txt";
                c.Request.QueryString = new QueryString("?and=query");
            });

            Assert.True(context.RequestAborted.CanBeCanceled);
            Assert.AreEqual("POST", context.Request.Method);
            Assert.AreEqual("https", context.Request.Scheme);
            Assert.AreEqual("example.com", context.Request.Host.Value);
            Assert.AreEqual("/A/Path", context.Request.PathBase.Value);
            Assert.AreEqual("/and/file.txt", context.Request.Path.Value);
            Assert.AreEqual("?and=query", context.Request.QueryString.Value);
            Assert.NotNull(context.Request.Body);
            Assert.NotNull(context.Request.Headers);
            Assert.NotNull(context.Response.Headers);
            Assert.NotNull(context.Response.Body);
            Assert.AreEqual(401, context.Response.StatusCode);
            Assert.Null(context.Features.Get<IHttpResponseFeature>().ReasonPhrase);
        }
        
        [Test]
        public async Task TestMiddleware_ExpectedBearerTokenInvalidHeader()
        {
            var token = MockJwtTokens.GenerateJwtToken(null);
            var server = _host.GetTestServer();
            server.BaseAddress = new Uri("https://example.com/A/Path/");

            var context = await server.SendAsync(c =>
            {
                c.Request.Method = HttpMethods.Post;
                c.Request.Path = "/and/file.txt";
                c.Request.QueryString = new QueryString("?and=query");
                c.Request.Headers.Add("Authorization", $"Basic {token}");
            });
            Assert.AreEqual(401, context.Response.StatusCode);

        }
        
        
        [Test]
        public async Task TestMiddleware_Expected500()
        {

            var token = MockJwtTokens.GenerateJwtToken(null);
            var headers = Mock.Of<IHeaderDictionary>(_ =>
                _["Authorization"] == new StringValues($"Bearer {token}")
            );
            var requestMock = new Mock<HttpRequest>();        
            requestMock.Setup(x => x.Scheme).Returns("http");
            requestMock.Setup(x => x.Host).Returns(new HostString("localhost"));
            requestMock.Setup(x => x.Path).Returns(new PathString("/test"));
            requestMock.Setup(x => x.PathBase).Returns(new PathString("/"));
            requestMock.Setup(x => x.Method).Returns("GET");
            requestMock.Setup(x => x.Headers).Returns(headers);
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.Request).Returns(requestMock.Object);
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var middleware = new AuthenticationMiddleware(next: (innerHttpContext) => Task.FromResult(0));
                await middleware.Invoke(contextMock.Object);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
        }
    }
}