using UnityEngine;

public class Interakcja : MonoBehaviour
{
    public string nazwaPrzedmiotu;

    private void Start()
    {
        Debug.Log("Skrypt działa na: " + gameObject.name);
    }

    private void OnMouseDown()
    {
        if (ZarzadzaniePauza.Instance != null && ZarzadzaniePauza.Instance.CzyGraJestZapauzowana())
        {
            return; 
        }
        Debug.Log("SUKCES! Kliknięto w: " + nazwaPrzedmiotu);
    }
}