using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnableText : MonoBehaviour
{
    private TextMeshProUGUI text;

    public void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void TextEnable(Toggle toggle)
    {
        if (toggle.isOn) text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
        else text.color = new Color(text.color.r, text.color.g, text.color.b, .25f);

    }
    
    public void TextEnable(bool enabled)
    {
        if(enabled)
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
        else text.color = new Color(text.color.r, text.color.g, text.color.b, .25f);
    }
   
}
