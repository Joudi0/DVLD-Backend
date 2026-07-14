using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string secretKey = builder.Configuration["JwtSettings:SecretKey"];

if (string.IsNullOrWhiteSpace(secretKey))
{
    secretKey = Environment.GetEnvironmentVariable("JwtSettings__SecretKey");
}

if (string.IsNullOrWhiteSpace(secretKey))
{
    secretKey = "Fallback_Temporary_Key_Only_For_Local_Development_To_Prevent_Crashes!";
}
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// Configure IP-based rate limiting policies automatically with custom error responses
builder.Services.AddRateLimiter((Microsoft.AspNetCore.RateLimiting.RateLimiterOptions options) =>
{
    options.RejectionStatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status429TooManyRequests;

    // Handle rejected requests globally and return a clean JSON message
    options.OnRejected = async (Microsoft.AspNetCore.RateLimiting.OnRejectedContext context, System.Threading.CancellationToken token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        string errorMessage = "{\"error\": \"Too many requests. Please try again later.\"}";
        await context.HttpContext.Response.WriteAsync(errorMessage, System.Text.Encoding.UTF8, token);
    };

    // 1. Strict Policy for Authentication endpoints (5 requests per minute)
    options.AddFixedWindowLimiter(Shared.clsProjectPolicies.AuthPolicy, (System.Threading.RateLimiting.FixedWindowRateLimiterOptions fixedOptions) =>
    {
        fixedOptions.PermitLimit = 5;
        fixedOptions.Window = TimeSpan.FromMinutes(1);
        fixedOptions.QueueLimit = 0;
        fixedOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });

    // 2. Medium Policy for Write operations like Add, Update, Delete (30 requests per minute)
    options.AddFixedWindowLimiter(Shared.clsProjectPolicies.WritePolicy, (System.Threading.RateLimiting.FixedWindowRateLimiterOptions fixedOptions) =>
    {
        fixedOptions.PermitLimit = 30;
        fixedOptions.Window = TimeSpan.FromMinutes(1);
        fixedOptions.QueueLimit = 2;
        fixedOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });

    // 3. Loose Policy for Read operations like Get, Paging, GetAll (100 requests per minute)
    options.AddFixedWindowLimiter(Shared.clsProjectPolicies.ReadPolicy, (System.Threading.RateLimiting.FixedWindowRateLimiterOptions fixedOptions) =>
    {
        fixedOptions.PermitLimit = 100;
        fixedOptions.Window = TimeSpan.FromMinutes(1);
        fixedOptions.QueueLimit = 5;
        fixedOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddEndpointsApiExplorer();
// For .NET 10 OpenAPI native support
builder.Services.AddOpenApi();

// Add authorization services
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOwnerOrAdmin", policy =>
        policy.Requirements.Add(new WebAPI.Authorization.UserOwnerOrAdminRequirement()));
});

System.Collections.Generic.IEnumerable<System.Type> handlerTypes = typeof(Program).Assembly.GetTypes()
    .Where((System.Type t) => typeof(IAuthorizationHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

foreach (System.Type handler in handlerTypes)
{
    builder.Services.AddSingleton(typeof(IAuthorizationHandler), handler);
}

builder.Services.AddControllers();
builder.Services.AddScoped<WebAPI.Services.clsTokenService>(); // to make the tokens in login works

WebApplication app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference(); // New API Reference for .NET 10
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();

app.Run();