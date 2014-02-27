using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HandyGestures;

public class HandyDetector : MonoBehaviour
{

    #region Helper enums & editor variables

	public enum Gesture
	{
		Press,
		LongPress,
		Tap,
		Pan,
		Fling
	}

	public enum CollisionMode
	{
		Mode3D,
		Mode2D
	}

	public enum CollisionMethodType
	{
		Raycast,
		Spherecast,
		Custom,
	}

	public enum CollisionMethodType2D
	{
		Point,
		Circle,
		Custom
	}

	public delegate RaycastHit[] CollisionFunction(Vector3 position,float distance,int mask);

	public delegate Collider2D[] CollisionFunction2D(
		Vector3 position,int mask,float minDepth,float maxDepth);

    #endregion

    #region settings

	public CollisionMode CastMode;
	public CollisionMethodType CollisionMethod;
	public CollisionMethodType2D CollisionMethod2D;
	public CollisionFunctor CustomFunction;
	public CollisionFunctor2D CustomFunction2D;
	CollisionFunction _function;
	CollisionFunction2D _function2D;
	public float longTime = 0.2f;
	public float flingTime = 0.2f;
	public GameObject interceptor;
	public GameObject defaultObject;
	public int layerMask = -1;
	public float castDistance = Mathf.Infinity;
	public float maxDepth = Mathf.Infinity;
	public float minDepth = Mathf.NegativeInfinity;
	public float circleRadius = 16;
	public float sphereRadius = 1;
	public float moveThreshold = 1;
	// Analysis disable once FieldCanBeMadeReadOnly.Local

    #endregion

	Device _device;
	InputState _activeState;
	InputState _idle;
	Vector2 _startPos;
	Vector2 _prevPos;
	Vector2 _deltaMove;
	List<Transform> _objects;
	Gesture _prevType;
	IGesture _target;

	void Reset()
	{
		_target = null;
		//_prevType = Gesture.Press;
		if (_activeState != null && _activeState != _idle) {
			_idle.Activate();
		}
		_activeState = _idle;
	}

	bool moved()
	{
		return _deltaMove.magnitude >= moveThreshold;
	}

    #region Start & Setup States

	void Start()
	{
		_idle = _activeState = new InputState(this);
		InputState deadState = new InputState(this);
		InputState downState = new InputState(this);
		InputState panState = new InputState(this);
		InputState longState = new InputState(this);
		InputState slideState = new InputState(this);

		deadState.Up += Reset;
		deadState.Interrupt += Reset;

		_idle.Down += delegate {
			_startPos = _device.pos1;
			_objects = GetAllObjects(_device.pos1);
			var args1 = new PressArgs(Gesture.Press, _startPos);
			_target = DoGesture<IPress, PressArgs>(_objects, (t, a) => t.OnGesturePress(a), args1);
			_prevType = Gesture.Press;
			downState.Activate();
		};

		downState.Up += delegate {
			if (downState.time < longTime) {
				TapArgs args = new TapArgs(Gesture.Tap, _startPos);
				if (_target != null) {
					ITap tap = _target as ITap;
					if (IsEnabled(tap as Behaviour)) {
						_prevType = Gesture.Tap;
						tap.OnGestureTap(args); 
					}
				} else {
					_prevType = Gesture.Tap;
					DoGesture<ITap, TapArgs>(_objects, (t, a) => t.OnGestureTap(a), args);
				}
			}
			Reset();
		};

		downState.Moved += delegate {
			slideState.Activate();
		};

		downState.Hold += delegate {
			if (downState.time > longTime) {
				LongPressArgs largs = new LongPressArgs(Gesture.Press, LongPressArgs.State.Down, downState.startPos);
				PanArgs pArgs = new PanArgs(Gesture.Press, PanArgs.State.Down, downState.startPos, downState.startPos, Vector2.zero);
				IPan pan;
				ILongPress lpress;
				if (_target != null) {
					lpress = _target as ILongPress;
					bool handled = false;
					if (IsEnabled(lpress as Behaviour)) {
						lpress.OnGestureLongPress(largs);
						handled = largs.handled;
						_prevType = Gesture.LongPress;
						longState.Activate();
					}
					if (!handled) {
						pan = _target as IPan;
						if (IsEnabled(pan as Behaviour)) {
							pan.OnGesturePan(pArgs);
							if (pArgs.handled) {
								panState.Activate();
							} else {
								deadState.Activate();
							}
						}
					}
				} else {
					foreach (var item in _objects) {
						lpress = item.GetComponent(typeof(ILongPress)) as ILongPress;
						if (IsEnabled(lpress as Behaviour)) {
							lpress.OnGestureLongPress(largs);
							if (largs.handled) {
								_target = lpress;
								_prevType = Gesture.LongPress;
								longState.Activate();
								break;
							}
						}
						pan = item.GetComponent(typeof(IPan)) as IPan;
						if (IsEnabled(pan as Behaviour)) {
							pan.OnGesturePan(pArgs);
							if (pArgs.handled) {
								_target = pan;
								_prevType = Gesture.Pan;
								panState.Activate();
								break;
							}
						}
					}
				}
				if (_target == null) {
					deadState.Activate();
				}
			}
		};

		slideState.Enter += delegate {
			FlingArgs sArgs = new FlingArgs(Gesture.Press, FlingArgs.State.Start, slideState.startPos, slideState.startPos);
			PanArgs pArgs = new PanArgs(Gesture.Press, PanArgs.State.Down, slideState.startPos, slideState.startPos, Vector2.zero);
			IPan pan;
			IFling fling;
			if (_target != null) {
				fling = _target as IFling;
				bool handled = false;
				if (IsEnabled(fling as Behaviour)) {
					fling.OnGestureFling(sArgs);
					handled = sArgs.handled;
					if (handled) {
						//Stay in slide state!
						_prevType = Gesture.Fling;
					}
				}
				if (!handled) {
					pan = _target as IPan;
					if (IsEnabled(pan as Behaviour)) {
						pan.OnGesturePan(pArgs);
						if (pArgs.handled) {
							_prevType = Gesture.Pan;
							//Change state!
							panState.Activate();
						}
					}
				}
			} else {
				foreach (var item in _objects) {
					fling = item.GetComponent(typeof(IFling)) as IFling;
					if (IsEnabled(fling as Behaviour)) {
						fling.OnGestureFling(sArgs);
						if (sArgs.handled) {
							//Stay in slide state!
							_target = fling;
							_prevType = Gesture.Fling;
							break;
						}
					}
					pan = item.GetComponent(typeof(IPan)) as IPan;
					if (IsEnabled(pan as Behaviour)) {
						pan.OnGesturePan(pArgs);
						if (pArgs.handled) {
							//Change state!
							_target = pan;
							_prevType = Gesture.Pan;
							panState.Activate();
							break;
						}
					}
				}
			}
			if (_target == null) {
				deadState.Activate();
			}
		};

		slideState.Up += delegate {
			if (slideState.time <= flingTime) {
				FlingArgs sArgs = new FlingArgs(_prevType, FlingArgs.State.End, slideState.startPos, _device.pos1);
				IFling f = _target as IFling;
				if (IsEnabled(f as Behaviour)) {
					f.OnGestureFling(sArgs);
				}
			}

			Reset();
		};

		slideState.Hold += delegate {
			if (slideState.time > flingTime) {
				FlingArgs sArgs = new FlingArgs(Gesture.Press, FlingArgs.State.Interrupt, slideState.startPos, _device.pos1);
				IFling f = _target as IFling;
				if (IsEnabled(f as Behaviour)) {
					f.OnGestureFling(sArgs);
				}
				IPan p = _target as IPan;
				bool handled = false;
				if (IsEnabled(p as Behaviour)) {
					PanArgs pArgs = new PanArgs(Gesture.Fling, PanArgs.State.Down, _device.pos1, _device.pos1, Vector2.zero);
					p.OnGesturePan(pArgs);
					handled = pArgs.handled;
				}
				if (!handled) {
					deadState.Activate();
				} else {
					panState.Activate();
				}
			}
		};

		slideState.Interrupt += delegate {
			if (slideState.time <= flingTime) {
				FlingArgs sArgs = new FlingArgs(Gesture.Fling, FlingArgs.State.Interrupt, slideState.startPos, _device.pos1);
				IFling f = _target as IFling;
				if (f != null) {
					f.OnGestureFling(sArgs);
				}
			}

			Reset();
		};

		longState.Hold += delegate {
			ILongPress lpress = _target as ILongPress;
			if (IsEnabled(lpress as Behaviour)) {
				LongPressArgs args = new LongPressArgs(Gesture.Press, LongPressArgs.State.Hold, longState.startPos);
				lpress.OnGestureLongPress(args);
			}
		};

		longState.Up += delegate {
			ILongPress lpress = _target as ILongPress;
			if (IsEnabled(lpress as Behaviour)) {
				LongPressArgs args = new LongPressArgs(Gesture.Press, LongPressArgs.State.Up, longState.startPos);
				lpress.OnGestureLongPress(args);
			}

			Reset();
		}; 

		longState.Interrupt += delegate {
			ILongPress lpress = _target as ILongPress;
			if (lpress != null) {
				LongPressArgs args = new LongPressArgs(Gesture.Press, LongPressArgs.State.Interrupt, longState.startPos);
				lpress.OnGestureLongPress(args);
			}

			Reset();
		};

		longState.Moved += delegate {
			ILongPress lpress = _target as ILongPress;
			if (lpress != null && IsEnabled(lpress as Behaviour)) {
				LongPressArgs args = new LongPressArgs(Gesture.Press, LongPressArgs.State.Interrupt, longState.startPos);
				lpress.OnGestureLongPress(args);
				if (!args.handled) {
					deadState.Activate();
				} else {
					IPan pan = _target as IPan;
					if (IsEnabled(pan as Behaviour)) {
						PanArgs pArgs = new PanArgs(Gesture.LongPress, PanArgs.State.Down, _device.pos1, _device.pos1, Vector2.zero);
						pan.OnGesturePan(pArgs);
						if (pArgs.handled) {
							panState.Activate();
						} else {
							deadState.Activate();
						}
					}
				}
			}
		};

		panState.Hold += delegate {
			if (!moved()) {
				IPan pan = _target as IPan;
				if (IsEnabled(pan as Behaviour)) {
					PanArgs pArgs = new PanArgs(Gesture.Pan, PanArgs.State.Hold, panState.startPos, _device.pos1, Vector2.zero);
					pan.OnGesturePan(pArgs);
				}
			}
		}; 

		panState.Moved += delegate {
			IPan pan = _target as IPan;
			if (IsEnabled(pan as Behaviour)) {
				PanArgs pArgs = new PanArgs(Gesture.Pan, PanArgs.State.Move, panState.startPos, _device.pos1, _deltaMove);
				pan.OnGesturePan(pArgs);
			}
		};

		panState.Up += delegate {
			IPan pan = _target as IPan;
			if (IsEnabled(pan as Behaviour)) {
				PanArgs pArgs = new PanArgs(Gesture.Pan, PanArgs.State.Up, panState.startPos, _device.pos1, Vector2.zero);
				pan.OnGesturePan(pArgs);
			}
			Reset();
		};

		panState.Interrupt += delegate {
			IPan pan = _target as IPan;
			if (pan != null) {
				PanArgs pArgs = new PanArgs(Gesture.Pan, PanArgs.State.Interrupt, panState.startPos, _device.pos1, Vector2.zero);
				pan.OnGesturePan(pArgs);
			}

			Reset();
		};
		Reset();
	}

    #endregion

	private delegate void GestureAction<T,TA>(T obj,TA args);

	T DoGesture<T, TA>(Vector2 pos, GestureAction<T, TA> action, TA arg)
		where TA: TouchArg  where T: class, IGesture
	{
		return DoGesture(GetAllObjects(pos), action, arg);
	}

	static T DoGesture<T, TA>(List<Transform> list, GestureAction<T, TA> action, TA arg)
		where TA: TouchArg  where T: class, IGesture
	{
		foreach (var item in list) {
			var gesture = item.GetComponent(typeof(T)) as T;
			var behaviour = gesture as Behaviour;
			if (IsEnabled(behaviour)) {
				action(gesture, arg);
				if (arg.handled) {
					return gesture;
				}
			}
		}
		return null;
	}
	// Update is called once per frame
	void Update()
	{
		_device.Update();
		_deltaMove = _prevPos - _device.pos1;
		_prevPos = _device.pos1;

		UpdateCollisionTypes();

		if (_target != null && !IsEnabled(_target as Behaviour)) {
			_activeState.InvokeInterrupt();
			Reset();
		}

		if (_device.state1 == Device.DeviceState.Down) {
			_activeState.InvokeDown();
		}
		if (_device.state1 == Device.DeviceState.Hold) {
			if (moved()) {
				_activeState.InvokeMoved();
			} else {
				_activeState.Update();
			}
		}
		if (_device.state1 == Device.DeviceState.Up) {
			IGesture gesture = _target;
			_activeState.InvokeUp();

			if (gesture != null) {
				IFinished finished = gesture as IFinished;
				if (finished != null) {
					FinishedArgs args = new FinishedArgs(_prevType, false, _startPos, _device.pos1);
					finished.OnGestureFinished(args);
				}
			}
		}
	}

	Camera getCamera()
	{
		//UNDONE: Placeholder?
		//return camera == null ? Camera.main : camera;
		return Camera.main;
	}

    #region Receiving objects

	void UpdateCollisionTypes()
	{
		if (CastMode == CollisionMode.Mode3D) {
			switch (CollisionMethod) {
				case CollisionMethodType.Raycast:
					_function = Raycast3D;
					break;
				case CollisionMethodType.Spherecast:
					_function = Spherecast3D;
					break;
				default:
					_function = CustomResults;
					break;
			}
		} else {
			switch (CollisionMethod2D) {
				case CollisionMethodType2D.Point:
					_function2D = Raycast2D;
					break;
				case CollisionMethodType2D.Circle:
					_function2D = Circlecast2D;
					break;
				default:
					_function2D = CustomResults2D;
					break;
			}
		}
	}

	List<Transform> GetAllObjects(Vector2 pos)
	{
		switch (CastMode) {
			case CollisionMode.Mode2D:
				return GetAllObjects2D(pos);
			default:
				return GetAllObjects3D(new Vector3(pos.x, pos.y, 0));
		}
	}

	List<Transform> GetAllObjects2D(Vector2 pos)
	{
		//var results = Physics2D.RaycastAll(pos, -Vector2.up, castDistance, layerMask);
		var results = _function2D(pos, layerMask, minDepth, maxDepth);
		if (results == null) {
			return _empty;
		}
		var list = new List<Transform>();
		if (interceptor != null) {
			list.Add(interceptor.transform);
		}
		list.AddRange(
            from _ in results
               where (interceptor == null || _.transform != interceptor.transform)
			&& (defaultObject == null || _.transform != defaultObject.transform)
               orderby _.transform.position.z
               select _.transform);
		//results.Where(_ => _.transform != interceptor && _.transform != defaultObject)
		//   .OrderBy(_ => _.transform.position.z).Select(_ => _.transform));
		if (defaultObject != null) {
			list.Add(defaultObject.transform);
		}
		return list;
	}

	readonly List<Transform> _empty = new List<Transform>();

	static bool IsEnabled(Behaviour b) {
		return b != null && (b.enabled || b.GetType().IsDefined(typeof(PersistentGestureAttribute), false));
	}

	List<Transform> GetAllObjects3D(Vector3 pos)
	{
		//var results = Physics2D.RaycastAll(pos, -Vector2.up, castDistance, layerMask);
		var results = _function(pos, castDistance, layerMask);
		if (results == null) {
			return _empty;
		}
		var list = new List<Transform>();
		if (interceptor != null) {
			list.Add(interceptor.transform);
		}
		list.AddRange(
            from _ in results
			where (interceptor == null || _.transform != interceptor.transform)
			&& (defaultObject == null || _.transform != defaultObject.transform)
               orderby _.distance
               select _.transform);
		//   results.Where(_ => _.transform != interceptor && _.transform != defaultObject)
		// .OrderBy(_ => _.distance).Select(_ => _.transform));
		if (defaultObject != null) {
			list.Add(defaultObject.transform);
		}
		return list;
	}

	RaycastHit[] CustomResults(Vector3 position, float distance, int mask)
	{
		if (CustomFunction != null) {
			return CustomFunction.GetResults(position, distance, mask);
		}
		return null;
	}

	Collider2D[] CustomResults2D(Vector3 screenPoint, int mask, float min, float max)
	{
		if (CustomFunction2D != null) {
			return CustomFunction2D.GetResults(getCamera().ScreenToWorldPoint(screenPoint).xy(), mask, min, max);
		}
		return null;
	}

	Collider2D[] Raycast2D(Vector3 screenPoint, int mask, float min, float max)
	{
		return Physics2D.OverlapPointAll(getCamera().ScreenToWorldPoint(screenPoint).xy(), mask, min, max);
	}

	Collider2D[] Circlecast2D(Vector3 position, int mask, float min, float max)
	{
		return Physics2D.OverlapCircleAll(position, circleRadius, mask, min, max);
	}

	RaycastHit[] Spherecast3D(Vector3 position, float distance, int mask)
	{
		Ray r = getCamera().ScreenPointToRay(position);
		return Physics.SphereCastAll(r, circleRadius, distance, mask);
	}

	RaycastHit[] Raycast3D(Vector3 position, float distance, int mask)
	{
		Ray r = getCamera().ScreenPointToRay(position);
		return Physics.RaycastAll(r, distance, mask);
	}

    #endregion

    #region InputState

	class InputState
	{
		public event Callback Enter;
		public event Callback Exit;
		public event Callback Hold;
		public event Callback Down;
		public event Callback Up;
		public event Callback Moved;
		public event Callback Interrupt;

		readonly HandyDetector _parent;

		public float time { get; private set; }

		public Vector2 startPos { get; private set; }

		public delegate void Callback();

		public void Activate()
		{
			if (_parent._activeState.Exit != null) {
				_parent._activeState.Exit();
			}
			_parent._activeState.time = 0;
			_parent._activeState = this;
			startPos = _parent._device.pos1;
			if (Enter != null) {
				Enter();
			}
		}

		public void Update()
		{
			time += Time.deltaTime;
			if (Hold != null) {
				Hold();
			}
		}

		public void InvokeInterrupt()
		{
			if (Interrupt != null) {
				Interrupt();
			}
		}

		public void InvokeDown()
		{
			if (Down != null) {
				Down();
			}
		}

		public void InvokeMoved()
		{
			if (Moved != null) {
				Moved();
			}
		}

		public void InvokeUp()
		{
			if (Up != null) {
				Up();
			}
		}

		public InputState(HandyDetector parent)
		{
			_parent = parent;
		}
	}

    #endregion

    #region Device

	/// <summary> Abstract mouse and touch input </summary>
	struct Device
	{
		public enum DeviceState
		{
			None,
			Up,
			Down,
			Hold,
			Error
		}

		public DeviceState state1 { get ; private set; }

		public DeviceState state2 { get ; private set; }

		public Vector2 pos1 { get ; private set; }

		public Vector2 pos2 { get ; private set; }

		public static DeviceState FingerState(uint index)
		{
			if (Input.touchCount <= index) {
				return DeviceState.None;
			}
			switch (Input.touches [index].phase) {
				case TouchPhase.Began:
					return DeviceState.Down;
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					return DeviceState.Hold;
				case TouchPhase.Ended:
					return DeviceState.Up;
				default:
					return DeviceState.Error;
			}
		}

		public static int FingerCount()
		{
			return Input.touchCount;
		}

		public Vector2 FingerPosition(int index)
		{
			if (index == 0) {
				return pos1;
			}
			return Input.touches [index].position;
		}

		public DeviceState MouseState(int index)
		{
			if (Input.GetMouseButtonDown(index)) {
				return DeviceState.Down;
			}
			if (Input.GetMouseButton(index)) {
				return DeviceState.Hold;
			}
			if (Input.GetMouseButtonUp(index)) {
				return DeviceState.Up;
			} 
			return DeviceState.None;
		}

		public void Update()
		{
			if (Input.touchCount > 0) {
				pos1 = Input.touches [0].position;
				state1 = FingerState(0);
				if (Input.touchCount == 1) {
					pos2 = pos1;
					state2 = DeviceState.None;
				} else {
					pos2 = Input.touches [1].position;
					state2 = FingerState(1);
				}
			} else {
				//Mouse input
				pos1 = pos2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				state1 = MouseState(0);
				state2 = MouseState(1);
			}
		}
	}

    #endregion

}
