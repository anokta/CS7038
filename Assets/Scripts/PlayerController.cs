using UnityEngine;
using HandyGestures;
using System;
using Grouping;

public class PlayerController : MonoBehaviour, IPan
{
    private Transform player;
    private SpriteRenderer spriteRenderer;

    private Animator animator;

    private bool canSwitch;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = new Timer();
        timer.duration = 0.4f;
        timer.repeating = true;
        timer.Complete += CompleteMoving;

        var detector = FindObjectOfType<HandyDetector>();
        if (detector != null)
        {
            detector.defaultObject = gameObject;
        }

        previousPosition = player.position;

        animator = this.GetComponent<Animator>();

        canSwitch = true;

        Grouping.GroupManager.main.group["Game"].Add(this);
    }

    private Vector2 previousPosition;
    private Vector2 movement;
    private Vector2 nextMovement;
    private bool playerMoving;
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

                if (Math.Abs(x - y) >= 1f)
                {
                    if (Math.Abs(x) > Math.Abs(y))
                    {
                        nextMovement = new Vector2(x < 0 ? 1 : -1, 0);
                    }
                    else
                    {
                        nextMovement = new Vector2(0, y < 0 ? 1 : -1);
                    }
                }

                if (!timer.running && CanMove())
                {
                    timer.Reset();
                    movement = nextMovement;
                }

                return true;
            case PanArgs.State.Hold:
                return true;
            case PanArgs.State.Interrupt:
            case PanArgs.State.Up:
                canSwitch = true;
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

        spriteRenderer.sortingOrder = - Mathf.RoundToInt(4 * player.position.y) + 1;

        timer.Update();

        if (timer.running)
        {
            var newPosition = previousPosition + timer.progress * movement;
            if (objectPushing != null)
            {
                objectPushing.position += newPosition.xy0() - player.position;
                animator.SetBool("Pushing", true);
            }
            else
            {
                animator.SetBool("Pushing", false);
            }

            animator.SetBool("Moving", true);
            player.position = newPosition;
        }
        else
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Pushing", false);
        }

        // Set animation
        int direction = -1;
        if (movement == new Vector2(0, -1))
        {
            direction = 0;
        }
        else if (movement == new Vector2(0, 1))
        {
            direction = 2;
        }
        else if (movement == new Vector2(1, 0))
        {
            direction = 1;
        }
        else if (movement == new Vector2(-1, 0))
        {
            direction = 3;
        }

        animator.SetInteger("Direction", direction);
    }

    private bool CanMove()
    {
        objectPushing = null;

        // Get the next position
        var nextPosition = previousPosition + nextMovement;

        // Check collisions
        var hit = Physics2D.Raycast(nextPosition, nextMovement, 0.0f);

        if (hit.collider == null) return true;

        Debug.Log("Collided with " + hit.collider.name + " [" + hit.collider.tag + "].");

        switch (hit.collider.tag)
        {
            case "Wall":
                return false;

            case "Pushable":
                var pushable = hit.collider.GetComponent<Pushable>();
                var canPush = pushable.Push(nextMovement);
                if (canPush && pushable.MovingWithPlayer)
                {
                    objectPushing = pushable.transform;
                    previousPushablePosition = objectPushing.position;
                }

                return canPush;

            case "Collectible":
                var collectible = hit.collider.GetComponent<Collectible>();
                collectible.Collect();
                return true;

            case "Accessible":
                var accessible = hit.collider.GetComponent<Accessible>();
                return accessible.Enter();

            case "Switchable":
                if (canSwitch)
                {
                    Switchable switchable = hit.collider.GetComponent<Switchable>();
                    switchable.Switch();

                    canSwitch = false;
                }
                return false;

            default:
                return true;
        }
    }

    private void CompleteMoving()
    {
        previousPosition += movement;
        player.position = previousPosition;

        if (objectPushing != null)
        {
            previousPushablePosition += movement;
            objectPushing.position = previousPushablePosition;
        }

        if (playerMoving && CanMove())
        {
            movement = nextMovement;
            canSwitch = true;
        }
        else
        {
            timer.Stop();

            objectPushing = null;
        }
    }
}
