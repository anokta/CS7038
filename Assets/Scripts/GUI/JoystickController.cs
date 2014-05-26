using UnityEngine;
using System.Collections;
using Grouping;

public class JoystickController : MonoBehaviour {

    public Texture2D[] directions;

    public float size;

    private bool isHeld;
    public bool IsHeld
    {
        get { return isHeld; }
        set
        {
            isHeld = value;

            if (!isHeld)
            {
                currentDirection = Vector2.zero;
                targetAlpha = 0.0f;
                alphaSpeed = 1.0f;
            }
            else
            {
                currentAlpha = 0.0f;
                targetAlpha = 1.0f;
                currentPosition = TargetPosition;
            }
        }
    }

    private Vector2 currentDirection;
    public Vector2 CurrentDirection
    {
        get { return currentDirection; }
        set { if (value != currentDirection) currentPosition = TargetPosition; currentDirection = value; if (currentDirection != Vector2.zero) alphaSpeed = 4.0f;  }
    }

    private Vector2 currentPosition, targetPosition;
    public Vector2 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = Vector2.Min(Vector2.Max(value, new Vector2(size, size)), new Vector2(Screen.width - size, Screen.height - size)); }
    }

    private float currentAlpha, targetAlpha, alphaSpeed;


    void ResetScreen()
    {
        size *= Screen.height;
    }

	void Start () {
        GroupManager.main.group["Running"].Add(this);

        ResetScreen();
	}
	
	void Update () {
        if (GUIManager.ScreenResized)
        {
            ResetScreen();
        }

        currentPosition = Vector2.Lerp(currentPosition, targetPosition, Time.deltaTime * 12);

        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * alphaSpeed);
	}

    void OnGUI()
    {
        if (isHeld)
        {
            Color c = GUI.color;
            c.a = currentAlpha;
            GUI.color = c;

            Rect rect = new Rect(currentPosition.x - 0.5f * size, currentPosition.y - 0.5f * size, size, size);
            Rect current;

            float offset = 1.1f * size;

            // Up
            if (currentDirection == Vector2.zero || currentDirection == Vector2.up)
            {
                current = rect;
                current.y -= offset;
                GUI.DrawTexture(current, directions[0]);
            }
            // Right
            if (currentDirection == Vector2.zero || currentDirection == Vector2.right)
            {
                current = rect;
                current.x += offset;
                GUI.DrawTexture(current, directions[1]);
            }
            // Down
            if (currentDirection == Vector2.zero || currentDirection == -Vector2.up)
            {
                current = rect;
                current.y += offset;
                GUI.DrawTexture(current, directions[2]);
            }
            // Left
            if (currentDirection == Vector2.zero || currentDirection == -Vector2.right)
            {
                current = rect;
                current.x -= offset;
                GUI.DrawTexture(current, directions[3]);
            }
        }
    }
}
