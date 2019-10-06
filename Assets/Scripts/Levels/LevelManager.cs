using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject walls;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject ennemies;
    [SerializeField] private GameObject spawns;

    public List<GameObject> GetSpawns()
    {
        List<GameObject> spawnsList = new List<GameObject>();
        for (int i = 0; i < spawns.transform.childCount; i++)
        {
            spawnsList.Add(spawns.transform.GetChild(i).gameObject);
        }

        return spawnsList;
    }

    public void AddEnnemy(GameObject ennemy)
    {
        ennemy.transform.SetParent(ennemies.transform);
    }
}
