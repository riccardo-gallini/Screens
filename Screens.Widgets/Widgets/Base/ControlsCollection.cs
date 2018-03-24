using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace Screens
{



    public class ControlsCollection : Collection<Control>
    {

        // enforce uniqueness of name
        private Dictionary<string, Control> _ByName = new Dictionary<string, Control>();
        public IDictionary<string, Control> ByName
        {
            get
            {
                return _ByName;
            }
        }

        internal ControlsCollection(Control control)
        {
            _owner = control;
        }

        private Control _owner;
        public Control Owner
        {
            get
            {
                return _owner;
            }
        }

        protected override void InsertItem(int index, Control c)
        {
            _ByName.Add(c.Name, c); // errors when c.Name is not unique

            c._parent = _owner;
            c._form = null;
            c._ZOrderNode = _ZOrderList.AddLast(c);

            TabIndex_Update(c);

            base.InsertItem(index, c);
        }

        protected override void RemoveItem(int index)
        {
            var c = this[index];

            _ByName.Remove(c.Name);

            c._parent = null;
            _ZOrderList.Remove(c);

            TabIndex_Update(c);

            base.RemoveItem(index);
        }

        // linked list for controls ordered according to Z-Order
        private LinkedList<Control> _ZOrderList = new LinkedList<Control>();
        public LinkedList<Control> ZOrderList
        {
            get
            {
                return _ZOrderList;
            }
        }

        // linked list for controls ordered accordingo to tabindex
        private LinkedList<Control> _tabIndexList = new LinkedList<Control>();
        public LinkedList<Control> TabIndexList
        {
            get
            {
                return _tabIndexList;
            }
        }

        // insert/update control in the right place of the tabindex list
        internal void TabIndex_Update(Control c)
        {

            // only controls that can focus can be listed in tabindex
            if (c.CanFocus == false)
                return;

            // is tabindex really changed?
            if (c.TabIndex == c._oldTabIndex)
                return;

            // then find the new location in the list
            var node = c._tabIndexNode;
            var isNew = false;

            if (node == null)
            {
                node = new LinkedListNode<Control>(c);
                c._tabIndexNode = node;
                isNew = true;
            }

            if (isNew)
            {
                var current = _tabIndexList.First;
                var found = false;

                while (current != null)
                {
                    if (c.TabIndex < current.Value.TabIndex)
                    {
                        found = true;
                        break;
                    }

                    current = current.Next;
                }

                // put in the new location or at the end
                if (found)
                    _tabIndexList.AddBefore(current, node);
                else
                    _tabIndexList.AddLast(node);
            }
            else if (c.TabIndex > c._oldTabIndex)
            {
                var current = node;
                var found = false;

                while (current != null)
                {
                    if (c.TabIndex < current.Value.TabIndex)
                    {
                        found = true;
                        break;
                    }

                    current = current.Next;
                }

                if (node != current)
                {

                    // remove from the old location
                    _tabIndexList.Remove(node);

                    // put in the new location or at the end
                    if (found == true)
                        _tabIndexList.AddBefore(current, node);
                    else
                        _tabIndexList.AddLast(node);
                }
            }
            else
            {
                var current = node;
                var found = false;

                while (current != null)
                {
                    if (c.TabIndex > current.Value.TabIndex)
                    {
                        found = true;
                        break;
                    }

                    current = current.Previous;
                }

                if (node != current)
                {

                    // remove from the old location
                    _tabIndexList.Remove(node);

                    // put in the new location or at the end
                    if (found == true)
                        _tabIndexList.AddAfter(current, node);
                    else
                        _tabIndexList.AddFirst(node);
                }
            }

            // ok done
            c._oldTabIndex = c.TabIndex;
        }

        public void InvalidateAll()
        {
            foreach (var c in this)
                c.Invalidate();
        }
    }

}
