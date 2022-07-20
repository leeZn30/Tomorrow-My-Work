using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource MapBGMSource;
    [SerializeField] private AudioSource GameSFXSource;

    [SerializeField] private List<AudioClip> MapBGMS;
    [SerializeField] private List<AudioClip> gameSFXs;

    public void playMapBGM(int stageNum)
    {
        MapBGMSource.clip = MapBGMS[stageNum];
        MapBGMSource.Play();

        if (stageNum == 1)
        {
            MapBGMSource.volume = 0.5f;
        }
    }

    public void StopMapBGM()
    {
        MapBGMSource.Stop();
    }

    public void playGameSFX(int type)
    {
        GameSFXSource.clip = gameSFXs[type];
        GameSFXSource.PlayOneShot(GameSFXSource.clip);
    }



}
