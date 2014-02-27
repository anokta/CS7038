using System;
using UnityEngine;
using System.Collections.Generic;

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
			_activeWizard = this;
			GroupManager.main = manager;
			if (firstState != null) {
				manager.activeGroup = firstState;
			}
		}	
	}

	private static GroupWizard _activeWizard;

	public void Start()
	{
		if (!main) {
			DestroyImmediate(this);
		}
	}

}