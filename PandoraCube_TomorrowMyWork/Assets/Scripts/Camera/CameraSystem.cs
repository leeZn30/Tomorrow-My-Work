using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public static Camera currentCamera;
}
