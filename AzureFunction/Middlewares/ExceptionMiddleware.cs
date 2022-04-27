using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PhotoStorageIsolated.Middlewares
{
    public class ExceptionLoggingMiddleware : IFunctionsWorkerMiddleware
    {
        public static string CorrelationId = Guid.NewGuid().ToString();

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var log = context.GetLogger<ExceptionLoggingMiddleware>();
                log.LogError(ex, $"Exception in function {context?.FunctionDefinition.Name} with Id {context?.FunctionId}");
            }
        }
    }
}
