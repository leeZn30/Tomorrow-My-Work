using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource MapBGMSource;
    [SerializeField] private AudioSource GameSFXSource;

    [SerializeField] private List<AudioClip> MapBGMS;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    public void playMapBGM(int stageNum)
    {
        MapBGMSource.clip = MapBGMS[stageNum];
        MapBGMSource.Play();
    }

}
