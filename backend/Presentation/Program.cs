
using Domain.Database;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Presentation;


var builder = WebApplication.CreateBuilder(args);

// Check for seed command
var shouldSeed = args.Contains("--seed") || args.Contains("seed");

builder.Services.AddControllers().AddControllersAsServices().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddDatabaseContext(builder.Configuration)
    .AddSwagger()
    //.AddServices(builder.Configuration)
    .AddRepositories()
    .AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<RoleManager<IdentityRole>>();

// Validate JWT configuration
var jwtSigningKey = builder.Configuration["JWT:SigningKey"];
var jwtIssuer = builder.Configuration["JWT:Issuer"];
var jwtAudience = builder.Configuration["JWT:Audience"];

if (string.IsNullOrEmpty(jwtSigningKey))
{
    throw new InvalidOperationException("JWT:SigningKey is not configured. Please add JWT configuration to appsettings.json");
}
if (string.IsNullOrEmpty(jwtIssuer))
{
    throw new InvalidOperationException("JWT:Issuer is not configured. Please add JWT configuration to appsettings.json");
}
if (string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("JWT:Audience is not configured. Please add JWT configuration to appsettings.json");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
                options.DefaultScheme =
                    options.DefaultSignInScheme =
                        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSigningKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["access_token"];
                return Task.CompletedTask;
            }
        };
    }
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // esențial pentru cookie-uri
    });
});

var app = builder.Build();

// Handle seeding if requested
if (shouldSeed)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DatabaseContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            
            // Ensure database is created and migrations are applied
            await context.Database.EnsureCreatedAsync();
            
            // Ensure roles exist
            string[] roles = { "User", "Owner" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            
            // Run seeder
            var seeder = new DataSeeder(context, userManager);
            await seeder.SeedAsync();
            
            Console.WriteLine("Seeding completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during seeding: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return;
        }
    }
    return;
}

// Enable static files for images
app.UseStaticFiles();

app.UseSwagger(c => { c.RouteTemplate = "api/swagger/{documentName}/swagger.json"; });
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "CF-Backend API");
    c.RoutePrefix = "api/swagger";
    c.DocumentTitle = $"CF-Backend {app.Environment.EnvironmentName} - Swagger UI";
    c.DisplayRequestDuration();
    c.EnablePersistAuthorization();
    c.DefaultModelsExpandDepth(0);
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandlerMiddleware();
app.MapControllers();
app.UseCors("AllowFrontend");

await app.RunAsync();
