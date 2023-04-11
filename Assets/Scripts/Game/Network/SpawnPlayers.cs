using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class SpawnPlayers : MonoBehaviour, INetworkRunnerCallbacks
{

    public GameObject PlayerPrefab2;
    public Vector3 P1Position;
    public Vector3 P2Position;

    public NetworkObject P1;
    public NetworkObject P2;
    
    public static SpawnPlayers Local;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForNetwork());
    }

    public void BeginGame()
    {
        //PhotonNetwork.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }

    IEnumerator WaitForNetwork()
    {
        while (NetworkManager.Instance.Runner == null)
        {
            yield return null;
        }
       
        NetworkManager.Instance.Runner.AddCallbacks(this);
        //spawn once

        //NetworkManager.Instance.Runner.Spawn(PlayerPrefab, P1Position, Quaternion.identity, NetworkManager.Instance.Runner.LocalPlayer);
    }

    private void OnDisable()
    {
        NetworkManager.Instance.Runner.RemoveCallbacks(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("SpawnPlayer, Player joined: " + player);
        if (runner.IsServer)
        {
            //spawn player
            if (player == runner.LocalPlayer)
            {
                P1 = runner.Spawn(PlayerPrefab2, P1Position, Quaternion.identity, player);
            }else  P2 = runner.Spawn(PlayerPrefab2, P2Position, Quaternion.identity, player);

            Debug.Log("This is server");
        }else Debug.Log("This is client");

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
       
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
         
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
         
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
      if(runner.IsServer)
      { 
          
       
          Debug.Log("Server disconnected");
      }else Debug.Log("Client disconnected");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
 
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
      
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
         
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
     
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
       
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
}
