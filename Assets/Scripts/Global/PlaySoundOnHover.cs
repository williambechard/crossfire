using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnHover : MonoBehaviour, IPointerEnterHandler
{
    bool updateOnce = true;

    //Define our text files that we will pull clip names from
    TextAsset sfxList = null;

    //Enables a drop down in Unity Editor for us to select the
    //  clip names from a list
    public static List<string> allSfxNames = new();
#if UNITY_EDITOR
    [ListToDropDown(typeof(PlaySoundOnHover), "allSfxNames")]
#endif
    public string SoundFXName;


    //Called when the scene is loaded and when a change is made to a property in the Inspector
    //   this allows us to catch and update any changes
    void updateSound(TextAsset file, string fileName, List<string> clipNames)
    {
        if (file == null) file = Resources.Load(fileName) as TextAsset;

        clipNames.Clear();

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
            updateSound(sfxList, "sfxNames", allSfxNames);
            updateOnce = false;
        }
    }
#endif
    public void OnPointerEnter(PointerEventData ped)
    {
        AudioManager.Instance.PlaySFXOneShot(SoundFXName);
    }

}