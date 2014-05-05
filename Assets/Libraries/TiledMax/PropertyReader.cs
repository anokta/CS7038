using System;
using System.Collections.Generic;
using System.Collections;

public class PropertyReader : IEnumerable<KeyValuePair<string, string>>
{
	IDictionary<string, string> _data;

	public PropertyReader(IDictionary<string, string> data)
	{
		_data = data;
	}

	public string this[string key] {
		get { return _data[key]; }
	}

	public int getInt(string name) {
		return int.Parse(_data[name]);
	}
	public float getFloat(string name) {
		return float.Parse(_data[name]);
	}
	public string getTag(string name) {
		return _data[name];
	}
	public bool getBoolean(string name) {
		return bool.Parse(_data[name]);
	}

	public ICollection<string> Keys {
		get { return _data.Keys; }
	}

	#region IEnumerable implementation

	public ICollection<string> Values {
		get { return _data.Values; }
	}
		
	public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
	{
		return _data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _data.GetEnumerator();
	}

	#endregion
	
	public bool getInt(string name, out int value) {
		string strVal;
		return (_data.TryGetValue(name, out strVal) && int.TryParse(strVal, out value));
	}

	public bool getFloat(string name, out float value) {
		string strVal;
		return (_data.TryGetValue(name, out strVal) && float.TryParse(strVal, out value));
	}

	public bool getBoolean(string name, out bool value) {
		string strVal;
		return (_data.TryGetValue(name, out strVal) && bool.TryParse(strVal, out value));
	}

	public bool getTag(string name, out string value) {
		return (_data.TryGetValue(name, out value));
	}
}

