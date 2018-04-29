using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Screens
{

    public class Timer : Control
    {
        private System.Threading.Timer _inner;

        public Timer() : base()
        {
            CanFocus = false;
            TabStop = false;
            CausesValidation = false;

            _inner = new System.Threading.Timer((obj) => _timerCallBack());

        }

        private void _timerCallBack()
        {
            var app = FindForm().Application;
            app?.SendTimerTick(this);
        }

        public int Interval { get; set; }

        public void Start()
        {
            _inner.Change(0, Interval);
        }

        public void Stop()
        {
            _inner.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }


        //TODO: test timers
        public event TickEventHandler Tick;

        public delegate void TickEventHandler(Control sender, EventArgs e);

        public virtual void OnTick(EventArgs e)
        {
            Tick?.Invoke(this, e);
        }
    }

}