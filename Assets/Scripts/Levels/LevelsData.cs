using System.Collections.Generic;
using UnityEngine;

public class LevelsData : MonoBehaviour
{
    [Header("Ennemies")]
    [SerializeField] private GameObject braum;
    [SerializeField] private GameObject cone;
    [SerializeField] private GameObject dasher;
    [SerializeField] private GameObject finalBoss;
    [SerializeField] private GameObject healer;
    [SerializeField] private GameObject shooter;
    [SerializeField] private GameObject waterSplasher;

    private int counter;
    private List<List<(GameObject, float)>> levels;

    public List<(GameObject, float)> NextLevel()
    {
        if (counter >= levels.Count)
        {
            return null;
        }

        return levels[counter++];
    }

    private void Start()
    {
        levels = new List<List<(GameObject, float)>>();
        FillLevels();
        counter = 0;
    }

    private void FillLevels()
    {
        // Level 1
        levels.Add(new List<(GameObject, float)>
        {
            (braum, 1f),
            (cone, 1f),
            (dasher, 1f),
            (healer, 1f),
            (shooter, 1f),
            (waterSplasher, 1f),
        });

        // Level 2
        levels.Add(new List<(GameObject, float)>
        {
            (healer, 1f),
        });

        // Level 2
        levels.Add(new List<(GameObject, float)>
        {
            (dasher, 1f),
        });
    }
}
