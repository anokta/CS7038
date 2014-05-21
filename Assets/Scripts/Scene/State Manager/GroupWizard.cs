using System;
using UnityEngine;
using System.Collections.Generic;
using Grouping;

public class GroupWizard : MonoBehaviour
{
	public GroupManager manager { get; private set; }

	public List<string> states;
	public bool caseSensitive = true;
	public bool main = false;

	public GroupWizard()
	{
		states = new List<string>();
	}

	void Awake()
	{
		manager = new GroupManager(caseSensitive);
		GroupManager.Group firstState = null;
		if (states != null) {
			foreach (string name in states) {
				if (!StringExtensions.IsNullOrWhitespace(name)) {
					GroupManager.Group s;
					s = manager.Add(name);
					if (firstState == null) {
						firstState = s;
					}
				}
			}
		}
		if (main) {
			GroupManager.main = manager;
			if (firstState != null) {
				manager.activeGroup = firstState;
			}
		}	
	}

	public void Start()
	{
		if (!main) {
			DestroyImmediate(this);
		}
	}

}