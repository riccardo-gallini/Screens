using System;
using System.Collections.Generic;
using System.Linq;

namespace Screens
{

    public class AppMessageEventArgs : EventArgs
    {
        public object Data { get; set; }
        public Application Application { get; }


        public AppMessageEventArgs(Application _app, object _data)
        {
            Data = _data;
            Application = _app;
        }
    }

}
