using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Token
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(i=>i.EnableEndpointRouting=false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            //do�rulama �emalar�m� ekliyorum.
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
                )
                //options parametresinin �zelliklerini belirliyorum.
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = "kuthaygumus.silverlab.com",
                        ValidateIssuer = true,
                        ValidIssuer = "kuthay-gumus.silverlab.com",
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("bu_benim_muhtesem_uzunluktaki_muhtesem_saklanmis_guvelik_keyim"))
                    };

                    // token do�ruland���nda ve �al���r olma s�resi doldu�unda devreye giren iki eventim.
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = ctx => {
                            //Gerekirse burada da gelen token i�erisindeki �e�itli bilgilere g�re do�rulama yapabilmemiz m�mk�n.
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = ctx => {
                            Console.WriteLine("Exception:{0}", ctx.Exception.Message);
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
