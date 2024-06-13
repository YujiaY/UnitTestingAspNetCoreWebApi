namespace EmployeeManagement.Middleware
{
    public class EmployeeManagementSecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public EmployeeManagementSecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            IHeaderDictionary requestHeaders = context.Request.Headers;
            IHeaderDictionary responseHeaders = context.Response.Headers;
             
            // Add CSP + X-Content-Type
            requestHeaders.ContentSecurityPolicy = "default-src 'self';frame-ancestors 'none';"; 
            requestHeaders.XContentTypeOptions = "nosniff";
            responseHeaders.ContentSecurityPolicy = "default-src 'self';frame-ancestors 'none';"; 
            responseHeaders.XContentTypeOptions = "nosniff";
            Console.WriteLine("Security headers added.");

            await _next(context);
        }
    }
}

 