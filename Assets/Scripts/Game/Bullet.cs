using Fusion;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour, IDeactivate
{

    [SerializeField]
    bool _isActive = false;

    [SerializeField] private string _id;
    [SerializeField] private bool _isIDDependant = true;

    PhotonView PV;

    public bool IsIDDependant
    {
        get { return _isIDDependant; }
        set { _isIDDependant = value; }
    }

    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public BulletHandler bulletHandler;

    [SerializeField] private float TTL;
    public Rigidbody rb;

    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
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
        gameObject.SetActive(true);
        StartCoroutine(DestroyAfterLifetime());

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ReportBullets()
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

            EventManager.TriggerEvent("BulletUpdate", new Dictionary<string, object> { { "player", "1" }, { "slider", slider } });
        }
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_DisableObject(Bullet bullet)
    {
        //if (bullet == this)
        //{
        bullet.gameObject.SetActive(false);
        bullet.RPC_ReportBullets();
        //}

    }

    private void Start()
    {
        if (NetworkManager.Instance.Runner.IsClient)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void DeActivate(string id = null)
    {
        IsActive = false;
        rb.velocity = Vector3.zero;

        gameObject.SetActive(false);



        if (NetworkManager.Instance.Runner.IsServer)
            Rpc_DisableObject(this);

        //PV.RPC("DisableObject", RpcTarget.All, 0);

        RPC_ReportBullets();
    }
}
