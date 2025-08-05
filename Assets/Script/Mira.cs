using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class Mira : MonoBehaviour
{
    public Camera camera;
    public float alturaY = 0f;
    public float escalaMin = 2.3f;
    public float escalaMax = 5f;
    public float velocidadeEscala = 3f;

    private bool podeSeguir = true;

    private float tempoAtual = 0f;

    public Image fill;
    private UnityEvent myEvent;

    public GameObject sphereCollider;
    public Detector detector;

    private bool clicaDnovo;

    public AudioSource audioSource;
    public AudioClip preencheAudio;
    public AudioClip cliqueAudio;

    private bool audioTocando = false;

    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        podeSeguir = true;
        //sphereCollider.SetActive(false);
    }



    void Update()
    {
        // segue o mouse
        if (podeSeguir)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Plane planoXZ = new Plane(Vector3.up, new Vector3(0, alturaY, 0));

            if (planoXZ.Raycast(ray, out float distancia))
            {
                Vector3 pontoNoMundo = ray.GetPoint(distancia);
                transform.position = new Vector3(pontoNoMundo.x, alturaY, pontoNoMundo.z);
            }

            // Aumentar ou diminuir escala com scroll, Z ou X
            float scroll = Input.mouseScrollDelta.y;

            // Tecla Z diminui, tecla X aumenta
            if (Input.GetKey(KeyCode.Z)) scroll = -1f;
            if (Input.GetKey(KeyCode.X)) scroll = 1f;

            if (scroll != 0f)
            {
                audioSource.PlayOneShot(cliqueAudio);
                Vector3 novaEscala = transform.localScale + Vector3.one * scroll * velocidadeEscala * Time.deltaTime;
                novaEscala = Vector3.Max(novaEscala, Vector3.one * escalaMin);
                novaEscala = Vector3.Min(novaEscala, Vector3.one * escalaMax);
                transform.localScale = novaEscala;
            }
        }

        // Clique esquerdo OU tecla espaço
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            if (clicaDnovo) return;

            tempoAtual += Time.deltaTime;
            fill.fillAmount = tempoAtual;
            fill.enabled = true;
            podeSeguir = false;
            clicaDnovo = false;

            if (!audioTocando)
            {
                audioSource.clip = preencheAudio;
                audioSource.loop = false; // opcional
                audioSource.Play();
                audioTocando = true;
            }

            if (tempoAtual >= 1)
            {
                audioSource.Stop();
                tempoAtual = 0f;
                fill.fillAmount = 0f;
                fill.enabled = false;
                detector.Checagem();
                podeSeguir = true;
                clicaDnovo = true;
                audioTocando = false;
            }

        }
        else
        {
             if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
            {
                audioSource.Stop();
                audioSource.PlayOneShot(cliqueAudio);
                tempoAtual = 0f;
                fill.fillAmount = 0f;
                fill.enabled = false;
                podeSeguir = true;
                clicaDnovo = false;
                audioTocando = false;
            }
        }
    }


    
        

        
}
