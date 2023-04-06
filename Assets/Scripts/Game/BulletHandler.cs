using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    
    public int numberOfBullets;
    public Player player;
    [SerializeField] private float fireForce;
    
    public GameObject bulletPrefab;
    public GameObject bulletQuiverParent;
    
    public List<Bullet> quiver = new List<Bullet>();
    
 
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
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.GetComponent<Bullet>().DeActivate();
                quiver.Add(bullet.GetComponent<Bullet>());
            }
        }
    }

    public void ReportBullets(int activeBullets)
    {
        //let ui know
        
            float slider = (float)activeBullets / (float)numberOfBullets;
            EventManager.TriggerEvent("BulletUpdate",
                new Dictionary<string, object> { { "player", player.id}, {"slider", slider}  });
         
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

        //only fire if we are under our number of bullets threshold
        if (activeBullets.Count < numberOfBullets)
        {
            Bullet bullet = inactiveBullets[0];
            bullet.ActivateBullet();
            bullet.bulletHandler = this;
            bullet.Id = player.id;
            bullet.transform.position = transform.parent.position;
            //FIRE
            bullet.rb.AddForce(forwardVector * fireForce, ForceMode.Impulse);
            
            ReportBullets(activeBullets.Count+1);
        }
        else
        {
            //out of bullets effect!
            EventManager.TriggerEvent("OutOfBullets",
                new Dictionary<string, object> { { "player", player.id}  });
        }
    }
}
