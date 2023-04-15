using Fusion;
using System.Collections;
using UnityEngine;

public class SpawnNetworkObject : MonoBehaviour
{

    public NetworkObject networkObjectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForNetwork());
    }

    IEnumerator WaitForNetwork()
    {
        while (NetworkManager.Instance.Runner == null)
        {
            yield return null;
        }


        NetworkManager.Instance.Runner.Spawn(networkObjectPrefab);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
