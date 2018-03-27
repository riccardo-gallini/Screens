using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Drawing;
using System.ComponentModel;

namespace Screens
{



    public class Application
    {
        public event AppMessageEventHandler AppMessage;
        public delegate void AppMessageEventHandler(Application sender, AppMessageEventArgs e);

        private ITerminal _terminal;
        internal ITerminal Terminal => _terminal;
        
        /// <summary>
        /// 
        /// </summary>
        [ThreadStatic]
        private static Application _currentApp = null;
        public static Application Current => _currentApp;

        private MessageQueue _messageQueue;
        private Buffer current_buffer;

        public Application(ITerminal terminal)
        {
            _terminal = terminal;
            _messageQueue = new MessageQueue(this);
        }
        
        public Size ScreenSize { get; set; }

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
                _terminal.BlackAndWhite = value;
            }
        }

        
        public void Run(Form f)
        {
            _terminal.SetScreenSize(ScreenSize.Width, ScreenSize.Height);

            start:
            
            try
            {
                _mainForm = f;

                _show(f);

                _terminal.Clear();
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
                _terminal.Clear();
                Show(error_form);
                PerformPaint();

                goto start;
            }

            _terminal.Clear();
        }

        private int _cursorX;
        public int CursorX
        {
            get
            {
                return _cursorX;
            }
        }

        private int _cursorY;
        public int CursorY
        {
            get
            {
                return _cursorY;
            }
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

                current_buffer = new Buffer(ScreenSize);
                last_buffer = (Buffer)current_buffer.Clone();

                f.OnActivated(EventArgs.Empty);
                f.Invalidate();
                if (f.FocusedControl == null)
                {
                    if (f.Controls.TabIndexList.Count > 0)
                        f.FocusedControl = f.Controls.TabIndexList.First.Value;
                }

                ResetBuffer();
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
                _terminal.HideCursor();

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

                flushBufferToTerminal(current_buffer); // TODO: should be from another class
            }

            if (ActiveForm.FocusedControl != null)
            {
                {
                    var c = ActiveForm.FocusedControl;
                    _cursorX = c.Location.X + c.CursorPosition.X;
                    _cursorY = c.Location.Y + c.CursorPosition.Y;
                    _terminal.SetCursorPosition(_cursorX, _cursorY);
                    _terminal.ShowCursor();
                }
            }
            else
            {
                _cursorX = 0;
                _cursorY = 0;
                _terminal.SetCursorPosition(_cursorX, _cursorY);
                _terminal.HideCursor();
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
            _terminal.Beep();
        }


        // ////////////'' send buffer stuff ==> needs a new place
        /// 
        private Buffer last_buffer;

        private void flushBufferToTerminal(Buffer buffer)
        {

            // send changed lines in buffer to the context (console or terminal)
            // in an optimized way

            var xs = 0;
            var ys = 0;


            // scrittura ottimizzata del buffer al terminale
            // a) manda una riga solo se è cambiata
            // b) manda la riga a 'pezzi' scrivendo stringhe intere a pari colore

            while (ys < buffer.Height)
            {
                if (IsLineChanged(buffer, last_buffer, ys))
                {
                    xs = 0;
                    var str = new System.Text.StringBuilder();
                    var cur_fore = buffer[xs, ys].ForeColor;
                    var cur_back = buffer[xs, ys].BackColor;
                    var cur_x = 0;

                    while (xs < buffer.Width)
                    {
                        var buf_char = buffer[xs, ys];
                        if (buf_char.ForeColor != cur_fore || buf_char.BackColor != cur_back)
                        {
                            _terminal.Write(str.ToString(), cur_fore, cur_back, cur_x, ys);
                            str = new System.Text.StringBuilder();
                            cur_fore = buf_char.ForeColor;
                            cur_back = buf_char.BackColor;
                            cur_x = xs;
                        }
                        str.Append(buf_char.Ch);

                        xs += 1;
                    }
                    _terminal.Write(str.ToString(), cur_fore, cur_back, cur_x, ys);
                }
                ys += 1;
            }

            last_buffer = (Buffer)buffer.Clone();
        }

        private static bool IsLineChanged(Buffer a, Buffer b, int y)
        {
            if (b == null || a == null)
                return true;

            var x = 0;
            while (x < a.Width)
            {
                if (a[x, y] != b[x, y])
                    return true;
                x += 1;
            }
            return false;
        }

        public void ResetBuffer()
        {
            if (current_buffer != null)
            {
                last_buffer.Clear();
                current_buffer.Clear();
                _terminal.Clear();
            }
        }

        protected virtual void OnAppMessage(AppMessageEventArgs e)
        {
            AppMessage?.Invoke(this, e);
        }
    }

}