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
        //StartNetwork();
    }

    private async Task StartNetwork()
    {
        
        var result = await NetworkManager.Instance.Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            //SessionName = "TestRoom",
            SessionName = "TestRoom",
            CustomLobbyName = "OurLobbyID",
        });
        //JoinLobby();
    }
    
    public void ClearList()
    {
         foreach (Transform child in vGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void Handle_SessionListInfo(Dictionary<string, object> message)
    {
        SessionInfo sessionInfo = (SessionInfo) message["value"];
        AddToList(sessionInfo);
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
        SessionInfo sessionInfo = (SessionInfo) message["value"];
        NetworkManager.Instance.JoinGame(sessionInfo.Name);
        
    }
    
    void Handle_Info(Dictionary<string, object> message)
    {
        string result = (string) message["value"];
        info.text = result;
        refreshBtn.gameObject.SetActive(true);
        info.GetComponent<BlinkingTextSimple>().isRunning = false;
    }
    
    IEnumerator WaitForEventManager()
    {
        while(EventManager.instance == null)
        {
            yield return null;
        }
        EventManager.StartListening("SessionListUpdate", Handle_SessionListInfo);
        EventManager.StartListening("JoinRoom", Handle_JoinRoom);
        EventManager.StartListening("InfoUpdate", Handle_Info);
    }

    private void OnDisable()
    {
        EventManager.StopListening("SessionListUpdate", Handle_SessionListInfo);
        EventManager.StopListening("JoinRoom", Handle_JoinRoom);
        EventManager.StopListening("InfoUpdate", Handle_Info);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForEventManager());
    }

    public void OnNoGameSessionFound()
    {
        info.gameObject.SetActive(true);
        info.text = "No game session found";
    }

    public void OnSearchingForSessions()
    {
        info.gameObject.SetActive(true);
        info.text = "Searching for sessions";
    }

    private async Task JoinLobby()
    {
        string LobbyID= "OurLobbyID";

        var result = await NetworkManager.Instance.Runner.JoinSessionLobby(SessionLobby.Custom, LobbyID);
    }

    public void CreateGame()
    {
        NetworkManager.Instance.CreateGame(createRoomName.text, "Main");
    }

    public void OnJoinLobby()
    {
        var clientTask = JoinLobby();
    }


}
