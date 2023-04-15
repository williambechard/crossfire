using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    public Slider P1slider;
    public Slider P2slider;
    public UnifyText P1Text;
    public UnifyText P2Text;
    public GameObject CountDownPrefab;
    public Bullet[] allBullets;


    public IEnumerator WaitForEventManager()
    {

        while (EventManager.instance == null)
        {
            yield return null;
        }

        EventManager.StartListening("Init", Handle_GameState_Init);
        EventManager.StartListening("UpdateScore", Handle_Score);
        EventManager.StartListening("BulletUpdate", Handle_Bullets);
        EventManager.StartListening("OutOfBullets", Handle_OutOfBullets);

        //StartCoroutine(onTick());
    }

    public void Handle_GameState_Init(Dictionary<string, object> message)
    {
        //spawn the countdown prefab
        //Instantiate(CountDownPrefab, transform);
    }



    public void Handle_Score(Dictionary<string, object> message)
    {
        Debug.Log("player " + (string)message["player"] + " score " + ((int)message["score"]).ToString());
        //determine player and adjust ui appropriately
        switch ((string)message["player"])
        {
            case "1":
                P1Text.Text = ((int)message["score"]).ToString();
                break;
            case "2":
                P2Text.Text = ((int)message["score"]).ToString();
                break;
        }

        //RPC_Score((string)message["player"], ((int)message["score"]).ToString());

    }
    public void Handle_OutOfBullets(Dictionary<string, object> message)
    {
        //determine player and adjust ui appropriately
        switch ((string)message["player"])
        {
            case "1":
                P1slider.GetComponent<SliderShakeBlink>().StartShakeBlink();
                break;
            case "2":
                P2slider.GetComponent<SliderShakeBlink>().StartShakeBlink();
                break;
        }
    }
    public void Handle_Bullets(Dictionary<string, object> message)
    {
        Debug.Log("Handle Bullets called for player " + (string)message["player"] + " slider " + ((float)message["slider"]).ToString("0.00"));
        //determine player and adjust ui appropriately
        switch ((string)message["player"])
        {
            case "1":
                Debug.Log("adjusting slider for P1");
                P1slider.value = (float)message["slider"];
                P1slider.GetComponent<SliderShakeBlink>().StopShakeBlink();
                break;
            case "2":
                Debug.Log("adjusting slider for P2");
                P2slider.value = (float)message["slider"];
                P2slider.GetComponent<SliderShakeBlink>().StopShakeBlink();
                break;
        }
    }

    public void Start()
    {
        StartCoroutine(WaitForEventManager());
    }




    private void OnDisable()
    {

        EventManager.StopListening("Init", Handle_GameState_Init);
        EventManager.StopListening("UpdateScore", Handle_Score);
        EventManager.StopListening("BulletUpdate", Handle_Bullets);
        EventManager.StopListening("OutOfBullets", Handle_OutOfBullets);

    }
}
