using UnityEngine;
using System;
using Grouping;

public class HandController : MonoBehaviour
{
    //public Material GUInormal;
	// public Material GUIpie;
	public static readonly int MaxValue = 8;
	public static readonly int MinValue = 0;
	public static readonly int DefValue = 4;
	public static readonly int InfectionThreshold = -3;
    public GameObject cleanParticles;
    public GameObject infectionParticles;

    private ParticleSystem stars, infection;
	bool _showing;

	public bool showingWarning {
		get { return _showing; }
	}

	Transform _transform;

	//Removes overhead?
	public new Transform transform {
		get {	return _transform; }
	}

	void Awake() {
		_transform = base.transform;
	}


    public enum HandState
    {
        Clean,
        Dirty,
        Filthy
    }

    public HandState state
    {
        get
        {
            if (Math.Abs(_value - MaxValue) < 0.0001f)
            {
                return HandState.Clean;
            }
            if (Ratio >= 0.5f)
            {
                return HandState.Dirty;
            }
            return HandState.Filthy;
        }
    }

	int _value;

	public HandController() {
		_value = DefValue;
	}

	public int value
    {
        get { return _value; }
		set { _value = Math.Min(MaxValue, Math.Max(InfectionThreshold, value));
        if (_value <= MinValue)
        {
            AudioManager.PlaySFX("Heartbeat");
			if (_value <= -1)
            {
                AudioManager.FasterHeartBeat();
            }
        }
        else AudioManager.StopSFX("Heartbeat"); 
        }
    }

    float _guiValue = 4.0f;

    public float Ratio { get { return _guiValue / (MaxValue - MinValue); } }

    private int lastTouchedId;
    public int LastTouchedID
    {
        get { return lastTouchedId; }
        set { lastTouchedId = value; }
    }

	public Vector3 LastTouchedDir {get; set; }

	public void SpoilHand(int id)
	{
		--value;//value += amount;
	    lastTouchedId = id;
		++score;
	}

	public void SpoilHand(int id, Vector3 dir) {
		SpoilHand(id);
		LastTouchedDir = dir;
	}

	int _score;
	public int score {
		get { return _score; }
		private set {
			#if UNITY_EDITOR
			Debug.Log("Score: " + value);
			#endif
			_score = value;
		}
	}

	public void ResetHand(int id) {
		value = MinValue;
		lastTouchedId = id;
		++score;
	}

	public void RestoreHand(int id) {
		value = MaxValue;
		lastTouchedId = id;
		++score;
	}

	public static HandController activeHand {
		get;
		set;
	}

    void Start()
    {
		activeHand = this;
//		GameWorld.score = 0;
        GroupManager.main.group["Running"].Add(this);
        stars = (GameObject.Instantiate(cleanParticles) as GameObject).particleSystem;
        stars.transform.parent = transform.parent;
        stars.renderer.sortingOrder = short.MaxValue;

        infection = (GameObject.Instantiate(infectionParticles) as GameObject).particleSystem;
        infection.transform.parent = transform.parent;
        infection.renderer.sortingOrder = short.MaxValue;
    }

    bool resuming;

    void OnEnable()
    {
        if (resuming && stars != null)
        {
            stars.Play();
            infection.Play();
        }
    }

    void OnDisable()
    {
        if (stars != null && stars.isPlaying)
        {
            resuming = true;
            stars.Stop();
        }
        if (infection != null && infection.isPlaying)
        {
            infection.Stop();
        }
    }

    void Update()
    {
        if (_guiValue != _value)
        {
            _guiValue = Mathf.Lerp(_guiValue, _value, Time.deltaTime * 4);

            if (Mathf.Abs(_guiValue - _value) < 0.01f)
                _guiValue = _value;
        }
        if (this.state == HandState.Clean)
        {
            var size = Camera.main.ScreenToWorldLength(new Vector3(Screen.height * 0.025f, Screen.height * 0.025f, 0));

            //stars.transform.localScale = new Vector3(size.x, size.y, 1);
			//if (GUIManager.ScreenResized || GUIManager.CameraChanged) {
				stars.startSize = size.y;
				stars.transform.position =
                Camera.main.ScreenToWorldPoint(
					new Vector3(
						GUIManager.OffsetX() + Screen.height * 0.15f, GUIManager.OffsetY() + Screen.height * 0.15f,
						0)).xy().xy_(-1);
				stars.startSpeed = size.y * 3.5f;
			//}
            if (!stars.isPlaying)
            {
                stars.Play();
            }
        }
        else
        {
            if (!stars.isStopped)
            {
				stars.Stop();
            }
        }

		if (value <= MinValue)
        {
            var size = Camera.main.ScreenToWorldLength(new Vector3(Screen.height * 0.03f, Screen.height * 0.03f, 0));

            infection.startSize = size.y;
            infection.transform.position =
                Camera.main.ScreenToWorldPoint(
                new Vector3(
                    GUIManager.OffsetX() + Screen.height * 0.15f, GUIManager.OffsetY() + Screen.height * 0.15f,
                        0)).xy().xy_(-1);
            infection.startSpeed = size.y;
            if (!infection.isPlaying)
            {
                infection.Play();
            }

            _showing = AudioManager.HeartBeatProgress() < 0.4f;

        }
        else
        {
            if (!infection.isStopped)
            {
                infection.Stop();
            }
        }
    }

    public bool isInfected
    {
		get { return value <= InfectionThreshold; }
    }
}

