using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderShakeBlink : MonoBehaviour
{
    public Slider slider;
    public float shakeAmount = 0.1f;
    public float blinkSpeed = 10f;
    public float duration = 5f;

    private Color originalColor;
    private bool isShaking;
    private Color fillColor;  
    private void Start()
    {
        originalColor = slider.fillRect.GetComponent<Image>().color;
        fillColor = slider.fillRect.GetComponent<Image>().color;
    }

    public void StartShakeBlink()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeBlink());
        }
    }

    public void StopShakeBlink()
    {
        isShaking = false;
    }

    private IEnumerator ShakeBlink()
    {
        isShaking = true;
        float timer = 0f;

        while (timer < duration && isShaking)
        {
            // Shake the slider
            float offset = Random.Range(-shakeAmount, shakeAmount);
            Vector3 pos = slider.transform.position;
            pos.x += offset;
            slider.transform.position = pos;

            // Blink the foreground color between red and original color
            float blink = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            Color color = Color.Lerp(originalColor, Color.red, blink);
            slider.fillRect.GetComponent<Image>().color = color;

            timer += Time.deltaTime;
            yield return null;
        }

        // Reset slider position and color
        slider.transform.position = transform.position;
        slider.fillRect.GetComponent<Image>().color = originalColor;

        isShaking = false;
    }
}