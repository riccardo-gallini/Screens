using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Drawing;

namespace Screens
{
    
    public class ListBox : ListBase
    {
        public ListBox() : base()
        {
        }

        public override int SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                base.SelectedIndex = value;
                RecalcPages();
            }
        }

        private int _actualPage;
        private int _totalPages;
        private int _selectedIndexInPage;
        private int _topPageSelectedIndex;

        public int ActualPage
        {
            get
            {
                return _actualPage;
            }
        }

        public int TotalPages
        {
            get
            {
                return _totalPages;
            }
        }

        protected virtual void RecalcPages()
        {
            _totalPages = this.Items.Count / (this.Height - 2) + 1;
            _actualPage = this.SelectedIndex / (this.Height - 2) + 1;
        }

        protected internal override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);

            RecalcPages();
        }

        protected internal override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.Handled == false)
                _performKeyPress(e.KeyInfo);
        }

        protected internal override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (e.Handled == false)
                _performPaint(e);
        }

        private void _performKeyPress(KeyInfo key)
        {
            switch (key.SpecialKey)
            {
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
                        if (SelectedIndex < this.Items.Count - 1)
                            SelectedIndex += 1;
                        break;
                    }

                case SpecialKey.UpArrow:
                    {
                        if (SelectedIndex > 0)
                            SelectedIndex -= 1;
                        break;
                    }

                case SpecialKey.PageDown:
                    {
                        if (_actualPage + 1 <= _totalPages)
                            SelectedIndex = _actualPage * (this.Height - 2) + 1;
                        break;
                    }

                case SpecialKey.PageUp:
                    {
                        if (_actualPage - 1 > 0)
                            SelectedIndex = ((_actualPage - 1) * (this.Height - 2)) - (this.Height - 2);
                        break;
                    }
            }
        }

        private int _topVisibleItem = 0;

        private void _performPaint(PaintEventArgs e)
        {
            var shift = 1;

            // on row 0 the list title plus page/pages
            if (TotalPages != 0)
            {
                if (this.TotalPages > 1)
                {
                    _paintListHeader(e);
                    shift = 1;
                }

                var i = shift;
                var abs_i = (_actualPage - 1) * (this.Height) + i - shift;
                while (i <= this.Height - shift)
                {
                    if (abs_i < Items.Count)
                    {
                        if (SelectedIndex == abs_i)
                            _paintSelectedRow(e, i, abs_i);
                        else
                            _paintUnSelectedRow(e, i, abs_i);
                    }
                    else
                        _paintBackgroundRow(e, i, abs_i);

                    i += 1;
                    abs_i = (_actualPage - 1) * (this.Height) + i - shift;
                }
            }
        }

        private void _paintSelectedRow(PaintEventArgs e, int i, int abs_i)
        {
            if (this.FindForm().Application.BlackAndWhite == false)
            {

                // if selected, draw with 'inverted' colors
                e.Buffer.Fill(' ', new Rectangle(0, i, Size.Width, 1), this.BackColor, this.ForeColor);
                e.Buffer.DrawString(GetItemText(abs_i), new Rectangle(0, i, Size.Width, 1), this.BackColor, this.ForeColor);
            }
            else
            {

                // if selected in b/w => draw with a * before text
                e.Buffer.Fill(' ', new Rectangle(0, i, Size.Width, 1), this.BackColor, this.ForeColor);
                e.Buffer.DrawString("* " + GetItemText(abs_i), new Rectangle(0, i, Size.Width, 1), this.BackColor, this.ForeColor);
            }
        }

        private void _paintUnSelectedRow(PaintEventArgs e, int i, int abs_i)
        {
            if (this.FindForm().Application.BlackAndWhite == false)
            {

                // if not selected, draw with normal colors
                e.Buffer.Fill(' ', new Rectangle(0, i, Size.Width, 1), this.ForeColor, this.BackColor);
                e.Buffer.DrawString(GetItemText(abs_i), new Rectangle(0, i, Size.Width, 1), this.ForeColor, this.BackColor);
            }
            else
            {

                // if unselected in b/w => draw with a ' ' before text
                e.Buffer.Fill(' ', new Rectangle(0, i, Size.Width, 1), this.ForeColor, this.BackColor);
                e.Buffer.DrawString("  " + GetItemText(abs_i), new Rectangle(0, i, Size.Width, 1), this.ForeColor, this.BackColor);
            }
        }


        private void _paintBackgroundRow(PaintEventArgs e, int i, int abs_i)
        {
            // if empty, draw background only

            e.Buffer.Fill(' ', new Rectangle(0, i, Size.Width, 1), this.ForeColor, this.BackColor);
        }

        private void _paintListHeader(PaintEventArgs e)
        {
            var str = "PG " + System.Convert.ToString(this.ActualPage) + System.Convert.ToString("/") + System.Convert.ToString(this.TotalPages);
            e.Buffer.DrawString(str, new Rectangle(0, 0, str.Length, 1), this.BackColor, this.ForeColor);
        }
    }

}