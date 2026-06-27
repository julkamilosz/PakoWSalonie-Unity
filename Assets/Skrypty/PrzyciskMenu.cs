using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PrzyciskMenu : MonoBehaviour
{
    public enum TypPrzycisku { StartGry, Kontynuuj, Opcje, Wyjscie, ZamknijOpcje }
    
    [Header("Ustawienia przycisku:")]
    public TypPrzycisku coRobiTenPrzycisk;
    public GameObject okienkoOpcji;

    [Header("Głos Pako dla tego przycisku:")]
    public AudioClip glosMalpki;

    private AudioSource glosnik;
    private bool czyZablokowany = false;

    private void Start()
    {
        glosnik = GetComponent<AudioSource>();

        if (coRobiTenPrzycisk == TypPrzycisku.Kontynuuj)
        {
            if (PlayerPrefs.HasKey("CzyJestZapis"))
            {
                ZmienAktywnoscPrzycisku(true);
                czyZablokowany = false;
            }
            else
            {
                ZmienAktywnoscPrzycisku(false);
                czyZablokowany = true;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (czyZablokowany) return;

        ZatrzymajInneDzwieki();

        if (glosnik != null && glosMalpki != null)
        {
            glosnik.clip = glosMalpki;
            glosnik.Play();
        }
    }

    private void ZatrzymajInneDzwieki()
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
    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }
        
        if (czyZablokowany) return;

        switch (coRobiTenPrzycisk)
        {
            case TypPrzycisku.StartGry:
                PlayerPrefs.SetInt("CzyJestZapis", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("SalonScene");
                break;

            case TypPrzycisku.Kontynuuj:
                Debug.Log("Wczytuję zapis i ładuję salon!");
                SceneManager.LoadScene("SalonScene"); 
                break;

            case TypPrzycisku.Opcje:
                if(okienkoOpcji != null) okienkoOpcji.SetActive(true);
                break;

            case TypPrzycisku.ZamknijOpcje:
                if(okienkoOpcji != null) okienkoOpcji.SetActive(false);
                break;

            case TypPrzycisku.Wyjscie:
                Debug.Log("Zamykam grę!");
                Application.Quit();
                break;
        }
    }

    private void ZmienAktywnoscPrzycisku(bool czyAktywny)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Collider2D collider = GetComponent<Collider2D>();

        if (czyAktywny)
        {
            if (spriteRenderer != null) spriteRenderer.color = Color.white;
            if (collider != null) collider.enabled = true;
        }
        else
        {
            if (spriteRenderer != null) spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
            if (collider != null) collider.enabled = false;
        }
    }
}