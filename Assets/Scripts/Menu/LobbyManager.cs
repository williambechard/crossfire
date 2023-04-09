using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public GameObject PlayBTNArea;
    public TextMeshProUGUI RoomNameText;
    public VerticalLayoutGroup vGroup;
    public TextMeshProUGUI text;
    public RoomInfo RoomInfoPrefab;
    public TextMeshProUGUI info;
    public TextMeshProUGUI createRoomName;
    public Button refreshBtn;
 
    // Start is called before the first frame update
    void Start()
    {
        ClearList();
        JoinLobby();
    
    }
 
    public void ClearList()
    {
        foreach (Transform child in vGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddToList(SessionInfo sessionInfo)
    {
        RoomInfo roomInfo = Instantiate(RoomInfoPrefab, vGroup.transform);
        roomInfo.SetInformation(sessionInfo);
        
        roomInfo.OnJoinSession += RoomInfoOnOnJoinSession;
    }

    private void RoomInfoOnOnJoinSession(SessionInfo obj)
    {
        
    }

    public void Handle_JoinRoom(Dictionary<string, object> message)
    {
        Debug.Log("HandleJoin room called");
        SessionInfo sessionInfo = (SessionInfo) message["value"];
        NetworkManager.Instance.JoinGame(sessionInfo.Name);
    }
    
    void Handle_Info(Dictionary<string, object> message)
    {
        List<SessionInfo> sessionInfos = (List<SessionInfo>) message["value"];
    
        if(sessionInfos.Count==0) OnNoGameSessionFound();
        else
        {
            ClearList();
            foreach (SessionInfo sessionInfo in sessionInfos)
            {
                Debug.Log(sessionInfo.Name);
                AddToList(sessionInfo);
            }
        }
        refreshBtn.gameObject.SetActive(true);
        info.GetComponent<BlinkingTextSimple>().isRunning = false;
    }
    
    void Handle_SecondPlayerJoined(Dictionary<string, object> message)
    {
        PlayBTNArea.SetActive(true);
    }

 
    
    IEnumerator WaitForEventManager()
    {
        while(EventManager.instance == null)
        {
            yield return null;
        }
        EventManager.StartListening("JoinRoom", Handle_JoinRoom);
        EventManager.StartListening("SecondPlayerJoined", Handle_SecondPlayerJoined);
        EventManager.StartListening("InfoUpdate", Handle_Info);
    }

    private void OnDisable()
    {
        EventManager.StopListening("JoinRoom", Handle_JoinRoom);
        EventManager.StopListening("SecondPlayerJoined", Handle_SecondPlayerJoined);
        EventManager.StopListening("InfoUpdate", Handle_Info);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForEventManager());
    }

    public void OnNoGameSessionFound()
    {
        info.text = "No game session found";
    }

    public void RefreshLobby()
    {
      
    }
    
    public void OnSearchingForSessions()
    {
        info.gameObject.SetActive(true);
        info.text = "Searching for sessions";
    }

    private async Task JoinLobby()
    {
        var result = await NetworkManager.Instance.Runner.JoinSessionLobby(SessionLobby.Custom, "OurLobbyID");
    }

    public void CreateGame()
    {
        NetworkManager.Instance.CreateGame(createRoomName.text, "Lobby");
    }

    public void OnJoinLobby()
    {
        var clientTask = JoinLobby();
    }

    public void SetRoomNameText()
    {
        RoomNameText.text = createRoomName.text;
    }

}
