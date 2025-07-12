using Application.Command.Users;
using Application.Commands;
using Application.DTOs;
using Application.DTOs.Users;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AzureFunctionApp.Identities;

public class LoginUserFunction
{
    private readonly ILogger<LoginUserFunction> _logger;
    private readonly IMediator _mediator;
    private readonly IDistributedCache _cache;



    public LoginUserFunction(ILogger<LoginUserFunction> logger, IMediator mediator, IDistributedCache cache)
    {
        _logger = logger;
        _mediator = mediator;
        _cache = cache;
    }

    [Function("LoginUserFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        _logger.LogInformation("Processing request to login user.");
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var command = JsonSerializer.Deserialize<LoginUserCommand>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (command == null)
        {
            return new BadRequestObjectResult("Invalid request body");
        }

        var result = await _mediator.Send(command);

        //await _cache.RemoveAsync(cacheKey);
        //await _cache.RemoveAsync(cacheKey2);

        _logger.LogInformation("Login User successful.");

        return new OkObjectResult(result);
    }


}