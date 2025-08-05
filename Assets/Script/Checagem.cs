using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Nivel
{
    public string nome;
    public Level levelData;
    public GameObject preto;
    public GameObject unlock;
    public TextMeshProUGUI missaoText;
    public Button botão;
}

public class Checagem : MonoBehaviour
{
    public List<Nivel> niveis;
    public Sprite completoImg;
    public TextMeshProUGUI ganhouText;


    void Start()
    {
        ganhouText.gameObject.SetActive(false);
    }

    public void ChecarNiveis()
    {
        foreach (Nivel nivel in niveis)
        {

            if (nivel.levelData.isCompleted)
            {
                OnComplete(nivel.nome);
            }

            if (nivel.levelData.isUnlocked && !nivel.levelData.isCompleted)
            {
                OnProgress(nivel.nome);
            }

        }

        if (niveis.TrueForAll(n => n.levelData.isCompleted))
        {
            ganhouText.gameObject.SetActive(true);
        }
    }


    private void OnComplete(string levelName)
    {
        Nivel nivel = niveis.Find(n => n.nome == levelName);
        if (nivel != null)
        {
            nivel.preto.SetActive(false);
            nivel.missaoText.gameObject.SetActive(false);
            nivel.unlock.SetActive(true);
            nivel.unlock.GetComponent<UnityEngine.UI.Image>().sprite = completoImg;
        }

        //colocar o proximo nivel desbloqueado
        switch (levelName)
        {
            case "Fase1":
                if (niveis.Find(n => n.nome == "Fase2") != null)
                {
                    niveis.Find(n => n.nome == "Fase2").levelData.isUnlocked = true;

                }
                break;
            case "Fase2":
                if (niveis.Find(n => n.nome == "Fase3") != null)
                {
                    niveis.Find(n => n.nome == "Fase3").levelData.isUnlocked = true;
                }
                break;
        }
    }

    private void OnProgress(string levelName)
    {
        Nivel nivel = niveis.Find(n => n.nome == levelName);
        if (nivel != null)
        {
            nivel.preto.SetActive(false);
            nivel.missaoText.gameObject.SetActive(true);
            nivel.unlock.SetActive(false);

            nivel.levelData.isUnlocked = true; // Marca o nível como desbloqueado
            nivel.botão.gameObject.SetActive(true); // Habilita o botão do nível
        }
    }

    void Update()
    {
        ChecarNiveis();
    }


}
