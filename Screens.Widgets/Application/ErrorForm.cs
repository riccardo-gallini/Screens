using System;
using System.Collections.Generic;
using System.Linq;


namespace Screens
{
    public class ErrorForm : Form
    {
        private TextBox text_err;

        public ErrorForm()
        {
            text_err = new TextBox();

            text_err.Name = "text_err";
            text_err.Text = "UNHANDLED EXCEPTION";
            text_err.Top = 1;
            text_err.Left = 0;
            text_err.Height = 15;
            text_err.Width = 20;
            text_err.TabIndex = 1;
            text_err.BackColor = ConsoleColor.Red;
            text_err.ForeColor = ConsoleColor.White;
            text_err.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(text_err);

            this.Text = "UNHANDLED EXCEPTION";
            this.BackColor = ConsoleColor.Red;
            this.ForeColor = ConsoleColor.White;
            this.Width = 20;
            this.Height = 15;
            this.KeyPreview = true;
        }

        public string ErrorText
        {
            get
            {
                return text_err.Text;
            }
            set
            {
                text_err.Text = value;
            }
        }
    }

}