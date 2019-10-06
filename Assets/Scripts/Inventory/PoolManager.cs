using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Button inscreaseButton;
    [SerializeField] private Button reduceButton;
    [SerializeField] private GameObject slotsParent;
    [SerializeField] private List<GameObject> powerShapeModels;

    private List<GameObject> slotsElements;
    private List<GameObject> availableElements;
    private List<GameObject> currentVisibleElements;
    private List<GameObject> currentUsedElements;

    private List<int> currentElementInPool;

    private int firstVisibleElement;

    private bool updateVisibleElements;
    private bool canIncrease;
    private bool canReduce;

    public void IncreaseFirstVisibleElementIndex()
    {
        firstVisibleElement++;

        updateVisibleElements = true;
    }

    public void ReduceFirstVisibleElementIndex()
    {
        firstVisibleElement--;

        updateVisibleElements = true;
    }

    private void OnEnable()
    {
        InventoryPowerBloc.OnConfirmPowerShapeEvent += UpdatePowerShape;
    }

    private void OnDisable()
    {
        InventoryPowerBloc.OnConfirmPowerShapeEvent -= UpdatePowerShape;
    }

    private void Awake()
    {
        currentElementInPool = new List<int>();
        currentVisibleElements = new List<GameObject>();
        currentUsedElements = new List<GameObject>();

        availableElements = new List<GameObject>();
        for (int i = 0; i < powerShapeModels.Count; i++)
        {
            availableElements.Add(Instantiate(powerShapeModels[i]));
        }

        slotsElements = new List<GameObject>();
        for (int i = 0; i < slotsParent.transform.childCount; i++)
        {
            slotsElements.Add(slotsParent.transform.GetChild(i).gameObject);
        }

        firstVisibleElement = 0;
        updateVisibleElements = true;

        TMPAddAllPowerShapes();
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
        canIncrease = firstVisibleElement + (slotsElements.Count - 1) < currentElementInPool.Count - 1 - currentUsedElements.Count;

        inscreaseButton.enabled = canIncrease;
        reduceButton.enabled = canReduce;
    }

    private void TMPAddAllPowerShapes()
    {
        // TODO : Remove when the power shapes
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
                if (!currentUsedElements.Contains(item) &&
                    !item.GetComponent<InventoryPowerBloc>().IsMoving)
                {
                    item.SetActive(false);
                }
            }
            currentVisibleElements.Clear();

            // Find the min and max index of element to display in the current pool elements
            int min = firstVisibleElement;
            int max = firstVisibleElement + slotsElements.Count - 1;
            int numberOfElementToDisplay = slotsElements.Count;
            int numberOfElementInPool = currentElementInPool.Count - currentUsedElements.Count;

            Debug.Log("-:- " + currentUsedElements.Count);
            Debug.Log("-- " + min + " " + max);

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

                Debug.Log("OK1");
            }
            else if (min < 0)
            {
                min = 0;
                max = numberOfElementToDisplay - 1;
                Debug.Log("OK1");
            }

            Debug.Log(min + " " + max);

            // Create elements and add them to next available slot
            int currentSlotIndex = 0;
            int counter = 0;
            int offset = 0;
            int index;
            for (int i = min; i <= max; i++)
            {
                index = i + offset;
                GameObject powerShape = availableElements[currentElementInPool[index]];
                powerShape.SetActive(true);

                if (!currentUsedElements.Contains(powerShape))
                {
                    currentVisibleElements.Add(powerShape);

                    currentVisibleElements[currentSlotIndex].transform.SetParent(slotsElements[currentSlotIndex].transform);
                    currentVisibleElements[currentSlotIndex].transform.localScale = Vector3.one;
                    currentVisibleElements[currentSlotIndex].transform.localPosition = Vector3.zero;
                    currentSlotIndex++;
                }
                else
                {
                    i--;
                    offset++;
                    Debug.Log("OKOKOK");
                }

                counter++;
                if (counter >= availableElements.Count)
                {
                    Debug.Log("OUPS");
                    i = max + 1;
                }
            }
        }
    }

    private void UpdatePowerShape(GameObject powerShape, bool isConfirmed)
    {
        if (isConfirmed)
        {
            currentUsedElements.Add(powerShape);
        }
        else if (currentUsedElements.Contains(powerShape))
        {
            currentUsedElements.Remove(powerShape);
        }

        updateVisibleElements = true;

        Debug.Log(currentUsedElements.Count);
    }
}
