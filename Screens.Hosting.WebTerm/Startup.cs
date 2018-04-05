using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Screens.Hosting.WebTerm
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/err.html");
            }
            
            // static serving from wwwroot ---------------------------------------------------------

            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            var wwwroot = assembly.GetName().Name + ".wwwroot";
            var wwwroot_provider = new EmbeddedFileProvider(assembly, wwwroot);

            app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = wwwroot_provider });
            app.UseStaticFiles(new StaticFileOptions { FileProvider = wwwroot_provider });

            // signalR comm ------------------------------------------------------------------------
            app.UseSignalR(routes => { routes.MapHub<WebTermHub>("/hubs/chat"); });
        }
    }
}
