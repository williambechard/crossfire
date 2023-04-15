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

    public IEnumerator WaitForEventManager()
    {
        if (NetworkManager.Instance.Runner.IsServer)
        {
            while (!EventManager.instance)
            {
                yield return null;
            }

            EventManager.StartListening("Init", Handle_GameState_Init);
            EventManager.StartListening("UpdateScore", Handle_Score);
            EventManager.StartListening("BulletUpdate", Handle_Bullets);
            EventManager.StartListening("OutOfBullets", Handle_OutOfBullets);
        }

    }



    public void Handle_GameState_Init(Dictionary<string, object> message)
    {
        //spawn the countdown prefab
        //Instantiate(CountDownPrefab, transform);
    }





    public void Handle_Score(Dictionary<string, object> message)
    {
        Debug.Log("player " + (string)message["player"]);
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
        //determine player and adjust ui appropriately
        switch ((string)message["player"])
        {
            case "1":
                P1slider.value = (float)message["slider"];
                P1slider.GetComponent<SliderShakeBlink>().StopShakeBlink();
                break;
            case "2":
                P2slider.value = (float)message["slider"];
                P2slider.GetComponent<SliderShakeBlink>().StopShakeBlink();
                break;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForEventManager());
    }

    private void OnDisable()
    {
        if (NetworkManager.Instance.Runner.IsServer)
        {
            EventManager.StopListening("Init", Handle_GameState_Init);
            EventManager.StopListening("UpdateScore", Handle_Score);
            EventManager.StopListening("BulletUpdate", Handle_Bullets);
            EventManager.StopListening("OutOfBullets", Handle_OutOfBullets);
        }
    }
}
