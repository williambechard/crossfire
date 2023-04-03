using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    [SerializeField]
    bool _isActive = false;

    [SerializeField] private float TTL;
    public Rigidbody rb;

    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value;}
    }
    
    
    IEnumerator DestroyAfterLifetime() {
        yield return new WaitForSeconds(TTL);
        DeActivateBullet();
    }

    public void ActivateBullet()
    {
        IsActive = true;
        gameObject.SetActive(true);
        StartCoroutine(DestroyAfterLifetime());
    }
    
    public void DeActivateBullet()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }
}
