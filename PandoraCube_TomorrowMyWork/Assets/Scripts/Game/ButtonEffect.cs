using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEffect : MonoBehaviour
{
    private void OnMouseEnter()
    {
        SoundManager.Instance.playGameSFX(4);
        transform.localScale = Vector3.one * 1.3f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -7.5f));
    }

    private void OnMouseExit()
    {
        transform.localScale  = Vector3.one;
        transform.rotation = Quaternion.identity;
    }
}
