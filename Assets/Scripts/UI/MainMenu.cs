using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private ELanguage language;

    [SerializeField] private Text playText;
    [SerializeField] private Text quitText;
    [Space]
    [SerializeField] private Image imgFR;
    [SerializeField] private Image imgEN;

    private void Start()
    {
        ChangeLanguage(PlayerPrefs.GetInt("Language", 0));
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeLanguage(int _language)
    {
        if ((ELanguage)_language == language) return;
        
        language = (ELanguage)_language;
        PlayerPrefs.SetInt("Language", (int)language);

        switch (language)
        {
            case ELanguage.EN:
                playText.text = "Play";
                quitText.text = "Quit";
                
                imgEN.color = Color.grey;
                imgFR.color = Color.white;
                break;
            
            case ELanguage.FR:
                playText.text = "Jouer";
                quitText.text = "Quitter";
                
                imgEN.color = Color.white;
                imgFR.color = Color.grey;
                break;
        }
    }
}
