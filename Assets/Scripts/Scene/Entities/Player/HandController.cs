using UnityEngine;
using System;
using Grouping;

public class HandController : MonoBehaviour
{
    //public Material GUInormal;
	// public Material GUIpie;
    public Texture hand;
    public Texture circle;
    public Texture background;
	public static readonly int MaxValue = 8;
	public static readonly int MinValue = 0;
	public static readonly int InfectionThreshold = -3;
    public GameObject cleanParticles;
    public GameObject infectionParticles;

	public Texture handEmpty;
	public Texture warning1;
	public Texture warning2;
	public Texture warningSign;

    private ParticleSystem stars, infection;
	bool _showing;

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

	int _value = 4;
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

	public void SpoilHand(int id)
	{
		--value;//value += amount;
	    lastTouchedId = id;
		++score;
	}

	int _score;
	public int score {
		get { return _score; }
		private set { _score = value;
			Debug.Log("Current score: " + _score);
		}
	}

	public void ResetHand(int id) {
		value = MinValue;
		lastTouchedId = id;
		++score;
	}

	public void AddHandValue(int value, int id)
	{
		value = value;
		lastTouchedId = id;
		score += Math.Abs(value);
	}

	public void RestoreHand(int id) {
		value = MaxValue;
		lastTouchedId = id;
		++score;
	}

    void Start()
    {
		GameWorld.score = 0;
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

    void OnGUI()
    {
        if (Event.current.type.Equals(EventType.Repaint))
        {
			Material GUIpie = GUIManager.GUIPie;
            Rect drawPos = new Rect(GUIManager.OffsetX(), Screen.height - Screen.height * 0.1f - GUIManager.OffsetY() - Screen.height * 0.1f, Screen.height * 0.2f, Screen.height * 0.2f);
            GUIpie.SetFloat("Value", 1);
            GUIpie.color = new Color(1, 1, 1, 0.6f);
            //GUIpie.color = Color.white;
            Graphics.DrawTexture(drawPos, background, GUIpie);

            GUIpie.SetFloat("Value", Ratio);
            GUIpie.SetFloat("Clockwise", 0);
            GUIpie.color = new Color((1 - Ratio) * 0.75f, 0.75f, 0, 0.5f);

            Graphics.DrawTexture(drawPos, circle, GUIpie);

          //  GUIpie.SetFloat("Value", 1);
          //  GUIpie.color = Color.white;

			if (value <= MinValue) {
				if (value == MinValue) {
					Graphics.DrawTexture(drawPos, handEmpty);
				} else if (value == -1) {
					Graphics.DrawTexture(drawPos, warning1);
				} else if (value >= -2) {
					Graphics.DrawTexture(drawPos, warning2);
				}
				if (_showing) {
					Graphics.DrawTexture(drawPos, warningSign);
				}
			} else {
				Graphics.DrawTexture(drawPos, hand);
			}
        }
    }

    public bool isInfected
    {
		get { return value <= InfectionThreshold; }
    }
}

