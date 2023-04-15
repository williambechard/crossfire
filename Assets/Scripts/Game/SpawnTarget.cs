using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTarget : MonoBehaviour
{
    public GameObject SpawnObject;
    public string targetId;
    public void Spawn()
    {
        GameObject spawnedObject =
            NetworkManager.Instance.Runner.Spawn(SpawnObject, transform.position, Quaternion.identity, null).gameObject; 
        //Instantiate(SpawnObject, transform.position, Quaternion.identity);
        spawnedObject.GetComponent<Target>().Id= targetId;
    }

     
}
