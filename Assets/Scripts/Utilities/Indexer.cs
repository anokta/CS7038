using System;
using System.Collections.Generic;


namespace Zootools {
	public class Indexer<Tkey, Tvalue>
	{
		IDictionary<Tkey, Tvalue> _dictionary;

		public Indexer(IDictionary<Tkey, Tvalue> dictionary)
		{
			_dictionary = dictionary;
		}

		public Tvalue this[Tkey key] {
			get { return _dictionary[key]; }
		}

		public bool TryGetValue(Tkey key, out Tvalue value) {
			return _dictionary.TryGetValue(key, out value);
		}

		public bool ContainsKey(Tkey key) {
			return _dictionary.ContainsKey(key);
		}
	}
}