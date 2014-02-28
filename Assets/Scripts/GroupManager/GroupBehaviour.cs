using UnityEngine;
using System.Collections;

namespace Grouping
{
	public class GroupBehaviour : MonoBehaviour
	{
		public GroupItem groupItem { get; set; }

		void Awake()
		{
			enabled = false;
		}
		// Use this for initialization
		void Start()
		{
			if (groupItem != null) {
				groupItem.Start();
			}
		}
	
		// Update is called once per frame
		void Update()
		{
			if (groupItem != null) {
				groupItem.Update();
			}
		}

		void OnEnable()
		{
			if (groupItem != null) {
				groupItem.OnEnable();
			}
		}

		void OnDisable()
		{
			if (groupItem != null) {
				groupItem.OnDisable();
			}
		}
	}
}