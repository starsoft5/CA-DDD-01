using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace SimpleFunctionApp;

public class SimpleHelloFunction
{
    [Function("SimpleHelloFunction")]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        FunctionContext context)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteString("Hello from Azure Function (dotnet-isolated)!");
        return response;
    }
}
