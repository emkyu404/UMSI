using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelInfinite");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void GoOption()
    {
        SceneManager.LoadScene("Option");
    }
    public void GoConnexionMenu()
    {
        SceneManager.LoadScene("MenuConnexion");

    }
    public void GoMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void GoInscription()
    {
        Application.OpenURL("https://pjs4-umsi.000webhostapp.com/index.php?controleur=utilisateur&action=redirectionInscription");// peut-être à changer
    }
    public void GoClassement()
    {
        Application.OpenURL("https://pjs4-umsi.000webhostapp.com/index.php?controleur=utilisateur&action=redirectionClassement");
    }
}
