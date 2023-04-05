using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class PlayerUIHandler : MonoBehaviour
{
    public Slider P1slider;
    public Slider P2slider;
    public UnifyText P1Text;
    public UnifyText P2Text;
    
    void SetupListener()
    {
        if (EventManager.instance != null)
        {
            EventManager.StartListening("UpdateScore", Handle_Score);
            EventManager.StartListening("BulletUpdate", Handle_Bullets);
            EventManager.StartListening("OutOfBullets", Handle_OutOfBullets);
        }else Debug.Log("event manager is null");
    }

    public void Handle_Score(Dictionary<string, object> message)
    {
        //determine player and adjust ui appropriately
        switch ((string)message["player"])
        {
            case"1":
                P1Text.Text = ((int)message["score"]).ToString();
                break;
            case"2":
                P2Text.Text = ((int)message["score"]).ToString();
                break;
        }
    }
    public void Handle_OutOfBullets(Dictionary<string, object> message)
    {
        //determine player and adjust ui appropriately
        switch ((string)message["player"])
        {
            case"1":
                P1slider.GetComponent<SliderShakeBlink>().StartShakeBlink();
                break;
            case"2":
                P2slider.GetComponent<SliderShakeBlink>().StartShakeBlink();
                break;
        }
    }
    public void Handle_Bullets(Dictionary<string, object> message)
    {
        //determine player and adjust ui appropriately
        switch ((string)message["player"])
        {
            case"1":
                P1slider.value = (float)message["slider"];
                P1slider.GetComponent<SliderShakeBlink>().StopShakeBlink();
                break;
            case"2":
                P2slider.value = (float)message["slider"];
                P2slider.GetComponent<SliderShakeBlink>().StopShakeBlink();
                break;
        }
    }
    
    private void OnEnable()
    {
        Invoke("SetupListener", .1f);
    }

    private void OnDisable()
    {
        EventManager.StopListening("UpdateScore", Handle_Score);
        EventManager.StopListening("BulletUpdate", Handle_Bullets);
        EventManager.StopListening("OutOfBullets", Handle_OutOfBullets);
    }
}
