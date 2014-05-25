using System;
using System.IO;
using System.Collections.Generic;

public class DialogueMap
{
	Dictionary<string, DialogueInstance> _instances;

	public Dictionary<string, DialogueInstance> data {
		get { return _instances; }
	}

	public DialogueManager manager { get; private set;}

	public DialogueMap(DialogueManager manager) {
		this.manager = manager;
		_instances = new Dictionary<string, DialogueInstance>(
			StringComparer.InvariantCultureIgnoreCase);
	}

	bool TryGetValue(string key, out DialogueInstance value) {
		value = null;
		if (key.StartsWith(DefaultChar)) {
			return manager.defaultMap.TryGetValue(key.Substring(1), out value);
		}
		return _instances.TryGetValue(key, out value);
	}

	bool HasValue(string key) {
		if (key.StartsWith(DefaultChar)) {
			return manager.defaultMap.HasValue(key.Substring(1));
		}
		return _instances.ContainsKey(key);
	}

	DialogueInstance Get(string key)
	{
		if (key.StartsWith(DefaultChar)) {
			return manager.defaultMap.Get(key.Substring(1));
		}
		return _instances[key];
	}

	void Set(string key, DialogueInstance value) {
		if (key.StartsWith(DefaultChar)) {
			manager.defaultMap.Set(key.Substring(1), value);
		} else {
			_instances[key] = value;
		}
	}

	public static readonly char AuthorChar = '$';
	public static readonly char DefaultChar = '@';
	public static readonly char InstanceChar = '#';
	public static readonly char CommentChar = '%';
	public static readonly char EscapeChar = '\\';

	public DialogueInstance this[string key] {
		get { return Get(key); }
		set { Set(key, value); }
	}


	public static DialogueMap Open(DialogueManager manager, TextReader reader)
	{
		DialogueMap map = new DialogueMap(manager);
		map._instances = new Dictionary<string, DialogueInstance>();
		int lnum = 0;
		string line;
		string currentDialogue = null;
		List<DialogueEntry> currentEntries = null;
		Author currentAuthor = null;
		while ((line = reader.ReadLine()) != null) {
			++lnum;
			line = line.TrimStart();
			bool escape = false;
			if (line.StartsWith(EscapeChar)) {
				line = line.Substring(1);
				escape = true;
			}
			if (!escape && (line.StartsWith(CommentChar) || StringExtensions.IsNullOrWhitespace(line))) {
				continue;
			}
			else if (!escape && line.StartsWith(InstanceChar)) {
				if (currentEntries != null) {
					map._instances[currentDialogue] = new DialogueInstance(currentEntries, manager.audio);
				}
				currentAuthor = null;
				currentDialogue = line.Substring(1).Trim();
				currentEntries = new List<DialogueEntry>();
			}
			else if (!escape && line.StartsWith(AuthorChar)) {
				if (currentEntries == null) {
					throw new IOException(string.Format("Invalid dialogue format at line {0}", lnum));
				}
				string author = line.Substring(1).Trim();
				currentAuthor = manager.GetAuthor(author);
			}
			else {
				if (currentEntries == null || currentAuthor == null) {
					throw new IOException(string.Format("Invalid dialogue format at line {0}", lnum));
				}
				currentEntries.Add(new DialogueEntry(currentAuthor, line));
			}
		}
		if (currentEntries != null) {
			map._instances[currentDialogue] = new DialogueInstance(currentEntries, manager.audio);
		}

		return map;
	}
}
