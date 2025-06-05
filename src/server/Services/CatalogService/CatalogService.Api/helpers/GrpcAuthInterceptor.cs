using CommonLibrary.Helpers;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace CatalogService.Api.helpers
{
    public class GrpcAuthInterceptor(IHttpContextAccessor httpClient) : Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var headers = context.Options.Headers ?? new Metadata();

            var token = httpClient.GetToken();

            headers.Add("Authorization", token);

            var options = context.Options.WithHeaders(headers);
            var newContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method,
                context.Host,
                options);

            return continuation(request, newContext);
        }
    }
}
