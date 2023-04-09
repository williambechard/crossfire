using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public GameObject Field1, Field2;
    
    private void Start()
    {
        JoinLobby();
    }
    private async Task JoinLobby()
    {
        string LobbyID= "OurLobbyID";

        var result = await NetworkManager.Instance.Runner.JoinSessionLobby(SessionLobby.Custom, LobbyID);
    }
    public void CreateGame()
    {
        NetworkManager.Instance.CreateGame("ABC", "Main");
    }


    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("test");
    }

    public override void  OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message + " " + returnCode);
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message + " " + returnCode);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message + " " + returnCode);
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        GetComponent<LoadScene>().loadNetworkScene();
    }
    
    public virtual void OnJoinedLobby()
    {
        Field1.SetActive(true);
        Field2.SetActive(true);
    }
}
