using UnityEngine;
using System.Collections;

public class ManagerMinigryKsiazek : MonoBehaviour
{
    private bool czyGraSieSkonczila = false;
    private bool czyLektorWlasnieMowi = false; 

    private void OnEnable()
    {
        czyGraSieSkonczila = false;
        czyLektorWlasnieMowi = false;
        StartCoroutine(PetlaSprawdzaniaWygranej());
    }

    private IEnumerator PetlaSprawdzaniaWygranej()
    {
        while (!czyGraSieSkonczila)
        {
            yield return new WaitForSeconds(0.5f);

            if (czyLektorWlasnieMowi) continue;

            PolkaZadaniowa[] wszystkiePolki = FindObjectsByType<PolkaZadaniowa>(FindObjectsSortMode.None);
            PrzeciaganieKsiazki[] wszystkieKsiazki = FindObjectsByType<PrzeciaganieKsiazki>(FindObjectsSortMode.None);

            int liczbaZajetychSlotow = 0;
            int liczbaPoprawnychKsiazek = 0;

            foreach (PolkaZadaniowa polka in wszystkiePolki)
            {
                if (polka.sloty == null) continue;

                foreach (Transform slot in polka.sloty)
                {
                    if (slot == null) continue;

                    foreach (PrzeciaganieKsiazki ksiazka in wszystkieKsiazki)
                    {
                        if (Vector3.Distance(ksiazka.transform.position, slot.position) < 0.5f)
                        {
                            liczbaZajetychSlotow++;

                            if (slot.name.Contains(ksiazka.name))
                            {
                                liczbaPoprawnychKsiazek++;
                            }
                            break;
                        }
                    }
                }
            }

            if (liczbaZajetychSlotow == wszystkieKsiazki.Length && liczbaPoprawnychKsiazek == wszystkieKsiazki.Length && wszystkieKsiazki.Length > 0)
            {
                czyGraSieSkonczila = true;
                UruchomSukces();
            }

            else if (liczbaZajetychSlotow == wszystkieKsiazki.Length && liczbaPoprawnychKsiazek < wszystkieKsiazki.Length && wszystkieKsiazki.Length > 0)
            {
                StartCoroutine(SekwencjaZlejKolejnosci());
            }
        }
    }

    private void UruchomSukces()
    {
        Debug.Log("🏆 HURRA! Wszystkie książki na właściwych miejscach!");

        MebelZadanie regal = FindFirstObjectByType<MebelZadanie>();
        if (regal != null)
        {
            regal.OznaczZadanieJakoZrobione();
            StartCoroutine(SekwencjaZakonczenia(regal));
        }
    }

    private IEnumerator SekwencjaZakonczenia(MebelZadanie regal)
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

    private IEnumerator SekwencjaZlejKolejnosci()
    {
        czyLektorWlasnieMowi = true;
        Debug.Log("❌ Książki ułożone, ale kolejność jest zła!");

        MebelZadanie regal = FindFirstObjectByType<MebelZadanie>();
        if (regal != null)
        {
            AudioSource glosnikRegalu = regal.GetComponent<AudioSource>();
            if (glosnikRegalu != null && regal.dzwiekZlejKolejnosciMinigry != null)
            {
                glosnikRegalu.clip = regal.dzwiekZlejKolejnosciMinigry;
                glosnikRegalu.Play();
                
                yield return new WaitForSeconds(regal.dzwiekZlejKolejnosciMinigry.length + 1.0f);
            }
            else
            {
                yield return new WaitForSeconds(3f);
            }
        }

        czyLektorWlasnieMowi = false; 
    }
}