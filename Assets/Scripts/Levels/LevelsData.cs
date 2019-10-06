using System.Collections.Generic;
using UnityEngine;

public class LevelsData : MonoBehaviour
{
    [Header("Ennemies")]
    [SerializeField] private GameObject shooter;

    private List<List<(GameObject, float)>> levels;

    public List<(GameObject, float)> GetLevel(int index)
    {
        if (index >= levels.Count)
        {
            return null;
        }

        return levels[index];
    }

    private void Start()
    {
        levels = new List<List<(GameObject, float)>>();
        FillLevels();
    }

    private void FillLevels()
    {
        // Level 1
        levels.Add(new List<(GameObject, float)>
        {
            (shooter, 1f),
            (shooter, 3f),
            (shooter, 5f),
            (shooter, 7f),
        });

        // Level 2
        levels.Add(new List<(GameObject, float)>
        {
            (shooter, 1f),
            (shooter, 3f),
            (shooter, 5f),
            (shooter, 7f),
        });
    }
}
