using System;
using System.Collections.Generic;
using System.Linq;


namespace Screens
{



    public class Panel
    {
        public virtual string Name { get; set; }

        private List<Control> _controlsCollection = new List<Control>();

        public IList<Control> Controls
        {
            get
            {
                return _controlsCollection;
            }
        }

        private bool _visible;

        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                foreach (var c in _controlsCollection)
                    c.Visible = value;
            }
        }

        public void BringToFront()
        {
            foreach (var c in _controlsCollection)
                c.BringToFront();
        }

        public void SendToBack()
        {
            foreach (var c in _controlsCollection)
                c.SendToBack();
        }
    }

}