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
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using MusicApplicationAPI.Repositories;
using MusicApplicationAPI.Services.TokenService;
using MusicApplicationAPI.Services.UserService;
using System.Text;
using WatchDog;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Services.SongService;
using MusicApplicationAPI.Services;

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

            #region Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ISongRepository, SongRepository>();
            builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            builder.Services.AddScoped<IPlaylistSongRepository, PlaylistSongRepository>();
            builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
            builder.Services.AddScoped<IRepository<int, Album>, AlbumRepository>();
            builder.Services.AddScoped<IRepository<int, Favorite>, FavoriteRepository>();
            builder.Services.AddScoped<IRepository<int, Rating>, RatingRepository>();
            #endregion

            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            #endregion

            #region Services

            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddScoped<IAuthRegisterService<UserRegisterReturnDTO, UserRegisterDTO>, UserAuthService>();
            builder.Services.AddScoped<IAuthLoginService<UserLoginReturnDTO, UserLoginDTO>, UserAuthService>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISongService, SongService>();
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<IPlaylistSongService, PlaylistSongService>();
            builder.Services.AddScoped<IArtistService, ArtistService>();

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
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
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

            #region CORS
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("AllowAllCorsPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            #region Swagger Configurations
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            #endregion

            #region Pipeline Configurations

            app.UseHttpsRedirection();
            app.UseCors("AllowAllCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

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

            app.Run();
        }
    }
}
