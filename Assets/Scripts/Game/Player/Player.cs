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
        set
        {
            score = value; 
            
            if (EventManager.instance != null)
                EventManager.TriggerEvent("UpdateScore", new Dictionary<string, object> {{"player", id}, {"score", value}});
        }
    }

    public bool CanMove
    {
        get { return _canMove; } 
        set { _canMove = value; }
    }

    public void Attack(Dictionary<string, object> message)
    {
        if (CanMove)
        {
            OnAttack?.Invoke();

            bulletHandler.FireBullet(transform.forward);
        }
    }

    public void Handle_PlayerMove(Dictionary<string, object> message)
    {
        if (CanMove)
        {
            Vector2 moveVector = (Vector2)message["value"];
            // do something for player move

            Move(new Vector2(moveVector.y, -moveVector.x));
        }
    }
  
    IEnumerator WaitForEventManager()
    {
        while(EventManager.instance == null)
        {
            yield return null;
        }
        EventManager.StartListening("PlayerMove", Handle_PlayerMove);
        EventManager.StartListening("PlayerStop", Handle_PlayerMove);
        EventManager.StartListening("PlayerLook", Handle_PlayerLook);
        EventManager.StartListening("PlayerFire", Attack);
    }

    void Handle_PlayerLook (Dictionary<string, object> message)
    {
        mousePosition = (Vector2) message["value"];
        Vector2 playerPosition = cam.WorldToScreenPoint(transform.position);
    }
    
    private void OnEnable()
    {
        StartCoroutine(WaitForEventManager());
        cam = Camera.main;
        bulletHandler.player = this;
        bulletHandler.FillQuiver(10);
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
        velocity = (new UnityEngine.Vector3(moveVector.x, RigidBody.velocity.y, moveVector.y) * Speed);
    }

    private void FixedUpdate()
    {
        RigidBody.AddForce(velocity, ForceMode.VelocityChange);
    }

    private void Update() {
        // Convert the mouse position to world coordinates
        var position = transform.position;
        Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, cam.transform.position.y - position.y));

        // Get the direction from the object to the mouse position, but only on the y-axis
        Vector3 direction = worldMousePosition - position;
        direction.y = 0;

        // Rotate the object to face the mouse position
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public Player(ISpecialStrategy specialStrategy, int health, int score, bool canMove) : base(specialStrategy, health, score, canMove)
    {
        
    }
}
