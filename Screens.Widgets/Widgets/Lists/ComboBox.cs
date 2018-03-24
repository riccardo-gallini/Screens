using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Drawing;

namespace Screens
{
    
    public class ComboBox : ListBase
    {
        public ComboBox() : base()
        {
            SelectedIndex = -1;
        }

        public bool ReadOnly { get; set; }

        public string ComboBoxTitle { get; set; } = "";

        public override int SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                base.SelectedIndex = value;
                if (SelectedIndex != -1)
                    Text = GetDisplay(SelectedIndex);
                else
                    Text = "";
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

        public void ShowChooser()
        {
            var chooser = new ComboBoxForm(this);
            chooser.Text = ComboBoxTitle;
            FindForm()?.Application?.Show(chooser);
        }

        private void _performKeyPress(KeyInfo key)
        {
            if (Enabled == false)
                return;

            switch (key.SpecialKey)
            {
                case SpecialKey.F1: //select
                    {
                        if (this.ReadOnly == false)
                            ShowChooser();
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

                case SpecialKey.DownArrow:
                    {
                        if (this.ReadOnly == false)
                        {
                            if (SelectedIndex < this.Items.Count - 1)
                                SelectedIndex += 1;
                        }

                        break;
                    }

                case SpecialKey.UpArrow:
                    {
                        if (this.ReadOnly == false)
                        {
                            if (SelectedIndex > 0)
                                SelectedIndex -= 1;
                        }

                        break;
                    }

                case SpecialKey.Delete:
                    {
                        if (this.ReadOnly == false)
                            this.SelectedIndex = -1;
                        break;
                    }
            }
        }

        private void _performPaint(PaintEventArgs e)
        {
            e.Buffer.Fill(' ', e.ClipRectangle, this.ForeColor, this.BackColor);
            e.Buffer.DrawString(this.Text, this.ForeColor, this.BackColor);
            e.Buffer.DrawString("...", new Rectangle(this.Width - 3, 0, this.Width, this.Height), this.ForeColor, this.BackColor);
        }

        protected internal override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.CursorPosition = new Point(0, 0);
        }
    }


}