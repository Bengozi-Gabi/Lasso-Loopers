using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pause : MonoBehaviour
{
    public  bool isPaused = false;
    public TextMeshProUGUI timerText;
    public float totalTimeSec;
    private float tempooo;
    public float timerFormat; // Formato do temporizador
    public bool acabou = false;

    public GameObject pauseMenu;
    public GerenciadorAnimais gerenciadorAnimais;
    public Botoes botoes;

    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1; // Garantir que o tempo começa normalmente
        acabou = false; // Resetar fim do jogo
       

    }

    void Update()
    {
        if (acabou) return; // Se o jogo acabou, não faz nada

        if (!isPaused)
        {
            totalTimeSec -= Time.deltaTime;
        }

        timerFormat = totalTimeSec;
        timerText.text = Mathf.Floor(totalTimeSec / 60).ToString("00") +
        ":" + Mathf.FloorToInt(totalTimeSec % 60).ToString("00");


        if (Input.GetKeyDown(KeyCode.P) && !acabou && !botoes.noTuto) // Tecla para pausar/retomar
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Paused();
        }



        if (totalTimeSec <= 0)
            {
                timerText.text = "00:00";
                gerenciadorAnimais.ComeçaResultados();


            }
    }

    public void PausarBotao()
    {
        if (!acabou && !botoes.noTuto)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Paused();
        }
        
    }

    public void PauseForTutorial()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void Paused()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
}
