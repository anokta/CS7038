
namespace Grouping
{
	public class GroupDelegator : GroupItem
	{

		public GroupDelegator() {
		}

		public GroupDelegator(Callback update, Callback enable, Callback disable) {
			this.update = update;
			this.enable = enable;
			this.disable = disable;
		}

		public delegate void Callback();

		private Callback _update = () => { };
		private Callback _enable = () => { };
		private Callback _disable = () => { };

		public Callback update {
			get { return _update; }
			set { 
				if (value == null) {
					_update = () => { };
				} else {
					_update = value;
				}
			}
		}

		public Callback enable {
			get { return _enable; }
			set { 
				if (value == null) {
					_enable = () => { };
				} else {
					_enable = value;
				}
			}
		}

		public Callback disable {
			get { return _disable; }
			set { 
				if (value == null) {
					_disable = () => { };
				} else {
					_disable = value;
				}
			}
		}

		public override void Update()
		{
			_update();
		}

		public override void OnEnable()
		{
			_enable();
		}

		public override void OnDisable()
		{
			_disable();
		}
	}
}