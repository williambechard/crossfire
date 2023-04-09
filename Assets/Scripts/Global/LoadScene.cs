using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public string[] scenes;
#if UNITY_EDITOR
    private static string[] ReadNames()
    {
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                temp.Add(name);
            }
        }
        return temp.ToArray();
    }
    [UnityEditor.MenuItem("CONTEXT/ReadSceneNames/Update Scene Names")]
    private static void UpdateNames(UnityEditor.MenuCommand command)
    {
        LoadScene context = (LoadScene)command.context;
        context.scenes = ReadNames();
    }

    private void Reset()
    {
        scenes = ReadNames();
    }
#endif


    //Enables a drop down in Unity Editor for us to select the
    //  clip names from a list
    public static List<string> allSceneNames = new();
#if UNITY_EDITOR
    [ListToDropDown(typeof(LoadScene), "allSceneNames")]
#endif
    public string sceneToLoad;

#if UNITY_EDITOR
    private void OnValidate()
    {
        scenes = ReadNames();
        foreach (string name in scenes) allSceneNames.Add(name);
    }
#endif
    //public helper functions. These are what the Menu/UI Buttons will call when clicked to load a new scene
    public void loadScene() => LevelManager.Instance.LoadScene(sceneToLoad);

    public void loadNetworkScene()=> LevelManager.Instance.LoadNetworkScene(sceneToLoad);
    
    //public helper functions. These are what the Menu/UI Buttons will call when clicked to load a new scene
    public void loadSceneAdditive() => LevelManager.Instance.LoadSceneAdditive(sceneToLoad);

}
