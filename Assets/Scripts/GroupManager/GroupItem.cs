using System;

namespace Grouping {

	public interface IGroupItem
	{
		void Start();
		void Update();
		void OnEnable();
		void OnDisable();
	}

	public class GroupItem : IGroupItem {
		public virtual void Start() { }
		public virtual void Update() { }
		public virtual void OnEnable() { }
		public virtual void OnDisable() { }
	}

}

//public class 

