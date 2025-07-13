using Application.Command.Users;
using Application.Commands;
using Application.DTOs;
using Application.DTOs.Users;
using Application.Interfaces.Users;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AzureFunctionApp.Identities;

public class CreateUserFunction
{
    private readonly ILogger<CreateUserFunction> _logger;
    private readonly IMediator _mediator;
    private readonly IDistributedCache _cache;
    private readonly IUserService _userService;

    public CreateUserFunction(ILogger<CreateUserFunction> logger, IMediator mediator, IDistributedCache cache, IUserService userService)
    {
        _logger = logger;
        _mediator = mediator;
        _cache = cache;
        _userService = userService;
    }

    [Function("CreateUserFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        _logger.LogInformation("Processing request to create user.");
        var cacheKey = "users_all";
        var cacheKey2 = "user_by_id";

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var dto = JsonSerializer.Deserialize<UserCreateDto>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (dto == null)
        {
            _logger.LogWarning("Invalid request body.");
            return new BadRequestObjectResult("Invalid request body.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            _logger.LogWarning("Email or password is empty.");
            return new BadRequestObjectResult("Email and password are required.");
        }

        if (_userService.GetByEmail(dto.Email) != null)
        {
            _logger.LogWarning("User with email {Email} already exists.", dto.Email);
            return new ConflictObjectResult("User with this email already exists.");
        }


        var passwordHasher = new PasswordHasher();
        var (hash, salt) = passwordHasher.Hash(dto.Password);

        dto.PasswordHash = hash;
        dto.Salt = salt;

        var result = await _mediator.Send(new CreateUserCommand(dto));
        if (result == null)
        {
            _logger.LogWarning("User create failed.");
            return new NotFoundResult();
        }

        await _cache.RemoveAsync(cacheKey);
        await _cache.RemoveAsync(cacheKey2);

        _logger.LogInformation("Creating User successful.");

        return new OkObjectResult(result);
    }


}