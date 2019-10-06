using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject ennemies = null;
    [SerializeField] private GameObject spawns = null;

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

    public bool AreAllEnnemiesDead()
    {
        return ennemies.transform.childCount == 0;
    }
}
