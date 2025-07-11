using Application.DTOs;
using Application.Queries;
using Infrastructure.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AzureFunctionApp.Orders;

public class GetAllProductsFunction
{
    private readonly ILogger<GetAllProductsFunction> _logger;
    private readonly IMediator _mediator;
    private readonly IDistributedCache _cache;

    public GetAllProductsFunction(ILogger<GetAllProductsFunction> logger, IMediator mediator, IDistributedCache cache)
    {
        _logger = logger;
        _mediator = mediator;
        _cache = cache;
    }

    [Function("GetAllProductsFunction")]
    public async Task<IActionResult> Run([Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        //return new OkObjectResult("[{'name':'God is faithful forever!'}]");
        _logger.LogInformation("Processing request to retrieve all products.");

        var cacheKey = "products_all";
        var cached = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cached))
        {
            _logger.LogInformation("Returning products from cache.");
            var cachedOrders = JsonSerializer.Deserialize<List<ProductDTO>>(cached);
            return new OkObjectResult(cachedOrders);
        }

        _logger.LogInformation("Cache miss. Sending GetAllProductsQuery to MediatR.");

        var result = await _mediator.Send(new GetAllProductsQuery());

        _logger.LogInformation("Products retrieved from database. Caching result.");

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        var json = JsonSerializer.Serialize(result);
        await _cache.SetStringAsync(cacheKey, json, options);

        _logger.LogInformation("Returning products to client.");

        return new OkObjectResult(result);
    }

}