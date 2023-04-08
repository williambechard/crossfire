using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkingTextSimple : MonoBehaviour
{
    public float blinkSpeed; // Speed at which the text blinks
    public float blinkDuration = 2.0f; // Duration of the blink effect
    TextMeshProUGUI text; // Reference to the TextMeshProUGUI component
    public bool isRunning =true;
    void Start()
    {
        // Get the TextMeshProUGUI component
        text = GetComponent<TextMeshProUGUI>();
        StartEffect();
        
    }

    public void StartEffect()
    {
        
        // Start the blink coroutine
        StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine()
    {
        float t = 0.0f;
        while (isRunning)
        {
            
          
                text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1.0f, 0.05f, t));
          
           

            // Increase t based on the blink speed
            t += Time.deltaTime * blinkSpeed;

            // If t exceeds 1, reset it to 0 and switch to increasing the alpha value
            if (t > 1.0f)
            {
                t = 0.0f;
            }

            // Wait for the next frame
            yield return null;
        }
        // Reset the alpha value to 1 when the blink effect is done
 
 
        text.color =  new Color(text.color.r, text.color.g, text.color.b, 1.0f);
      
    }
}
