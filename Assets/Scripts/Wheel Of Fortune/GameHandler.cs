﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public delegate void SetWheelRulesAction(List<Rule> rules);
    public delegate void TransmitNewPowerAction(GameObject newPower);
    public static event SetWheelRulesAction OnSetWheelRulesAction;
    public static event TransmitNewPowerAction OnTransmitNewPowerAction;

    [Header("Wheel of fortune")]
    [SerializeField] Canvas wheelCanvas = null;
    [SerializeField] GameObject winner = null;
    [SerializeField] TMP_Text description = null;
    [SerializeField] Button continueButton = null;
    [SerializeField] List<Rule> rules = null;

    [Header("General informations")]
    [SerializeField] GameObject levelsContainer;
    [SerializeField] GameObject player;
    [SerializeField] Camera mainCam;

    private enum GameState { LOAD_LEVEL, PLAYING, WHEEL, INVENTORY };
    private enum SwapState { FADE_OUT, DISPLAY_WHEEL, WAIT_CHOICE, FADE_IN, FINISHED };

    private GameObject display;
    private GameObject currentLevel;
    private LevelManager levelManager;
    private List<(GameObject, float)> currentEnnemies;
    private List<GameObject> currentSpawns;
    private List<GameObject> levelsLeft;
    private LevelsData levelsData;
    private Vector3 initCamPos;
    private GameState gameState;
    private SwapState swapState;

    private float timePassed;
    private float initSwapTime;
    private bool swapDirectionLeft;

    private const float SCALE = 0.5f;
    private const float SWAP_DURATION = 1.5f;
    private const float CAMERA_MAX_OFFSET = 100;

    private void OnEnable()
    {
        WheelOfFortune.OnResultAction += ResultRule;
    }

    private void OnDisable()
    {
        WheelOfFortune.OnResultAction -= ResultRule;
    }

    private void Start()
    {
        // Variables
        timePassed = 0f;

        continueButton.gameObject.SetActive(false);

        // Fill the levels in script
        levelsLeft = new List<GameObject>();
        for (int i = 0; i < levelsContainer.transform.childCount; i++)
        {
            levelsLeft.Add(levelsContainer.transform.GetChild(i).gameObject);
        }
        
        levelsData = levelsContainer.GetComponent<LevelsData>();

        gameState = GameState.LOAD_LEVEL;
    }

    private void Update()
    {
        timePassed += Time.deltaTime;

        switch (gameState)
        {
            case GameState.LOAD_LEVEL:
                LoadLevel();
                break;
            case GameState.PLAYING:
                Playing();
                break;
            case GameState.WHEEL:
                Wheel();
                break;
            default:
                break;
        }
    }

    private void LoadLevel()
    {
        if (currentLevel != null)
        {
            currentLevel.SetActive(false);
            timePassed = 0f;
        }

        currentLevel = NextLevel();
        if (currentLevel != null)
        {
            // Activate level
            currentLevel.SetActive(true);

            // Retrieve level infos
            string levelName = currentLevel.gameObject.name;
            if (levelName.Length > 2)
            {
                int levelIndex = (int)char.GetNumericValue(levelName[levelName.Length - 1]) - 1;
                currentEnnemies = new List<(GameObject, float)>(levelsData.GetLevel(levelIndex));

                if (currentEnnemies != null)
                {
                    levelManager = currentLevel.GetComponent<LevelManager>();
                    currentSpawns = new List<GameObject>(levelManager.GetSpawns());

                    player.transform.position = NextSpawnPoint();

                    gameState = GameState.PLAYING;
                }
                else
                {
                    Debug.Log("Can't load ennemies of " + levelName);
                }
            }
            else
            {
                Debug.Log("Error on level name: " + levelName);
            }
        }
        else
        {
            // TODO END
        }
    }

    private void Playing()
    {
        timePassed += Time.deltaTime;

        // Spawns ennemies
        for (int i = currentEnnemies.Count - 1; i >= 0; i--)
        {
            (GameObject, float) ennemy = currentEnnemies[i];

            float spawnTime = ennemy.Item2;
            if (timePassed > spawnTime)
            {
                GameObject nextEnnemy = Instantiate(ennemy.Item1);
                currentLevel.GetComponent<LevelManager>().AddEnnemy(nextEnnemy);

                nextEnnemy.transform.position = NextSpawnPoint();
                currentEnnemies.RemoveAt(i);
            }
        }

        // End of the level
        if (currentEnnemies.Count == 0 && levelManager.AreAllEnnemiesDead())
        {
            gameState = GameState.WHEEL;
            swapState = SwapState.FADE_OUT;
            initCamPos = mainCam.transform.position;
            initSwapTime = Time.time;
        }
    }

    private void Wheel()
    {
        float swapPassed = Time.time - initSwapTime;
        float factor = Mathf.Log(CAMERA_MAX_OFFSET) / Mathf.Log(SWAP_DURATION);
        float direction = swapDirectionLeft ? -1 : 1;

        switch (swapState)
        {
            case SwapState.FADE_OUT:
                float cameraOffsetOut = Mathf.Pow(swapPassed, factor);
                mainCam.transform.position = initCamPos + direction * new Vector3(cameraOffsetOut, 0, 0);
                if (swapPassed > SWAP_DURATION)
                {
                    swapState = SwapState.DISPLAY_WHEEL;
                    wheelCanvas.gameObject.SetActive(true);
                    OnSetWheelRulesAction(rules);
                }
                break;

            case SwapState.DISPLAY_WHEEL:
                break;

            /*case SwapState.WAIT_CHOICE:
                break;

            case SwapState.FADE_IN:
                if (Mathf.Abs(SWAP_DURATION - timePassed) > 0.1f)
                {
                    float cameraOffsetIn = Mathf.Pow(SWAP_DURATION - timePassed, factor);
                    mainCam.transform.position = initCamPos - direction * new Vector3(cameraOffsetIn, 0, 0);
                }
                if (timePassed >= SWAP_DURATION)
                {
                    swapState = SwapState.FINISHED;
                }
                break;

            case SwapState.FINISHED:
                mainCam.transform.position = initCamPos;
                swapLevel = false;
                Description.text = "";
                currentGameState = GameState.START;
                break;*/

            default:
                break;
        }
    }

    private GameObject NextLevel()
    {
        if (levelsLeft.Count > 0)
        {
            int rand = Random.Range(0, levelsLeft.Count);
            GameObject nextLevel = levelsLeft[rand];
            levelsLeft.RemoveAt(rand);

            return nextLevel;
        }

        return null;
    }

    private Vector3 NextSpawnPoint()
    {
        int rand = Random.Range(0, currentSpawns.Count);
        GameObject spawn = currentSpawns[rand];
        return spawn.transform.position;
    }

    private void ResultRule(GameObject power)
    {
        if (power != null)
        {
            if (display != null)
            {
                Destroy(display);
            }

            display = Instantiate(power);
            display.transform.SetParent(winner.transform);
            display.transform.localScale = new Vector3(SCALE, SCALE, SCALE);
            display.transform.localPosition = new Vector3(100, 0, 0);
            display.SetActive(true);

            description.text = power.GetComponent<PowerShape>().Description;
            continueButton.gameObject.SetActive(true);

            //OnTransmitNewPowerAction(power);
        }
        else
        {
            Debug.Log(":(");
        }
    }
}
