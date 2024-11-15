using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip musicMenu;
    public AudioClip musicMap;

    private void Awake()
    {
        
    }

    private void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Menu")
        {
            musicSource.clip = musicMenu;
            musicSource.Play();
        }
        else
        {
            musicSource.clip = musicMap;
            musicSource.Play();
        }
    }
}
