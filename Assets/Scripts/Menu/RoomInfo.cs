using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;


public class RoomInfo : MonoBehaviour
{
    
    SessionInfo sessionInfo;
    
    public TextMeshProUGUI RoomName;
    public TextMeshProUGUI RoomPlayers;
    public GameObject JoinRoomBtn;
    
    //event
    public event Action<SessionInfo> OnJoinSession;
    
    public void SetInformation(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;
        RoomName.text = sessionInfo.Name;
        RoomPlayers.text = sessionInfo.PlayerCount + "/" + sessionInfo.MaxPlayers;
        if(sessionInfo.PlayerCount< sessionInfo.MaxPlayers) JoinRoomBtn.SetActive(true);

    }

    public void onClick()
    {
        Debug.Log("Join room clicked! "+ sessionInfo);
        EventManager.TriggerEvent("JoinRoom", new Dictionary<string, object> {{"value", sessionInfo} });
    }
    
    
    
    
}
