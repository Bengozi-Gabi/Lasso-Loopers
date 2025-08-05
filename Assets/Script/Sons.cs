using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class Sons : MonoBehaviour
{
    [Space]
    [Header("Audio Settings")]
    public AudioSource musicAudioSource;
    public List<AudioSource> audiosSources;
  
    public Image soundImg;
    public Image musicImg;

    public Sprite soundOnIcon;
    public Sprite soundOffIcon;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    private bool mutedSounds = false;
    private bool mutedMusic = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (AudioSource audioSource in audiosSources)
        {
            audioSource.mute = false; // Garantir que os sons começam ativados
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void MuteAllSounds()
    {
        mutedSounds = !mutedSounds;

        // Encontra todos os AudioSources ativos na cena
        AudioSource[] allSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allSources)
        {
            // Silencia tudo, exceto a música
            if (source != musicAudioSource)
            {
                source.mute = mutedSounds;
            }
        }

        // Atualiza o ícone do botão de som
        soundImg.sprite = mutedSounds ? soundOffIcon : soundOnIcon;
    }





    public void MuteMusic()
    {

        if (musicAudioSource != null && mutedMusic == false)
        {
            musicAudioSource.mute = true;
            mutedMusic = true;
            musicImg.sprite = musicOffIcon;
        }
        else
        {
            musicAudioSource.mute = false;
            mutedMusic = false;
            musicImg.sprite = musicOnIcon;
        }
    }

}
