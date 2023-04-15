using System.Collections;
using UnityEngine;

public class SpawnTarget : MonoBehaviour
{
    public GameObject SpawnObject;
    public string targetId;

    public void Start()
    {
        Debug.Log("Start for SpawnTarget");
        //StartCoroutine(WaitForGameManager());
        Invoke("AssignToGameManager", 3f);
        /*
       */
    }


    public void AssignToGameManager()
    {
        if (targetId == "1")
        {
            Debug.Log("Assigned Target 1 to GameManager");
            GameManager.instance.SpawnTarget1 = this;
        }
        else
        {
            Debug.Log("Assigned Target 2 to GameManager");
            GameManager.instance.SpawnTarget2 = this;
        }
    }

    IEnumerator WaitForGameManager()
    {
        Debug.Log("WaitForGameManager");
        while (GameManager.instance == null)
        {
            Debug.Log("Instance = " + GameManager.instance == null);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("GameManager Found");
        if (targetId == "1")
            GameManager.instance.SpawnTarget1 = this;
        else
            GameManager.instance.SpawnTarget2 = this;
    }

    public void WaitToSpawn()
    {
        if (SpawnObject == null) Debug.Log("No object to spawn");

        NetworkManager.Instance.Runner.Spawn(SpawnObject, transform.position, Quaternion.identity, null);
    }

    public void Spawn()
    {
        Invoke("WaitToSpawn", 1f);

        //spawnedObject.GetComponent<Target>().Id = targetId;

    }


}
