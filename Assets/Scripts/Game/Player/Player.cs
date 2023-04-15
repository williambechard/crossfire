using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : Entity, IMovable, IAttack, IDamageable, IPlayerControlled, INetworkRunnerCallbacks
{
    [SerializeField]
    [Networked] int TotalBullets { get; set; }
    [SerializeField]
    [Networked] int TotalFired { get; set; }

    public PlayerRef playerRef;
    public event Action OnMove;
    public event Action OnAttack;
    public event Action OnDamaged;
    public event Action OnDestroyed;

    public Transform RotateObject;
    private Vector2 inputVelocity;
    private Camera cam;

    public void Damage(int amount)
    {
        throw new NotImplementedException();
    }

    public string id;
    public GameObject bulletPrefab;
    public BulletHandler bulletHandler;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector2 mousePosition;
    [SerializeField]
    private int score;
    private bool _canMove;
    [SerializeField]
    private int _health;
    [SerializeField]

    public float Speed;

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
        }
    }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            Debug.Log("player " + id + " score event " + score + " " + EventManager.instance);

            if (EventManager.instance != null)
                EventManager.TriggerEvent("UpdateScore", new Dictionary<string, object> { { "player", id }, { "score", value } });

        }
    }

    public bool CanMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }

    float IMovable.Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private CharacterController controller;

    public void Attack(Dictionary<string, object> message)
    {
        if (CanMove)
        {
            //OnAttack?.Invoke();

            //bulletHandler.FireBullet(RotateObject.forward);
        }
    }

    public void Handle_PlayerMove(Dictionary<string, object> message)
    {
        Debug.Log("Handle Player Move!");
        if (CanMove)
        {
            Vector2 moveVector = (Vector2)message["value"];

            inputVelocity = moveVector;
            Move(new Vector2(moveVector.y, -moveVector.x));
        }
    }

    public void Start()
    {
        TotalBullets = 10;
        TotalFired = 0;
    }

    IEnumerator WaitForGameManager()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }
        //assign id
        if (GameManager.instance.P1 == null)
        {
            GameManager.instance.P1 = this;
            GameManager.instance.P1.id = "1";
        }
        else if (GameManager.instance.P2 == null)
        {
            GameManager.instance.P2 = this;
            GameManager.instance.P2.id = "2";
        }
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.instance == null)
        {
            yield return null;
        }
        EventManager.StartListening("PlayerMove", Handle_PlayerMove);
        EventManager.StartListening("PlayerStop", Handle_PlayerMove);
        EventManager.StartListening("PlayerLook", Handle_PlayerLook);
        EventManager.StartListening("PlayerFire", Attack);
    }

    IEnumerator WaitForNetworkManager()
    {
        while (NetworkManager.Instance == null)
        {
            yield return null;
        }
        bulletHandler.player = this;
        bulletHandler.FillQuiver(10, playerRef);
    }

    void Handle_PlayerLook(Dictionary<string, object> message)
    {
        //mousePosition = (Vector2)message["value"];
        //Vector2 playerPosition = cam.WorldToScreenPoint(transform.position);
    }

    private void OnEnable()
    {

        controller = GetComponent<CharacterController>();
        StartCoroutine(WaitForEventManager());
        StartCoroutine(WaitForGameManager());
        NetworkManager.Instance.Runner.AddCallbacks(this);
        cam = Camera.main;
        StartCoroutine(WaitForNetworkManager());

    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.StopListening("PlayerMove", Handle_PlayerMove);
            EventManager.StopListening("PlayerStop", Handle_PlayerMove);
            EventManager.StopListening("PlayerLook", Handle_PlayerLook);
            EventManager.StopListening("PlayerFire", Attack);
        }

        InputManager.Instance.input.Player.Move.canceled -= CancelMove;
        NetworkManager.Instance.Runner.RemoveCallbacks(this);
    }
    public void Move(Vector2 moveVector)
    {
        //velocity = (new UnityEngine.Vector3(moveVector.x, RigidBody.velocity.y, moveVector.y) * Speed);
    }

    public override void FixedUpdateNetwork()
    {
        if (NetworkManager.Instance.Runner.IsServer)
        {
            if (GetInput(out MyInput data))
            {

                velocity = (new UnityEngine.Vector3(data.moveDirection.y, 0, -data.moveDirection.x) * Speed);
                if (data.fire)
                {
                    Debug.Log("Player fires: " + name);
                    bulletHandler.RPC_FireBullet();
                    if (TotalFired < TotalBullets)
                        TotalFired++;

                }

                mousePosition = data.mousePosition;
                controller.Move(velocity * NetworkManager.Instance.Runner.DeltaTime);
            }
        }

    }



    private void Update()
    {
        // Convert the mouse position to world coordinates

        Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, cam.transform.position.y - 10));

        // Get the direction from the object to the mouse position, but only on the y-axis
        Vector3 direction = worldMousePosition - transform.position;
        direction.y = 0;

        // Rotate the object to face the mouse position
        RotateObject.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public Player(ISpecialStrategy specialStrategy, int health, int score, bool canMove) : base(specialStrategy, health, score, canMove)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    public void CancelMove(InputAction.CallbackContext callbackContext)
    {
        velocity = new Vector3(0, 0, 0);
    }

    bool canFire = true;
    public void OnInput(NetworkRunner runner, NetworkInput netInput)
    {

        if (InputManager.Instance != null)
        {
            var myInput = new MyInput(); //create new Network Input struct

            //Vector2 moveVector = InputManager.Instance.input.Player.Move.ReadValue<Vector2>();

            //capture and assign
            if (InputManager.Instance.input.Player.Move.IsPressed())
                myInput.moveDirection = InputManager.Instance.input.Player.Move.ReadValue<Vector2>();
            else myInput.moveDirection = new Vector2(0, 0);


            myInput.mousePosition = InputManager.Instance.input.Player.Look.ReadValue<Vector2>();

            if (InputManager.Instance.input.Player.Fire.ReadValue<float>() == 1 && canFire)
            {
                canFire = false;
                myInput.fire = true;

            }
            else myInput.fire = false;


            if (!InputManager.Instance.input.Player.Fire.IsPressed()) canFire = true;

            //now set it across the network
            netInput.Set(myInput);
        }
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

    public static explicit operator NetworkObject(Player v)
    {
        throw new NotImplementedException();
    }
}
