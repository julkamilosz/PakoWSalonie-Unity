using UnityEngine;
using UnityEngine.EventSystems;

public class PrzeciaganieKsiazki : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [Header("Ustawienia edukacyjne:")]
    public string kolorKsiazki; 
    public char literaNaKsiazce; 
    public AudioClip dzwiekLitery; 

    private Vector2 pozycjaStartowa;
    private AudioSource glosnik;
    private RectTransform rectTransform;
    private Canvas canvas;
    
    private Transform przypisanySlot = null;

    private void Start()
    {
        glosnik = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        pozycjaStartowa = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ZarzadzaniePauza.Instance != null && ZarzadzaniePauza.Instance.CzyGraJestZapauzowana()) return;

        if (przypisanySlot != null)
        {
            przypisanySlot = null;
        }

        if (glosnik != null && dzwiekLitery != null)
        {
            glosnik.clip = dzwiekLitery;
            glosnik.Play();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ZarzadzaniePauza.Instance != null && ZarzadzaniePauza.Instance.CzyGraJestZapauzowana()) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PolkaZadaniowa[] wszystkiePolki = FindObjectsByType<PolkaZadaniowa>(FindObjectsSortMode.None);
        PolkaZadaniowa dopasowanaPolka = null;

        foreach (PolkaZadaniowa polka in wszystkiePolki)
        {
            RectTransform polkaRect = polka.GetComponent<RectTransform>();

            if (polkaRect != null && RectTransformUtility.RectangleContainsScreenPoint(polkaRect, eventData.position, canvas.worldCamera))
            {
                if (polka.kolorPolki == kolorKsiazki)
                {
                    dopasowanaPolka = polka;
                    break;
                }
            }
        }

        if (dopasowanaPolka != null)
        {
            Transform wolnySlot = dopasowanaPolka.ZnajdzPierwszyWolnySlot();

            if (wolnySlot != null)
            {
                przypisanySlot = wolnySlot;

                transform.position = wolnySlot.position;
                return; 
            }
        }

        WrocNaMiejsce();
        Debug.Log($"[Test] Książka upuszczona. Dopasowana półka: {(dopasowanaPolka != null ? dopasowanaPolka.name : "BRAK")}");
        SprawdzCzyKoniecMinigry();
    }

    public void WrocNaMiejsce()
    {
        przypisanySlot = null;
        rectTransform.anchoredPosition = pozycjaStartowa;
    }

    public bool CzyStoiszWTimSlocie(Transform slot)
    {
        return przypisanySlot == slot;
    }
private void SprawdzCzyKoniecMinigry()
    {
        PolkaZadaniowa[] wszystkiePolki = FindObjectsByType<PolkaZadaniowa>(FindObjectsSortMode.None);
        PrzeciaganieKsiazki[] wszystkieKsiazki = FindObjectsByType<PrzeciaganieKsiazki>(FindObjectsSortMode.None);

        int liczbaZajetychSlotow = 0;

        foreach (PolkaZadaniowa polka in wszystkiePolki)
        {
            if (polka.sloty == null) continue;

            foreach (Transform slot in polka.sloty)
            {
                if (slot == null) continue;

                foreach (PrzeciaganieKsiazki ksiazka in wszystkieKsiazki)
                {
                    if (ksiazka.CzyStoiszWTimSlocie(slot))
                    {
                        liczbaZajetychSlotow++;
                        break; 
                    }
                }
            }
        }

        Debug.Log("TEST KOŃCA: Widzę zajętych slotów: " + liczbaZajetychSlotow + " z wymaganych " + wszystkieKsiazki.Length);

        if (liczbaZajetychSlotow == wszystkieKsiazki.Length)
        {
            Debug.Log("🏆 HURRA! Wszystkie książki są na swoim miejscu! Koniec minigry.");
            
            MebelZadanie regal = FindFirstObjectByType<MebelZadanie>();
            if (regal != null)
            {
                regal.OznaczZadanieJakoZrobione();
                StartCoroutine(SekwencjaZakonczenia(regal));
            }
        }
    }

    private System.Collections.IEnumerator SekwencjaZakonczenia(MebelZadanie regal)
    {
        AudioSource glosnikRegalu = regal.GetComponent<AudioSource>();
        
        if (glosnikRegalu != null && regal.dzwiekSukcesuMinigry != null)
        {
            glosnikRegalu.clip = regal.dzwiekSukcesuMinigry;
            glosnikRegalu.Play();
            
            yield return new WaitForSeconds(regal.dzwiekSukcesuMinigry.length + 0.5f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        regal.ZamknijMinigre();
    }
}