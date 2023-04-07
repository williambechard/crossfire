using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFader : MonoBehaviour {

    public float fadeDuration = 1f;

    public Image image;
    private Color startColor;
    private Color endColor;
    private Color fadeColor = new Color(77f/255, 77/255f, 77f/255, 1);
    private Color fullColor = new Color(1, 1, 1, 1);
 
    void Start () {
        // Get the Image component and store it
        image = GetComponent<Image>();
        
        // set initial color
        //image.color = new Color(255f, 255f, 255f, 1f);
    }
    
    public void FadeOut() {
        StopCoroutine(FadeImage());
        startColor = image.color;
        endColor = fadeColor;
        StartCoroutine(FadeImage());
    }
    
    public void FadeIn() {
        
        StopCoroutine(FadeImage());
        startColor = image.color;
        endColor = fullColor;
        StartCoroutine(FadeImage());
    }
    
    IEnumerator FadeImage() {
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

    }
}