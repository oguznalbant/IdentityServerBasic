using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GarantiAPI
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
            services.AddControllers();

        // IdentityModelEventSource.ShowPII = true; //For event log 

        //https://jwt.io/ sayfas�ndan �retilen jwt token decode edilebilir.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    //Token'� yay�nlayan Auth Server adresi bildiriliyor. Yani yetkiyi da��tan mekanizman�n adresi bildirilerek ilgili API ile ili�kilendiriliyor.
                    options.Authority = "https://localhost:44351";

                    //Auth Server uygulamas�ndaki 'Garanti' isimli resource ile bu API ili�kilendiriliyor.
                    options.Audience = "Garanti";
                });

            // claim gerektiren policyler tan�mlan�yor
            services.AddAuthorization(_ =>
            {
                _.AddPolicy("ReadGaranti", policy => policy.RequireClaim("scope", "Garanti.Read"));
                _.AddPolicy("WriteGaranti", policy => policy.RequireClaim("scope", "Garanti.Write"));
                _.AddPolicy("ReadWriteGaranti", policy => policy.RequireClaim("scope", "Garanti.Write", "Garanti.Read"));
                _.AddPolicy("AllGaranti", policy => policy.RequireClaim("scope", "Garanti.Admin"));
                _.AddPolicy("ReadHalkBank", policy => policy.RequireClaim("scope", "HalkBank.Read"));
                _.AddPolicy("WriteHalkBank", policy => policy.RequireClaim("scope", "HalkBank.Write"));
                _.AddPolicy("ReadWriteHalkBank", policy => policy.RequireClaim("scope", "HalkBank.Write", "HalkBank.Read"));
                _.AddPolicy("AllHalkBank", policy => policy.RequireClaim("scope", "HalkBank.Admin"));
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

            app.UseAuthentication(); //do�rulama
            app.UseAuthorization(); //yetkilendirme

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
