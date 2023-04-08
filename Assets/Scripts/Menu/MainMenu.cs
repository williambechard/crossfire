using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject StartRoomBtn;
    public GameObject LoadingText;
    private void OnEnable()
    {
        StartCoroutine(WaitForEventManager());
    }

    public void LoadNetworkRoom()
    {
        LevelManager.Instance.LoadSceneOnly("RoomMenu");
    }
    
    void Handle_NetworkRoomSetupDone(Dictionary<string, object> message)
    {
        StartRoomBtn.SetActive(true);
        LoadingText.SetActive(false);
    }

    IEnumerator WaitForEventManager()
    {
        while(EventManager.instance== null)
        {
            yield return null;
        }
        EventManager.StartListening("NetworkRoomSetupDone", Handle_NetworkRoomSetupDone);
    }
    private void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.StopListening("NetworkRoomSetupDone", Handle_NetworkRoomSetupDone);
       
        }else Debug.Log("event manager is null");
    }
}
