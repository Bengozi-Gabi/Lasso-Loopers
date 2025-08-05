using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movimento : MonoBehaviour
{
    public string nome;
    public NavMeshAgent agent;
    private float wanderRadius = 15f;
    string[] tags = { "Vaca", "Ovelha", "Galinha", "Porco" };
    public bool isTrancadoAtivo;
    public GerenciadorAnimais gerenciadorAnimais;
    public AudioSource audioSource;


    //private float distanciaFuga = 10f;
    public bool estaDetectado = false;



    public float velocidade = 2.5f;
    private Vector3 direcaoAtual;
    public bool isOvelha = false;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        audioSource = GetComponent<AudioSource>();


        gerenciadorAnimais = FindFirstObjectByType<GerenciadorAnimais>();
        if(gerenciadorAnimais == null)
        {
            Debug.LogError("GerenciadorAnimais não encontrado na cena.");
            return;
        }

        switch (nome)
            {
                case "Vaca":
                    agent.speed = 2f;
                    StartCoroutine(MoverVaca());
                    break;

                case "Ovelha":
                    direcaoAtual = DirecaoAleatoriaXZ();
                    break;

                case "Galinha":
                    agent.speed = 4f;
                    StartCoroutine(MoverGalinha());
                    break;

                case "Porco":
                    agent.speed = 3f;
                    StartCoroutine(MoverPorco());
                    break;
            }
    }

    void Update()
    {
        if (nome == "Trancado" && !isTrancadoAtivo)
        {
            StopAllCoroutines();

            if (agent == null)
            {
                gameObject.AddComponent<NavMeshAgent>();
                agent = GetComponent<NavMeshAgent>();
            }
                

            wanderRadius = 3f;
            agent.speed = 1f;
            StartCoroutine(MoverVaca());

            isTrancadoAtivo = true;
        }


        if (isOvelha)
        {
            // Movimento
            transform.position += direcaoAtual * velocidade * Time.deltaTime;

            // Rotação na direção atual
            if (direcaoAtual != Vector3.zero)
            {
                Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoAtual);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * 5f); // 5f = velocidade de rotação
            }
        }

    }

    IEnumerator MoverVaca()
    {
        while (true)
        {
            SetRandomDestination();
            yield return new WaitForSeconds(Random.Range(1.5f, 2f));

            agent.ResetPath();
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }

    IEnumerator MoverGalinha()
    {
        while (true)
        {
            SetRandomDestination();
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isOvelha)
        {
            if (other.CompareTag("Vaca") || other.CompareTag("Ovelha") || other.CompareTag("Galinha") ||
                other.CompareTag("Porco") || other.CompareTag("Cerca"))
            {
                Vector3 normal = (transform.position - other.ClosestPoint(transform.position)).normalized;
                direcaoAtual = Vector3.Reflect(direcaoAtual, normal).normalized;
                direcaoAtual.y = 0f;
            }


        }
            
    }

    Vector3 DirecaoAleatoriaXZ()
    {
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }
    

    IEnumerator MoverPorco()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));

            List<GameObject> animaisDisponiveis = new List<GameObject>();

            if (gerenciadorAnimais != null)
            {
                animaisDisponiveis = gerenciadorAnimais.GetAnimaisFora();

                // Remover a si mesmo e outros porcos
                animaisDisponiveis.RemoveAll(a =>
                    a == this.gameObject || a.CompareTag("Porco"));
            }

            if (animaisDisponiveis.Count > 0)
            {
                GameObject alvo = animaisDisponiveis[Random.Range(0, animaisDisponiveis.Count)];

                if (alvo != null)
                {
                    agent.SetDestination(alvo.transform.position);
                    continue; // vai para o próximo ciclo sem comportamento da vaca
                }
            }

            // Comportamento da vaca (aleatório) se não houver nenhum alvo
            SetRandomDestination();
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
            agent.ResetPath();
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    List<GameObject> BuscarAnimaisProximos()
    {
        List<GameObject> proximos = new List<GameObject>();

        foreach (string tag in tags)
        {
            GameObject[] encontrados = GameObject.FindGameObjectsWithTag(tag);

            foreach (var animal in encontrados)
            {
                if (animal != this.gameObject && Vector3.Distance(transform.position, animal.transform.position) <= wanderRadius)
                {
                    proximos.Add(animal);
                }
            }
        }

        return proximos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
