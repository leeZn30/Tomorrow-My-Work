using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.playMapBGM(2);
    }
}
