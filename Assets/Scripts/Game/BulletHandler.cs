using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : NetworkBehaviour
{
    public int numberOfBullets;
    public Player player;
    [SerializeField] private float fireForce;

    public GameObject bulletPrefab;
    public GameObject bulletQuiverParent;

    public List<Bullet> quiver = new List<Bullet>();


    public void FillQuiver(int num, PlayerRef player)
    {
        //if (NetworkManager.Instance.Runner.IsServer)
        //{
        Debug.Log("Fill Quiver for " + player);
        numberOfBullets = num;
        if (quiver.Count < numberOfBullets)
        {
            int total = numberOfBullets - quiver.Count;

            // //if (NetworkManager.Instance.Runner.IsServer)
            //{
            for (int i = 0; i < total; i++)
            {
                GameObject bullet = NetworkManager.Instance.Runner.Spawn(bulletPrefab).gameObject;
                bullet.GetComponent<Bullet>().DeActivate();
                quiver.Add(bullet.GetComponent<Bullet>());
            }
            //}

        }


        //}

    }

    public void ReportBullets(int activeBullets)
    {
        //let ui know
        if (NetworkManager.Instance.Runner.IsServer)
        {
            float slider = (float)activeBullets / (float)numberOfBullets;
            EventManager.TriggerEvent("BulletUpdate",
                new Dictionary<string, object> { { "player", player.id }, { "slider", slider } });
        }

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_FireBullet()
    {

        Vector3 forwardVector = bulletQuiverParent.transform.forward;


        List<Bullet> activeBullets = new List<Bullet>();
        List<Bullet> inactiveBullets = new List<Bullet>();

        foreach (Bullet bullet in quiver)
        {
            if (bullet.IsActive)
                activeBullets.Add(bullet);
            else inactiveBullets.Add(bullet);
        }

        //only fire if we are under our number of bullets threshold
        if (activeBullets.Count < numberOfBullets && inactiveBullets.Count > 0)
        {
            Bullet bullet = inactiveBullets[0];
            bullet.RPC_ActivateBullet();
            bullet.bulletHandler = this;
            bullet.Id = player.id;
            bullet.transform.position = transform.parent.position;
            bullet.rb.AddForce(forwardVector * fireForce, ForceMode.Impulse);

            ReportBullets(activeBullets.Count + 1);
        }
        else
        {
            //out of bullets effect!
            EventManager.TriggerEvent("OutOfBullets",
                new Dictionary<string, object> { { "player", player.id } });
        }

    }
}
