using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HalkbankAPI
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                    //Token'ý yayýnlayan Auth Server adresi bildiriliyor. Yani yetkiyi daðýtan mekanizmanýn adresi bildirilerek ilgili API ile iliþkilendiriliyor.
                    options.Authority = "https://localhost:44351";
                    //Auth Server uygulamasýndaki 'Garanti' isimli resource ile bu API iliþkilendiriliyor.
                    options.Audience = "Halkbank";
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
