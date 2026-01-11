
using Domain.Database;
using Domain.Entities;
using Infrastructure.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Presentation;


var builder = WebApplication.CreateBuilder(args);

// Check for seed argument
var shouldSeed = args.Contains("--seed");

builder.Services.AddControllers().AddControllersAsServices().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
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
    }).AddEntityFrameworkStores<DatabaseContext>();

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
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
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

// Handle seeding if --seed argument is provided
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
            await SeedData.SeedAsync(context, userManager, roleManager);
            Console.WriteLine("Seeding completed successfully. Exiting...");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
}

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
app.UseCors("AllowFrontend"); // CORS must be before routing
app.UseRouting();
app.UseStaticFiles(); // Enable static files serving
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandlerMiddleware();
app.MapControllers();

await app.RunAsync();
