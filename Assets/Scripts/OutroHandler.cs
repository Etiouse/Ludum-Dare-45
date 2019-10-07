using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text characterText;
    [SerializeField] private TMP_Text roomText;

    private float elapsedTime;

    private void Awake()
    {
        characterText.enabled = false;
        roomText.enabled = false;

        elapsedTime = 0;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 2)
        {
            roomText.enabled = true;
        }

        if (elapsedTime > 5)
        {
            roomText.enabled = false;
            characterText.enabled = true;
        }

        if (elapsedTime > 10)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
