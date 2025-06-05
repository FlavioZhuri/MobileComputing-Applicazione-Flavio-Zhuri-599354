using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton static instance
    private static AudioManager instance;

    [Header("---------- AUDIO SOURCE ----------")]
    [SerializeField] AudioSource musicSource;

    [Header("---------- AUDIO CLIP ----------")]
    public AudioClip background;

    private void Start()
    {
        // Se non esiste già un AudioManager, lo crea e lo rende persistente
        if (instance == null)
        {
            instance = this;

            musicSource.clip = background;
            musicSource.Play();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evita duplicati
        }
    }
}
