using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] AnimationCurve curve;

    [SerializeField] AudioSource loopMusic;
    [SerializeField] AudioSource loopAmbient;
    [SerializeField] AudioSource winMusic;
    [SerializeField] AudioSource defeatMusic;

    List<AudioSource> fadeIns = new List<AudioSource>();
    List<AudioSource> fadeOuts = new List<AudioSource>();
    List<AudioSource> fadeAux = new List<AudioSource>();

    float activityCooldown = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        fadeIns.Add(loopAmbient);
        loopAmbient.Play();
        fadeIns.Add(loopMusic);
        loopMusic.Play();
        activityCooldown = 5f;
    }

    void Update()
    {
        if (activityCooldown <= 0) return;

        fadeAux = new List<AudioSource>(fadeIns);
        foreach(AudioSource fadeIn in fadeAux)
        {
            fadeIn.volume = Mathf.Clamp01(fadeIn.volume + 0.3f * Time.deltaTime);
            if (fadeIn.volume == 0)
            {
                fadeIns.Remove(fadeIn);
            }
        }

        fadeAux = new List<AudioSource>(fadeOuts);
        foreach (AudioSource fadeOut in fadeAux)
        {
            fadeOut.volume = Mathf.Clamp01(fadeOut.volume - 0.3f * Time.deltaTime);
            if(fadeOut.volume == 1)
            {
                fadeIns.Remove(fadeOut);
            }
        }
        activityCooldown -= Time.deltaTime;
    }

    public void fadeInFadeOut(AudioSource sourceOut, AudioSource sourceIn)
    {
        fadeIns.Add(sourceIn);
        sourceIn.Play();
        fadeOuts.Add(sourceOut);
        sourceOut.Play();
        activityCooldown = 5f;
    }

    public void win()
    {
        fadeInFadeOut(loopMusic, winMusic);
    }

    public void defeat()
    {
        fadeInFadeOut(loopMusic, defeatMusic);
    }
}
