using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManagerMinigryZegar : MonoBehaviour
{
    [System.Serializable]
    public class PoraDniaZadanie
    {
        public string nazwaPory;
        public Sprite grafikaZegara;
        public int indeksPoprawnegoObrazka;
    }

    [Header("Konfiguracja Pór Dnia:")]
    public PoraDniaZadanie[] zadaniaZegara;

    [Header("Elementy UI (Przeciągnij z Canvasa Zegara):")]
    public Image wyswietlaczZegara;
    public Button[] przyciskiCzynnosci;

    private int aktualneZadanie = 0;
    private bool czyLektorMowi = false;

    private void OnEnable()
    {
        aktualneZadanie = 0;
        czyLektorMowi = false;
        OdblokujPrzyciski(true);
        PokazZadanie(aktualneZadanie);
    }

    private void PokazZadanie(int indeks)
    {
        if (indeks >= zadaniaZegara.Length)
        {
            UruchomSukces();
            return;
        }

        if (wyswietlaczZegara != null && zadaniaZegara[indeks].grafikaZegara != null)
        {
            wyswietlaczZegara.sprite = zadaniaZegara[indeks].grafikaZegara;
        }

        for (int i = 0; i < przyciskiCzynnosci.Length; i++)
        {
            int nrPrzycisku = i;
            przyciskiCzynnosci[i].onClick.RemoveAllListeners();
            przyciskiCzynnosci[i].onClick.AddListener(() => KliknietoOdpowiedz(nrPrzycisku));
        }
    }

    public void KliknietoOdpowiedz(int indeksPrzycisku)
    {
        if (czyLektorMowi) return;

        int poprawnyIndeks = zadaniaZegara[aktualneZadanie].indeksPoprawnegoObrazka;

        if (indeksPrzycisku == poprawnyIndeks)
        {
            Debug.Log("👍 Dobrze! To właściwa czynność.");
            aktualneZadanie++;
            PokazZadanie(aktualneZadanie);
        }
        else
        {
            Debug.Log("❌ To nie ta czynność, spróbuj ponownie.");
            StartCoroutine(SekwencjaBledu());
        }
    }

    private void UruchomSukces()
    {
        OdblokujPrzyciski(false);
        
        MebelZadanie zegarMebel = ZnajdzTenMebel("ZegarScienny"); 
        
        if (zegarMebel != null)
        {
            zegarMebel.OznaczZadanieJakoZrobione();
            StartCoroutine(SekwencjaZakonczenia(zegarMebel));
        }
        else
        {
            MebelZadanie awaryjny = FindFirstObjectByType<MebelZadanie>();
            if (awaryjny != null)
            {
                awaryjny.OznaczZadanieJakoZrobione();
                StartCoroutine(SekwencjaZakonczenia(awaryjny));
            }
        }
    }

    private IEnumerator SekwencjaZakonczenia(MebelZadanie mebel)
    {
        AudioSource glosnik = mebel.GetComponent<AudioSource>();
        if (glosnik != null && mebel.dzwiekSukcesuMinigry != null)
        {
            glosnik.clip = mebel.dzwiekSukcesuMinigry;
            glosnik.Play();
            yield return new WaitForSeconds(mebel.dzwiekSukcesuMinigry.length + 0.5f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        mebel.ZamknijMinigre();
    }

    private IEnumerator SekwencjaBledu()
    {
        czyLektorMowi = true;
        
        MebelZadanie mebel = ZnajdzTenMebel("ZegarScienny");
        
        if (mebel == null) mebel = FindFirstObjectByType<MebelZadanie>();

        if (mebel != null)
        {
            AudioSource glosnik = mebel.GetComponent<AudioSource>();
            if (glosnik != null && mebel.dzwiekZlejKolejnosciMinigry != null)
            {
                glosnik.clip = mebel.dzwiekZlejKolejnosciMinigry;
                glosnik.Play();
                yield return new WaitForSeconds(mebel.dzwiekZlejKolejnosciMinigry.length + 0.5f);
            }
        }
        czyLektorMowi = false;
    }

    private void OdblokujPrzyciski(bool stan)
    {
        foreach (Button b in przyciskiCzynnosci)
        {
            if (b != null) b.interactable = stan;
        }
    }
    private MebelZadanie ZnajdzTenMebel(string unikalnaNazwa)
    {
        MebelZadanie[] wszyscy = FindObjectsByType<MebelZadanie>(FindObjectsSortMode.None);
        foreach (MebelZadanie m in wszyscy)
        {
            if (m.unikalnaNazwaZadania == unikalnaNazwa)
            {
                return m;
            }
        }
        return null;
    }
}