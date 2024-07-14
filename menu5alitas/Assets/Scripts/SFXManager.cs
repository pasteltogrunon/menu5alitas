using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    static SFXManager Instance;

    [SerializeField] GameObject audioSourcePrefab;

    [SerializeField] AudioMixerGroup normal;

    static List<GameObject> audioSourcePool = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }
    


    public static void PlaySound(AudioClip clip, AudioMixerGroup mixerGroup)
    {
        AudioSource source = PoolAudioSource();

        source.PlayOneShot(clip);

        source.outputAudioMixerGroup = mixerGroup;

        Instance.StartCoroutine(Instance.returnSourceToPool(clip.length + 1, source));
    }

    public static void PlaySound(AudioClip clip)
    {
        PlaySound(clip, Instance.normal);
    }

    public static void PlayRandomSoundFromArray(AudioClip[] clips)
    {
        PlaySound(clips[Random.Range(0, clips.Length)]);
    }

    public static void PlayRandomSoundFromArray(AudioClip[] clips, AudioMixerGroup mixerGroup)
    {
        PlaySound(clips[Random.Range(0, clips.Length)], mixerGroup);
    }

    static AudioSource PoolAudioSource()
    {
        if(audioSourcePool.Count > 0)
        {
            GameObject sourceObject = audioSourcePool[0];
            audioSourcePool.RemoveAt(0);
            sourceObject.SetActive(true);

            return sourceObject.GetComponent<AudioSource>();
        }
        return Instantiate(Instance.audioSourcePrefab, Instance.transform).GetComponent<AudioSource>();
    }

    IEnumerator returnSourceToPool(float delay, AudioSource source)
    {
        yield return new WaitForSeconds(delay);
        source.gameObject.SetActive(false);
        audioSourcePool.Add(source.gameObject);
    }
}
