using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject blocHighlightModel = null;
    [SerializeField] private PoolManager poolManager = null;
    [SerializeField] private GameObject activePowerColsParent = null;
    [Header("Text Field")]
    [SerializeField] private TMP_Text activePowerTextModel = null;
    [SerializeField] private TMP_Text powerShapeTitle = null;
    [SerializeField] private TMP_Text powerShapeDesc = null;
    [SerializeField] private TMP_Text warningMsg = null;

    private List<PowerShape> powerShapes;
    private List<TMP_Text> activePowerTexts;
    private List<GameObject> activePowerCols;

    private int maxNumberOfElementInOneCol;

    private bool isChangingDisplayEnabled;

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

        powerShapeTitle.text = "";
        powerShapeDesc.text = "";

        isChangingDisplayEnabled = true;
        //Debug.Log(maxNumberOfElementInOneCol);
    }

    private void OnEnable()
    {
        PoolManager.OnUpdateUsedPowerShapesEvent += UpdateUsedPowerShapes;
        PowerShape.OnMouseOverPowerShapeEvent += UpdateMouseOverTitleAndDesc;
        PowerShape.OnEnableChangingDisplayEvent += EnableChangingDisplay;
    }

    private void OnDisable()
    {
        PoolManager.OnUpdateUsedPowerShapesEvent -= UpdateUsedPowerShapes;
        PowerShape.OnMouseOverPowerShapeEvent -= UpdateMouseOverTitleAndDesc;
        PowerShape.OnEnableChangingDisplayEvent -= EnableChangingDisplay;
    }

    private void UpdateActivePower()
    {
        //Reset
        PlayerCharacteristics.ResetAll();
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
            PlayerCharacteristics.ChangeValue(powerShape.PowerShapeType, true);
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

    private void EnableChangingDisplay(bool value)
    {
        isChangingDisplayEnabled = value;
    }

    private void UpdateMouseOverTitleAndDesc(PowerShape powerShape)
    {
        if (isChangingDisplayEnabled)
        {
            if (powerShape == null)
            {
                powerShapeTitle.text = "";
                powerShapeDesc.text = "";
                warningMsg.enabled = false;
            }
            else
            {
                Transform currentTransform = powerShape.gameObject.transform;

                for (int i = 0; i < currentTransform.childCount; i++)
                {
                    GameObject item = Instantiate(blocHighlightModel);
                    item.transform.SetParent(currentTransform.GetChild(i));
                    item.transform.localScale = Vector3.one;
                    item.transform.localPosition = Vector3.zero;
                }

                powerShapeTitle.text = powerShape.DisplayedName;
                powerShapeDesc.text = powerShape.Description;

                if (!powerShape.CanBePlacedOnInventory())
                {
                    warningMsg.enabled = true;
                }
            }
        }
    }
}
