using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSensitivity : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI textComponent;

    public void OnChange() {
        float rawValue = gameObject.GetComponent<Slider>().value;
        playerController.SetSensitivity(rawValue * 0.1f);
        textComponent.text = rawValue.ToString();
    }
}
