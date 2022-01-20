using System.Runtime.Serialization.Json;
using System.Text.Json;
namespace D4;

public class MyCustomMiddleware
{
    private readonly RequestDelegate _next;

    public MyCustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
    //    var scheme = context.Request.Scheme;
    //    var host = context.Request.Host;
    //    var path = context.Request.Path;
    //    var queryString = context.Request.QueryString;
       var reader = new StreamReader(context.Request.Body);
       var body = await reader.ReadToEndAsync();
       var requestData = new {
           Scheme = context.Request.Scheme,
           Host = context.Request.Host.ToString(),
           Path = context.Request.Path.ToString(),
           QueryString = context.Request.QueryString.ToString(),
           Body = body
       };
       
        using (StreamWriter writer = File.AppendText("file.txt"))
        {
            var data = JsonSerializer.Serialize(requestData);
            await writer.WriteLineAsync(data);
        }
        // await context.Response.WriteAsync("Welcome to DC");

        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}

public static class MyCustomMiddlewareExtensions
{
    public static IApplicationBuilder UseMyCustomMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyCustomMiddleware>();
    }
}