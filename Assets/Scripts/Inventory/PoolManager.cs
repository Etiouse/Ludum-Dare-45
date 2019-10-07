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
    [SerializeField] private GameObject models = null;
    [SerializeField] private List<GameObject> powerShapeModels = null;

    public List<GameObject> UsedPowerShapes { get; private set; }

    private List<GameObject> slotsElements;
    private List<GameObject> availableElements;
    private List<GameObject> visibleElements;

    private List<int> currentElementInPool;

    private int firstVisibleElement;

    private bool updateVisibleElements;
    private bool canIncrease;
    private bool canReduce;

    public void IncreaseFirstVisibleElementIndex()
    {
        bool isOk = false;

        while (!isOk)
        {
            firstVisibleElement++;

            GameObject powerShape = availableElements[currentElementInPool[firstVisibleElement]];
            if (!UsedPowerShapes.Contains(powerShape) ||
                firstVisibleElement > availableElements.Count)
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
            firstVisibleElement--;

            GameObject powerShape = availableElements[currentElementInPool[firstVisibleElement]];
            if (!UsedPowerShapes.Contains(powerShape) ||
                firstVisibleElement <= 0)
            {
                isOk = true;
            }
        }

        updateVisibleElements = true;
    }

    private void Awake()
    {
        currentElementInPool = new List<int>();
        visibleElements = new List<GameObject>();
        UsedPowerShapes = new List<GameObject>();

        availableElements = new List<GameObject>();
        for (int i = 0; i < powerShapeModels.Count; i++)
        {
            GameObject model = Instantiate(powerShapeModels[i]);
            model.transform.SetParent(models.transform);
            availableElements.Add(model);
        }

        slotsElements = new List<GameObject>();
        for (int i = 0; i < slotsParent.transform.childCount; i++)
        {
            slotsElements.Add(slotsParent.transform.GetChild(i).gameObject);
        }

        firstVisibleElement = 0;
        updateVisibleElements = true;

        TMPAddAllPowerShapesToPool();
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
        TMPCheckIfNewPowerShape();

        CheckChangeVisibleElementsInPool();

        UpdateSlotsElements();
    }

    private void TMPCheckIfNewPowerShape()
    {
        // TODO : Add element between each levels
        for (int i = 0; i < 10; i++)
        {
            KeyCode key = KeyCode.Alpha1 + i;

            if (Input.GetKeyDown(key))
            {
                if (!currentElementInPool.Contains(i))
                {
                    currentElementInPool.Add(i);

                    updateVisibleElements = true;
                }
            }
        }
    }

    private void CheckChangeVisibleElementsInPool()
    {
        canReduce = firstVisibleElement > 0;
        canIncrease = firstVisibleElement + (slotsElements.Count - 1) < currentElementInPool.Count - 1;

        inscreaseButton.enabled = canIncrease;
        reduceButton.enabled = canReduce;
    }

    private void TMPAddAllPowerShapesToPool()
    {
        // TODO : Remove when the power shapes are added between each levels
        for (int i = 0; i < availableElements.Count; i++)
        {
            currentElementInPool.Add(i);
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
            int min = firstVisibleElement;
            int max = firstVisibleElement + slotsElements.Count - 1;
            int numberOfElementToDisplay = slotsElements.Count;
            int numberOfElementInPool = currentElementInPool.Count - UsedPowerShapes.Count;

            //Debug.Log("-:- " + UsedPowerShapes.Count);
            //Debug.Log("-- " + min + " " + max);

            if (numberOfElementToDisplay > numberOfElementInPool)
            {
                numberOfElementToDisplay = numberOfElementInPool;
            }

            if (max > currentElementInPool.Count - 1)
            {
                max = currentElementInPool.Count - 1;
                min = max - numberOfElementToDisplay - 1;

                if (min < 0)
                {
                    min = 0;
                }

                //Debug.Log("OK1");
            }
            else if (min < 0)
            {
                min = 0;
                max = numberOfElementToDisplay - 1;
                //Debug.Log("OK1");
            }

            //Debug.Log(min + " " + max);

            // Create elements and add them to next available slot
            int currentSlotIndex = 0;
            int counter = 0;
            int offset = 0;
            int index;
            for (int i = min; i <= max; i++)
            {
                index = i + offset;
                //Debug.Log("--<> " + index);
                GameObject powerShape = availableElements[currentElementInPool[index]];
                powerShape.SetActive(true);

                if (!UsedPowerShapes.Contains(powerShape))
                {
                    visibleElements.Add(powerShape);

                    visibleElements[currentSlotIndex].transform.SetParent(slotsElements[currentSlotIndex].transform);
                    visibleElements[currentSlotIndex].transform.localScale = Vector3.one;
                    visibleElements[currentSlotIndex].transform.localPosition = Vector3.zero;
                    currentSlotIndex++;
                }
                else
                {
                    i--;
                    offset++;
                    //Debug.Log("OKOKOK");
                }

                counter++;
                if (counter >= availableElements.Count)
                {
                    //Debug.Log("OUPS");
                    i = max + 1;
                }
            }
        }
    }

    private void ConfirmPowerShape(GameObject powerShape, bool isConfirmed)
    {
        if (isConfirmed)
        {
            UsedPowerShapes.Add(powerShape);
        }
        else if (UsedPowerShapes.Contains(powerShape))
        {
            UsedPowerShapes.Remove(powerShape);

            // Remove all dependencies
            foreach (GameObject item in UsedPowerShapes)
            {
                foreach (PowerShape.Type type in powerShape.GetComponent<PowerShape>().Enabled)
                {
                    if (item.GetComponent<PowerShape>().PowerShapeType == type)
                    {
                        UsedPowerShapes.Remove(item);
                    }
                }
            }
        }

        updateVisibleElements = true;
        OnUpdateUsedPowerShapesEvent();

        //Debug.Log(UsedPowerShapes.Count);
    }
}
