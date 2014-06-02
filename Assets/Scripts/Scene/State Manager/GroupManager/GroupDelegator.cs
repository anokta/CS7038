using System;

namespace Grouping
{
	public class GroupDelegator
	{

		//public GroupDelegator() {
		//}

		public GroupDelegator(System.Action update, System.Action enable, System.Action disable)
		{
			if (update == null) {
				Update = DefaultAction;
			} else {
				Update = update;
			}
			if (enable == null) {
				OnEnable = DefaultAction;
			} else {
				OnEnable = enable;
			}
			if (disable == null) {
				OnDisable = DefaultAction;
			} else {
				OnDisable = disable;
			}
		}

		public static void DefaultAction()
		{
		}

		public readonly System.Action Update;
		public readonly System.Action OnEnable;
		public readonly System.Action OnDisable;
	}
}