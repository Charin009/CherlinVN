using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource playerSFX;
    public AudioSource playerSong;
    [SerializeField]private List<AudioClip> audioSFXList;
    [SerializeField]private List<AudioClip> audioSongList;
    public float maxSongVolume;
    public float maxSFXVolume;
    private float minVolume = 0;
    private float fadeDuration = 5f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            maxSongVolume = PlayerPrefs.GetFloat("SongVolume");
            playerSong.volume = PlayerPrefs.GetFloat("SongVolume");
            maxSFXVolume = PlayerPrefs.GetFloat("SFXVolume");
            playerSFX.volume = PlayerPrefs.GetFloat("SongVolume");

        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void PlaySFX(string name)
    {
        AudioClip effect = FindAudio(name, audioSFXList);
        playerSFX.clip = effect;
        playerSFX.volume = maxSFXVolume;
        playerSFX.Play();
    } 

    public void PlaySong(string name)
    {
        AudioClip song = FindAudio(name, audioSongList);
        playerSong.clip = song;
        playerSong.volume = maxSongVolume;
        playerSong.Play();
        StartCoroutine(FaingInSound(playerSong, maxSongVolume));
    }

    public void ChangeSong(string name)
    {
        AudioClip song = FindAudio(name, audioSongList);
        StartCoroutine(TransitionSong(song));
    }

    private AudioClip FindAudio(string name, List<AudioClip> audioList)
    {
        AudioClip targetClip = null;
        foreach(AudioClip clip in audioList)
        {
            if(clip.name == name)
            {
                targetClip = clip;
                break;
            }
           
        }
        return targetClip;
    }
   
    IEnumerator FaingInSound(AudioSource source, float maxVolume)
    {
        float currentTime = 0;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            source.volume = Mathf.Lerp(minVolume, maxVolume, currentTime / fadeDuration);
            yield return null;
        }
        yield break;
    }

    IEnumerator TransitionSong(AudioClip newSong)
    {
        float currentTime = 0;
        float startVolume = playerSong.volume;
        while(currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            playerSong.volume = Mathf.Lerp(startVolume, minVolume, currentTime / fadeDuration);
            yield return null;
        }
        currentTime = 0;
        startVolume = minVolume;
        playerSong.clip = newSong;
        playerSong.Play();
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            playerSong.volume = Mathf.Lerp(startVolume, maxSongVolume, currentTime / fadeDuration);
            yield return null;
        }
        yield break;
    }

    
}
