using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class Detector : MonoBehaviour
{
    public GameObject pontoV, pontoO, pontoG, pontoP;

    private List<GameObject> animaisDetectados = new List<GameObject>();

    private Dictionary<string, GameObject> pontosDestino;
    public Dictionary<GameObject, List<GameObject>> animaisPorPonto; // Agora usamos GameObject como chave
    

    private void Start()
    {
        pontosDestino = new Dictionary<string, GameObject>()
        {
            { "Vaca", pontoV },
            { "Ovelha", pontoO },
            { "Galinha", pontoG },
            { "Porco", pontoP }
        };

        animaisPorPonto = new Dictionary<GameObject, List<GameObject>>()
        {
            { pontoV, new List<GameObject>() },
            { pontoO, new List<GameObject>() },
            { pontoG, new List<GameObject>() },
            { pontoP, new List<GameObject>() }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        var mov = other.gameObject.GetComponent<Movimento>();
        if (pontosDestino.ContainsKey(other.tag) && mov != null && !mov.isTrancadoAtivo && !mov.estaDetectado)
        {
            animaisDetectados.Add(other.gameObject);
            mov.estaDetectado = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var mov = other.gameObject.GetComponent<Movimento>();
        if (mov != null && mov.estaDetectado)
        {
            animaisDetectados.Remove(other.gameObject);
            mov.estaDetectado = false;
        }
    }


    public void Checagem()
    {
        if (animaisDetectados.Count == 0)
            return;

        string tagEscolhida = Contador();

        if (tagEscolhida == null || !pontosDestino.ContainsKey(tagEscolhida))
        {
            Debug.LogWarning("Nenhuma tag válida foi escolhida.");
            return;
        }

        GameObject pontoDestino = pontosDestino[tagEscolhida];

        Debug.Log($"Todos os animais serão movidos para o ponto de: {tagEscolhida} ({pontoDestino.name})");

        foreach (GameObject animal in animaisDetectados)
        {
            animal.GetComponent<Movimento>().isTrancadoAtivo = false;
            animal.GetComponent<Movimento>().nome = "Trancado";
            animal.GetComponent<Movimento>().isOvelha = false;
            animal.GetComponent<Movimento>().audioSource.Play();

            NavMeshAgent agent = animal.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                animal.AddComponent<NavMeshAgent>();
                agent = animal.GetComponent<NavMeshAgent>();
            }
            agent.Warp(pontoDestino.transform.position);

            //Debug.Log($"Movendo {animal.name} (Tag: {animal.tag}) para {pontoDestino.name}");

            // Adiciona o animal à lista do ponto no novo dicionário
            if (animaisPorPonto.ContainsKey(pontoDestino))
            {
                animaisPorPonto[pontoDestino].Add(animal);
            }
        }

        animaisDetectados.Clear();

        // Exibe o total de animais em cada ponto
        foreach (var par in animaisPorPonto)
        {
            Debug.Log($"Ponto: {par.Key.name}, Quantidade de animais: {par.Value.Count}");
        }
    }

    private string Contador()
    {
        Dictionary<string, int> contaAnimais = new Dictionary<string, int>()
        {
            { "Vaca", 0 },
            { "Ovelha", 0 },
            { "Galinha", 0 },
            { "Porco", 0 }
        };

        foreach (var animal in animaisDetectados)
        {
            if (contaAnimais.ContainsKey(animal.tag))
                contaAnimais[animal.tag]++;
        }

        int maiorQtd = contaAnimais.Values.Max();

        var maisComuns = contaAnimais
            .Where(x => x.Value == maiorQtd && x.Value > 0)
            .Select(x => x.Key)
            .ToList();

        if (maisComuns.Count > 0)
            return maisComuns[Random.Range(0, maisComuns.Count)];

        return null;
    }
    
    // Cria listas para armazenar os tipos por ponto
    public List<GameObject> quantidaVacas = new List<GameObject>();
    public List<GameObject> quantidaOvelhas = new List<GameObject>();
    public List<GameObject> quantidaGalinhas = new List<GameObject>();
    public List<GameObject> quantidaPorcos = new List<GameObject>();
    public List<GameObject> quantidadeErrados = new List<GameObject>();

    public    int vacasCorretas = 0;
    public    int ovelhasCorretas = 0;
    public    int galinhasCorretas = 0;
    public    int porcosCorretos = 0;
    
   

    public void MostrarAnimaisPorPonto()
    {

        quantidaVacas.Clear();
        quantidaOvelhas.Clear();
        quantidaGalinhas.Clear();
        quantidaPorcos.Clear();
        quantidadeErrados.Clear();
        vacasCorretas = 0;
        ovelhasCorretas = 0;
        galinhasCorretas = 0;
        porcosCorretos = 0;




        foreach (var par in animaisPorPonto)
        {
            GameObject ponto = par.Key;
            List<GameObject> animais = par.Value;

            foreach (var animal in animais)
            {
                switch (animal.tag)
                {
                    case "Vaca":
                        quantidaVacas.Add(animal);
                        if (ponto == pontoV)
                            vacasCorretas++;
                        else
                            quantidadeErrados.Add(animal);
                        break;

                    case "Ovelha":
                        quantidaOvelhas.Add(animal);
                        if (ponto == pontoO)
                            ovelhasCorretas++;
                        else
                            quantidadeErrados.Add(animal);
                        break;

                    case "Galinha":
                        quantidaGalinhas.Add(animal);
                        if (ponto == pontoG)
                            galinhasCorretas++;
                        else
                            quantidadeErrados.Add(animal);
                        break;

                    case "Porco":
                        quantidaPorcos.Add(animal);
                        if (ponto == pontoP)
                            porcosCorretos++;
                        else
                            quantidadeErrados.Add(animal);
                        break;

                    default:
                        quantidadeErrados.Add(animal);
                        break;
                }
            }
        }

    }


}
