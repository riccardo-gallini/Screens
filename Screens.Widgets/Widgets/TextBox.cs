using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Screens
{
    
    public class TextBox : Control
    {
        public TextBox() : base()
        {
            CanFocus = true;
            TabStop = true;
            CausesValidation = true;
            Overwrite = true;
            _passwordChar = ' ';
        }

        public bool ReadOnly { get; set; }
        public bool Overwrite { get; set; }

        private char _passwordChar;
        public char PasswordChar
        {
            get
            {
                return _passwordChar;
            }
            set
            {
                _passwordChar = value;
                Invalidate();
            }
        }


        protected internal override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.Handled == false)
            {
                _performKeyPress(e.KeyInfo);
                e.Handled = true;
            }
        }

        protected internal override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (e.Handled == false)
            {
                _performPaint(e);
                e.Handled = true;
            }
        }

        public void AppendText(string text)
        {
            this.Text += text;
        }

        public void ResetText(string text)
        {
            this.Text = "";
        }

        public void Paste(string text)
        {
            this.Text = this.Text.Substring(0, this._selectionStart) + text + this.Text.Substring(this._selectionStart);
        }

        public void OverwriteText(char ch)
        {
            if (this.Text == "" || this.Text == null)
                this.Text = ch.ToString();
            else
            {
                var chars = this.Text.ToCharArray();
                if (this.SelectionStart == chars.Length)
                    this.Text += ch;
                else
                {
                    chars[SelectionStart] = ch;
                    this.Text = new string(chars);
                }
            }
        }

        private void _deleteCurrentChar()
        {
            try
            {
                if (this.Text != "")
                    this.Text = this.Text.Substring(0, this._selectionStart) + this.Text.Substring(this._selectionStart + 1);
            }
            catch
            {
            }
        }

        private int _selectionStart;
        public int SelectionStart
        {
            get
            {
                return _selectionStart;
            }
            set
            {
                _selectionStart = value;
                if (_selectionStart < 0)
                {
                    _selectionStart = 0;
                    SelectNextControl(this, forward: false, tabStopOnly: true);
                }

                if (_selectionStart > Text.Length)  //TODO: works?
                    _selectionStart = Text.Length;

                CursorPosition = new Point(_selectionStart, CursorPosition.Y);
            }
        }

        public void SelectAll()
        {
            SelectionStart = 0;
        }

        private void _performKeyPress(KeyInfo key)
        {
            if (Enabled == false)
                return;

            switch (key.SpecialKey)
            {
                case SpecialKey.Home:
                    {
                        SelectionStart = 0;
                        break;
                    }

                case SpecialKey.End:
                    {
                        SelectionStart = Text.Length; //TODO: works??
                        break;
                    }

                case SpecialKey.LeftArrow:
                    {
                        SelectionStart -= 1;
                        break;
                    }

                case SpecialKey.RightArrow:
                    {
                        SelectionStart += 1;
                        break;
                    }

                case SpecialKey.UpArrow:
                    {
                        if (this.Height == 1)
                            SelectionStart -= 1;
                        break;
                    }

                case SpecialKey.DownArrow:
                    {
                        if (this.Height == 1)
                            SelectionStart += 1;
                        break;
                    }

                case SpecialKey.Delete:
                    {
                        _deleteCurrentChar();
                        Invalidate();
                        break;
                    }

                case SpecialKey.Backspace:
                    {
                        if (_selectionStart > 0)
                        {
                            SelectionStart -= 1;
                            _deleteCurrentChar();
                            Invalidate();
                        }

                        break;
                    }

                case SpecialKey.Enter:
                    {
                        break;
                    }

                default:
                    {
                        if (this.ReadOnly == false)
                        {
                            if (this.Overwrite == false)
                                OverwriteText(key.KeyChar);
                            else
                                // Paste(key.KeyChar)
                                OverwriteText(key.KeyChar);
                            Invalidate();
                            SelectionStart += 1;
                        }

                        break;
                    }
            }
        }

        private void _performPaint(PaintEventArgs e)
        {
            //TODO: check 1) bw 2) all these Fill
            if (this.FindForm()?.Application.BlackAndWhite == false)
            {
                e.Buffer.Fill(' ', e.ClipRectangle, this.ForeColor, this.BackColor);
                e.Buffer.DrawString(this.Text, this.ForeColor, this.BackColor);
            }
            else
            {
                var ch = '_';
                if (this.ReadOnly || this.Enabled == false || this.CanFocus == false)
                    ch = ' ';

                e.Buffer.Fill(ch, e.ClipRectangle, this.ForeColor, this.BackColor);
                e.Buffer.DrawString(this.Text.Replace(' ', ch), this.ForeColor, this.BackColor);
            }
        }

        protected internal override void OnGotFocus(EventArgs e)
        {
            this.SelectionStart = 0;
            base.OnGotFocus(e);
        }

        protected internal override void OnTextChanged(System.EventArgs e)
        {
            if (this.Text == "")
                this.SelectionStart = 0;
            base.OnTextChanged(e);
        }
    }

}