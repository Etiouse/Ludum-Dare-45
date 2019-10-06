using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private PoolManager poolManager = null;
    [SerializeField] private TMP_Text activePowerTextModel = null;
    [SerializeField] private GameObject activePowerColsParent = null;

    private List<PowerShape> powerShapes;
    private List<TMP_Text> activePowerTexts;
    private List<GameObject> activePowerCols;

    private int maxNumberOfElementInOneCol;

    private void Awake()
    {
        powerShapes = new List<PowerShape>();
        activePowerTexts = new List<TMP_Text>();
        activePowerCols = new List<GameObject>();
        for (int i = 0; i < activePowerColsParent.transform.childCount; i++)
        {
            activePowerCols.Add(activePowerColsParent.transform.GetChild(i).gameObject);
        }

        maxNumberOfElementInOneCol = (int)(activePowerCols[0].GetComponent<RectTransform>().rect.height /
            (activePowerTextModel.GetComponent<RectTransform>().rect.height + 20));
        //Debug.Log(maxNumberOfElementInOneCol);
    }

    private void OnEnable()
    {
        PoolManager.OnUpdateUsedPowerShapesEvent += UpdateUsedPowerShapes;
    }

    private void OnDisable()
    {
        PoolManager.OnUpdateUsedPowerShapesEvent -= UpdateUsedPowerShapes;
    }

    private void UpdateActivePower()
    {
        // TODO : Update values for power

        foreach (TMP_Text item in activePowerTexts)
        {
            DestroyImmediate(item.gameObject);
        }
        activePowerTexts.Clear();
        //Debug.Log("--> " + activePowerCols[0].transform.childCount);

        int currentCol = 0;
        foreach (PowerShape powerShape in powerShapes)
        {
            if (activePowerCols[currentCol].transform.childCount > maxNumberOfElementInOneCol)
            {
                currentCol++;
            }

            TMP_Text text = Instantiate(activePowerTextModel);
            text.text = powerShape.DisplayedName;
            // TODO : Change col when full
            text.transform.SetParent(activePowerCols[currentCol].transform);
            text.transform.localScale = Vector3.one;

            activePowerTexts.Add(text);
        }
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

    private void UpdateUsedPowerShapes()
    {
        UpdatePowerShapesList();

        UpdateActivePower();
    }
}
