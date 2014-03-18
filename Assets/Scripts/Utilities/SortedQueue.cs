using System;
using System.Collections.Generic;

public class SortedQueue<T> : List<T> where T : IComparable<T>
{
    public bool Ascending { get; private set; }

    public SortedQueue(bool ascending = true)
    {
        Ascending = ascending;
    }

    public new void Add(T item)
    {
        int i;

        if (Ascending)
        {
            for (i = Count - 1; i >= 0; i--)
            {
                if (this[i].CompareTo(item) < 0)
                {
                    Insert(i + 1, item);
                    break;
                }
            }

            if (i == -1) Insert(0, item);
        }
        else
        {
            for (i = 0; i < Count; i++)
            {
                if (this[i].CompareTo(item) < 0)
                {
                    Insert(i, item);
                    break;
                }
            }

            if (i == Count) base.Add(item);
        }
    }
}

public class ComparablePair<TK, TV> : IComparable<ComparablePair<TK, TV>> where TK : IComparable<TK>
{
    public TK Key { get; set; }
    public TV Value { get; set; }

    public ComparablePair(TK key, TV value)
    {
        Key = key;
        Value = value;
    }

    public int CompareTo(ComparablePair<TK, TV> other)
    {
        return Key.CompareTo(other.Key);
    }
}

public class SortedQueue<TK, TV> : SortedQueue<ComparablePair<TK, TV>> where TK : IComparable<TK>
{
    public SortedQueue(bool ascending = true) : base(ascending)
    {
    }

    public void Add(TK key, TV value)
    {
        Add(new ComparablePair<TK, TV>(key, value));
    }
}
