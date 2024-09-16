using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                     .AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json")
                     .AddEnvironmentVariables();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("GatewayAuthenticationScheme", options =>
                {
                    options.Authority = builder.Configuration["IdentityServerUrl"];
                    options.Audience = "resource_gateway";
                    options.RequireHttpsMetadata = false;
                });

builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();

app.Run();
