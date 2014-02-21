using UnityEngine;
using HandyGestures;
using System;

public class PlayerController : MonoBehaviour, IPan
{
    private Transform player;
    public enum HandState
    {
        Clean,
        Dirty,
        Filthy
    }

    int _cleanLevel;
    public int cleanLevel
    {
        get { return _cleanLevel; }
        set { _cleanLevel = Mathf.Clamp(value, 0, 4); }
    }

    public HandState handState
    {
        get
        {
            switch (_cleanLevel)
            {
                case 0:
                    return HandState.Clean;
                case 1:
                case 2:
                    return HandState.Dirty;
                default:
                    return HandState.Filthy;
            }
        }
    }

    public void clean()
    {
        _cleanLevel = 0;
    }

    public void improveHand()
    {
        --cleanLevel;
    }

    public void spoilHand()
    {
        ++cleanLevel;
    }

    // Use this for initialization
    void Start()
    {
        player = transform;
        timer = new Timer();
        timer.duration = 0.45f;
        timer.repeating = false;
        timer.Complete += CompleteMoveing;

        var detector = FindObjectOfType<HandyDetector>();
        if (detector != null)
        {
            detector.defaultObject = gameObject;
        }

        previousPosition = player.position;
    }

    private Vector2 previousPosition;
    private Vector2 movement;
    private Vector2 nextMovement;
    private bool playerMoving;
    private bool newMovementReady;
    private Transform objectPushing;
    private Vector2 previousPushablePosition;
    private Timer timer;

    #region Gestures

    public void OnGesturePan(PanArgs args)
    {
        if (timer == null) timer = new Timer();

        playerMoving = PlayerMoving(args);
    }

    private bool PlayerMoving(PanArgs args)
    {
        switch (args.state)
        {
            case PanArgs.State.Move:
                var x = args.delta.x;
                var y = args.delta.y;

                if (Math.Abs(x - y) < 1f) return false;

                if (Math.Abs(x) > Math.Abs(y))
                {
                    nextMovement = new Vector2(x < 0 ? 1 : -1, 0);
                }
                else
                {
                    nextMovement = new Vector2(0, y < 0 ? 1 : -1);
                }

                if (!timer.running) newMovementReady = true;

                return true;
            case PanArgs.State.Interrupt:
            case PanArgs.State.Up:
                return false;
            default:
                return false;
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (GameEventManager.CurrentState != GameEventManager.GameState.Running) return;

        if (newMovementReady)
        {
            movement = nextMovement;
            if (CanMove()) timer.Reset();
        }
        else
        {
            timer.Update();

            if (timer.running)
            {
                var newPosition = previousPosition + timer.progress * movement;
                if (objectPushing != null) objectPushing.position += newPosition.xy0() - player.position;
                player.position = newPosition;
            }
        }

        newMovementReady = false;
    }

    private bool CanMove()
    {
        // Get the next position
        var nextPosition = previousPosition + movement;

        // Check collisions
        var hit = Physics2D.Raycast(nextPosition, movement, 0.0f);

        if (hit.collider == null) return true;

        Debug.Log("Collided with " + hit.collider.name + " [" + hit.collider.tag + "].");

        switch (hit.collider.tag)
        {
            case "Wall":
                return false;

            case "Pushable":
                var pushable = hit.collider.GetComponent<Pushable>();
                if (pushable.MovingWithPlayer)
                {
                    objectPushing = pushable.transform;
                    previousPushablePosition = objectPushing.position;
                }
                return pushable.Push(movement);

            case "Collectible":
                var collectible = hit.collider.GetComponent<Collectible>();
                collectible.Collect();
                return true;

            case "Accessible":
                var accessible = hit.collider.GetComponent<Accessible>();
                return accessible.Enter();

            default:
                return true;
        }
    }

    private void CompleteMoveing()
    {
        previousPosition += movement;
        player.position = previousPosition;

        if (objectPushing != null)
        {
            objectPushing.position = previousPushablePosition + movement;
            objectPushing = null;
        }

        newMovementReady = playerMoving;
    }
}
