using System;
using System.Collections.Generic;
using System.Linq;

namespace Karabaev.GameKit.Entities.Reactive
{
  public class ReactiveCollection<TItem>
  {
    public delegate void ItemAddedHandler(TItem newItem, int index);
    public delegate void ItemsAddedHandler(IEnumerable<TItem> newItems, int startingIndex);
    public delegate void ItemRemovedHandler(TItem oldItem, int index);
    public delegate void ItemReplacedHandler(TItem oldItem, TItem newItem, int index);
    public delegate void CollectionReinitializedHandler();
    public delegate void CollectionCleanedHandler();
    
    private readonly List<TItem> _collection;

    public IReadOnlyList<TItem> Collection => _collection;

    public int Count => _collection.Count;

    public bool IsEmpty => Count == 0;

    public event ItemAddedHandler? ItemAdded;
    public event ItemsAddedHandler? ItemsAdded;
    public event ItemRemovedHandler? ItemRemoved;
    public event ItemReplacedHandler? ItemReplaced;
    public event CollectionReinitializedHandler? Reinitialized;
    public event CollectionCleanedHandler? Cleaned;
    
    public void Add(TItem item)
    {
      _collection.Add(item);
      ItemAdded?.Invoke(item, _collection.Count - 1);
    }

    public void AddRange(IReadOnlyList<TItem> items)
    {
      var index = _collection.Count - 1;
      _collection.AddRange(items);
      ItemsAdded?.Invoke(items, index);
    }

    public void Remove(TItem item)
    {
      var index = _collection.IndexOf(item);

      if(index == -1)
        return;

      _collection.RemoveAt(index);
      ItemRemoved?.Invoke(item, index);
    }

    public void Remove(Predicate<TItem> predicate)
    {
      foreach(var item in _collection.ToList())
        if(predicate.Invoke(item))
          Remove(item);
    }

    public void Reinitialize(IEnumerable<TItem> items)
    {
      _collection.Clear();
      _collection.AddRange(items);
      Reinitialized?.Invoke();
    }

    public void Clear()
    {
      _collection.Clear();
      Cleaned?.Invoke();
    }
    
    public bool Contains(TItem item) => _collection.Contains(item);
    
    public TItem this[int index]
    {
      get => _collection[index];
      set
      {
        var oldItem = _collection[index];
        _collection[index] = value;
        ItemReplaced?.Invoke(oldItem, value, index);
      }
    }

    public ReactiveCollection() => _collection = new List<TItem>();
    
    public ReactiveCollection(int initialCapacity) => _collection = new List<TItem>(initialCapacity);

    public ReactiveCollection(IEnumerable<TItem> items) => _collection = new List<TItem>(items);
  }
}