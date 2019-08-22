using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource Source;
    public AudioClip Title, Ingame, GameOver, GameClear;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlayTitle()
    {
        Source.clip = Title;
        Source.Play();
    }

    public void PlayIngame()
    {
        Source.clip = Ingame;
        Source.Play();
    }

    public void PlayGameOver()
    {
        Source.clip = GameOver;
        Source.Play();
    }

    public void PlayGameClear()
    {
        Source.clip = GameClear;
        Source.Play();
    }
}
