using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCamera : MonoBehaviour
{
    private void Start()
    {
        CameraSystem.currentCamera = GetComponent<Camera>();
        EventManager.Instance.OnCameraChanged.Raise();
    }
}
