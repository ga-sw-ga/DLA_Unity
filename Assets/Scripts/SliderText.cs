using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    public string textBeforeValue;
    public string valueFormat = "F2";
    
    private TextMeshProUGUI _text;
    private Slider _slider;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _slider = transform.parent.GetComponent<Slider>();
    }

    private void Update()
    {
        _text.text = textBeforeValue + _slider.value.ToString(valueFormat);
    }
}
