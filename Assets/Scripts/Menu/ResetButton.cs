using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI text;
    private TextMeshProUGUI altText;
    public Button altButton;
    public void Start()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        if(altButton != null)
            altText = altButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void LoadScene()
    {
        StartCoroutine(WaitForNetwork());
    }

    IEnumerator WaitForNetwork()
    {
        while(NetworkManager.Instance.Runner==null)
        {
            yield return null;
        }
        
        LevelManager.Instance.LoadScene("Lobby");
    }
    
    public void OnButtonClick()
    {
        Debug.Log("Button clicked");
        // Do something when the button is clicked
        text.color = new Color(text.color.r, text.color.g, text.color.b, .25f);
        if (altButton != null)
        {
            altButton.interactable=false;
            altText.color = new Color(altText.color.r, altText.color.g, altText.color.b, .25f);
        }
          
        // Reset the button state to normal
      
        button.interactable = false;
    }
}
