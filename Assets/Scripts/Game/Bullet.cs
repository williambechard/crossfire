using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour, IDeactivate
{


    [SerializeField] private bool _isIDDependant = true;

    public bool IsIDDependant
    {
        get { return _isIDDependant; }
        set { _isIDDependant = value; }
    }
    [SerializeField]
    [Networked]
    public string Id { get; set; }


    [Networked]
    public BulletHandler bulletHandler { get; set; }

    [SerializeField] private float TTL;
    public Rigidbody rb;

    [SerializeField]
    [Networked]
    public bool IsActive { get; set; }

    // NetworkId is backed by one int value.
    // A zero value (default) represents null.
    [Networked] public NetworkId SyncedNetworkObjectId { get; set; }


    private void SetObject(NetworkObject netObj)
    {
        SyncedNetworkObjectId = netObj != null ? netObj.Id : default;
    }

    bool TryResolve(out NetworkObject no)
    {
        // A default (0) value indicates an explicit null value.
        // Return true to represent successfully resolving this to null.
        if (SyncedNetworkObjectId == default)
        {
            no = null;
            return true;
        }

        // Find the object using the non-null id value.
        // Return true if the lookup was successful.
        // Return false and null if the NetworkObject could not be found on the local Runner.
        bool found = NetworkManager.Instance.Runner.TryFindObject(SyncedNetworkObjectId, out var obj);
        no = obj;
        return found;
    }

    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(TTL);

        DeActivate();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ActivateBullet()
    {
        IsActive = true;
        //gameObject.SetActive(true);
        //transform.position = bulletHandler.transform.position;
        StartCoroutine(DestroyAfterLifetime());
        StartCoroutine(onTick());

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ReportBullets()
    {
        List<Bullet> activeBullets = new List<Bullet>();

        if (bulletHandler != null)
        {
            foreach (Bullet bullet in FindObjectsOfType<Bullet>())
            {
                if (bullet.IsActive && bullet.Id == this.Id)
                    activeBullets.Add(bullet);
            }

            //let ui know
            float slider = (float)activeBullets.Count / (float)bulletHandler.numberOfBullets;
            Debug.Log("slider = " + slider + " activeBullets.Count / bulletHandler.nunberofBullets " + activeBullets.Count + " /  " + bulletHandler.numberOfBullets);
            EventManager.TriggerEvent("BulletUpdate", new Dictionary<string, object> { { "player", Id }, { "slider", slider } });
        }
    }


    IEnumerator onTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            RPC_ReportBullets();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_DisableObject(Bullet bullet)
    {
        //if (bullet == this)
        //{
        bullet.transform.position = new Vector3(1000, 1000, 1000);
        bullet.RPC_ReportBullets();
        //}

    }

    private void Start()
    {
        /*
            if (NetworkManager.Instance.Runner.IsClient)
            {
                Bullet[] allBullets = FindObjectsOfType<Bullet>(true);
                Player[] allPlayers = FindObjectsOfType<Player>(true);
                int nBullets = 0;
                foreach (Bullet bullet in allBullets)
                {
                    nBullets++;

                    if (nBullets <= 10 && bullet.Id == null)
                    {
                        //assign to first player
                        this.gameObject.SetActive(false);
                        Id = "1";
                        foreach (Player p in allPlayers)
                        {
                            if (p.id == "1")
                            {
                                bulletHandler = p.bulletHandler;
                                break;
                            }
                        }

                    }
                    else if (nBullets > 10 && bullet.Id == null)
                    {
                        //asign to second player
                        this.gameObject.SetActive(false);
                        Id = "2";
                        foreach (Player p in allPlayers)
                        {
                            if (p.id == "2")
                            {
                                bulletHandler = p.bulletHandler;
                                break;
                            }
                        }
                    }
                }
                this.gameObject.SetActive(false);
                Id = "2";
            }
            */
    }

    public void DeActivate(string id = null)
    {
        IsActive = false;
        rb.velocity = Vector3.zero;

        //gameObject.SetActive(false);
        transform.position = new Vector3(1000, 1000, 1000);


        if (NetworkManager.Instance.Runner.IsServer)
            Rpc_DisableObject(this);

        //PV.RPC("DisableObject", RpcTarget.All, 0);

        RPC_ReportBullets();
    }
}
