using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public class AudioSetup : MonoBehaviour
{
    //Define our text files that we will pull clip names from
    TextAsset musicList = null;
    TextAsset ambientList = null;

    bool updateOnce = true;

    //Enables a drop down in Unity Editor for us to select the
    //  clip names from a list
    public static List<string> allMusicNames = new();
#if UNITY_EDITOR
    [ListToDropDown(typeof(AudioSetup), "allMusicNames")]
#endif
    public string BackgroundMusic;

    //Enables a drop down in Unity Editor for us to select the
    //  clip names from a list
    public static List<string> allAmbientNames = new();
#if UNITY_EDITOR
    [ListToDropDown(typeof(AudioSetup), "allAmbientNames")]
#endif
    public string AmbientSound;

    private void Start()
    {
        if (AudioManager.Instance != null) { AudioReady(); }
    }

    //Called when the scene is loaded and when a change is made to a property in the Inspector
    //   this allows us to catch and update any changes
    void updateSound(TextAsset file, string fileName, List<string> clipNames)
    {
        if (file == null) file = Resources.Load(fileName) as TextAsset;

        clipNames.Clear();

        clipNames.Add("None");
        clipNames.Add("Continue");

        string fs = file.text.Trim();

        string[] fLines = Regex.Split(fs, "\n");

        for (int i = 0; i < fLines.Length; i++) clipNames.Add(fLines[i].Trim());
    }

#if UNITY_EDITOR
    //Update our lists with potentially any new values
    //    as values shouldnt change, this probably could be called just 1x with a boolean
    private void OnValidate()
    {
        if (updateOnce)
        {
            updateSound(musicList, "musicNames", allMusicNames);
            updateSound(ambientList, "ambientNames", allAmbientNames);
            updateOnce = false;
        }
    }
#endif
    public void AudioReady()
    {

        if (BackgroundMusic == "None")
        {
            AudioManager.Instance.StopMusic();
        }
        else if (BackgroundMusic != "Continue" && BackgroundMusic != AudioManager.Instance.musicSource?.clip?.name)
        {
            Debug.Log("Background Music Name = " + BackgroundMusic);
            AudioManager.Instance.PlayMusic(BackgroundMusic);
        }

        if (AmbientSound == "None")
        {
            AudioManager.Instance.StopAmbient();
        }
        else if (AmbientSound != "Continue" && AmbientSound != AudioManager.Instance.ambientSource?.clip?.name)
        {
            Debug.Log("Ambient Sound Name = " + AmbientSound);
            AudioManager.Instance.PlayAmbient(AmbientSound);
        }

    }
}
