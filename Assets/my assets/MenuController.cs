using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject MenuUI;
    private bool menuIsOpen = false;

    // Unity methods

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // custom methods

    public bool GetMenuState() {
        return menuIsOpen;
    }

    private void CursorLock(bool state) {
        if (state) {
            MenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        } else {
            MenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // hooks

    public void ShowMenu() {
        menuIsOpen = !menuIsOpen;
        Debug.Log(menuIsOpen);
        CursorLock(menuIsOpen);
    }
}
