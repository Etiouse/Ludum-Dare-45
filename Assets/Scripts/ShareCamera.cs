using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShareCamera : MonoBehaviour
{
    public static Camera SharedCam;

    private void Awake()
    {
        SharedCam = GetComponent<Camera>();
    }
}
