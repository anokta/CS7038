using UnityEngine;
using HandyGestures;
using System;

public class PlayerController : MonoBehaviour, IPan
{
    private Transform player;
    private SpriteRenderer spriteRenderer;

    private Animator animator;

    public enum PlayerAnimState { Idle = 0, Walk = 1, Push = 2, Action = 3, Wash = 4 }
    private PlayerAnimState animState;
    public PlayerAnimState AnimState { get { return animState; }  set { animState = value; } } 

    private Timer actionTimer;

    private const float PLAYER_SPEED = 0.4f;

    private bool isHeld;
    public bool IsHeld { get { return isHeld; } }

    private bool canSwitch;
    private bool canMove;
    private bool canSpoilHand;

    private Vector2 lastSwitchDirection;

    private HandController hands;
    private PlayerKeyboardController keyboardController;

    private Vector2 previousPosition;
    public Vector2 movement;
    private Vector2 nextMovement;
    public Vector2 NextDirection { get { return nextMovement; } }
    public bool playerMoving;
    private Transform objectPushing;
    private Vector2 previousPushablePosition;
    private Timer timer;
    private bool Moving { get { return timer.running; } }

    // Use this for initialization
    void Start()
    {
        player = transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        timer = new Timer(PLAYER_SPEED, CompleteMoving);
        timer.repeating = true;

        hands = GetComponent<HandController>();

        actionTimer = new Timer(PLAYER_SPEED, CompleteAction);

        keyboardController = new PlayerKeyboardController(this);

        var detector = FindObjectOfType<HandyDetector>();
        if (detector != null)
        {
            detector.defaultObject = gameObject;
        }

        previousPosition = player.position;

        animator = GetComponent<Animator>();
        animState = PlayerAnimState.Idle;

        canSwitch = true;
        canMove = true;
        canSpoilHand = true;

        Grouping.GroupManager.main.group["Running"].Add(this);
    }

    #region Gestures

    public void OnGesturePan(PanArgs args)
    {
        if (timer == null) timer = new Timer();

        PlayerMoving(args);
    }

    public void PlayerMoving(PanArgs args)
    {
        //Debug.Log(args.state);
        switch (args.state)
        {
            case PanArgs.State.Move:
                var x = args.delta.x;
                var y = args.delta.y;

                if (Math.Abs(x - y) >= 1f)
                {
                    nextMovement = Math.Abs(x) > Math.Abs(y) ? new Vector2(x < 0 ? 1 : -1, 0) : new Vector2(0, y < 0 ? 1 : -1);
                }

                if ((canMove || nextMovement != movement) && !Moving && CanMove())
                {
                    timer.Reset();
                    canSwitch = true;
                    canMove = true;
                    canSpoilHand = true;
                }

                isHeld = true;
                playerMoving = true;
                break;
            case PanArgs.State.Hold:
                playerMoving = true;
                isHeld = true;
                break;
            case PanArgs.State.Interrupt:
            case PanArgs.State.Up:
                canSwitch = true;
                canMove = true;
                canSpoilHand = true;
                playerMoving = false;
                isHeld = false;
                break;
            default:
                playerMoving = false;
                break;
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(4 * player.position.y) + 1;

        keyboardController.Update();

        timer.Update();
        actionTimer.Update();

        if (Moving)
        {
            var newPosition = previousPosition + timer.progress * movement;
            if (objectPushing != null)
            {
                objectPushing.position += newPosition.xy0() - player.position;
                animState = PlayerAnimState.Push;
            }
            else
            {
                animState = PlayerAnimState.Walk;
            }

            player.position = newPosition;
        }
        else
        {
            if ((int)animState < 3)
            {
                animState = PlayerAnimState.Idle;
            }
        }

        SetAnimationState(Moving ? movement : nextMovement);
        animator.SetInteger("State", (int)animState);

        if (lastSwitchDirection != nextMovement)
        {
            canSwitch = true;
        }
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
            //Debug.Log(animState.ToString() + " " + direction);
            animator.CrossFade(animState.ToString() + " " + direction, 0.0f);
        }
    }

    private bool CanMove()
    {
        objectPushing = null;

        movement = nextMovement;

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

                if(accessible.name.StartsWith("Fountain"))
                {
                    animState = PlayerAnimState.Wash;
                }

                return accessible.Enter();

            case "Switchable":
                if (canSwitch)
                {
                    Switchable switchable = hit.collider.GetComponent<Switchable>();
                    switchable.Switch();

                    canSwitch = false;
                    lastSwitchDirection = nextMovement;

                    StartAction();
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

    private void StartAction()
    {
        animState = PlayerAnimState.Action;
        actionTimer.Reset();
    }

    private void CompleteAction()
    {
        if (animState == PlayerAnimState.Action)
        {
            animState = PlayerAnimState.Idle;
        }
    }
}
