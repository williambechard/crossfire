using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDeactivate
{
    
    [SerializeField]
    bool _isActive = false;

    [SerializeField] private string _id;
    [SerializeField] private bool _isIDDependant = true;
    
    public bool IsIDDependant
    {
        get { return _isIDDependant;}
        set { _isIDDependant = value; }
    }

    public string Id
    {
        get { return _id;}
        set { _id = value; }
    }

 

    public BulletHandler bulletHandler;
    
    [SerializeField] private float TTL;
    public Rigidbody rb;

    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value;}
    }

    IEnumerator DestroyAfterLifetime() {
        yield return new WaitForSeconds(TTL);
        DeActivate();
    }

    public void ActivateBullet()
    {
        IsActive = true;
        gameObject.SetActive(true);
        StartCoroutine(DestroyAfterLifetime());
 
    }

    public void ReportBullets()
    {
        List<Bullet> activeBullets = new List<Bullet>();

        if (bulletHandler != null)
        {
            foreach (Bullet bullet in bulletHandler.quiver)
            {
                if (bullet.IsActive)
                    activeBullets.Add(bullet);
            }

            //let ui know
            float slider = (float)activeBullets.Count / (float)bulletHandler.numberOfBullets;
            
            EventManager.TriggerEvent("BulletUpdate", new Dictionary<string, object> { { "player", "1"}, {"slider", slider}  });
        }
    }



    public void DeActivate(string id = null)
    {
        IsActive = false;
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
        ReportBullets();
    }
}
