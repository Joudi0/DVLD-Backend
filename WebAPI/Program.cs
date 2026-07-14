using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Linq;
using System.Text;
using BLL;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
BLLSettings.Initialize(connStr);


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

    options.OnRejected = async (Microsoft.AspNetCore.RateLimiting.OnRejectedContext context, System.Threading.CancellationToken token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        string errorMessage = "{\"error\": \"Too many requests. Please try again later.\"}";
        await context.HttpContext.Response.WriteAsync(errorMessage, System.Text.Encoding.UTF8, token);
    };

    // 1. Strict Policy
    options.AddFixedWindowLimiter(Shared.clsProjectPolicies.AuthPolicy, (System.Threading.RateLimiting.FixedWindowRateLimiterOptions fixedOptions) =>
    {
        fixedOptions.PermitLimit = 5;
        fixedOptions.Window = TimeSpan.FromMinutes(1);
        fixedOptions.QueueLimit = 0;
        fixedOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });

    // 2. Medium Policy
    options.AddFixedWindowLimiter(Shared.clsProjectPolicies.WritePolicy, (System.Threading.RateLimiting.FixedWindowRateLimiterOptions fixedOptions) =>
    {
        fixedOptions.PermitLimit = 30;
        fixedOptions.Window = TimeSpan.FromMinutes(1);
        fixedOptions.QueueLimit = 2;
        fixedOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });

    // 3. Loose Policy
    options.AddFixedWindowLimiter(Shared.clsProjectPolicies.ReadPolicy, (System.Threading.RateLimiting.FixedWindowRateLimiterOptions fixedOptions) =>
    {
        fixedOptions.PermitLimit = 100;
        fixedOptions.Window = TimeSpan.FromMinutes(1);
        fixedOptions.QueueLimit = 5;
        fixedOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddScoped<WebAPI.Services.clsTokenService>();

WebApplication app = builder.Build();

app.UseCors("AllowAllOrigins");

app.MapOpenApi();
app.MapScalarApiReference();

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapGet("/", () => "DVLD WebAPI is running successfully on MonsterASP!");
app.Run();