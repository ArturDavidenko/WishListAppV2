using GraphQLWishList.Repository;
using GraphQLWishList.Repository.Interfaces;
using GraphQLWishList.Schema;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace GraphQLWishList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ConnectionsOptions>(builder.Configuration.GetSection("ConnectionStrings"));

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IEmployeerRepository, EmployeerRepository>();
            builder.Services.AddScoped<Query>(); 

            builder.Services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>();

            //builder.Services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(30);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   options.RequireHttpsMetadata = false;
                   options.SaveToken = true;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration["Jwt:Issuer"],
                       ValidAudience = builder.Configuration["Jwt:Issuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
                       RoleClaimType = ClaimTypes.Role
                   };


                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var token = context.Request.Cookies["authToken"];
                           if (!string.IsNullOrEmpty(token))
                           {
                               context.Token = token;
                           }
                           return Task.CompletedTask;
                       }
                   };
               });


            var app = builder.Build();

            app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseSession();

            app.MapGraphQL();

            app.Run();
        }
    }
}
