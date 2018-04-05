using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Screens.Hosting.WebTerm
{
    public class WebTermHost : IHost
    {
        public Action<Terminal> Main { get; set; } 

        public void StartHost()
        {
            if (Main == null) throw new InvalidOperationException(" 'Main' was null!");

            BuildWebHost().Run();
        }

        public void StopHost()
        {
            throw new NotImplementedException();
        }
        
        public IWebHost BuildWebHost() =>
            WebHost.CreateDefaultBuilder()
                   .UseStartup<Startup>()
                   .Build();
    }
}
