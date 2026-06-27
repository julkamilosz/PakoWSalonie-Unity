using UnityEngine;
using UnityEngine.SceneManagement;

public class ZarzadzaniePauza : MonoBehaviour
{
    public static ZarzadzaniePauza Instance;

    public GameObject okienkoPauzy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (okienkoPauzy != null)
        {
            okienkoPauzy.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (czyTrwaMinigra)
            {
                return; 
            }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (okienkoPauzy != null)
            {
                bool stanOkienka = okienkoPauzy.activeSelf;
                okienkoPauzy.SetActive(!stanOkienka);
            }
        }
        }
    }

    public bool CzyGraJestZapauzowana()
    {
        if (okienkoPauzy != null)
        {
            return okienkoPauzy.activeSelf;
        }
        return false;
    }

    public void WrocDoMenuGlownego()
    {
        SceneManager.LoadScene("MenuScene");
    }
    private bool czyTrwaMinigra = false;

    public void UstawStanMinigry(bool wylaczonaCzyWlaczona)
    {
        czyTrwaMinigra = wylaczonaCzyWlaczona;
    }

    public bool CzyTrwaMinigra()
    {
        return czyTrwaMinigra;
    }
}