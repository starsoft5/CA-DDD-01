using Application.Command.Users;
using Application.Commands;
using Application.DTOs;
using Application.DTOs.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Application.Interfaces.Users;
using Infrastructure.Security;
using Domain.Entities;
using Microsoft.Azure.Functions.Worker.Http;


namespace AzureFunctionApp.Identities;

public class LoginUserFunction
{
    private readonly ILogger<LoginUserFunction> _logger;
    private readonly IMediator _mediator;
    private readonly IDistributedCache _cache;
    private readonly IUserService _userService;



    public LoginUserFunction(ILogger<LoginUserFunction> logger, IMediator mediator, IDistributedCache cache, IUserService userService)
    {
        _logger = logger;
        _mediator = mediator;
        _cache = cache;
        _userService = userService;
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

        User? user = _userService.GetByEmail(command.Email);
         

        if (user?.Email == null)
        {
            _logger.LogInformation("Email does not exists.");
            return new BadRequestObjectResult("Invalid request body");
        }

        var passwordHasher = new PasswordHasher();
        var isValid = passwordHasher.Verify(command.Password, user.PasswordHash, user.Salt);
        if (!isValid)
        {
            return new UnauthorizedObjectResult("Invalid login.");
        }
       
        var result = await _mediator.Send(command);

        //await _cache.RemoveAsync(cacheKey);
        //await _cache.RemoveAsync(cacheKey2);

        _logger.LogInformation("Login User successful.");

        return new OkObjectResult(result);
    }


}