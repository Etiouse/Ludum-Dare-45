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
    [SerializeField] private GameObject middleBoss;

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
            (shooter, 4f),
            (shooter, 4f),
        });


        // Level 2
        levels.Add(new List<(GameObject, float)>
        {
            (cone, 5f),
            (cone, 5f),
            (shooter, 12f),
            (shooter, 12f),
            (shooter, 20f),
            (cone, 25f),
        });

        // Level 3
        levels.Add(new List<(GameObject, float)>
        {
            (dasher, 4f),
            (shooter, 12f),
            (shooter, 12f),
            (cone, 20f),
            (dasher, 30f),
            (dasher, 35f),
            (dasher, 40f),
            (dasher, 45f),
        });

        // Level 4
        levels.Add(new List<(GameObject, float)>
        {
            (healer, 4f),
            (shooter, 12f),
            (shooter, 12f),
            (shooter, 20f),
            (healer, 30f),
            (cone, 40f),
            (cone, 45f),
            (cone, 50f),
            (dasher, 60f),
            (dasher, 65f),
            (dasher, 70f),
        });

        // Level 5
        levels.Add(new List<(GameObject, float)>
        {
            (waterSplasher, 4f),
            (braum, 4f),
            (waterSplasher, 12f),
            (dasher, 20f),
            (dasher, 25f),
            (dasher, 30f),
            (shooter, 40f),
            (shooter, 40f),
            (dasher, 50f),
            (dasher, 55f),
            (braum, 70f),
            (braum, 75f),
            (waterSplasher, 80f),
            (cone, 90f),
            (cone, 100f),
        });

        // Level 6
        levels.Add(new List<(GameObject, float)>
        {
            (middleBoss, 4f),
        });

        // Level 7
        levels.Add(new List<(GameObject, float)>
        {
            (braum, 4f),
            (braum, 4f),
            (braum, 4f),
            (waterSplasher, 20f),
            (dasher, 20f),
            (waterSplasher, 30f),
            (dasher, 30f),
            (healer, 50f),
            (braum, 50f),
            (braum, 50f),
            (cone, 60f),
            (cone, 75f),
            (cone, 80f),
        });

        // Level 8
        levels.Add(new List<(GameObject, float)>
        {
            (braum, 4f),
            (braum, 10f),
            (healer, 20f),
            (dasher, 20),
            (dasher, 20),
            (dasher, 25),
            (braum, 30f),
            (healer, 40f),
            (braum, 50f),
            (braum, 60f),
            (waterSplasher, 65f),
            (waterSplasher, 65f),
            (waterSplasher, 65f),
            (braum, 70f),
            (healer, 80f),
            (shooter, 80),
            (shooter, 80),
            (shooter, 80),
            (braum, 90f),
            (braum, 100f),
        });

        // Level 9
        levels.Add(new List<(GameObject, float)>
        {
            (braum, 4f),
            (braum, 10f),
            (healer, 20f),
            (dasher, 20),
            (dasher, 20),
            (dasher, 25),
            (dasher, 25),
            (braum, 30f),
            (healer, 40f),
            (braum, 50f),
            (waterSplasher, 55),
            (waterSplasher, 55),
            (waterSplasher, 55),
            (braum, 60f),
            (dasher, 61f),
            (dasher, 63f),
            (dasher, 65f),
            (shooter, 70f),
            (shooter, 70f),
            (shooter, 70f),
            (braum, 70f),
            (dasher, 75),
            (healer, 80f),
            (dasher, 85),
            (braum, 90f),
            (dasher, 95),
            (braum, 100f),
        });

        // Level 10
        levels.Add(new List<(GameObject, float)>
        {
            (dasher, 4f),
            (braum, 10f),
            (braum, 10f),
            (dasher, 10f),
            (dasher, 20f),
            (dasher, 25f),
            (dasher, 30f),
            (dasher, 35f),
            (healer, 40f),
            (healer, 40f),
            (dasher, 50f),
            (waterSplasher, 60f),
            (waterSplasher, 65f),
            (waterSplasher, 70f),
            (dasher, 90f),
            (dasher, 95f),
            (dasher, 100f),
            (shooter, 110f),
            (shooter, 110f),
            (shooter, 110f),
            (shooter, 120f),
            (shooter, 120f),
            (shooter, 120f),
            (healer, 140f),
            (healer, 140f),
            (cone, 140f),
            (cone, 140f),
            (dasher, 140f),
            (braum, 160f),
            (dasher, 160f),
            (braum, 180f),
            (dasher, 180f),
        });

        // Level 11
        levels.Add(new List<(GameObject, float)>
        {
            (finalBoss, 4f),
        });
    }
}
