using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text characterText;
    [SerializeField] private TMP_Text enemyText1;
    [SerializeField] private TMP_Text enemyText2;

    private float elapsedTime;

    private void Awake()
    {
        characterText.enabled = false;
        enemyText1.enabled = false;
        enemyText2.enabled = false;

        elapsedTime = 0;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 2)
        {
            characterText.enabled = true;
        }

        if (elapsedTime > 7)
        {
            characterText.enabled = false;
            enemyText1.enabled = true;
        }

        if (elapsedTime > 14)
        {
            enemyText1.enabled = false;
            enemyText2.enabled = true;
        }

        if (elapsedTime > 21)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
