using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource MapBGMSource;
    [SerializeField] private AudioSource GameSFXSource;

    [SerializeField] private List<AudioClip> MapBGMS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playMapBGM(int stageNum)
    {
        MapBGMSource.clip = MapBGMS[stageNum];
        MapBGMSource.Play();
    }

}
