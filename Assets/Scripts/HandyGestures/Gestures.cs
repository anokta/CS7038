namespace HandyGestures
{
	public interface IGesture
	{
	}

	public interface IPress : IGesture
	{
		void OnGesturePress(PressArgs args);
	}

	public interface IFinished : IGesture
	{
		void OnGestureFinished(FinishedArgs args);
	}

	public interface ITap : IGesture
	{
		void OnGestureTap(TapArgs args);
	}

	public interface ILongPress : IGesture
	{
		void OnGestureLongPress(LongPressArgs args);
	}
	//public interface ISlide : IGesture
	//{
	//	void OnGestureSlide(SlideArgs args);
	//}
	public interface IFling : IGesture
	{
		void OnGestureFling(FlingArgs args);
	}

	public interface IPan : IGesture
	{
		void OnGesturePan(PanArgs args);
	}
}