using APICatalog_NetCore.Context;
using APICatalog_NetCore.DTOs.Mappings;
using APICatalog_NetCore.Extensions;
using APICatalog_NetCore.Filters;
using APICatalog_NetCore.Logging;
using APICatalog_NetCore.Repository;
using APICatalog_NetCore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APICatalog_NetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddScoped<ApiLoggingFilter>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ApiLoggingFilter>();
            services.AddDbContext<AppDbContext>(options => 
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //JWT
            //adiciona o manipulador de autenticacao e define o 
            //esquema de autenticacao usado : Bearer
            //valida o emissor, a audiencia e a chave
            //usando a chave secreta valida a assinatura
            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidAudience = Configuration["TokenConfiguration:Audience"],
                     ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                 });

            services.AddTransient<IMeuServico, MeuServico>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
            {
                LogLevel = LogLevel.Information
            }));

            //adiciona o middleware de tratamento de erros
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            //adiciona o middleware de roteamento
            app.UseRouting();

            //adiciona o middleware de autorização
            app.UseAuthorization();

            //adiciona o middleware que habilita a autenticação
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
