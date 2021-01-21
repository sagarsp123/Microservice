using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
//using System.IdentityModel.Tokens.Jwt;


namespace Gateway.WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
public void ConfigureServices(IServiceCollection services)
{
            //auth logic
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowOrigin", options => options.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());
            //});
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                // .AllowCredentials());
            });
            string textKey = "this is the secret key to add some default jwt token, lets see how it works";

            var key = Encoding.ASCII.GetBytes(textKey);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(textKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = "EmpPortalIssuer",
                ValidateAudience = true,
                ValidAudience = "EmpPortalAudience",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer("EmplPortalKey", x =>
              {
                  x.RequireHttpsMetadata = false;
                  x.TokenValidationParameters = tokenValidationParameters;
              });


            //services.AddOcelot(Configuration);
            services.AddOcelot();
        }
       // services.AddOcelot();


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseCors("CorsPolicy");

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
    await app.UseOcelot();
}
    }
}
