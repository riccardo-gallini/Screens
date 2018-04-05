using System;
using System.Collections.Generic;
using System.Text;

namespace Screens.Hosting
{
    public interface IHost
    {
        Action<Terminal> Main { get; set; }
        void StartHost();
        void StopHost();
    }
}
