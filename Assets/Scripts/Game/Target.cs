using Fusion;
using UnityEngine;

public class Target : MonoBehaviour, IDeactivate
{
    [SerializeField] private string _id;
    [SerializeField] private bool _isIDDependant = false;
    private bool isActive = true;
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
    public void DeActivate(string id = null)
    {
        if (isActive)
        {

            //let spawner know which target to spawn
            SpawnTarget[] allSpawners = FindObjectsOfType<SpawnTarget>();

            //score
            switch (id)
            {
                case "1":
                    GameManager.instance.P1.Score++;
                    break;
                case "2":
                    GameManager.instance.P2.Score++;
                    break;
            }

            switch (Id)
            {
                case "1":
                    GameManager.instance.SpawnTarget1.Spawn();
                    break;
                case "2":
                    GameManager.instance.SpawnTarget2.Spawn();
                    break;
            }

            isActive = false;

            //destroy this gameobject
            NetworkManager.Instance.Runner.Despawn(this.GetComponent<NetworkObject>());
            //Destroy(this.gameObject);
        }
    }
}

