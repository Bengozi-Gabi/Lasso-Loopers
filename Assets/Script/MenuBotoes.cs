using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuBotoes : MonoBehaviour
{


    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SelectLevel()
    {
        Transform transformObj = transform.Find("SelectLevel");
        GameObject selectLevel = transformObj.gameObject;
        selectLevel.SetActive(true);
    }

    public void Fase1()
    {
        SceneManager.LoadScene("Fase1");
    }

    public void Fase2()
    {
        SceneManager.LoadScene("Fase2");
    }

    public void Fase3()
    {
        SceneManager.LoadScene("Fase3");
    }
}
