using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public SoundList[] soundEffectList;
    [SerializeField] public AudioClip[] musicList;
    [SerializeField] public AudioSource soundFXAudioSource;
    [SerializeField] public AudioSource musicAudioSource;
    [SerializeField] public AudioSource dayAmbience;
    [SerializeField] public AudioSource waterAmbience;
    [SerializeField] public AudioSource backgroundAmbience;
    private static AudioManager Instance;
    
    private void Awake(){
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Audio Manager in the scene");
            Destroy(gameObject);
        }
    } 

    public static AudioManager GetInstance(){
        return Instance;
    }
    public void PlayAudio(SoundType sound, float volume = 1){
        AudioClip[] clips = soundEffectList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        soundFXAudioSource.PlayOneShot(randomClip, volume);
        //soundFXAudioSource.PlayOneShot(soundEffectList[(int)sound], volume);
    }
    public void SetAmbience(bool isDay, SoundType background, bool isWater, float fadeDuration = 1f)
    {
        // Play day or night ambience with fade-in
        if (!isDay)
        {
            AudioClip[] clips = soundEffectList[(int)SoundType.NIGHT].Sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
            dayAmbience.clip = randomClip;
            dayAmbience.volume = 0f;  // Start at 0 volume for fade-in
            dayAmbience.Play();
            StartCoroutine(FadeInAmbience(dayAmbience, 0.3f, fadeDuration)); // Fade to 0.3 volume
        }
        else
        {
            AudioClip[] clips = soundEffectList[(int)SoundType.DAY].Sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
            dayAmbience.clip = randomClip;
            dayAmbience.volume = 0f;
            dayAmbience.Play();
            StartCoroutine(FadeInAmbience(dayAmbience, 0.3f, fadeDuration));
        }

        // Play water ambience with fade-in if required
        if (isWater)
        {
            AudioClip[] waterClips = soundEffectList[(int)SoundType.WATER].Sounds;
            AudioClip randomWaterClip = waterClips[UnityEngine.Random.Range(0, waterClips.Length)];
            waterAmbience.clip = randomWaterClip;
            waterAmbience.volume = 0f;
            waterAmbience.Play();
            StartCoroutine(FadeInAmbience(waterAmbience, 0.4f, fadeDuration));
        }

        // Play background ambience with fade-in
        AudioClip[] backgroundClips = soundEffectList[(int)background].Sounds;
        AudioClip randomBGClip = backgroundClips[UnityEngine.Random.Range(0, backgroundClips.Length)];
        backgroundAmbience.clip = randomBGClip;
        backgroundAmbience.volume = 0f;
        backgroundAmbience.Play();
        StartCoroutine(FadeInAmbience(backgroundAmbience, 0.4f, fadeDuration));
    }
    public void SetAmbienceDay(bool isDay, float fadeDuration = 1f)
    {
        // Play day or night ambience with fade-in
        if (!isDay)
        {
            AudioClip[] clips = soundEffectList[(int)SoundType.NIGHT].Sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
            dayAmbience.clip = randomClip;
            dayAmbience.volume = 0f;  // Start at 0 volume for fade-in
            dayAmbience.Play();
            StartCoroutine(FadeInAmbience(dayAmbience, 0.3f, fadeDuration)); // Fade to 0.3 volume
        }
        else
        {
            AudioClip[] clips = soundEffectList[(int)SoundType.DAY].Sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
            dayAmbience.clip = randomClip;
            dayAmbience.volume = 0f;
            dayAmbience.Play();
            StartCoroutine(FadeInAmbience(dayAmbience, 0.3f, fadeDuration));
        }
    }

    // Coroutine for fading in ambience
    public IEnumerator FadeInAmbience(AudioSource ambienceSource, float targetVolume, float fadeDuration)
    {
        float elapsedTime = 0f;
        ambienceSource.Play();
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            ambienceSource.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }

        ambienceSource.volume = targetVolume;  // Ensure the final volume is set correctly
    }


    public void StopAmbience(float fadeDuration = 1f)
    {
        // Start fade-out for all ambience sounds
        StartCoroutine(FadeOutAmbience(dayAmbience, fadeDuration));
        StartCoroutine(FadeOutAmbience(waterAmbience, fadeDuration));
        StartCoroutine(FadeOutAmbience(backgroundAmbience, fadeDuration));
    }

    public IEnumerator FadeOutAmbience(AudioSource ambienceSource, float fadeDuration)
    {
        float startVolume = ambienceSource.volume;
        float elapsedTime = 0f;

        // Gradually reduce the volume to 0
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            ambienceSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Stop the audio source after fading out
        ambienceSource.Stop();
        ambienceSource.volume = startVolume;  // Reset the volume if needed for later reuse
    }

    public void PlayMusic(MusicType sound, float volume = 1, float fadeDuration = 1f)
    {
        AudioClip newMusic = musicList[(int)sound];

        if (musicAudioSource.isPlaying && musicAudioSource.clip == newMusic)
        {
            return; // If the new music is already playing, no need to do anything
        }

        StartCoroutine(FadeMusic(newMusic, volume, fadeDuration));
    }

    // Coroutine to handle fading out and fading in the music
    private IEnumerator FadeMusic(AudioClip newMusic, float targetVolume, float fadeDuration)
    {
        // Fade out the current music
        yield return StartCoroutine(FadeOut(fadeDuration));

        // Switch to the new music clip
        musicAudioSource.clip = newMusic;
        musicAudioSource.Play();

        // Fade in the new music
        yield return StartCoroutine(FadeIn(targetVolume, fadeDuration));
    }

    // Coroutine for fading out
    private IEnumerator FadeOut(float fadeDuration)
    {
        float startVolume = musicAudioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        musicAudioSource.volume = 0f;
    }

    // Coroutine for fading in
    private IEnumerator FadeIn(float targetVolume, float fadeDuration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }

        musicAudioSource.volume = targetVolume;
    }


    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMasterVolume()
    {
        audioMixer.SetFloat("masterVolume", GetMasterVolume());
    }

    public float GetMasterVolume()
    {
        float volume = 0f;
        if (audioMixer.GetFloat("masterVolume", out volume))
        {
            return volume;
        }
        else
        {
            Debug.LogError("Master volume not found!");
            return -1f; // Return a default value or handle the error as needed
        }
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
    }
    public void SetSoundFXVolume()
    {
        audioMixer.SetFloat("soundFXVolume", GetSoundFXVolume());
    }
    public float GetSoundFXVolume()
    {
        float volume = 0f;
        if (audioMixer.GetFloat("soundFXVolume", out volume))
        {
            return volume;
        }
        else
        {
            Debug.LogError("Master volume not found!");
            return -1f; // Return a default value or handle the error as needed
        }
    }
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }
    public void SetMusicVolume()
    {
        audioMixer.SetFloat("musicVolume", GetSoundMusicVolume());
    }
    public float GetSoundMusicVolume()
    {
        float volume = 0f;
        if (audioMixer.GetFloat("musicVolume", out volume))
        {
            return volume;
        }
        else
        {
            Debug.LogError("Master volume not found!");
            return -1f; // Return a default value or handle the error as needed
        }
    }
    #if UNITY_EDITOR
    private void OnEnable(){
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundEffectList, names.Length);
        for(int i = 0; i < soundEffectList.Length; i++)soundEffectList[i].name = names[i];
    }
    #endif
}