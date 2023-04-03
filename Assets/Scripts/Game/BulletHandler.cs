using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    [SerializeField]
    int numberOfBullets;

    [SerializeField] private float fireForce;
    
    public GameObject bulletPrefab;
    public GameObject bulletQuiverParent;
    
    [SerializeField] private List<Bullet> quiver = new List<Bullet>();
    
   
    //add bullets to quiver
    //  to remove bullets may be best done at fire time (a check
    //  against quiver.count and numberofBullets before firing)
    public void FillQuiver(int num)
    {
        numberOfBullets = num;
        if (quiver.Count < numberOfBullets)
        {
            int total = numberOfBullets - quiver.Count;
            Debug.Log("Filling quiver " + total);
            for (int i = 0; i < total; i++)
            {
                Debug.Log("loop " + i.ToString());
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.GetComponent<Bullet>().DeActivateBullet();
                quiver.Add(bullet.GetComponent<Bullet>());
            }
        }
    }
    
    //fire a bullet
    public void FireBullet(Vector3 forwardVector)
    {
        List<Bullet> activeBullets = new List<Bullet>();
        List<Bullet> inactiveBullets = new List<Bullet>();
        
        foreach (Bullet bullet in quiver)
        {
            if (bullet.IsActive)
                activeBullets.Add(bullet);
            else inactiveBullets.Add(bullet);
        }

        //only fire if we are under oour number of bullets threshold
        if (activeBullets.Count < numberOfBullets)
        {
            Bullet bullet = inactiveBullets[0];
            bullet.ActivateBullet();
            
            bullet.transform.position = transform.parent.position;
            //FIRE
            bullet.rb.AddForce(forwardVector* fireForce, ForceMode.Impulse);
        } 
    }
}
