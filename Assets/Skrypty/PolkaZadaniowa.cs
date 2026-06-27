using UnityEngine;

public class PolkaZadaniowa : MonoBehaviour
{
    public string kolorPolki;
    public Transform[] sloty;

    public Transform ZnajdzPierwszyWolnySlot()
    {
        if (sloty == null || sloty.Length == 0)
        {
            Debug.LogError($"[SlotTest] Półka {name} nie ma przypisanych żadnych slotów w tablicy 'sloty'!");
            return null;
        }

        PrzeciaganieKsiazki[] wszystkieKsiazki = FindObjectsByType<PrzeciaganieKsiazki>(FindObjectsSortMode.None);

        foreach (Transform slot in sloty)
        {
            if (slot == null) continue; 

            bool czyZajety = false;

            foreach (PrzeciaganieKsiazki ksiazka in wszystkieKsiazki)
            {
                if (ksiazka.CzyStoiszWTimSlocie(slot))
                {
                    czyZajety = true;
                    break;
                }
            }

            if (!czyZajety)
            {
                Debug.Log($"[SlotTest] Znaleziono wolny slot: {slot.name}");
                return slot; 
            }
        }

        Debug.LogWarning($"[SlotTest] Półka {name} sprawdziła wszystkie sloty, ale wszystkie są ZAJĘTE!");
        return null; 
    }
}