using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GerenciadorAnimais : MonoBehaviour
{
    public bool hasAnimal = false;
    public GameObject painel;
    public Detector detector;
    public Pause pause;


    private List<GameObject> animaisFora = new List<GameObject>();

    [Header("Texto")]

    public TextMeshProUGUI textoVaca, textoOvelha, textoGalinha, textoPorco;
    public TextMeshProUGUI textoIncorretos, textoBonusTempo, textoTotal;    
    

    [Header("Imagens")]
    public GameObject imagemVaca, imagemOvelha, imagemGalinha, imagemPorco;

    public GameObject imagemIncorretos, imagemBonusTempo;

    [Header("Exibir Pontuação")]
    public bool vacaPontos = true;
    public bool ovelhaPontos = true;
    public bool galinhaPontos = true;
    public bool porcoPontos = true;

    [Header("Levels Data")]
    public Level levelUm;
    public Level levelDois;
    public Level levelTres; 

    public Button proxFaseButton;

    [Header("Audio")]
    public AudioSource musicAudioSource;
    public AudioClip victoryClip, derrotaClip, musicClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ComeçouJogo());
        painel.SetActive(false);
        

        textoBonusTempo.gameObject.SetActive(false);
        textoIncorretos.gameObject.SetActive(false);
        textoTotal.gameObject.SetActive(false);

        textoVaca.gameObject.SetActive(false);
        textoOvelha.gameObject.SetActive(false);
        textoGalinha.gameObject.SetActive(false);
        textoPorco.gameObject.SetActive(false);

        imagemVaca.SetActive(false);
        imagemOvelha.SetActive(false);
        imagemGalinha.SetActive(false);
        imagemPorco.SetActive(false);
        imagemIncorretos.SetActive(false);
        imagemBonusTempo.SetActive(false);

    }
    private bool podeComeçar = false;

    // Update is called once per frame
    void Update()
    {
        if (podeComeçar)
        {
            if (animaisFora.Count == 0)
            {
                //Debug.Log("Nenhum animal detectado.");
                hasAnimal = false;
                ComeçaResultados();
            }
        }
        

        
    }

    IEnumerator ComeçouJogo()
    {
        yield return new WaitForSeconds(2f);
        podeComeçar = true;
        
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vaca") || other.CompareTag("Ovelha") || other.CompareTag("Galinha") || other.CompareTag("Porco"))
        {
            animaisFora.Add(other.gameObject);
            hasAnimal = true;

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vaca") || other.CompareTag("Ovelha") || other.CompareTag("Galinha") || other.CompareTag("Porco"))
        {
            animaisFora.Remove(other.gameObject);

        }
    }

    public List<GameObject> GetAnimaisFora()
    {
        return new List<GameObject>(animaisFora);
    }


    public void ComeçaResultados()
    {
        StartCoroutine(MostrarResultado());
    }

    

    IEnumerator MostrarResultado()
    {
        painel.SetActive(true);
        detector.MostrarAnimaisPorPonto();

        int valorBonus = 0;
        float valorFloat;

        int acertos = detector.vacasCorretas + detector.ovelhasCorretas +
        detector.porcosCorretos + detector.galinhasCorretas;

        int erros = detector.quantidadeErrados.Count * 3;
       

        if (vacaPontos)
        {
            textoVaca.text = "Caught Correctly: +" + detector.vacasCorretas;
            textoVaca.gameObject.SetActive(true);
            imagemVaca.SetActive(true);
            yield return new WaitForSeconds(0.7f);
        }


        if (ovelhaPontos)
        {
            textoOvelha.text = "Caught Correctly: +" + detector.ovelhasCorretas;
            textoOvelha.gameObject.SetActive(true);
            imagemOvelha.SetActive(true);
            yield return new WaitForSeconds(0.7f);
        }

        if (galinhaPontos)
        {
            textoGalinha.text = "Caught Correctly: +" + detector.galinhasCorretas;
            textoGalinha.gameObject.SetActive(true);
            imagemGalinha.SetActive(true);
            yield return new WaitForSeconds(0.7f);
        }


   
        if (porcoPontos)
        {
            textoPorco.text = "Caught Correctly: +" + detector.porcosCorretos;
            textoPorco.gameObject.SetActive(true);
            imagemPorco.SetActive(true);
            yield return new WaitForSeconds(0.7f);
        }
        

        if (detector.quantidadeErrados.Count > 0)
        {
            textoIncorretos.text = "Caught Incorrectly: -" + erros.ToString();
            textoIncorretos.gameObject.SetActive(true);
            imagemIncorretos.SetActive(true);
            yield return new WaitForSeconds(0.7f);
        }

        if (pause.totalTimeSec > 0)
        {
            valorFloat = pause.timerFormat / 10;
            valorBonus = (int)valorFloat;

            textoBonusTempo.text = "Time bonus: +" + valorBonus;
            textoBonusTempo.gameObject.SetActive(true);
            imagemBonusTempo.SetActive(true);
            yield return new WaitForSeconds(0.7f);
        }
        else
        {
            valorBonus = 0;
        }

        int total = acertos - erros + valorBonus;

        textoTotal.text = "Total: " + total;
        textoTotal.gameObject.SetActive(true);
        pause.acabou = true;

        if ((SceneManager.GetActiveScene().name) == "Fase1")
        {
            if (total >= 20)
            {
                if (musicAudioSource.clip != victoryClip)
                {
                    musicAudioSource.clip = victoryClip;
                    musicAudioSource.Play();
                }

                proxFaseButton.interactable = true;
                // Desbloqueia o próximo nível
                levelUm.isCompleted = true;
                levelDois.isUnlocked = true;
            }
            else
            {
                if (musicAudioSource.clip != derrotaClip)
                {
                    musicAudioSource.clip = derrotaClip;
                    musicAudioSource.Play();
                }

                proxFaseButton.interactable = false;
            }
        }
        else if ((SceneManager.GetActiveScene().name) == "Fase2")
        {
            if (total >= 40)
            {
                if (musicAudioSource.clip != victoryClip)
                {
                    musicAudioSource.clip = victoryClip;
                    musicAudioSource.Play();
                }
                proxFaseButton.interactable = true;
                levelDois.isCompleted = true;
                levelTres.isUnlocked = true;
            }
            else
            {
                if (musicAudioSource.clip != derrotaClip)
                {
                    musicAudioSource.clip = derrotaClip;
                    musicAudioSource.Play();
                }
                proxFaseButton.interactable = false;
            }
        }
        else if ((SceneManager.GetActiveScene().name) == "Fase3")
        {
            if (total >= 70)
            {
                if (musicAudioSource.clip != victoryClip)
                {
                    musicAudioSource.clip = victoryClip;
                    musicAudioSource.Play();
                }

           
                // Desbloqueia o próximo nível
                levelTres.isCompleted = true;
            }
            else
            {
                if (musicAudioSource.clip != derrotaClip)
                {
                    musicAudioSource.clip = derrotaClip;
                    musicAudioSource.Play();
                }
       
                Debug.Log("Ainda nao!");
            }
        }

        yield break;
    }

}
