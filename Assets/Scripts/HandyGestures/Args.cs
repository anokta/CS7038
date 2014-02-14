using HandyGestures;
using Gesture = HandyDetector.Gesture;
using UnityEngine;

namespace HandyGestures
{
	public class TouchArg
	{
		public TouchArg(Gesture sender)
		{
			handled = true;
			this.sender = sender;
		}

		public Gesture sender { get; protected set; }

		public bool handled { get; set; }
	}

	public class PressArgs : TouchArg
	{
		public PressArgs(Gesture sender, Vector2 position)
			: base(sender)
		{
			this.position = position;
		}

		public readonly Vector2 position;
	}

	public class GestureArgs
	{
	}

	public class LongPressArgs : TouchArg
	{
		public enum State
		{
			Interrupt,
			Down,
			Hold,
			Up
		}

		public LongPressArgs(Gesture sender, State state, Vector2 position)
			: base(sender)
		{
			this.state = state;
			this.position = position;
		}

		public readonly State state;
		public readonly Vector2 position;
	}

	public class PanArgs : TouchArg
	{
		public PanArgs(Gesture sender, State state, Vector2 start, Vector2 position, Vector2 delta)
			: base(sender)
		{
			this.state = state;
			this.start = start;
			this.position = position;
			this.delta = delta;
		}

		public enum State
		{
			Interrupt,
			Hold,
			Down,
			Move,
			Up
		}

		public readonly Vector2 start;
		public readonly Vector2 delta;
		public readonly Vector2 position;
		public readonly State state;
	}

	public class TapArgs : TouchArg
	{
		public TapArgs(Gesture sender, Vector2 position) 
			: base(sender)
		{
			this.position = position;
		}

		public readonly Vector2 position;
	}

	public class FinishedArgs : TouchArg {
		public FinishedArgs(Gesture sender, bool interrupted, Vector2 start, Vector2 position) 
			: base(sender)
		{
			this.start = start;
			this.position = position;
			this.interrupted = interrupted;
		}

		public readonly bool interrupted;
		public readonly	Vector2 start;
		public readonly Vector2 position;
	}

	public class FlingArgs : TouchArg
	{
		public FlingArgs(Gesture sender, State state, Vector2 start, Vector2 position) 
			: base(sender)
		{
			this.start = start;
			this.position = position;
			this.state = state;
		}

		public enum State
		{
			Start,
			End,
			Interrupt
		}

		public readonly State state;
		public readonly	Vector2 start;
		public readonly Vector2 position;
	}
}