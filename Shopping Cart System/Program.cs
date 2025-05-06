using Microsoft.OpenApi.Models;
using Shopping_Cart_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping Cart API", Version = "v1" });

    // Add User ID header parameter for testing - this simulates user identification
    c.AddSecurityDefinition("UserIdHeader", new OpenApiSecurityScheme //Token
    {
        Name = "X-User-Id",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        In = ParameterLocation.Header,
        Description = "User ID for cart identification"
    });

    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "UserIdHeader"
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement
    {
        { scheme, new List<string>() }
    };

    c.AddSecurityRequirement(requirement);
});
// Register our cart service
builder.Services.AddSingleton<ICartService, CartService>();

// CORS policy for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Use the CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
