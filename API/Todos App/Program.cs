using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TodosApp.DAL;

namespace Todos_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Context Configuration
            var connectionString = builder.Configuration.GetConnectionString("TodosDB");
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString));
            #endregion

            #region Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            #endregion

            #region Repos & Services

            builder.Services.AddScoped<ITodoRepository, TodoRepository>();
            builder.Services.AddScoped<ITodoService, TodoService>();

            builder.Services.AddHttpClient<IExternalTodoService, ExternalTodoService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region Authentication Scheme

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer("Bearer", options =>
            {
                var secretKeyString = builder.Configuration.GetValue<string>("SecretKey");
                var secretyKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString ?? string.Empty);
                var secretKey = new SymmetricSecurityKey(secretyKeyInBytes);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = secretKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            #endregion

            #region Authorization
            builder.Services.AddAuthorization(
            options =>
            {
                options.AddPolicy("AdminOnly", policy => policy
                   .RequireClaim(ClaimTypes.Role, "Admin")
                   .RequireClaim(ClaimTypes.NameIdentifier));

                options.AddPolicy("AdminAndUsers", policy => policy
                    .RequireClaim(ClaimTypes.Role, "Admin", "User")
                    .RequireClaim(ClaimTypes.NameIdentifier));
            });

        
            #endregion

            #region Auto Mapper

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}