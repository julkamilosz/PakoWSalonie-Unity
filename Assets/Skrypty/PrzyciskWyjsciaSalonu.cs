using UnityEngine;
using UnityEngine.SceneManagement;

public class PrzyciskWyjsciaSalonu : MonoBehaviour
{
    [Header("Głos Pako dla tego przycisku:")]
    public AudioClip glosMalpki;

    private AudioSource glosnik;

    private void Start()
    {
        glosnik = GetComponent<AudioSource>();
    }

    private void OnMouseEnter()
    {
        UciszInneGlosniki();

        if (glosnik != null && glosMalpki != null)
        {
            glosnik.clip = glosMalpki;
            glosnik.Play();
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown zadziałało! Wracam do menu!");
        SceneManager.LoadScene("MenuScene");
    }

    private void UciszInneGlosniki()
    {
        AudioSource[] wszystkieGlosniki = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource g in wszystkieGlosniki)
        {
            if (g != null && g.isPlaying)
            {
                g.Stop();
            }
        }
    }
}