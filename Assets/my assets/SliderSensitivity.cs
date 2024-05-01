using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSensitivity : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI textComponent;

    void Start() {
        SetLocalSens(1.0f);
    }

    public void OnChange() {
        float rawValue = gameObject.GetComponent<Slider>().value;
        SetLocalSens(rawValue);
    }

    void SetLocalSens(float sens) {
        playerController.SetSensitivity(sens * 0.1f);
        textComponent.text = sens.ToString();
    }
}
