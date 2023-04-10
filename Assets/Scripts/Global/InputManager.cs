using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;



public class InputManager : MonoBehaviour, INetworkRunnerCallbacks
{

    public static InputManager Instance;
    public PlayerControls input = null;
    private Vector2 moveVector = Vector2.zero;

    public Input customInput;

    public bool verticalMovement = false;
    public bool horizontalMovement = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            input = new PlayerControls();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitForNetwork()
    {
        while (NetworkManager.Instance == null)
        {
            yield return null;
        }
        
        NetworkManager.Instance.Runner.AddCallbacks(this);
    }
    
    private void OnEnable()
    {
        input.Enable();
        input.Player.Look.performed += onLook;
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        input.Player.Move.canceled += onMoveCancel;
        input.Player.Fire.performed += onFire;
        

    }

    private void OnDisable()
    {
        input.Player.Look.performed -= onLook;
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        input.Player.Move.canceled -= onMoveCancel;
        input.Player.Fire.performed -= onFire;
        
   
            if(NetworkManager.Instance.Runner != null){
                // disabling the input map
                input.Disable();

                NetworkManager.Instance.Runner.RemoveCallbacks( this );
            }
    
    }

    public void onLook(InputAction.CallbackContext value)
    {
      
        if (EventManager.instance != null)
        {
            EventManager.TriggerEvent("PlayerLook",
                new Dictionary<string, object> { { "value", value.ReadValue<Vector2>()} });
        }
    }
    
    public void onFire(InputAction.CallbackContext value)
    {
        if (EventManager.instance != null)
            EventManager.TriggerEvent("PlayerFire", null);

    }

    public void onMoveCancel(InputAction.CallbackContext value)
    {
        if (moveVector.x == 0) horizontalMovement = false;
        if (moveVector.y == 0) verticalMovement = false;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>().normalized;
         
        if (EventManager.instance != null)
            EventManager.TriggerEvent("PlayerMove", new Dictionary<string, object> { { "value", moveVector }, {"playerId", "1"} });
    }

    
    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new System.NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput netInput)
    {/*
        Debug.Log("netInput" + netInput);
        var myInput = new MyInput(); //create new Network Input struct

        //capture and assign
        myInput.moveDirection.Set(input.Player.Move.ReadValue<Vector2>().x, input.Player.Move.ReadValue<Vector2>().y);

        //now set it across the network
        netInput.Set(myInput);*/
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new System.NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
}
