using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Screens
{

    public class Timer : Control
    {
        private System.Threading.Timer _inner;
        private Application app;

        public Timer() : base()
        {
            CanFocus = false;
            TabStop = false;
            CausesValidation = false;

            _inner = new System.Threading.Timer((obj) => _timerCallBack());

        }
        public int Interval { get; set; }

        private bool _timer_enabled = false;
        public override bool Enabled
        {
            get
            {
                return _timer_enabled;
            }
            set
            {
                if (_timer_enabled!=value)
                {
                    _timer_enabled = value;
                    if (value==true)
                        //starts the timer
                        _inner.Change(0, Interval);
                    else
                        //stops the timer
                        _inner.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                }
            }
        }

        public void Start()
        {
            this.Enabled = true;
        }

        public void Stop()
        {
            this.Enabled = false;
        }

        //this is called by the inner timer, sends the TimerTick event to Application event-loop
        private void _timerCallBack()
        {
            if (app == null) app = FindForm().Application;
            app?.SendTimerTick(this);
        }


        //this part is called by the Application event-loop (runs on App 'UI' thread)
        public event TickEventHandler Tick;
        public delegate void TickEventHandler(Control sender, EventArgs e);

        public virtual void OnTick(EventArgs e)
        {
            Tick?.Invoke(this, e);
        }
    }

}