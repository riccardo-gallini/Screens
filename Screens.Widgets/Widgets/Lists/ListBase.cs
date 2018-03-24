using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections;

namespace Screens
{
    
    public abstract class ListBase : Control
    {
        public ListBase() : base()
        {
            CanFocus = true;
            TabStop = true;
            CausesValidation = true;

            ForeColor = ConsoleColor.White;
            BackColor = ConsoleColor.Black;

            Items = new ObjectCollection(this);
        }


        public ObjectCollection Items { get; set; }

        private int _selectedIndex;
        public virtual int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnSelectedIndexChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        public object SelectedItem
        {
            get
            {
                if (Items.Count > 0)
                    return Items[_selectedIndex];
                else
                    return null;
            }
            set
            {
                var idx = FindIndexByObject(value);
                if (idx != -1)
                    SelectedIndex = idx;
            }
        }

        public object SelectedValue
        {
            get
            {
                return GetValue(_selectedIndex);
            }
            set
            {
                var idx = FindIndexByPropertyValue(value);
                if (idx != -1)
                    SelectedIndex = idx;
            }
        }

        //TODO: untested
        private object internalGetValue(string member, int index)
        {
            if (index != -1)
            {
                if (_isDataTable == false)
                {
                    //bound to object property (value member)
                    var item = Items[index];
                    try
                    {
                        var bound_prop = item.GetType().GetProperty(member);
                        return bound_prop.GetValue(item);
                    }
                    catch
                    {
                        return "";
                    }
                }
                else
                {
                    //bound to datarow
                    var row = (DataRow)Items[index];
                    try
                    {
                        return row[member];
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            else
                return "";
        }

        //TODO: untested
        internal virtual object GetValue(int index)
        {
            return internalGetValue(ValueMember, index);
        }

        //TODO: untested
        internal virtual string GetDisplay(int index)
        {
            return internalGetValue(DisplayMember, index).ToString();
        }

        public virtual string GetItemText(int index)
        {
            if (index != -1)
            {
                if (DisplayMember == null || DisplayMember == "")
                    return Items[index].ToString();
                else
                    return GetDisplay(index);
            }
            else
                return "";
        }

        public virtual bool GetSelected(int index)
        {
            return index == SelectedIndex;
        }

        private int FindIndexByObject(object obj)
        {
            var i = 0;
            while (i < Items.Count)
            {
                if (Items[i] == obj)
                    return i;
                i += 1;
            }
            return -1;
        }

        private int FindIndexByPropertyValue(object value)
        {
            var i = 0;
            while (i < Items.Count)
            {
                if (this.GetValue(i) == value)
                    return i;
                i += 1;
            }
            return -1;
        }



        private object _dataSource;
        private DataTable _dataTable;
        private bool _isDataTable;

        public virtual object DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;

                if (_dataSource is DataTable)
                {
                    _dataTable = (DataTable)_dataSource;
                    
                    _isDataTable = true;
                    Items.Clear();

                    foreach (DataRow dr in _dataTable.Rows)
                        Items.Add(dr);
                }
                else if (_dataSource.GetType().IsSubclassOf(typeof(IEnumerable)))
                {
                    _dataTable = null;
                    _isDataTable = false;
                    Items.Clear();
                    Items.AddRange((IEnumerable)_dataSource);
                }
            }
        }

        public string DisplayMember { get; set; } = "";
        public string ValueMember { get; set; } = "";



        protected internal virtual void OnSelectedIndexChanged(EventArgs e)
        {
            SelectedIndexChanged?.Invoke(this, e);
        }

        public event SelectedIndexChangedEventHandler SelectedIndexChanged;

        public delegate void SelectedIndexChangedEventHandler(ListBase sender, EventArgs e);
    }


}