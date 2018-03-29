using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Drawing;
using System.ComponentModel;
using Screens.Hosting;

namespace Screens
{
    
    public class Application
    {
        public event AppMessageEventHandler AppMessage;
        public delegate void AppMessageEventHandler(Application sender, AppMessageEventArgs e);

        public Terminal Terminal { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [ThreadStatic]
        private static Application _currentApp = null;
        public static Application Current => _currentApp;

        private MessageQueue _messageQueue;

        public Size ScreenSize
        {
            get
            {
                return this.Terminal.ScreenSize;
            }
            set
            {
                this.Terminal.ScreenSize = value;
            }
        }


        public Application(Terminal terminal)
        {
            Terminal = terminal;
            Terminal.KeyPressed = (key) => this.SendKey(key);
            _messageQueue = new MessageQueue(this);
        }
        


        private bool _blackAndWhite = false;
        public bool BlackAndWhite
        {
            get
            {
                return _blackAndWhite;
            }
            set
            {
                _blackAndWhite = value;
                Terminal.BlackAndWhite = value;
            }
        }

        
        public void Run(Form f)
        {
            Terminal.SetScreenSize(ScreenSize.Width, ScreenSize.Height);

            start:
            
            try
            {
                _mainForm = f;

                _show(f);

                Terminal.Clear();
                PerformPaint();
                                
                // Start the message loop
                Message msg = null;
                _currentApp = this;
 
                while ((_messageQueue.GetMessage(ref msg)))
                {
                    if (msg.MessageType == WM_MessageType.WM_PAINT)
                    {
                    }
                    else if (msg.MessageType == WM_MessageType.WM_KEY)
                    {
                        var key_info = (KeyInfo)msg.Parameter;
                        ActiveForm.SendKey(key_info);
                    }
                    else if (msg.MessageType == WM_MessageType.WM_SHOW_FORM)
                    {
                        var form = (Form)msg.Parameter;
                        _show(form);
                    }
                    else if (msg.MessageType == WM_MessageType.WM_RESIZE)
                        ActiveForm.OnResize(new EventArgs());
                    else if (msg.MessageType == WM_MessageType.WM_TIMER)
                    {
                        var timer = (Timer)msg.Parameter;
                        timer.OnTick(EventArgs.Empty);
                    }
                    else if (msg.MessageType == WM_MessageType.WM_APP_MSG)
                        this.OnAppMessage(new AppMessageEventArgs(this, msg.Parameter));
                    else if (msg.MessageType == WM_MessageType.WM_QUIT)
                        break;

                    PerformPaint();
                }
            }
            catch (Exception ex)
            {
                var error_form = new ErrorForm();
                error_form.ErrorText = ex.ToString();
                var old_size = error_form.Size;
                error_form.Size = ScreenSize;
                error_form.PerformAnchoring(old_size, error_form.Size);
                Terminal.Clear();
                Show(error_form);
                PerformPaint();

                goto start;
            }

            Terminal.Clear();
        }

        public void Close(Form f)
        {
            var e = new CancelEventArgs();
            f.OnFormClosing(e);

            if (e.Cancel == false)
            {
                f.OnDeactivate(EventArgs.Empty);
                f.OnFormClosed(EventArgs.Empty);

                _openForms.Pop();

                if (_openForms.Count == 0)
                    this.Exit();
                else
                    SetActiveForm(_openForms.Peek());
            }
        }

        public void Show(Form f)
        {
            _messageQueue.PostMessage(Message.WM_SHOW_FORM(f));
        }

        private void _show(Form f)
        {
            if (_mainForm == null)
            {
                Run(f);
                return;
            }

            f.Application = this;
            _openForms.Push(f);

            f.OnLoad(EventArgs.Empty);

            // Forms are always "full screen"
            var old_size = f.Size;
            f.Size = this.ScreenSize;
            f.PerformAnchoring(old_size, f.Size);

            f.OnShown(EventArgs.Empty);

            SetActiveForm(f);
        }

        internal void SetActiveForm(Form f)
        {
            if (_activeForm != f)
            {
                if (_activeForm != null)
                    _activeForm.OnDeactivate(EventArgs.Empty);

                _activeForm = f;

                f.OnActivated(EventArgs.Empty);
                f.Invalidate();
                if (f.FocusedControl == null)
                {
                    if (f.Controls.TabIndexList.Count > 0)
                        f.FocusedControl = f.Controls.TabIndexList.First.Value;
                }

                Terminal.ResetBuffer();  //TODO: here or at [[[XXX]]]?
            }
        }

        private Form _activeForm;
        public Form ActiveForm
        {
            get
            {
                return _activeForm;
            }
        }

        private Form _mainForm;
        public Form MainForm
        {
            get
            {
                return _mainForm;
            }
        }

        private Stack<Form> _openForms = new Stack<Form>();
        public IList<Form> OpenForms
        {
            get
            {
                return new ReadOnlyCollection<Form>(_openForms.ToList());
            }
        }

        public void Exit()
        {

            // send wm_quit
            _messageQueue.PostMessage(Message.WM_QUIT());
        }

        private void PerformPaint()
        {

            // form paint
            if (ActiveForm.IsInvalidated)
            {
                Terminal.HideCursor();

                Buffer current_buffer = Terminal.CurrentBuffer;

                var form_event = new PaintEventArgs(current_buffer.ClipRectangle, current_buffer);
                ActiveForm.OnPaint(form_event);
                ActiveForm.IsInvalidated = false;


                var controls = ActiveForm.Controls.ZOrderList;
                var node = controls.First;

                while (node != null)
                {
                    var control = node.Value;

                    if (control.Visible)
                    {
                        var control_buffer = new Buffer(control.Size);
                        var e = new PaintEventArgs(control_buffer.ClipRectangle, control_buffer);
                        control.OnPaint(e);
                        control.IsInvalidated = false;
                        current_buffer.WriteBuffer(control_buffer, control.Location);
                    }

                    node = node.Next;
                }

                Terminal.FlushBuffer();
            }

            if (ActiveForm.FocusedControl != null)
            {
                {
                    var c = ActiveForm.FocusedControl;
                                       
                    //move cursor to the focused control
                    Terminal.SetCursorPosition(c.Location.X + c.CursorPosition.X, c.Location.Y + c.CursorPosition.Y);
                    Terminal.ShowCursor();
                }
            }
            else
            {
                Terminal.SetCursorPosition(0, 0);
                Terminal.HideCursor();
            }
        }

        public void MessageBox(string text, string caption, MessageBoxButtons buttons, int icon, int defaultButton)
        {
        }

        public void SendKey(KeyInfo key_info)
        {
            _messageQueue.PostMessage(Message.WM_KEY(key_info));
        }

        public void SendTimerTick(Timer t)
        {
            _messageQueue.PostMessage(Message.WM_TIMER(t));
        }

        public void SendAppMessage(object data)
        {
            _messageQueue.PostMessage(Message.WM_APP_MSG(data));
        }



        public void Beep()
        {
            Terminal.Beep();
        }



       

        protected virtual void OnAppMessage(AppMessageEventArgs e)
        {
            AppMessage?.Invoke(this, e);
        }
    }

}