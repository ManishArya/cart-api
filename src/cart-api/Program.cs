using cart_api.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Serilog.Context;
using Microsoft.OpenApi.Models;
using cart_api.Services;
using cart_api.Repositories;
using Serilog;
using Microsoft.Extensions.Options;
using cart_api.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Host.UseSerilog((Ctx, lc) =>
{
    lc.ReadFrom.Configuration(configuration);
});
var serviceCollections = builder.Services;
// Add services to the container.
builder.Services.AddCors(o => o.AddPolicy("policy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.Configure<CartStoreDBSettings>(configuration.GetSection(nameof(CartStoreDBSettings)));
builder.Services.AddSingleton<ICartStoreDBSetting>(sp => sp.GetRequiredService<IOptions<CartStoreDBSettings>>().Value);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = configuration["jwt:audience"],
        ValidIssuer = configuration["jwt:issuer"],
        NameClaimType = ClaimTypes.NameIdentifier,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:secret_key"]))
    });
builder.Services.AddHttpContextAccessor()
    .AddSingleton<ICartDBContext, CartDBContext>()
    .AddScoped<ICartService, CartService>()
    .AddScoped<ICartRepository, CartRepository>()
    .AddAutoMapper(typeof(CartProfile))
    .AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
                 new OpenApiInfo { Title = "CartApi", Version = "v1" });
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
        new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            new List<string>()
            }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("policy");
app.UseAuthorization();
app.Use(async (context, next) =>
{
    var userId = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "";
    LogContext.PushProperty("UserName", userId);
    LogContext.PushProperty("IP", context.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4());
    await next();
});

app.MapControllers();

app.Run();
