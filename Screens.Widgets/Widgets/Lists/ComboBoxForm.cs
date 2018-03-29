using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Screens
{

    public class ComboBoxForm : Form
    {
        private ComboBox _callingControl;
        private ListBox ListBoxItems = new ListBox();/* TODO ERROR didn't convert: WithEvents */

        private bool _accept;
        private bool _cancel;

        private void InitializeComponents()
        {
            ListBoxItems.Name = "ListBoxItems";
            ListBoxItems.Text = "";
            ListBoxItems.Top = 1;
            ListBoxItems.Left = 0;
            ListBoxItems.Width = 31;
            ListBoxItems.Height = 19;
            ListBoxItems.TabIndex = 1;
            ListBoxItems.BackColor = _callingControl.BackColor;
            ListBoxItems.ForeColor = _callingControl.ForeColor;
            ListBoxItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(ListBoxItems);

            this.Name = "ComboBoxForm";
            this.Text = "";
            this.BackColor = ConsoleColor.Black;
            this.ForeColor = ConsoleColor.White;
            this.Width = 31;
            this.Height = 19;
            this.KeyPreview = true;
        }

        public ComboBoxForm(ComboBox callingControl)
        {
            _callingControl = callingControl;
            if (callingControl.DataSource != null)
            {
                ListBoxItems.DataSource = callingControl.DataSource;
                ListBoxItems.ValueMember = callingControl.ValueMember;
                ListBoxItems.DisplayMember = callingControl.DisplayMember;
            }
            else
            {
                ListBoxItems.Items.Clear();
                ListBoxItems.Items.AddRange(callingControl.Items);
            }

            InitializeComponents();
        }


        private void ListBoxItems_KeyPress(Screens.Control sender, Screens.KeyPressEventArgs e)
        {
            switch (e.SpecialKey)
            {
                case SpecialKey.Enter:
                    {
                        _accept = true;
                        _cancel = false;
                        e.Handled = true;
                        this.Close();
                        break;
                    }

                case SpecialKey.Escape:
                    {
                        _accept = false;
                        _cancel = true;
                        e.Handled = true;
                        this.Close();
                        break;
                    }

                case SpecialKey.F4:
                    {
                        _accept = false;
                        _cancel = true;
                        e.Handled = true;
                        this.Close();
                        break;
                    }
            }
        }

        private void ComboBoxForm_Deactivate(Screens.Form form, System.EventArgs e)
        {
            if (_accept)
                _callingControl.SelectedIndex = this.ListBoxItems.SelectedIndex;
        }
    }

}
