using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiMovies.DataAccess;
using ApiMovies.Repository;
using ApiMovies.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApiMovies.Mappers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ApiMovies.Helpers;
using Microsoft.OpenApi.Models;

namespace ApiMovies
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
            services.AddDbContext<PostgreSqlContext>(options => options.UseNpgsql(
                Configuration.GetConnectionString("PostgreSqlConnectionString")
            ));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Agregar dependencia del token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAutoMapper(typeof(MapperMovies));

            // Configuración para documentación de la app
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiMovies", new OpenApiInfo()
                {
                    Title = "API Movies",
                    Version = "1",
                    Description = "Ejemplo de Web api con ASP.NET Core",
                    TermsOfService = new Uri("http://tec.geo-estrategia.net"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Andres Martínez González",
                        Email = "andres.geotec@gmail.com",
                        Url = new Uri("http://tec.geo-estrategia.net"),
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT"),
                    }
                });

                options.SwaggerDoc("ApiCategorys", new OpenApiInfo()
                {
                    Title = "API Movies Categorys",
                    Version = "1",
                    Description = "Ejemplo de Web api con ASP.NET Core",
                    TermsOfService = new Uri("http://tec.geo-estrategia.net"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Andres Martínez González",
                        Email = "andres.geotec@gmail.com",
                        Url = new Uri("http://tec.geo-estrategia.net"),
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT"),
                    }
                });

                options.SwaggerDoc("ApiUsers", new OpenApiInfo()
                {
                    Title = "API Movies Users",
                    Version = "1",
                    Description = "Ejemplo de Web api con ASP.NET Core",
                    TermsOfService = new Uri("http://tec.geo-estrategia.net"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Andres Martínez González",
                        Email = "andres.geotec@gmail.com",
                        Url = new Uri("http://tec.geo-estrategia.net"),
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT"),
                    }
                });
                
                // Establezca la ruta de los comentarios para Swagger JSON y UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                //Primero definir el esquema de seguridad
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autenticación JWT (Bearer)",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            services.AddControllers();

            // Soporte para CORS
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseHttpsRedirection();

            // Habilite el middleware para servir Swagger generado como un punto final JSON.
            app.UseSwagger();
            // Habilite el middleware para servir swagger-ui (HTML, JS, CSS, etc.), especificando el punto final Swagger JSON.
            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint("/swagger/ApiMovies/swagger.json", "API Movies");
                options.SwaggerEndpoint("/swagger/ApiCategorys/swagger.json", "API Movies Categorys");
                options.SwaggerEndpoint("/swagger/ApiUsers/swagger.json", "API Movies Users");
                options.RoutePrefix = "";
            });

            app.UseRouting();

            // Para la Atenticación y Autorización
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Soporte para CORS
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
