using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PersonalAudioClip
{
    public string clipName;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    private float effectSize = 1;
    private float musicSize = 1;
    public static SoundManager instance = null;
    [Space]
    [SerializeField] List<PersonalAudioClip> PersonalAudioClips;
    List<AudioSource> playedEffects = new List<AudioSource>();
    [SerializeField] AudioSource playedMusic = new AudioSource();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    public void PlayMusic(string clipName)
    {
        playedMusic.clip = PersonalAudioClips.Find(clip => clip.clipName == clipName).clip;
        if (playedMusic.clip != null)
            playedMusic.Play();
    }
    public void PlayEffect(string clipName)
    {
        GameObject soundObject = new GameObject();
        AudioSource soundSource = soundObject.AddComponent<AudioSource>();
        soundSource.loop = false;
        soundSource.playOnAwake = true;
        soundSource.volume = 0.5f;

        soundObject.name = "Sound" + clipName;
        soundObject.transform.SetParent(transform);

        playedEffects.Add(soundSource);

        soundSource.clip = PersonalAudioClips.Find(clip => clip.clipName == clipName).clip;
        if (soundSource.clip != null)
            soundSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playedEffects.Count; i++)
        {
            if (playedEffects[i].volume != effectSize)
            {
                playedEffects[i].volume = effectSize;
            }

            if (playedEffects[i].isPlaying == false)
            {
                Destroy(playedEffects[i].gameObject);
                playedEffects.RemoveAt(i);
            }
        }
    }
}
