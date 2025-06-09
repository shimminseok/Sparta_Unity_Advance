using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            MovementInput = Vector2.zero;
        }
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UIManager.Instance.CheckOpenPopup(UIInventory.Instance);
        }
    }

    public void OnOpenCharInfo(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UIManager.Instance.CheckOpenPopup(UICharInfo.Instance);
        }
    }
}