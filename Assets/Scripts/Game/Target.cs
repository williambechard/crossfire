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
            /*
                //let spawner know which target to spawn
                SpawnTarget[] allSpawners = FindObjectsOfType<SpawnTarget>();
                Debug.Log("SCOOOOORE " + id);

                if (GameManager.instance.P1 == null)
                {
                    Player[] allPlayers = FindObjectsOfType<Player>();
                    foreach (Player p in allPlayers)
                    {
                        if (p.name.Contains("ALT"))
                        {
                            GameManager.instance.P2 = p;
                        }
                        else
                        {
                            GameManager.instance.P1 = p;
                        }
                    }
                }*/
            Debug.Log("provided id = " + id);
            //score
            switch (id) //add to score
            {
                case "1":
                    GameManager.instance.P1.Score++;
                    break;
                case "2":
                    GameManager.instance.P2.Score++;
                    break;
            }
            switch (Id) //spawn new object
            {
                case "1":
                    Debug.Log("delaySpawn target 1");
                    GameManager.instance.SpawnTarget1.Spawn();
                    break;
                case "2":
                    Debug.Log("delaySpawn target 2");
                    GameManager.instance.SpawnTarget2.Spawn();
                    break;
            }
            transform.position = new Vector3(1000, 1000, 1000);

            isActive = false;

            //destroy this gameobject
            NetworkManager.Instance.Runner.Despawn(this.GetComponent<NetworkObject>());
            //Destroy(this.gameObject);
        }
    }


}

