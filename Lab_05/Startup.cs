using Azure.Storage.Blobs;
using Lab_05.Data;
using Microsoft.EntityFrameworkCore;
namespace Lab_05
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            /* Define services here */
            var connection = Configuration.GetConnectionString("DefaultDBConnection");
            services.AddDbContext<AnswerImageDataContext>(options => options.UseSqlServer(connection));

            var blobConnection = Configuration.GetConnectionString("AzureBlobStorage");
            services.AddSingleton(new BlobServiceClient(blobConnection));

            services.AddRazorPages();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapRazorPages());

            /* Define routing here */
            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Shared}/{action=Index}/{id?}");
            });*/
        }
    }
}
