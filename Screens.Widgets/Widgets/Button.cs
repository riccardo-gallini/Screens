using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Drawing;

namespace Screens
{

    public class Button : Control
    {
        public Button() : base()
        {
            CanFocus = true;
            CanFocus = true;
            TabStop = true;
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = _parseForShortCut(value);
            }
        }

        protected internal override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.Handled == false)
            {
                switch (e.SpecialKey)
                {
                    case SpecialKey.Enter:
                        {
                            OnClick(EventArgs.Empty);
                            break;
                        }

                    case SpecialKey.DownArrow:
                        {
                            SelectNextControl(this, forward: true);
                            break;
                        }

                    case SpecialKey.UpArrow:
                        {
                            SelectNextControl(this, forward: false);
                            break;
                        }

                    case SpecialKey.LeftArrow:
                        {
                            SelectNextControl(this, forward: false);
                            break;
                        }

                    case SpecialKey.RightArrow:
                        {
                            SelectNextControl(this, forward: true);
                            break;
                        }
                }


                if (e.SpecialKey == SpecialKey.Enter)
                {
                }
            }
        }

        protected internal override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (e.Handled == false)
                _performPaint(e);
        }

        private string _parseForShortCut(string text)
        {
            var pos = text.IndexOf('&');
            string new_text;

            if (pos != -1 && pos < text.Length)
            {
                this.ClickShortCut = text[pos + 1];  //TODO: review if pos+1 ???? was Substring(pos+1,1)
                base.HasShortCut = true;
                new_text = text.Remove(pos, 1);
            }
            else
            {
                this.ClickShortCut = ' ';
                base.HasShortCut = false;
                new_text = text;
            }

            return new_text;
        }

        private void _performPaint(PaintEventArgs e)
        {
            if (this.FindForm().Application.BlackAndWhite == false)
            {

                // paint button in a color environment

                if (this.Focused)
                {
                    // if focused, draw with 'inverted' colors
                    e.Buffer.Fill(' ', new Rectangle(0, 0, Size.Width, 1), this.BackColor, this.ForeColor);
                    e.Buffer.DrawString("[", new Rectangle(0, 0, 1, 1), this.BackColor, this.ForeColor);
                    e.Buffer.DrawString("]", new Rectangle(Size.Width - 1, 0, Size.Width - 1, 1), this.BackColor, this.ForeColor);
                    e.Buffer.DrawString(this.Text, new Rectangle(1, 0, Size.Width - 1, 1), this.BackColor, this.ForeColor);
                }
                else
                {
                    // otherwise, draw with normal colors
                    e.Buffer.Fill(' ', new Rectangle(0, 0, Size.Width, 1), this.ForeColor, this.BackColor);
                    e.Buffer.DrawString("[", new Rectangle(0, 0, 1, 1), this.ForeColor, this.BackColor);
                    e.Buffer.DrawString("]", new Rectangle(Size.Width - 1, 0, Size.Width - 1, 1), this.ForeColor, this.BackColor);
                    e.Buffer.DrawString(this.Text, new Rectangle(1, 0, Size.Width - 1, 1), this.ForeColor, this.BackColor);
                }
            }
            else

            // paint button in a black & white environment (es. narrow band)

            if (this.Focused)
            {
                // if focused, draw with '[' ']' around
                e.Buffer.Fill(' ', new Rectangle(0, 0, Size.Width, 1), this.ForeColor, this.BackColor);
                e.Buffer.DrawString("[", new Rectangle(0, 0, 1, 1), this.ForeColor, this.BackColor);
                e.Buffer.DrawString("]", new Rectangle(Size.Width - 1, 0, Size.Width - 1, 1), this.ForeColor, this.BackColor);
                e.Buffer.DrawString(this.Text, new Rectangle(1, 0, Size.Width - 1, 1), this.ForeColor, this.BackColor);
            }
            else
            {
                // otherwise, draw without the '[' and ']'
                e.Buffer.Fill(' ', new Rectangle(0, 0, Size.Width, 1), this.ForeColor, this.BackColor);
                e.Buffer.DrawString(" ", new Rectangle(0, 0, 1, 1), this.ForeColor, this.BackColor);
                e.Buffer.DrawString(" ", new Rectangle(Size.Width - 1, 0, Size.Width - 1, 1), this.ForeColor, this.BackColor);
                e.Buffer.DrawString(this.Text, new Rectangle(1, 0, Size.Width - 1, 1), this.ForeColor, this.BackColor);
            }
        }
    }

}