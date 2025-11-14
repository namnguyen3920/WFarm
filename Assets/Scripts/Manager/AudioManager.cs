using System.Collections;
using UnityEngine;

public class AudioManager : Singleton_Mono_Method<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clip")]    
    public AudioClip[] buySFX;
    public AudioClip itemLickSFX;
    public AudioClip harvestSFX;

    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Start()
    {
        PlayGamePlayBG();
    }
    public void PlayBuyingSound(Vector3 position)
    {
        PlayRandomAudio(buySFX, position, sfxVolume);
    }
    public void PlayItemClickSound(Vector3 position)
    {
        PlayAudio(itemLickSFX, position, sfxVolume);
    }
    public void PlayHarvestSound(Vector3 position)
    {
        PlayAudio(harvestSFX, position, sfxVolume);
    }
    public void PlayGamePlayBG()
    {
        musicSource.Play();
    }
    private void PlayAudio(AudioClip clip, Vector3 position, float volume)
    {
        AudioSource audioSource = Instantiate(sfxSource, position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
    private void PlayRandomAudio(AudioClip[] clip, Vector3 position, float volume)
    {
        int rand = Random.Range(0, clip.Length);
        AudioSource audioSource = Instantiate(sfxSource, position, Quaternion.identity);
        audioSource.clip = clip[rand];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}