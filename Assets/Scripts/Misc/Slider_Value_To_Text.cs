using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slider_Value_To_Text : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    public void Start() {
        text.text = ((float)Math.Truncate(slider.value * 100)).ToString() + "%";
    }

    public void UpdateText() {
        text.text = ((float)Math.Truncate(slider.value * 100)).ToString() + "%";
    }
}

