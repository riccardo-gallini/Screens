using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Collections;

namespace Screens
{
    public class ObjectCollection : Collection<object>
    {
        private ListBase _owner;

        internal ObjectCollection(ListBase owner) : base()
        {
            _owner = owner;
        }

        protected override void InsertItem(int index, object item)
        {
            _owner.Invalidate();
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            _owner.Invalidate();
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, object item)
        {
            _owner.Invalidate();
            base.SetItem(index, item);
        }

        protected override void ClearItems()
        {
            _owner.Invalidate();
            base.ClearItems();
        }

        public void AddRange(IEnumerable list)
        {

            foreach (var item in list)
                this.Add(item);
        }
    }


}