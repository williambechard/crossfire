using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public interface IMovable
{
    public bool CanMove{get; set;}
    public float Speed{get; set;}
    void Move(Vector2 direction);
    public event Action OnMove;
}

public interface IAttack
{
    public event Action OnAttack;
    public void Attack(Dictionary<string, object> message);
}

public interface IDeactivate
{
    public bool IsIDDependant{get; set;}
    public string Id { get; set; }
    public void DeActivate( string id = null);
}
public interface IDamageable
{
    public int Health{get; set;}
    
    public event Action OnDamaged;
    public event Action OnDestroyed;
    public void Damage(int amount);
}

public interface IPlayerControlled
{
    public void Handle_PlayerMove(Dictionary<string, object> message);
}

public class Entity : NetworkBehaviour
{
      private ISpecialStrategy _specialStrategy = null;
      public event Action OnSetup;
      public event Action OnSpecial;
      public Rigidbody RigidBody;

     

      public Entity(ISpecialStrategy specialStrategy, int health, int score, bool canMove)
      {
          OnSetup?.Invoke();
      }
  
      public void UseSpecial()
      {
          OnSpecial?.Invoke();
          _specialStrategy.UseSpecial();
      }
  
      public void SetSpecialStrategy(ISpecialStrategy specialStrategy)
      {
          this._specialStrategy = specialStrategy;
      }
    
}

public interface ISpecialStrategy
{
    void UseSpecial();
}

public class SpecialStrategyA: ISpecialStrategy
{
    public void UseSpecial()
    {
        // do something for special A
    }
}

