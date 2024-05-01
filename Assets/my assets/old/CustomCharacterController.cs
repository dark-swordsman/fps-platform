using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomCharacterController : MonoBehaviour
{
    public GameObject playerCamera;
    public float strafeScaleFactor = 0.7f;
    public float runSpeed = 4f;
    public Slider slider;
    public TextMeshProUGUI tmpobj;
    private Vector2 turn;
    private String textForUI;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyUp(KeyCode.Tab)) Cursor.lockState = CursorLockMode.Locked;

        UpdateCamera();
        UpdateMovement(runSpeed);
    }

    void FixedUpdate() {
        // tmpobj.text = $"{slider.value}\n\n\n\n{textForUI}";
        tmpobj.text = $"{slider.value}";
    }

    // void FixedUpdate() {
    //     UpdateMovement(0.1f);
    // }  

    void UpdateCamera() {
        turn.x += Input.GetAxis("Mouse X") * slider.value;
        turn.y += Input.GetAxis("Mouse Y") * slider.value;
        transform.localRotation = Quaternion.Euler(0, turn.x, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(-turn.y, 0, 0); 
    }

    void UpdateMovement(float movementScale) {
        Vector3 capsuleForward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 forwardMovement = capsuleForward;
        Vector3 sidewaysMovement = Quaternion.Euler(0, 90, 0) * capsuleForward;
        Vector3 input = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        input.Normalize();
        input *= Math.Max(Math.Abs(Input.GetAxis("Vertical")), Math.Abs(Input.GetAxis("Horizontal")));
        textForUI = $"{input}\n{sidewaysMovement}\n{forwardMovement}";
        Vector3 output = ((forwardMovement * input.x) + (sidewaysMovement * input.z)) * movementScale * Time.deltaTime;
        transform.localPosition += output;
    }
}
