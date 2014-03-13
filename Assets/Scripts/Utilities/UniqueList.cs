using System.Collections.Generic;

public class UniqueList<T> : List<T>
{
    public T Last
    {
        get { return this[Count - 1]; }
    }

    public new void Add(T item)
    {
        if (!Contains(item)) base.Add(item);
    }
}
