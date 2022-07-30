using ExaminationSystem.API;
using ExaminationSystem.API.Hubs;
using ExaminationSystem.API.Mapping;
using ExaminationSystem.BLL.Extensions;
using ExaminationSystem.BLL.Managers.EmailManagers;
using ExaminationSystem.BLL.Managers.EmailManagers.Options;
using ExaminationSystem.BLL.Managers.SqlConnectionStringProviders;
using ExaminationSystem.BLL.Managers.SqlConnectionStringProviders.Extensions;
using ExaminationSystem.BLL.Managers.SqlConnectionStringProviders.Options;
using ExaminationSystem.BLL.Managers.TokenManagers.Options;
using ExaminationSystem.BLL.Mapping;
using ExaminationSystem.DAL.Contexts;
using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(ctx.Configuration);
    });

    // Add services to the container.
    Log.Information("Starting host");

    var jwtOptionsSection = builder.Configuration.GetSection("JwtOptions");
    builder.Services.Configure<JwtOptions>(jwtOptionsSection);
    builder.Services.AddBusinessLogicServices();
    builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));
    builder.Services.Configure<SqlConnectionStringOptions>(options =>
        builder.Configuration.BuildSqlConnectionStringOptions(options));

    builder.Services.AddDbContext<ExaminationDbContext>((services, options) =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.UseInMemoryDatabase("ExaminationSystemInMemoryDb");
        }
        else
        {
            var sqlConnectionStringProvider = services.GetService<ISqlConnectionStringProvider>();

            if (sqlConnectionStringProvider is null)
            {
                throw new ArgumentNullException(null,
                    $"Service {nameof(sqlConnectionStringProvider)} can't be null. Maybe service is not registered");
            }

            options.UseSqlServer(sqlConnectionStringProvider.GetSqlDatabaseConnectionString());
        }
    });

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddAutoMapper(typeof(ApiProfile), typeof(BllProfile));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
        {
            opt.SignIn.RequireConfirmedAccount = false;
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequiredLength = 6;
            opt.Password.RequireDigit = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Tokens.EmailConfirmationTokenProvider = "EmailConfirmation";
        }).AddEntityFrameworkStores<ExaminationDbContext>()
        .AddDefaultTokenProviders()
        .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("EmailConfirmation");

    builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
        opt.TokenLifespan = TimeSpan.FromDays(1));

    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(opt =>
    {
        var jwtOptions = jwtOptionsSection.Get<JwtOptions>();

        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidIssuer = jwtOptions.ValidIssuer,
            ValidAudience = jwtOptions.ValidAudience,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = jwtOptions.GetSymmetricSecurityKey()
        };
    });

    builder.Services.AddCors(opt =>
    {
        opt.AddDefaultPolicy(corsBuilder =>
        {
            corsBuilder.AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:4200");
        });
    });

    builder.Services.AddSignalR();
    builder.Services.AddMemoryCache();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1.0", new OpenApiInfo
        {
            Title = "Main API v1.0",
            Version = "v1.0"
        });

        var securitySchema = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            { securitySchema, new[] { JwtBearerDefaults.AuthenticationScheme } }
        };

        opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
        opt.AddSecurityRequirement(securityRequirement);
    });

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddTransient<DataSeeder>();
        using var serviceProvider = builder.Services.BuildServiceProvider();
        var scopeServiceFactory = serviceProvider.GetService<IServiceScopeFactory>();

        if (scopeServiceFactory is null)
        {
            throw new ArgumentNullException(null, $"Service nameof {scopeServiceFactory}, can't be null");
        }

        using var scope = scopeServiceFactory.CreateScope();
        var service = scope.ServiceProvider.GetService<DataSeeder>();
        service?.SeedUsers();
    }

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt => { opt.SwaggerEndpoint("v1.0/swagger.json", "Main API v1.0"); });
    }
    else
    {
        app.UseExceptionHandler();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapHub<RoomChatHub>("/chat", opt => { opt.CloseOnAuthenticationExpiration = true; });
    app.MapControllers();
    app.Run();

    return 0;
}
catch (Exception ex)
{
    if (Log.Logger.GetType().Name is "SilentLogger" or "ReloadableLogger")
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt")
            .CreateLogger();
    }

    Log.Fatal(ex, "fatal");

    return 1;
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}