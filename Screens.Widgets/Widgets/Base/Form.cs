using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Drawing;

namespace Screens
{



    public abstract class Form : Control
    {
        internal bool _isActive;

        internal Application _application;
        public Application Application
        {
            get
            {
                return _application;
            }
            internal set
            {
                _application = value;
            }
        }

        public bool KeyPreview { get; set; }

        public Button AcceptButton { get; set; }
        public Button CancelButton { get; set; }

        public void Activate()
        {
            Application.SetActiveForm(this);
        }

        public void Close()
        {
            Application.Close(this);
        }


        private Control _focusedControl;

        internal Control FocusedControl
        {
            get
            {
                return _focusedControl;
            }
            set
            {
                if (_focusedControl != value)
                {
                    if (value.CanFocus)
                    {
                        if (_focusedControl != null)
                        {
                            var cancel_event = new CancelEventArgs();
                            if (_focusedControl.CausesValidation)
                            {
                                _focusedControl.OnValidating(cancel_event);
                                if (cancel_event.Cancel == true)
                                    return;
                            }
                            _focusedControl.OnValidated(EventArgs.Empty);
                            _focusedControl.OnLeave(EventArgs.Empty);
                            _focusedControl.OnLostFocus(EventArgs.Empty);
                            _focusedControl.Invalidate();
                        }

                        _focusedControl = value;

                        _focusedControl.OnGotFocus(EventArgs.Empty);
                        _focusedControl.OnEnter(EventArgs.Empty);
                        _focusedControl.Invalidate();
                    }
                }
            }
        }

        public override void SendKey(KeyInfo key_info)
        {
            bool already_handled = false;

            // try the B/W toggle key (F12)
            if (key_info.SpecialKey == SpecialKey.F12)
            {
                this.Application.BlackAndWhite = !this.Application.BlackAndWhite;
                this.Invalidate();
                this.Controls.InvalidateAll();
                this.Application.Terminal.ResetBuffer();
                already_handled = true;
            }

            // try the repaintall key (F10)
            if (key_info.SpecialKey == SpecialKey.F10)
            {
                this.Invalidate();
                this.Controls.InvalidateAll();
                this.Application.Terminal.ResetBuffer(); //TODO: is this needed?
                already_handled = true;
            }

            // try next field key (TAB)
            if (key_info.SpecialKey == SpecialKey.Tab)
            {
                FocusedControl.SelectNextControl(FocusedControl, forward: true, tabStopOnly: true);
                already_handled = true;
            }

            // try form accept key (ENTER)
            if (already_handled == false)
            {
                if (key_info.SpecialKey == SpecialKey.Enter)
                {
                    if (this.AcceptButton != null)
                    {
                        if (this.AcceptButton.Enabled)
                        {
                            if (!(this.FocusedControl is Button))
                            {
                                this.AcceptButton.OnClick(EventArgs.Empty);
                                already_handled = true;
                            }
                        }
                    }
                }
            }

            // try form cancel key (ESC)
            if (already_handled == false)
            {
                if (key_info.SpecialKey == SpecialKey.F4)
                {
                    if (this.CancelButton != null)
                    {
                        if (this.CancelButton.Enabled)
                            this.CancelButton.OnClick(EventArgs.Empty);
                        already_handled = true;
                    }
                }
            }

            // try the registered shortcuts for any control
            if (already_handled == false)
            {
                var _controls = Controls.TabIndexList;
                var node = _controls.First;

                while (node != null)
                {
                    var c = node.Value;
                    if (c.Visible && c.Enabled && c.HasShortCut && key_info.KeyChar == c.ClickShortCut)
                    {
                        c.OnClick(EventArgs.Empty);
                        already_handled = true;
                        break;
                    }

                    node = node.Next;
                }
            }

            // try the key preview for the form
            if (already_handled == false)
            {
                if (this.KeyPreview == true || this.Focused)
                {
                    var e = new KeyPressEventArgs(key_info);
                    OnKeyPress(e);
                    already_handled = e.Handled;
                }
            }

            // otherwise send to the focused control for further management
            if (already_handled == false)
            {
                if (_focusedControl != null)
                    _focusedControl.SendKey(key_info);
            }
        }

        public override bool Focused
        {
            get
            {
                if (_focusedControl == null)
                    return true;
                else
                    return false;
            }
        }



        protected internal override void OnInvalidated(EventArgs e)
        {
            base.OnInvalidated(e);
        }

        protected internal override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (e.Handled == false)
                _performPaint(e);
        }

        private void _performPaint(PaintEventArgs e)
        {

            // Body
            e.Buffer.Fill(' ', e.ClipRectangle, this.ForeColor, this.BackColor);

            // Title
            e.Buffer.Fill(' ', new Rectangle(0, 0, e.ClipRectangle.Width, 1), ConsoleColor.White, ConsoleColor.DarkBlue);
            e.Buffer.DrawString(" " + this.Text, new Rectangle(0, 0, e.ClipRectangle.Width, 1), ConsoleColor.White, ConsoleColor.DarkBlue);
        }



        internal void PerformAnchoring(Size oldSize, Size newSize)
        {
            foreach (var c in Controls)
            {

                // horizontal anchoring

                var anchor_right = (c.Anchor & AnchorStyles.Right) == AnchorStyles.Right;
                var anchor_left = (c.Anchor & AnchorStyles.Left) == AnchorStyles.Left;

                if (anchor_right == true && anchor_left == true)
                {

                    // resize
                    var delta_width = newSize.Width - oldSize.Width;
                    c.Width += delta_width;
                }
                else if (anchor_right == true && anchor_left == false)
                {

                    // move
                    var delta_width = newSize.Width - oldSize.Width;
                    c.Left += delta_width;
                }

                // vertical anchoring

                var anchor_bottom = (c.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom;
                var anchor_top = (c.Anchor & AnchorStyles.Top) == AnchorStyles.Top;

                if (anchor_bottom == true && anchor_top == true)
                {

                    // resize
                    var delta_height = newSize.Height - oldSize.Height;
                    c.Height += delta_height;
                }
                else if (anchor_bottom == true && anchor_top == false)
                {

                    // move
                    var delta_Height = newSize.Height - oldSize.Height;
                    c.Top += delta_Height;
                }
            }
        }



        protected internal virtual void OnActivated(EventArgs e)
        {
            if (_isActive == false)
            {
                _isActive = true;
                Activated?.Invoke(this, e);
            }
        }

        protected internal virtual void OnDeactivate(EventArgs e)
        {
            if (_isActive == true)
            {
                _isActive = false;
                Deactivate?.Invoke(this, e);
            }
        }

        protected internal virtual void OnFormClosing(CancelEventArgs e)
        {
            FormClosing?.Invoke(this, e);
        }

        protected internal virtual void OnFormClosed(EventArgs e)
        {
            FormClosed?.Invoke(this, e);
        }

        protected internal virtual void OnLoad(EventArgs e)
        {
            Load?.Invoke(this, e);
        }

        protected internal virtual void OnShown(EventArgs e)
        {
            Shown?.Invoke(this, e);
        }

        public event ActivatedEventHandler Activated;
        public delegate void ActivatedEventHandler(Form sender, EventArgs e);

        public event DeactivateEventHandler Deactivate;
        public delegate void DeactivateEventHandler(Form form, EventArgs e);

        public event FormClosingEventHandler FormClosing;
        public delegate void FormClosingEventHandler(Form form, CancelEventArgs e);

        public event FormClosedEventHandler FormClosed;
        public delegate void FormClosedEventHandler(Form form, EventArgs e);

        public event LoadEventHandler Load;
        public delegate void LoadEventHandler(Form form, EventArgs e);

        public event ShownEventHandler Shown;
        public delegate void ShownEventHandler(Form form, EventArgs e);
               
    }
    
}