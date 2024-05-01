using PoC_CircuitBreaker;
using Polly;
var builder = WebApplication.CreateBuilder(args);

var policyWrap = Policy
    .WrapAsync( PolicyExtensions.RetryPolicy, PolicyExtensions.CircuitBreakerPolicy);

builder.Services
    .AddHttpClient<ILoginService, LoginService>()
    .AddPolicyHandler(policyWrap);

var retryPolicy = Policy.Handle<HttpRequestException>()
    .RetryAsync(3, onRetry: (exception, retryCount) =>
    {
        Console.WriteLine($"Tentativa {retryCount} após falha: {exception.Message}");
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/api/login", async (ILoginService _loginService) =>
{
    await _loginService.LoginAsync();
    return Results.Ok("Login finalizado");
});


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.Run();
