using UnityEngine;
using UnityEngine.UI;

public class KontrolaDzwieku : MonoBehaviour
{
    public Sprite ikonaDzwiekuWlaczona;
    public Sprite ikonaDzwiekuWylaczona;

    private Image obrazekPrzycisku;
    private bool czyWyciszony = false;

    private void Start()
    {
        obrazekPrzycisku = GetComponent<Image>();
    }

    public void PrzelaczDzwiek()
    {
        czyWyciszony = !czyWyciszony;

        if (czyWyciszony)
        {
            AudioListener.volume = 0f;
            if(ikonaDzwiekuWylaczona != null) obrazekPrzycisku.sprite = ikonaDzwiekuWylaczona;
            Debug.Log("Gra została wyciszona.");
        }
        else
        {
            AudioListener.volume = 1f;
            if(ikonaDzwiekuWlaczona != null) obrazekPrzycisku.sprite = ikonaDzwiekuWlaczona;
            Debug.Log("Dźwięk został włączony.");
        }
    }
}