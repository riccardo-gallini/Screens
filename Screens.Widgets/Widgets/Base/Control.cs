using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Drawing;
using System.ComponentModel;

namespace Screens
{
    public abstract class Control
    {
        public Control()
        {
            this._controlsCollection = new ControlsCollection(this);

            _Enabled = true;
            TabStop = true;
            _TabIndex = 1;
            TabStop = true;
            _Text = "";
            _Visible = true;
            _hasShortCut = false;
            Anchor = AnchorStyles.Top | AnchorStyles.Left;
            _ForeColor = ConsoleColor.White;
            _BackColor = ConsoleColor.Black;
            _size.Width = 1;
            _size.Height = 1;
        }

        private Point _location = new Point();
        public virtual Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnLocationChanged(EventArgs.Empty);
                    FindForm()?.Invalidate();
                }
            }
        }

        private Size _size = new Size();
        public virtual Size Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnSizeChanged(EventArgs.Empty);
                    OnResize(EventArgs.Empty);
                    FindForm()?.Invalidate();
                }
            }
        }

        public virtual int Top
        {
            get
            {
                return _location.Y;
            }
            set
            {
                if (_location.Y != value)
                {
                    _location.Y = value;
                    OnLocationChanged(EventArgs.Empty);
                    FindForm()?.Invalidate();
                }
            }
        }

        public virtual int Left
        {
            get
            {
                return _location.X;
            }
            set
            {
                if (_location.X != value)
                {
                    _location.X = value;
                    OnLocationChanged(EventArgs.Empty);
                    FindForm()?.Invalidate();
                }
            }
        }

        public virtual int Height
        {
            get
            {
                return _size.Height;
            }
            set
            {
                if (_size.Height != value)
                {
                    var _newSize = new Size(_size.Width, value);
                    Size = _newSize;
                }
            }
        }

        public virtual int Width
        {
            get
            {
                return _size.Width;
            }
            set
            {
                if (_size.Width != value)
                {
                    var _newSize = new Size(value, _size.Height);
                    Size = _newSize;
                }
            }
        }

        public virtual AnchorStyles Anchor { get; set; }

        public virtual string Name { get; set; }

        public virtual bool CausesValidation { get; set; }

        private string _Text = "";
        public virtual string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                if (_Text != value || value == "")
                {
                    _Text = value;
                    OnTextChanged(EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        private ConsoleColor _BackColor;
        public virtual ConsoleColor BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                if (_BackColor != value)
                {
                    _BackColor = value;
                    OnBackColorChanged(EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        private ConsoleColor _ForeColor;
        public virtual ConsoleColor ForeColor
        {
            get
            {
                return _ForeColor;
            }
            set
            {
                if (_ForeColor != value)
                {
                    _ForeColor = value;
                    OnForeColorChanged(EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        public virtual Rectangle Bounds
        {
            get
            {
                return new Rectangle(_location, _size);
            }
            set
            {
                if (_location != value.Location || _size != value.Size)
                {
                    _location = value.Location;
                    _size = value.Size;
                    OnLocationChanged(EventArgs.Empty);
                    OnSizeChanged(EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        public int Bottom
        {
            get
            {
                return _location.Y + _size.Height;
            }
        }

        private Point _ClientLocation = new Point();

        public Rectangle ClientRectangle
        {
            get
            {
                return new Rectangle(_ClientLocation, _ClientSize);
            }
        }

        private Size _ClientSize = new Size();

        public virtual Size ClientSize
        {
            get
            {
                return _ClientSize;
            }
            set
            {
                if (_ClientSize != value)
                {
                    _ClientSize = value;
                    OnClientSizeChanged(EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        public bool Contains(Control c)
        {
            return _controlsCollection.Contains(c);
        }

        private bool _Enabled;
        public virtual bool Enabled
        {
            get
            {
                if (_HiddenByMessageBox)
                    return false;
                else
                    return _Enabled;
            }
            set
            {
                if (_Enabled != value)
                {
                    _Enabled = value;
                    if (this.Focused)
                        this.SelectNextControl(this, forward: true);
                    TabStop = _Enabled;
                    OnEnabledChanged(EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        private bool _HiddenByMessageBox;
        internal virtual bool HiddenByMessageBox
        {
            get
            {
                return _HiddenByMessageBox;
            }
            set
            {
                _HiddenByMessageBox = value;
            }
        }


        internal Form _form;
        public virtual Form FindForm()
        {
            if (_form == null)
            {
                var ctrl = this;
                while (!ctrl.GetType().IsSubclassOf(typeof(Form)))
                {
                    ctrl = ctrl.Parent;
                    if (ctrl == null) return null;
                    if (ctrl == this) return null;
                }
                _form = (Form)ctrl;
            }

            return _form;
        }

        public virtual bool CanFocus { get; set; }

        public virtual void Focus()
        {
            FindForm().FocusedControl = this;
        }

        public virtual bool Focused
        {
            get
            {
                if (FindForm() == null)
                    return false;
                if (FindForm()?.FocusedControl == this)
                    return true;
                else
                    return false;
            }
        }

        internal Point CursorPosition { get; set; }

        public virtual bool HasChildren
        {
            get
            {
                if (_controlsCollection.Count == 0)
                    return false;
                else
                    return true;
            }
        }

        private ControlsCollection _controlsCollection;

        public ControlsCollection Controls
        {
            get
            {
                return _controlsCollection;
            }
        }

        internal LinkedListNode<Control> _ZOrderNode;

        public void BringToFront()
        {
            var next_node = _ZOrderNode.Next;

            if (next_node != null)
            {
                _ZOrderNode.Value = next_node.Value;
                next_node.Value = this;
            }
        }

        public void SendToBack()
        {
            var prev_node = _ZOrderNode.Previous;

            if (prev_node != null)
            {
                _ZOrderNode.Value = prev_node.Value;
                prev_node.Value = this;
            }
        }

        private int _TabIndex;
        internal int _oldTabIndex = -1;
        internal LinkedListNode<Control> _tabIndexNode;

        internal bool SelectNextControl(Control ctl, bool forward = true, bool tabStopOnly = true)
        {
            Control next_ctl;
            var f = FindForm();

            next_ctl = ctl.GetNextControl(ctl, forward);

            if (next_ctl != null)
            {
                f.FocusedControl = next_ctl;
                return true;
            }
            else
                return false;
        }

        private LinkedListNode<Control> get_next(LinkedListNode<Control> n, bool forward)
        {
            if (forward)
                return n.Next;
            else
                return n.Previous;
        }

        private LinkedListNode<Control> get_first(LinkedListNode<Control> n, bool forward)
        {
            if (forward)
                return n.List.First;
            else
                return n.List.Last;
        }




        public Control GetNextControl(Control ctl, bool forward = true, bool tabStopOnly = true)
        {
            var stop_condition = false;
            var node = get_next(ctl._tabIndexNode, forward);

            while (true)
            {
                if (node == null || node.Value == null)
                    return get_first(ctl._tabIndexNode, forward).Value;

                var c = node.Value;

                stop_condition = c.CanFocus && c.Enabled && c.Visible;
                if (tabStopOnly)
                    stop_condition = stop_condition && c.TabStop;

                if (stop_condition)
                    return c;
                else if (c == ctl)
                    return c;
                else
                    node = get_next(node, forward);
            }
        }

        public int TabIndex
        {
            get
            {
                return _TabIndex;
            }
            set
            {
                _TabIndex = value;
                if (this.Parent != null)
                    this.Parent.Controls.TabIndex_Update(this);
            }
        }

        public virtual bool TabStop { get; set; }

        internal Control _parent;
        public Control Parent
        {
            get
            {
                return _parent;
            }
        }

        public object Tag { get; set; }

        private bool _Visible;
        public virtual bool Visible
        {
            get
            {
                if (_HiddenByMessageBox)
                    return false;
                else
                    return _Visible;
            }
            set
            {
                _Visible = value;
                OnVisibleChanged(EventArgs.Empty);
                if (this.Focused)
                    this.SelectNextControl(this, forward: true);
                FindForm()?.Invalidate();
            }
        }

        private bool _isInvalidated = false;

        internal bool IsInvalidated
        {
            get
            {
                return _isInvalidated;
            }
            set
            {
                _isInvalidated = value;
            }
        }

        public void Invalidate()
        {
            _isInvalidated = true;
            var _form = FindForm();
            if (_form != null) _form._isInvalidated = true;
            OnInvalidated(EventArgs.Empty);
        }

        public virtual void SendKey(KeyInfo info)
        {
            if (Enabled)
            {
                var e = new KeyPressEventArgs(info);
                OnKeyPress(e);
            }
        }

        public char ClickShortCut { get; set; }

        private bool _hasShortCut;
        public bool HasShortCut
        {
            get
            {
                return _hasShortCut;
            }
            protected set
            {
                _hasShortCut = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        protected internal virtual void OnEnter(EventArgs e)
        {
            Enter?.Invoke(this, e);
        }

        protected internal virtual void OnGotFocus(EventArgs e)
        {
            GotFocus?.Invoke(this, e);
        }

        protected internal virtual void OnInvalidated(EventArgs e)
        {
            Invalidated?.Invoke(this, e);
        }

        protected internal virtual void OnKeyPress(KeyPressEventArgs e)
        {
            KeyPress?.Invoke(this, e);
        }

        protected internal virtual void OnLeave(EventArgs e)
        {
            Leave?.Invoke(this, e);
        }

        protected internal virtual void OnLostFocus(EventArgs e)
        {
            LostFocus?.Invoke(this, e);
        }

        protected internal virtual void OnPaint(PaintEventArgs e)
        {
            Paint?.Invoke(this, e);
        }

        protected internal virtual void OnValidated(EventArgs e)
        {
            Validated?.Invoke(this, e);
        }

        protected internal virtual void OnValidating(CancelEventArgs e)
        {
            Validating?.Invoke(this, e);
        }

        protected internal virtual void OnLocationChanged(EventArgs e)
        {
            LocationChanged?.Invoke(this, e);
        }

        protected internal virtual void OnSizeChanged(EventArgs e)
        {
            SizeChanged?.Invoke(this, e);
        }

        protected internal virtual void OnResize(EventArgs e)
        {
            Resize?.Invoke(this, e);
        }

        protected internal virtual void OnClientSizeChanged(EventArgs e)
        {
            ClientSizeChanged?.Invoke(this, e);
        }

        protected internal virtual void OnVisibleChanged(EventArgs e)
        {
            VisibleChanged?.Invoke(this, e);
        }

        protected internal virtual void OnTextChanged(EventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        protected internal virtual void OnForeColorChanged(EventArgs e)
        {
            ForeColorChanged?.Invoke(this, e);
        }

        protected internal virtual void OnEnabledChanged(EventArgs e)
        {
            EnabledChanged?.Invoke(this, e);
        }

        protected internal virtual void OnBackColorChanged(EventArgs e)
        {
            BackColorChanged?.Invoke(this, e);
        }

        protected internal virtual void OnClick(EventArgs e)
        {
            Click?.Invoke(this, e);
        }
        
        public event EnterEventHandler Enter;
        public delegate void EnterEventHandler(Control sender, EventArgs e);

        public event GotFocusEventHandler GotFocus;
        public delegate void GotFocusEventHandler(Control sender, EventArgs e);

        public event InvalidatedEventHandler Invalidated;
        public delegate void InvalidatedEventHandler(Control sender, EventArgs e);

        public event KeyPressEventHandler KeyPress;
        public delegate void KeyPressEventHandler(Control sender, KeyPressEventArgs e);

        public event LeaveEventHandler Leave;
        public delegate void LeaveEventHandler(Control sender, EventArgs e);

        public event LostFocusEventHandler LostFocus;
        public delegate void LostFocusEventHandler(Control sender, EventArgs e);

        public event PaintEventHandler Paint;
        public delegate void PaintEventHandler(Control sender, PaintEventArgs e);

        public event ValidatedEventHandler Validated;
        public delegate void ValidatedEventHandler(Control sender, EventArgs e);

        public event ValidatingEventHandler Validating;
        public delegate void ValidatingEventHandler(Control sender, CancelEventArgs e);

        public event ClickEventHandler Click;
        public delegate void ClickEventHandler(Control sender, EventArgs e);

        public event LocationChangedEventHandler LocationChanged;
        public delegate void LocationChangedEventHandler(Control sender, EventArgs e);

        public event SizeChangedEventHandler SizeChanged;
        public delegate void SizeChangedEventHandler(Control sender, EventArgs e);

        public event ClientSizeChangedEventHandler ClientSizeChanged;
        public delegate void ClientSizeChangedEventHandler(Control sender, EventArgs e);

        public event ResizeEventHandler Resize;
        public delegate void ResizeEventHandler(Control sender, EventArgs e);

        public event VisibleChangedEventHandler VisibleChanged;
        public delegate void VisibleChangedEventHandler(Control sender, EventArgs e);

        public event TextChangedEventHandler TextChanged;
        public delegate void TextChangedEventHandler(Control sender, EventArgs e);

        public event ForeColorChangedEventHandler ForeColorChanged;
        public delegate void ForeColorChangedEventHandler(Control sender, EventArgs e);

        public event EnabledChangedEventHandler EnabledChanged;
        public delegate void EnabledChangedEventHandler(Control sender, EventArgs e);

        public event BackColorChangedEventHandler BackColorChanged;
        public delegate void BackColorChangedEventHandler(Control sender, EventArgs e);
    }

}