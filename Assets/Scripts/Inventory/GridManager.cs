using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private GameObject currentPowerShape;
    private List<GameObject> gridCase;

    private void OnEnable()
    {
        InventoryPowerBloc.OnUpdateSelectedPowerShapeEvent += UpdatePowerShape;
    }

    private void OnDisable()
    {
        InventoryPowerBloc.OnUpdateSelectedPowerShapeEvent -= UpdatePowerShape;
    }

    private void Awake()
    {
        gridCase = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            for (int j = 0; j < child.childCount; j++)
            {
                gridCase.Add(child.GetChild(j).gameObject);
            }
        }
    }

    private void Update()
    {
        bool isPowerShapeOnOneCase = false;

        foreach (GameObject item in gridCase)
        {
            if (item.GetComponent<GridCaseManager>().IsPowerShapeOnCase)
            {
                isPowerShapeOnOneCase = true;
            }
        }

        if (currentPowerShape != null)
        {
            currentPowerShape.GetComponent<InventoryPowerBloc>().IsOnInventoryCase = isPowerShapeOnOneCase;
        }
    }

    private void UpdatePowerShape(GameObject powerShape)
    {
        currentPowerShape = powerShape;
    }
}
