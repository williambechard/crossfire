using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public struct MyInput : INetworkInput
{
    public Vector2 moveDirection;
}
public class NetworkManager  : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkManager Instance;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput netInput)
    {
       
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("Connected to server");}
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (sessionList.Count == 0)
        {
            if(EventManager.instance!=null) EventManager.TriggerEvent("InfoUpdate", new Dictionary<string, object> {{"value", "No Rooms Found..."} });
        }
        else
        {
            if(EventManager.instance!=null) EventManager.TriggerEvent("InfoUpdate", new Dictionary<string, object> {{"value", sessionList.Count + " Rooms Found."} });
        }

        foreach (SessionInfo session in sessionList)
        {
            if(EventManager.instance!=null) EventManager.TriggerEvent("SessionListUpdate", new Dictionary<string, object> {{"value", session} });
        }
        
        
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    public NetworkRunner Runner;

    public async void StartGame(GameMode mode, SessionInfo sessionInfo)
    {
        // Start or join (depends on gamemode) a session with a specific name
        await Runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            //SessionName = "TestRoom",
            SessionName = sessionInfo.Name,
            PlayerCount = sessionInfo.PlayerCount,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            CustomLobbyName = "OurLobbyID",
            /*
             *     // other args...
            SessionName = [string],
            SessionProperties = [Dictionary<string, SessionProperty>],
            CustomLobbyName = [string],
            DisableClientSessionCreation = [bool],
            PlayerCount = [int],
            DisableNATPunchthrough = [bool],
            CustomSTUNServer = [string],
            AuthValues = [AuthenticationValues],
             */
        });
        EventManager.TriggerEvent("NetworkRoomSetupDone" , null);
    
    }

    
    public void CreateGame(string sessionName, string sceneName)
    {
        var clientTask = NetworkManager.Instance.InitializeNetworkRunner( Runner, GameMode.Host, sessionName,NetAddress.Any(),SceneUtility.GetBuildIndexByScenePath($"scenes/{sceneName}"));
    }

    public void JoinGame(string sessionName)
    {
        var clientTask = NetworkManager.Instance.InitializeNetworkRunner( Runner, GameMode.Client, sessionName,NetAddress.Any(),SceneManager.GetActiveScene().buildIndex);
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Create the Fusion runner and let it know that we will be providing user input
        Runner = gameObject.AddComponent<NetworkRunner>();
        Runner.ProvideInput = true;
    }
    


    public void Handle_Host(Dictionary<string, object> message) => StartGame(GameMode.Host, null);
    public void Handle_Client(Dictionary<string, object> message) => StartGame(GameMode.Client, null);
    
    IEnumerator WaitForEventManager()
    {
        while(EventManager.instance== null)
        {
            yield return null;
        }
        EventManager.StartListening("Host", Handle_Host);
        EventManager.StartListening("Client", Handle_Client);
    }
    
    private void OnEnable()
    {
        StartCoroutine(WaitForEventManager());
       
    }
    
    private void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.StopListening("Host", Handle_Host);
            EventManager.StopListening("Client", Handle_Client);
        }else Debug.Log("event manager is null");
    }
    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionName,  NetAddress address, SceneRef scene) 
    {
        return runner.StartGame(new StartGameArgs {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            CustomLobbyName = "Lobby",
            SessionName = sessionName,
        });
    }
}
