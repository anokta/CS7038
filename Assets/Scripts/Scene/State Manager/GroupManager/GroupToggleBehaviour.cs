using UnityEngine;
using System.Collections;

namespace Grouping
{
	public class GroupToggleBehaviour : MonoBehaviour
	{
		public GroupDelegator groupItem { get; set; }

		void Awake()
		{
			enabled = false;
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