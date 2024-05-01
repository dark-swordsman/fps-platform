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
    public float speed, sprintMultiplier, sensitivity, maxForce, jumpForce;
    private bool grounded, sprint;
    private Vector2 move, look;
    private float lookRotation, rbRotation;

    /*
        TODO
        - fix vault yeet over ledges after jumping
        - add vaulting mechanic
    */

    // unity functions

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate() {
        MovePlayer();
    }

    void LateUpdate()
    {
        CameraLook();
        CheckLockCursor();
    }

    // custom functions

    void CameraLook() {
        if (Cursor.lockState == CursorLockMode.None) return;

        // transform.Rotate(Vector3.up * look.x * sensitivity);
        rbRotation += look.x * sensitivity;
        rb.MoveRotation(Quaternion.Euler(Vector3.up * rbRotation));
        lookRotation += -look.y * sensitivity;
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, rbRotation, camHolder.transform.eulerAngles.z);
    }

    // void CameraLook() {
    //     if (Cursor.lockState == CursorLockMode.None) return;

    //     lookRotation += new Vector2(look.x * sensitivity, Mathf.Clamp(-look.y * sensitivity, -90, 90));
    //     camHolder.transform.eulerAngles = new Vector3(lookRotation.x, lookRotation.y, camHolder.transform.eulerAngles.z);
    // }

    void CheckLockCursor() {
        if (Input.GetKeyDown(KeyCode.Tab)) Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyUp(KeyCode.Tab)) Cursor.lockState = CursorLockMode.Locked;
    }

    void MovePlayer() {
        if (!grounded) return;

        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= sprint && move.y > 0 ? speed * sprintMultiplier : speed;
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
        Jump();
    }

    public void SetSprint(InputAction.CallbackContext context) {
        sprint = context.performed ? true : false;
    }

    // public hooks

    public void SetGrounded(bool state) {
        grounded = state;
    }

    public void SetSensitivity(float sens) {
        sensitivity = sens;
    }

}
