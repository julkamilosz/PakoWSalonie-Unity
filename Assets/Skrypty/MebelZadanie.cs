using UnityEngine;

public class MebelZadanie : MonoBehaviour
{
    [Header("Nazwa zadania (do zapisu w pamięci):")]
    public string unikalnaNazwaZadania;

    [Header("Dźwięki Pako w SALONIE:")]
    public AudioClip dzwiekZaproszenia;
    public AudioClip dzwiekUkonczone;

    [Header("Dźwięki Pako w MINIGRZE (NOWOŚĆ):")]
    public AudioClip dzwiekZasadMinigry;
    public AudioClip dzwiekSukcesuMinigry;
    public AudioClip dzwiekZlejKolejnosciMinigry;

    [Header("Zarządzanie Ekranami (Przeciągnij z Hierarchy):")]
    public GameObject canvasSalon;
    public GameObject canvasMinigra;

    private AudioSource glosnik;

    private void Start()
    {
        glosnik = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ZarzadzaniePauza.Instance != null && ZarzadzaniePauza.Instance.CzyTrwaMinigra())
            {
                Debug.Log("Wciśnięto ESC - zamykam minigrę");
                ZamknijMinigre();
            }
        }
    }

    private void OnMouseEnter()
    {
        if (ZarzadzaniePauza.Instance != null && ZarzadzaniePauza.Instance.CzyTrwaMinigra()) return; 
        if (ZarzadzaniePauza.Instance != null && ZarzadzaniePauza.Instance.CzyGraJestZapauzowana()) return; 

        UciszWszystkieGlosnikiWSalonie();

        if (glosnik != null)
        {
            bool czyZrobione = PlayerPrefs.GetInt(unikalnaNazwaZadania, 0) == 1;

            if (czyZrobione)
            {
                if (dzwiekUkonczone != null)
                {
                    glosnik.clip = dzwiekUkonczone;
                    glosnik.Play();
                }
            }
            else
            {
                if (dzwiekZaproszenia != null)
                {
                    glosnik.clip = dzwiekZaproszenia;
                    glosnik.Play();
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (ZarzadzaniePauza.Instance != null && ZarzadzaniePauza.Instance.CzyGraJestZapauzowana()) return;
        if (ZarzadzaniePauza.Instance != null && ZarzadzaniePauza.Instance.CzyTrwaMinigra()) return;

        bool czyZrobione = PlayerPrefs.GetInt(unikalnaNazwaZadania, 0) == 1;

        if (!czyZrobione)
        {
            Debug.Log("Uruchamiam minigrę dla: " + unikalnaNazwaZadania);
            
            if (ZarzadzaniePauza.Instance != null) ZarzadzaniePauza.Instance.UstawStanMinigry(true);

            UciszWszystkieGlosnikiWSalonie();

            if (canvasSalon != null) canvasSalon.SetActive(false);
            if (canvasMinigra != null) canvasMinigra.SetActive(true);

            if (glosnik != null && dzwiekZasadMinigry != null)
            {
                glosnik.clip = dzwiekZasadMinigry;
                glosnik.Play();
                Debug.Log($"[Narrator] Odtwarzam zasady dla minigry: {unikalnaNazwaZadania}");
            }
        }
        else
        {
            Debug.Log("To zadanie jest już skończone, nie musimy go włączać ponownie.");
        }
    }

    public void ZamknijMinigre()
    {
        if (glosnik != null && glosnik.isPlaying)
        {
            glosnik.Stop();
        }

        if (canvasMinigra != null) canvasMinigra.SetActive(false);
        if (canvasSalon != null) canvasSalon.SetActive(true);

        if (ZarzadzaniePauza.Instance != null) ZarzadzaniePauza.Instance.UstawStanMinigry(false);
    }

    private void UciszWszystkieGlosnikiWSalonie()
    {
        AudioSource[] wszystkieGlosniki = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource g in wszystkieGlosniki)
        {
            if (g != null && g.isPlaying && g.gameObject.layer != LayerMask.NameToLayer("UI")) 
            {
                g.Stop();
            }
        }
    }

    public void OznaczZadanieJakoZrobione()
    {
        PlayerPrefs.SetInt(unikalnaNazwaZadania, 1);
        PlayerPrefs.Save();
        Debug.Log("Zadanie " + unikalnaNazwaZadania + " oficjalnie uznane za zrobione!");
    }
}