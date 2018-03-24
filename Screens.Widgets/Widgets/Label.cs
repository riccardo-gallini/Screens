using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Screens
{

    public class Label : Control
    {
        public Label() : base()
        {
            CanFocus = false;
            TabStop = false;
            CausesValidation = false;

            ForeColor = ConsoleColor.White;
            BackColor = ConsoleColor.Black;
        }

        public bool ReadOnly { get; set; }

        protected internal override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (e.Handled == false)
            {
                _performPaint(e);
                e.Handled = true;
            }
        }

        private void _performPaint(PaintEventArgs e)
        {
            e.Buffer.Fill(' ', e.ClipRectangle, this.ForeColor, this.BackColor);
            e.Buffer.DrawString(this.Text, this.ForeColor, this.BackColor);
        }
    }

}