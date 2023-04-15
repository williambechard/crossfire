using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HideLoading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void Handle_LoadingDone(Dictionary<string, object> message)
    {
        Debug.Log("Loading Done ");
        SceneManager.UnloadSceneAsync("Loading");
    }

    IEnumerator WaitForEventManager()
    {
        while (!EventManager.instance)
        {
            yield return null;
        }
        EventManager.StartListening("LoadingDone", Handle_LoadingDone);

    }

    private void OnDisable()
    {
        EventManager.StopListening("LoadingDone", Handle_LoadingDone);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
