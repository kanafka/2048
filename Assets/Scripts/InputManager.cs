using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public delegate void OnMoveInput(Vector2Int direction);
    public static event OnMoveInput OnMove;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping;

    private void Update()
    {
        ProcessKeyboard();
        ProcessMouseSwipe();
        ProcessTouchSwipe();
    }

    private void ProcessKeyboard()
    {
        Vector2Int moveDir = Vector2Int.zero;
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
            moveDir = Vector2Int.up;
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
            moveDir = Vector2Int.down;
        else if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
            moveDir = Vector2Int.left;
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
            moveDir = Vector2Int.right;

        if (moveDir != Vector2Int.zero)
        {
            Debug.Log("Keyboard input: " + moveDir);
            OnMove?.Invoke(moveDir);
        }
    }

    private void ProcessMouseSwipe()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startTouchPosition = Mouse.current.position.ReadValue();
            isSwiping = true;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame && isSwiping)
        {
            endTouchPosition = Mouse.current.position.ReadValue();
            Vector2 swipeDelta = endTouchPosition - startTouchPosition;
            if (swipeDelta.magnitude > 50f)
            {
                Vector2Int direction = GetSwipeDirection(swipeDelta);
                Debug.Log("Mouse swipe input: " + direction);
                OnMove?.Invoke(direction);
            }
            isSwiping = false;
        }
    }

    private void ProcessTouchSwipe()
    {

        if (Touchscreen.current == null || Touchscreen.current.primaryTouch == null)
            return;

        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (!isSwiping)
            {
                startTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                isSwiping = true;
            }
        }
        if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame && isSwiping)
        {
            endTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 swipeDelta = endTouchPosition - startTouchPosition;
            if (swipeDelta.magnitude > 50f)
            {
                Vector2Int direction = GetSwipeDirection(swipeDelta);
                Debug.Log("Touch swipe input: " + direction);
                OnMove?.Invoke(direction);
            }
            isSwiping = false;
        }
    }


    private Vector2Int GetSwipeDirection(Vector2 swipeDelta)
    {
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            return swipeDelta.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            return swipeDelta.y > 0 ? Vector2Int.up : Vector2Int.down;
        }
    }
}
