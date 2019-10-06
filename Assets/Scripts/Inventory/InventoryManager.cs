using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private PoolManager poolManager;

    private List<PowerShape> powerShapes;

    private void Awake()
    {
        powerShapes = new List<PowerShape>();
    }

    private void Update()
    {
        UpdatePowerShapesList();

        // TODO : Update enum values for power
    }

    private void UpdatePowerShapesList()
    {
        List<GameObject> usedPowerShapes = poolManager.UsedPowerShapes;
        PowerShape currentPowerShape;

        powerShapes.Clear();
        for (int i = 0; i < usedPowerShapes.Count; i++)
        {
            currentPowerShape = usedPowerShapes[i].GetComponent<PowerShape>();
            powerShapes.Add(currentPowerShape);
        }
    }
}
