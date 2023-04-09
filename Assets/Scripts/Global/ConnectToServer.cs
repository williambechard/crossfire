using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Photon.Realtime;
using UnityEngine;
using Photon.Pun;
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
   


    IEnumerator WaitForNetwork()
    {
        while (NetworkManager.Instance.Runner != null)
        {
            yield return null;
        }

        JoinLobby();
    }
    
    IEnumerator WaitForEventManager()
    {
        while(EventManager.instance == null)
        {
            yield return null;
        }
       
    }

    private void OnDisable()
    {
       
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForEventManager());
        StartCoroutine(WaitForNetwork());
    }

    private async Task JoinLobby()
    {
        string LobbyID= "OurLobbyID";
        Debug.Log("Attempting to join a lobby");
        var result = await NetworkManager.Instance.Runner.JoinSessionLobby(SessionLobby.Custom, LobbyID);
    }

    public void CreateGame()
    {
        NetworkManager.Instance.CreateGame("Main", "Main");
    }

    public void OnJoinLobby()
    {
        var clientTask = JoinLobby();
    }

}
