using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Botoes : MonoBehaviour
{

    public Pause pause;
    public GameObject tuto1;
    public GameObject tuto2;
    public GameObject TutorialPrincipal;
    public bool noTuto = false;


 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if ((SceneManager.GetActiveScene().name) == "Fase1")
        {
            TutorialPrincipal.SetActive(true);
            tuto1.SetActive(true);
            tuto2.SetActive(false);

            pause.PauseForTutorial();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Restart()
    {
        Time.timeScale = 1; // Certifique-se de que o tempo está correndo

        // Reinicia a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Tutorial()
    {
        pause.pauseMenu.SetActive(false);
        noTuto = true;
        //pause.PauseForTutorial();
        TutorialPrincipal.SetActive(true);
        tuto1.SetActive(true);
        tuto2.SetActive(false);
    }

    public void Tuorial1()
    {
        tuto1.SetActive(false);
        tuto2.SetActive(true);
    }

    public void Tuorial2()
    {
        tuto1.SetActive(false);
        tuto2.SetActive(false);
        TutorialPrincipal.SetActive(false);
        noTuto = false;
        pause.Paused(); // despausa o jogo
    }

    
}
