using System.Collections;
using UnityEngine;
using TMPro;
 

public class CountDown : MonoBehaviour
{
    public GameObject IntroTextPrefab;
    public string IntroText;
    public string OutroText;
    public float IntroDuration = 3f;
    public float ScaleDuration = 0.5f;
    public float PauseDuration = 0.5f;
    public float IntroPauseDuration = 1f;
    public int NumCountdowns = 3;
  

    private void Start() => StartCoroutine(StartCountDown());
    
    
    private IEnumerator StartCountDown()
    {
        yield return new WaitForSeconds(IntroPauseDuration);
        // Instantiate the IntroText prefab
        UnifyText timerText = Instantiate(IntroTextPrefab, transform).GetComponent<UnifyText>();
        
        // Set the intro text character by character over IntroDuration seconds
        float startTime = Time.time;
        float endTime = startTime + IntroDuration;
        int charIndex = 0;
        while (Time.time < endTime)
        {
            int endIndex = Mathf.Min(charIndex + 1, IntroText.Length);
            timerText.Text = IntroText.Substring(0, endIndex);
            yield return new WaitForSeconds((endTime - Time.time) / (IntroText.Length - charIndex));
            charIndex = endIndex;
        }

        // Hide the intro text and instantiate the TimerText prefab
        timerText.gameObject.SetActive(false);
        yield return new WaitForSeconds(PauseDuration);
        timerText.gameObject.SetActive(true);
        
        // Do the countdown animation NumCountdowns times
        for (int i = NumCountdowns; i > 0; i--)
        {
            // Set the timer text to the current countdown value
            timerText.Text = i.ToString();
            
            // Scale the timer text from (0.1, 0.1, 0.1) to (1, 1, 1) over ScaleDuration seconds
            float scaleStartTime = Time.time;
            float scaleEndTime = scaleStartTime + ScaleDuration;
            while (Time.time < scaleEndTime)
            {
                float t = (Time.time - scaleStartTime) / ScaleDuration;
                timerText.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), Vector3.one, t);
                yield return null;
            }
            timerText.transform.localScale = Vector3.one;
            
            // Pause for PauseDuration seconds
            yield return new WaitForSeconds(PauseDuration);
        }

        if (OutroText.Length>0)
        {
            timerText.Text = OutroText;
            // Scale the timer text from (0.1, 0.1, 0.1) to (1, 1, 1) over ScaleDuration seconds
            float scaleStartTime = Time.time;
            float scaleEndTime = scaleStartTime + ScaleDuration;
            while (Time.time < scaleEndTime)
            {
                float t = (Time.time - scaleStartTime) / ScaleDuration;
                timerText.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), Vector3.one, t);
                yield return null;
            }
            timerText.transform.localScale = Vector3.one;
        }
        
        //change game state
        GameManager.instance.CurrentState = GameManager.GameState.Playing;
        
        // Destroy the timer text prefab
        Destroy(timerText.gameObject);
    }
}

