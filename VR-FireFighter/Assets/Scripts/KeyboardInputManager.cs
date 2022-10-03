using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInputManager : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool analogMovement;

    public void OnMove(InputValue value) {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value) {
        LookInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value) {
        JumpInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection) {
        if (move != newMoveDirection) { 
            Debug.Log("I'm moving!!!!"); 
        }

        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection) {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState) {
        jump = newJumpState;
    }
}
