using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace Dantas.Core
{
    /// <summary>
    /// Collection for customize handle of entities in properties collections.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    public abstract class EntityCollection<T> : Collection<T>
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="list">The list for handle.</param>
        public EntityCollection(IList<T> list) : base(list) { }

        /// <summary>
        /// Event that notity any change in collection.
        /// </summary>
        public event EventHandler<NotifyChangeEventArgs> NotifyChangeEventHandler;

        private void OnChange(NotifyChangeEventArgs notifyChangeEventArgs)
        {
            if (NotifyChangeEventHandler != null)
                NotifyChangeEventHandler(this, notifyChangeEventArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            OnChange(new NotifyChangeEventArgs(ChangeAction.Add, item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, item);
            OnChange(new NotifyChangeEventArgs(ChangeAction.Edit, item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        protected override void RemoveItem(int index)
        {
            var item = Items[index];
            base.RemoveItem(index);
            OnChange(new NotifyChangeEventArgs(ChangeAction.Remove, item));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            OnChange(new NotifyChangeEventArgs(ChangeAction.Clear, null));
        }
    }
}
