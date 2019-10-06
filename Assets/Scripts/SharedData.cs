using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SharedData : MonoBehaviour
{
    public const float DEFAULT_SCREEN_WIDTH = 1920;

    public static Camera SharedCam;

    private void Awake()
    {
        SharedCam = GetComponent<Camera>();
    }

    public static float GetScreenRatio()
    {
        return Screen.width / DEFAULT_SCREEN_WIDTH;
    }
}
