using UnityEngine;
using HandyGestures;
using System;

public class PlayerController : MonoBehaviour, IPan
{
    private Transform player;
    private SpriteRenderer spriteRenderer;

    private Animator animator;

    private bool canSwitch;
    private bool canMove;
    private bool canSpoilHand;
    private bool wasPressingKey;

    private HandController hands;

    // Use this for initialization
    void Start()
    {
        player = transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = new Timer();
        timer.duration = 0.4f;
        timer.repeating = true;
        timer.Complete += CompleteMoving;
        hands = GetComponent<HandController>();

        var detector = FindObjectOfType<HandyDetector>();
        if (detector != null)
        {
            detector.defaultObject = gameObject;
        }

        previousPosition = player.position;

        animator = GetComponent<Animator>();

        canSwitch = true;
        canMove = true;
        canSpoilHand = true;
        wasPressingKey = false;

        Grouping.GroupManager.main.group["Game"].Add(this);
    }

    private Vector2 previousPosition;
    private Vector2 movement;
    private Vector2 nextMovement;
    private bool playerMoving;
    private Transform objectPushing;
    private Vector2 previousPushablePosition;
    private Timer timer;
    private bool Moving { get { return timer.running; } }

    #region Gestures

    public void OnGesturePan(PanArgs args)
    {
        if (timer == null) timer = new Timer();

        playerMoving = PlayerMoving(args);
    }

    private void UpdateKeyboardMovement()
    {
        var delta = new Vector2();
        PanArgs.State state;
        var pressingKey = true;

        if (Input.GetKey(KeyCode.W))
        {
            delta = DirectionExtensions.Down;
            state = Input.GetKeyDown(KeyCode.W) ? PanArgs.State.Move : PanArgs.State.Hold;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            delta = DirectionExtensions.Up;
            state = Input.GetKeyDown(KeyCode.S) ? PanArgs.State.Move : PanArgs.State.Hold;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            delta = DirectionExtensions.Right;
            state = Input.GetKeyDown(KeyCode.A) ? PanArgs.State.Move : PanArgs.State.Hold;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            delta = DirectionExtensions.Left;
            state = Input.GetKeyDown(KeyCode.D) ? PanArgs.State.Move : PanArgs.State.Hold;
        }
        else
        {
            pressingKey = false;
            state = PanArgs.State.Up;
        }

        if (pressingKey || wasPressingKey)
        {
            var args = new PanArgs(HandyDetector.Gesture.LongPress, state, Vector2.zero, Vector2.zero, delta);
            playerMoving = PlayerMoving(args);
        }

        wasPressingKey = pressingKey;
    }

    private bool PlayerMoving(PanArgs args)
    {
        Debug.Log(args.state);
        switch (args.state)
        {
            case PanArgs.State.Move:
                var x = args.delta.x;
                var y = args.delta.y;

                if (Math.Abs(x - y) >= 1f)
                {
                    nextMovement = Math.Abs(x) > Math.Abs(y) ? new Vector2(x < 0 ? 1 : -1, 0) : new Vector2(0, y < 0 ? 1 : -1);
                }

                if (canMove && !Moving && CanMove())
                {
                    timer.Reset();
                    movement = nextMovement;
                    canSwitch = true;
                    canMove = true;
                    canSpoilHand = true;
                }

                return true;
            case PanArgs.State.Hold:
                return true;
            case PanArgs.State.Interrupt:
            case PanArgs.State.Up:
                canSwitch = true;
                canMove = true;
                canSpoilHand = true;
                return false;
            default:
                return false;
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(4 * player.position.y) + 1;

        UpdateKeyboardMovement();

        timer.Update();

        if (Moving)
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

        SetAnimationState(Moving ? movement : nextMovement);
    }

    private void SetAnimationState(Vector2 directionVector)
    {
        string direction = null;

        if (directionVector == new Vector2(0, -1))
        {
            direction = "Down";
        }
        else if (directionVector == new Vector2(0, 1))
        {
            direction = "Up";
        }
        else if (directionVector == new Vector2(1, 0))
        {
            direction = "Right";
        }
        else if (directionVector == new Vector2(-1, 0))
        {
            direction = "Left";
        }

        if (direction != null)
        {
            if (Moving)
            {
                if (animator.GetBool("Pushing"))
                    animator.CrossFade("Push " + direction, 0.0f);
                else
                    animator.CrossFade("Walk " + direction, 0.0f);
            }
            else
            {
                animator.CrossFade("Idle " + direction, 0.0f);
            }
        }
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
                if (canPush && canSpoilHand && pushable.SpoilHand)
                {
                    hands.value -= 0.75f;
                    canSpoilHand = false;
                }

                canMove &= canPush && pushable.MovingWithPlayer;
                if (canMove)
                {
                    objectPushing = pushable.transform;
                    previousPushablePosition = objectPushing.position;
                }

                return canMove;

            case "Collectible":
                var collectible = hit.collider.GetComponent<Collectible>();
                collectible.Collect();

                // Test //
                hands.value += 1f;

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
        }
        else
        {
            timer.Stop();

            objectPushing = null;
        }
    }
}
