using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolManager : MonoBehaviour
{
    public delegate void UpdateUsedPowerShapesEvent();
    public static event UpdateUsedPowerShapesEvent OnUpdateUsedPowerShapesEvent;

    [SerializeField] private Button inscreaseButton = null;
    [SerializeField] private Button reduceButton = null;
    [SerializeField] private GameObject slotsParent = null;
    [SerializeField] private GameObject powerShapeParent = null;
    [SerializeField] private List<GameObject> powerShapeModelsDebugPurpose = null;

    public List<GameObject> UsedPowerShapes { get; private set; }

    private List<GameObject> slotsElements;
    private List<GameObject> availableElements;
    private List<GameObject> visibleElements;

    private List<int> currentElementInPoolIndex;
    private List<int> availableElementIndex;
    private List<int> usedPowerShapesIndex;

    private int firstVisibleElementIndex;

    private bool updateVisibleElements;
    private bool canIncrease;
    private bool canReduce;

    public void IncreaseFirstVisibleElementIndex()
    {
        bool isOk = false;

        while (!isOk)
        {
            firstVisibleElementIndex++;

            GameObject powerShape = availableElements[currentElementInPoolIndex[firstVisibleElementIndex]];

            if (!UsedPowerShapes.Contains(powerShape) ||
                firstVisibleElementIndex >= availableElements.Count)
            {
                isOk = true;
            }
        }

        updateVisibleElements = true;
    }

    public void ReduceFirstVisibleElementIndex()
    {
        bool isOk = false;

        while (!isOk)
        {
            firstVisibleElementIndex--;

            GameObject powerShape = availableElements[currentElementInPoolIndex[firstVisibleElementIndex]];

            if (!UsedPowerShapes.Contains(powerShape) ||
                firstVisibleElementIndex <= 0)
            {
                isOk = true;
            }
        }

        updateVisibleElements = true;
    }

    public void AddOnePowerShapeToPool(GameObject powerShape)
    {
        powerShape.transform.SetParent(powerShapeParent.transform);

        availableElements.Add(powerShape);
        availableElementIndex.Add(availableElements.Count - 1);
        currentElementInPoolIndex.Add(availableElementIndex.Count - 1);

        updateVisibleElements = true;
    }

    private void Awake()
    {
        visibleElements = new List<GameObject>();
        UsedPowerShapes = new List<GameObject>();

        availableElements = new List<GameObject>();

        currentElementInPoolIndex = new List<int>();
        availableElementIndex = new List<int>();
        usedPowerShapesIndex = new List<int>();

        slotsElements = new List<GameObject>();
        for (int i = 0; i < slotsParent.transform.childCount; i++)
        {
            slotsElements.Add(slotsParent.transform.GetChild(i).gameObject);
        }

        firstVisibleElementIndex = 0;
        updateVisibleElements = true;

        //AddAllPowerShapesToPoolDebug();
    }

    private void OnEnable()
    {
        PowerShape.OnConfirmPowerShapeEvent += ConfirmPowerShape;
    }

    private void OnDisable()
    {
        PowerShape.OnConfirmPowerShapeEvent -= ConfirmPowerShape;
    }

    private void Update()
    {
        CheckChangeVisibleElementsInPool();

        UpdateSlotsElements();
    }

    private void CheckChangeVisibleElementsInPool()
    {
        canReduce = firstVisibleElementIndex > 0;
        canIncrease = firstVisibleElementIndex + (slotsElements.Count - 1) < currentElementInPoolIndex.Count - 1;

        inscreaseButton.enabled = canIncrease;
        reduceButton.enabled = canReduce;
    }

    private void AddAllPowerShapesToPoolDebug()
    {
        foreach (GameObject item in powerShapeModelsDebugPurpose)
        {
            AddOnePowerShapeToPool(Instantiate(item));
        }

        for (int i = 0; i < availableElements.Count; i++)
        {
            currentElementInPoolIndex.Add(i);
        }
    }

    private void UpdateSlotsElements()
    {
        if (updateVisibleElements)
        {
            updateVisibleElements = false;

            // Reset
            foreach (GameObject item in availableElements)
            {
                if (!UsedPowerShapes.Contains(item) &&
                    !item.GetComponent<PowerShape>().IsMoving)
                {
                    item.SetActive(false);
                }
            }
            visibleElements.Clear();

            // Find the min and max index of element to display in the current pool elements
            int min = firstVisibleElementIndex;
            int max = firstVisibleElementIndex + slotsElements.Count - 1;
            int numberOfElementToDisplay = slotsElements.Count;
            int numberOfElementInPool = currentElementInPoolIndex.Count;

            //Debug.Log("-:- " + currentElementInPool.Count);
            //Debug.Log("-- " + min + " " + max);

            if (numberOfElementToDisplay > numberOfElementInPool)
            {
                numberOfElementToDisplay = numberOfElementInPool;
            }

            if (max > numberOfElementInPool - 1)
            {
                max = numberOfElementInPool - 1;
                min = max - numberOfElementToDisplay + 1;
                
                if (min < 0)
                {
                    min = 0;
                }
                
                Debug.Log("OK1");
            }
            else if (min < 0)
            {
                min = 0;
                max = numberOfElementToDisplay - 1;

                if (max > numberOfElementInPool - 1)
                {
                    max = numberOfElementInPool - 1;
                }
                Debug.Log("OK1");
            }

            //Debug.Log(min + " " + max);

            // Create elements and add them to next available slot
            int currentSlotIndex = 0;
            for (int i = min; i <= max; i++)
            {
                if (currentElementInPoolIndex.Count > 0)
                {
                    //Debug.Log("--<> " + i);
                    GameObject powerShape = availableElements[currentElementInPoolIndex[i]];
                    powerShape.SetActive(true);

                    visibleElements.Add(powerShape);

                    visibleElements[currentSlotIndex].transform.SetParent(slotsElements[currentSlotIndex].transform);
                    visibleElements[currentSlotIndex].transform.localScale = Vector3.one;
                    visibleElements[currentSlotIndex].transform.localPosition = Vector3.zero;
                    currentSlotIndex++;
                }
            }
        }
    }

    private void ConfirmPowerShape(GameObject powerShape, bool isConfirmed)
    {
        bool isOK = false;

        while (!isOK)
        {
            int availableIndex = availableElements.FindIndex(a => a == powerShape);
            int index = -1;

            isOK = true;

            for (int i = 0; i < availableElementIndex.Count; i++)
            {
                if (availableElementIndex[i] == availableIndex)
                {
                    index = i;
                }
            }
            Debug.Log("index : " + index);

            if (isConfirmed)
            {
                UsedPowerShapes.Add(powerShape);

                usedPowerShapesIndex.Add(index);

                if (index == firstVisibleElementIndex)
                {
                    firstVisibleElementIndex--;
                }
            }
            else if (UsedPowerShapes.Contains(powerShape))
            {
                UsedPowerShapes.Remove(powerShape);

                for (int i = usedPowerShapesIndex.Count - 1; i >= 0; i--)
                {
                    if (usedPowerShapesIndex[i] == index)
                    {
                        usedPowerShapesIndex.RemoveAt(i);
                    }
                }

                // Remove all dependencies
                foreach (GameObject item in UsedPowerShapes)
                {
                    foreach (PowerShape.Type type in powerShape.GetComponent<PowerShape>().Enabled)
                    {
                        if (item.GetComponent<PowerShape>().PowerShapeType == type)
                        {
                            powerShape = item;
                            isOK = false;
                        }
                    }
                }
            }
        }

        currentElementInPoolIndex.Clear();

        bool isUsed;
        foreach (int available in availableElementIndex)
        {
            isUsed = false;

            foreach (int used in usedPowerShapesIndex)
            {
                if (available == used)
                {
                    isUsed = true;
                }
            }

            if (!isUsed)
            {
                currentElementInPoolIndex.Add(available);
            }
        }

        Debug.Log("Elements : " + availableElementIndex.Count + " " + usedPowerShapesIndex.Count + " " + currentElementInPoolIndex.Count);

        updateVisibleElements = true;
        OnUpdateUsedPowerShapesEvent();
    }
}
