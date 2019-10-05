using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPowerBloc : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Color spriteColor;

    private bool isMoving;

    public void OnPointerDown(PointerEventData eventData)
    {
        isMoving = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMoving = false;
    }

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = spriteColor;
        }
    }

    private void Update()
    {
        CheckIsMoving();
    }

    private void CheckIsMoving()
    {
        if (isMoving)
        {
            Vector3 mousePos = GetMousePosInApplication();

            transform.position = mousePos;
        }
    }

    private Vector3 GetMousePosInApplication()
    {
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x < 0)
        {
            mousePos.x = 0;
        }
        else if (mousePos.x > Screen.width)
        {
            mousePos.x = Screen.width;
        }

        if (mousePos.y < 0)
        {
            mousePos.y = 0;
        }
        else if (mousePos.y > Screen.height)
        {
            mousePos.y = Screen.height;
        }

        return mousePos;
    }
}
