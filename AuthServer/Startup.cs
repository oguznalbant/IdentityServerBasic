using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthServer
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

            services
                    .AddIdentityServer()
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryApiScopes(Config.GetApiScopes())
                    .AddInMemoryClients(Config.GetClients())
                    .AddDeveloperSigningCredential();

            // IdentityServer4 framework’ü JWT’leri imzalamak için Asimetrik Þifreleme’yi kullanmaktadýr. 
            //Aþaðýdaki þemada olduðu gibi ‘AuthServer’ client’tan gelen talep neticesinde token daðýtmadan önce 
            //ilgili token’ý private key ile þifreler ve ardýndan ilgili client’a gönderir. 
            //Client bu þifrelenmiþ token deðeri ile API’a istekte bulunacak ve API’nda bu isteði doðrulamasý 
            //gerekecektir. Bunun için public key’e ihtiyacý vardýr. Dolayýsýyla API’da ‘AuthServer’dan public key’i 
            //alýr. Velhasýl, gelen istekteki private key ile API’da ki public key uyumu kontrol edilir ve doðrulama 
            // neticesinde istek baþarýyla sonuçlanýr.

            services.AddControllersWithViews(); //MVC için
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles(); //wwwroot'a eriþim için
            app.UseAuthentication(); //kimlik doðrulama için
            app.UseAuthorization(); //yetkilendirme için

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute(); //url rotasý için
            });
        }
    }
}
