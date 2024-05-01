using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject camHolder;
    public MenuController menuController;
    public float speed, sprintMultiplier, crouchMultiplier, crouchCameraHeightReduction, sensitivity, maxForce, jumpForce;
    private bool grounded, sprint, crouch;
    private Vector2 move, look;
    private float lookRotation, rbRotation, cameraDefaultHeight;

    /*
        TODO
        - fix vault yeet over ledges after jumping / add vaulting mechanic
    */

    // unity functions

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        cameraDefaultHeight = camHolder.transform.localPosition.y;
    }

    void FixedUpdate() {
        MovePlayer(menuController.GetMenuState());
    }

    void LateUpdate()
    {
        if (!menuController.GetMenuState()) CameraLook();
        // CheckLockCursor();
    }

    // custom functions

    void CameraLook() {
        transform.Rotate(Vector3.up * look.x * sensitivity);
        rbRotation += look.x * sensitivity;
        rb.MoveRotation(Quaternion.Euler(Vector3.up * rbRotation));
        lookRotation += -look.y * sensitivity;
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, rbRotation, camHolder.transform.eulerAngles.z);

        if (crouch && camHolder.transform.localPosition.y == cameraDefaultHeight) camHolder.transform.localPosition = new Vector3(0, cameraDefaultHeight - crouchCameraHeightReduction, 0);
        if (!crouch && camHolder.transform.localPosition.y != cameraDefaultHeight) camHolder.transform.localPosition = new Vector3(0, cameraDefaultHeight, 0);
    }

    void CheckLockCursor() {
        if (Input.GetKeyDown(KeyCode.Tab)) Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyUp(KeyCode.Tab)) Cursor.lockState = CursorLockMode.Locked;
    }

    void MovePlayer(bool disableMovement) {
        if (!grounded) return;

        Vector3 targetVelocity = disableMovement ? Vector3.zero : new Vector3(move.x, 0, move.y);

        if (crouch) {
            targetVelocity *= speed * crouchMultiplier;
        } else if (sprint && move.y > 0) { 
            targetVelocity *= speed * sprintMultiplier; 
        } else {
            targetVelocity *= speed;
        }

        targetVelocity = transform.TransformDirection(targetVelocity);
        Vector3 velocityChange = targetVelocity - rb.velocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        Vector3.ClampMagnitude(velocityChange, maxForce);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Jump() {
        Vector3 jumpForces = Vector3.zero;
        if (grounded) jumpForces = Vector3.up * jumpForce;
        rb.AddForce(jumpForces, ForceMode.VelocityChange);
    }

    // input hooks

    public void OnMove(InputAction.CallbackContext context) {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (!menuController.GetMenuState()) Jump();
    }

    public void SetSprint(InputAction.CallbackContext context) {
        sprint = context.performed ? true : false;
    }

    public void SetCrouch(InputAction.CallbackContext context) {
        crouch = context.performed ? true : false;
    }

    // public hooks
    
    public void SetGrounded(bool state) {
        grounded = state;
    }

    public void SetSensitivity(float sens) {
        sensitivity = sens;
    }
}
