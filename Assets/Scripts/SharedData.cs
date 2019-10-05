using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SharedData : MonoBehaviour
{
    public const float DEFAULT_SCREEN_WIDTH = 1920;

    public static Camera SharedCam;
    public static float ScreenRatio;

    private void Awake()
    {
        SharedCam = GetComponent<Camera>();
        ScreenRatio = Screen.width / DEFAULT_SCREEN_WIDTH;
    }
}
