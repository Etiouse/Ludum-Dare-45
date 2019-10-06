using System.Collections.Generic;
using UnityEngine;

public class LevelsData : MonoBehaviour
{
    [Header("Ennemies")]
    [SerializeField] private GameObject shooter;

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
            (shooter, 1f),
        });

        // Level 2
        levels.Add(new List<(GameObject, float)>
        {
            (shooter, 1f),
        });
    }
}
