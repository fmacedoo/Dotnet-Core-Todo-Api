using System;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;

namespace TodoList.Core.Middlewares
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        
        private readonly ObjectResultExecutor _oex;

        public ApiExceptionMiddleware(RequestDelegate next, ObjectResultExecutor oex)
        {
            _next = next;
            _oex = oex;
        }
        
        [DebuggerStepThrough]
        public async Task Invoke(HttpContext context)
        {
            try {
                await _next.Invoke(context);
            } catch (Exception exception) {
                if (context.Response.HasStarted) {
                    throw new Exception("The response has already started, the api exception middleware will not be executed", exception);
                }

                context.Response.Clear();
                context.Response.StatusCode = 500;

                dynamic error = new ExpandoObject();
                dynamic current = error;
                do {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine(exception.StackTrace);

                    current.message = exception.Message;
                    current.call_stack = exception.StackTrace;
                    current = current.inner = new ExpandoObject();

                    exception = exception.InnerException;
                }
                while (exception != null);

                await _oex.ExecuteAsync(new ActionContext() { HttpContext = context }, new ObjectResult(error));
           }
        }
    }
}