using Easy_Password_Validator.Models;
using Easy_Password_Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicApplicationAPI.Contexts;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service.AuthService;
using MusicApplicationAPI.Interfaces.Service.TokenService;
using MusicApplicationAPI.Mappers;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using MusicApplicationAPI.Repositories;
using MusicApplicationAPI.Services.TokenService;
using MusicApplicationAPI.Services.UserService;
using System.Text;
using WatchDog;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Services.SongService;
using MusicApplicationAPI.Services;
using MusicApplicationAPI.Services.FavoriteService;
using MusicApplicationAPI.Services.RatingService;
using System.Text.Json.Serialization;
using MusicApplicationAPI.Services.EmailService;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using System.Net.Sockets;
using System.Net;
using Stripe;

namespace MusicApplicationAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Controllers
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            #endregion

            #region CORS
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("MyAllowSpecificOrigins", options =>
                {
                    options.WithOrigins("https://localhost:3000","https://calm-mud-018043c1e.5.azurestaticapps.net").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            #endregion

            #region Hangfire

            // Add Hangfire services.
            builder.Services.AddHangfire(configuration =>
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangFireConnection"))
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings());

            // Add the processing server.
            builder.Services.AddHangfireServer();

            #endregion

            #region Swagger

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                     {
                           new OpenApiSecurityScheme
                           {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                                 }
                           },
                           new string[] {}
                     }
                });
            });

            #endregion

            #region DbContexts

            builder.Services.AddDbContext<MusicManagementContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"))
                );

            #endregion

            #region Repositories Injection
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ISongRepository, SongRepository>();
            builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            builder.Services.AddScoped<IPlaylistSongRepository, PlaylistSongRepository>();
            builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
            builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
            builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            builder.Services.AddScoped<IPremiumUserRepository, PremiumUserRepository>();
            builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
            builder.Services.AddSingleton<StripeService>();
            #endregion 

            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            #endregion

            #region Services Injection

            builder.Services.AddScoped<ITokenService, JwtTokenService>();
            builder.Services.AddScoped<IAuthRegisterService<UserRegisterReturnDTO, UserRegisterDTO>, UserAuthService>();
            builder.Services.AddScoped<IAuthLoginService<UserLoginReturnDTO, UserLoginDTO>, UserAuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISongService, SongService>();
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<IPlaylistSongService, PlaylistSongService>();
            builder.Services.AddScoped<IArtistService, ArtistService>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();
            builder.Services.AddScoped<IFavoriteService, FavoriteService>();
            builder.Services.AddScoped<IRatingService, RatingService>();

            builder.Services.AddScoped<IEmailSender, EmailSenderService>();
            builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();

            #endregion

            #region Logging
            builder.Services.AddLogging(l => l.AddLog4Net());
            #endregion

            #region Password Validator
            builder.Services.AddTransient(service => new PasswordValidatorService(new PasswordRequirements()));
            #endregion

            #region Authentication

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"])),
                        ClockSkew = TimeSpan.Zero   // to prevent JWT's additional 5mins expiry time
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Cookies["vibe-vault"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };

                });

            #endregion

            #region WatchDog

            builder.Services.AddWatchDogServices(opt =>
            {
                opt.SetExternalDbConnString = builder.Configuration.GetConnectionString("WatchDogConnection");
                opt.DbDriverOption = WatchDog.src.Enums.WatchDogDbDriverEnum.MSSQL;
            });

            #endregion

            

            #region JSON Enum Coverter Configuration
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            #endregion

            #region Build Phase
            var app = builder.Build();
            #endregion

            #region Pipeline Configurations

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("MyAllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.MapControllers();

            #endregion

            #region Hangfire Configuration

            var hangfireCredentials = builder.Configuration.GetSection("Hangfire");

            app.UseHangfireDashboard("/hangfire/dashboard", new DashboardOptions
            {
                DashboardTitle = "Vibe Vault - Job Dashboard",
                DarkModeEnabled = true,
                DisplayStorageConnectionString = false,
                Authorization = new[]
                {
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = hangfireCredentials["username"],
                        Pass = hangfireCredentials["password"]
                    }
                }
            });

            #endregion

            #region CRON JOB
            RecurringJob.AddOrUpdate<SubscriptionMailService>(
                "check-expiring-subscriptions",
                service => service.CheckAndNotifyExpiringSubscriptions(),
                Cron.Hourly); // Runs every hour
            #endregion 

            #region Swagger Configurations
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            #endregion

            

            #region WatchDog Configurations
            app.UseWatchDogExceptionLogger();

            var watchdogCredentials = builder.Configuration.GetSection("WatchDog");
            app.UseWatchDog(opt =>
            {
                opt.WatchPageUsername = watchdogCredentials["username"];
                opt.WatchPagePassword = watchdogCredentials["password"];
            });
            #endregion

            #region Run
            app.Run();
            #endregion
        }
    }

}
