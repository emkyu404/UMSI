using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;



public class Connexion : MonoBehaviour
{
    public TMP_InputField pseudo;
    public TMP_InputField password;
    public GameObject popup;
    public GameObject menu;



    public Button valider;

    EventSystem system;

    void Start()
    {
        system = EventSystem.current;
        valider.interactable = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }

            //Here is the navigating back part:
            else
            {
                next = Selectable.allSelectables[0];
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }

        }
    }
    public void CallRegister()
    {

        StartCoroutine(LoginPlayer());
    }

    IEnumerator LoginPlayer()
    {
        Debug.Log(pseudo.text);
        string urlrequest = "https://pjs4-umsi.000webhostapp.com/index.php?controleur=utilisateur&action=getlogingame&uname=" + pseudo.text + "&psw=" + password.text;
        UnityWebRequest www = UnityWebRequest.Get(urlrequest);
        Debug.Log("url request : " + urlrequest);
        yield return www.SendWebRequest(); // un espèce de wait qui attend que l'instruction au-dessu avant d'executer en bas
        if (www.isHttpError || www.isNetworkError)// vérifie également que la connexion a réussie et que le texte retour n'est pas sup a 30
        {
            Debug.Log("connexion failed" + www.downloadHandler.text);


            popup.SetActive(true);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            string[] text = www.downloadHandler.text.Split(' ');
            if (text.Length != 14 || text[0].Replace("\r\n", "") != pseudo.text)
            {
                popup.SetActive(true);
            }
            else
            {
                Debug.Log("connexion reussie");
                //DBManager.pseudo = text[0].Replace("\n", ""); // retour à la ligne sur le premier argument ?
                DBManager.pseudo = text[0].Replace("\r\n", "");
                Debug.Log(DBManager.pseudo);
                DBManager.score = int.Parse(text[1]);

                DBManager.IdCompte = int.Parse(text[2]);
                DBManager.mail = text[3];
                DBManager.mdp = text[4];
                DBManager.statut = text[6];
                DBManager.bConnecte = text[7];
                DBManager.IdJoueur = int.Parse(text[8]);
                DBManager.tpsJeu = text[12];
                Debug.Log(text[13]);
                DBManager.meilleurScore = int.Parse(text[13]);
                DBManager.nbMonstreTues = int.Parse(text[11]);
                SceneManager.LoadScene("Menu");
            }


        }



    }
    public void verifyInputs()
    {
        valider.interactable = (pseudo.text.Length >= 2 && password.text.Length > 2);
    }
}
