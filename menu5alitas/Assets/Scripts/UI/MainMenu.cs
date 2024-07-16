using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource Music;
    [SerializeField] Image image;
    [SerializeField] AnimationCurve curve;

    [SerializeField] Image mythicalChicken;

    private void Start()
    {
        StartCoroutine(fadeIn());
    }

    public void Play()
    {
        StartCoroutine(fadeOut());
    }

    IEnumerator fadeIn()
    {
        image.color = new Color(0, 0, 0, 1);

        mythicalChicken.gameObject.SetActive(true);


        Material mat = mythicalChicken.material;
        mat.SetFloat("_Phase", 0);

        for (float t = 0; t < 4f; t += Time.deltaTime)
        {
            mythicalChicken.color = new Color(1, 1, 1, Mathf.Clamp01(t * 2));
            mythicalChicken.transform.localScale = (1 + 0.2f * curve.Evaluate(t / 4)) * Vector3.one * 0.2f;
            yield return null;
        }
        yield return new WaitForSeconds(0.6f);

        mythicalChicken.transform.localScale = Vector3.one * 0.2f;
        for (float t = 0; t < 0.2f; t += Time.deltaTime)
        {
            mat.SetFloat("_Phase", t * 5);
            yield return null;
        }
        mat.SetFloat("_Phase", 1);

        //mythicalChicken2.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(2f);

        mythicalChicken.gameObject.SetActive(false);


        float time = 0.5f;
        Music.Play();
        for(float t= 0; t<time; t+=Time.deltaTime)
        {
            image.color = new Color(0, 0, 0, 1-curve.Evaluate(t / time));
            Music.volume = curve.Evaluate(t / time);
            yield return null;
        }
        Music.volume = 1;
    }

    IEnumerator fadeOut()
    {
        float time = 0.5f;
        for (float t = time; t >= 0; t -= Time.deltaTime)
        {
            image.color = new Color(0, 0, 0, 1-curve.Evaluate(t / time));
            Music.volume = curve.Evaluate(t / time);
            yield return null;
        }
        image.color = new Color(0, 0, 0, 1);
        Music.volume = 0;
        yield return null;
        SceneManager.LoadScene(1);
    }
}
