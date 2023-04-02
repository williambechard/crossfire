using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;

    public List<AudioClip> musicClips = new();
    public List<AudioClip> sfxClips = new();
    public List<AudioClip> ambientClips = new();



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        //Export any changes to the 3 lists to their text files
        //    this allows for a dropdown property in other objects
        //    to easily select a clip
        ExportClipsToText("musicNames", musicClips);
        ExportClipsToText("ambientNames", ambientClips);
        ExportClipsToText("sfxNames", sfxClips);
    }

    void ExportClipsToText(string fileName, List<AudioClip> clips)
    {
        // Write to disk
        StreamWriter writer = new StreamWriter("Assets/Resources/" + fileName + ".txt", false);
        foreach (AudioClip clip in clips) writer.WriteLine(clip.name);
        writer.Close();
    }

    //Normally scene manager, scene load will launch audio ready, however if
    //   its the first scene being played then it needs to be kicked off
    //   through start instead (as scene manager would not have run yet)
    void Start() => FindFirstObjectByType<AudioSetup>()?.AudioReady();

    AudioClip getClip(List<AudioClip> clips, string name)
    {
        AudioClip clip = null;
        for (int i = 0; i < clips.Count; i++)
        {
            if (clips[i].name == name)
            {
                clip = clips[i];
            }
        }

        return clip;
    }

    public void PlayMusic(string name)
    {
        AudioClip clip = getClip(musicClips, name);
        Debug.Log("Clip " + clip);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
            Debug.Log("Playing " + name);
        }
        else Debug.Log("PlayMusic failed! " + name + " is not found in the musicClipsDict");
    }

    public void PlaySFXOneShot(string name)
    {
        Debug.Log("Attempting to play " + name);
        AudioClip clip = getClip(sfxClips, name);
        if (clip != null)
        {
            Debug.Log("now playing " + name);
            sfxSource.clip = clip;
            sfxSource.PlayOneShot(sfxSource.clip);
        }
    }

    public void PlaySFX(string name, bool isLooping)
    {
        AudioClip clip = getClip(sfxClips, name);
        if (clip != null)
        {
            sfxSource.clip = clip;
            if (isLooping) sfxSource.loop = true;
            else sfxSource.loop = false;
            sfxSource.Play();
        }
    }

    public void PlayAmbient(string name)
    {
        AudioClip clip = getClip(ambientClips, name);
        if (clip != null)
        {
            ambientSource.clip = clip;
            ambientSource.Play();
        }
    }
    public void PlaySFXRandomPitch(string name, float minPitch, float maxPitch)
    {
        AudioClip clip = getClip(sfxClips, name);
        if (clip != null)
        {
            sfxSource.clip = clip;
            sfxSource.pitch = Random.Range(minPitch, maxPitch);
            sfxSource.Play();
        }

    }
    public void StopAmbient() => ambientSource.Stop();

    public void StopMusic() => musicSource.Stop();

    public void StopSFX() => sfxSource.Stop();

    public void PauseMusic() => musicSource.Pause();

    public void PauseSFX() => sfxSource.Pause();

    public void UnPauseMusic() => musicSource.UnPause();

    public void UnPauseSFX() => sfxSource.UnPause();

    public void SetMusicVolume(float volume) => musicSource.volume = volume;

    public void SetSFXVolume(float volume) => sfxSource.volume = volume;


}
