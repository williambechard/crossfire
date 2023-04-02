using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : Entity, IMovable, IAttack, IDamageable, IPlayerControlled
{
    public event Action OnMove;
    public event Action OnAttack;
    public event Action OnDamaged;
    public event Action OnDestroyed;
    
    public void Damage(int amount)
    {
        throw new NotImplementedException();
    }

    public GameObject bulletPrefab;

    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector2 mousePosition;
    [SerializeField]
    private int score;
    private bool _canMove;
    [SerializeField]
    private int _health;
    [SerializeField]
    private float _speed;
    public float Speed
    {
        get { return _speed;}
        set { _speed = value; }
    }
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
        set { score = value; }
    }

    public bool CanMove
    {
        get { return _canMove; } 
        set { _canMove = value; }
    }

    public void Attack(Dictionary<string, object> message)
    {
         OnAttack?.Invoke();

         GameObject bullet = Instantiate(bulletPrefab);
        
         bullet.transform.position = this.transform.position + (this.transform.forward * 2);
         //FIRE
         Rigidbody rb = bullet.GetComponent<Rigidbody>();
         rb.AddForce(transform.forward* 12f, ForceMode.Impulse);
    }

    public void Handle_PlayerMove(Dictionary<string, object> message)
    {
        Vector2 moveVector = (Vector2) message["value"];
        // do something for player move
        Debug.Log("message" + (Vector2) message["value"] + " " + (String)message["playerId"]);
        Move(new Vector2(moveVector.y, -moveVector.x));
    }
    void SetupListener()
    {
        if (EventManager.instance != null)
        {
            EventManager.StartListening("PlayerMove", Handle_PlayerMove);
            EventManager.StartListening("PlayerStop", Handle_PlayerMove);
            EventManager.StartListening("PlayerLook", Handle_PlayerLook);
            EventManager.StartListening("PlayerFire", Attack);
        }else Debug.Log("event manager is null");
    }

    void Handle_PlayerLook (Dictionary<string, object> message)
    {
        
        mousePosition = (Vector2) message["value"];
        Vector2 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        
       
        Debug.Log("look vector " + mousePosition);
        // Convert the mouse position to world coordinates
        //Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x,   mousePosition.y, -38.4f));

        // Get the direction from the object to the mouse position
       // lookVector = worldMousePosition - transform.position;
    }
    
    private void OnEnable()
    {
        Invoke("SetupListener", .1f);
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
    }
    public void Move(Vector2 moveVector)
    {
        velocity = (new UnityEngine.Vector3(moveVector.x, 0, moveVector.y) * Speed);
    }

    private void FixedUpdate()
    {
        RigidBody.AddForce(velocity, ForceMode.VelocityChange);
    }

    private void Update() {
       

        // Convert the mouse position to world coordinates
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y - transform.position.y));

        // Get the direction from the object to the mouse position, but only on the y-axis
        Vector3 direction = worldMousePosition - transform.position;
        direction.y = 0;

        // Rotate the object to face the mouse position
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public Player(ISpecialStrategy specialStrategy, int health, int score, bool canMove) : base(specialStrategy, health, score, canMove)
    {
    }
}
