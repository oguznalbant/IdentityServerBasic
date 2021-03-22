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

            // IdentityServer4 framework�� JWT�leri imzalamak i�in Asimetrik �ifreleme�yi kullanmaktad�r. 
            //A�a��daki �emada oldu�u gibi �AuthServer� client�tan gelen talep neticesinde token da��tmadan �nce 
            //ilgili token�� private key ile �ifreler ve ard�ndan ilgili client�a g�nderir. 
            //Client bu �ifrelenmi� token de�eri ile API�a istekte bulunacak ve API�nda bu iste�i do�rulamas� 
            //gerekecektir. Bunun i�in public key�e ihtiyac� vard�r. Dolay�s�yla API�da �AuthServer�dan public key�i 
            //al�r. Velhas�l, gelen istekteki private key ile API�da ki public key uyumu kontrol edilir ve do�rulama 
            // neticesinde istek ba�ar�yla sonu�lan�r.

            services.AddControllersWithViews(); //MVC i�in
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles(); //wwwroot'a eri�im i�in
            app.UseAuthentication(); //kimlik do�rulama i�in
            app.UseAuthorization(); //yetkilendirme i�in

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute(); //url rotas� i�in
            });
        }
    }
}
