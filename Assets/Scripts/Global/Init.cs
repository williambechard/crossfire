using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{

    //Mandatory scene that must be loaded
    List<string> MandatoryScenesToLoad = new List<string> { "SceneLoader", "AudioManager" };

    //Extra scenes to load depending on the need
    public List<string> ExtraScenesToLoad = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {

        foreach (string sceneToLoad in MandatoryScenesToLoad)
        {
            if (!SceneManager.GetSceneByName(sceneToLoad).IsValid()) //If scene isnt loaded
                SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive); //then load it additive(ly)
        }


        //Loop through the ExtraScenesToLoad List
        foreach (string s in ExtraScenesToLoad)
        {
            if (!SceneManager.GetSceneByName(s).IsValid()) //If scene isnt loaded
                SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive); //then load it additive(ly)
        }

    }





}
