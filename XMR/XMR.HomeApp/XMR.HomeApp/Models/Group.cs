using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace XMR.HomeApp.Models
{
    public class Group<K,T> : ObservableCollection<T>
    {
        public K Name { get; private set; }
        public Group(K name, IEnumerable<T> items)
        {
            Name = name;
            foreach (T item in items)
                Items.Add(item);
        }
    }
}
