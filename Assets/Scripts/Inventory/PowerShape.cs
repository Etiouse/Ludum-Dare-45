using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PowerShape : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void UpdateSelectedPowerShapeEvent(GameObject powerShape);
    public static event UpdateSelectedPowerShapeEvent OnUpdateSelectedPowerShapeEvent;

    public delegate void ConfirmPowerShapeEvent(GameObject powerShape, bool isConfirmed);
    public static event ConfirmPowerShapeEvent OnConfirmPowerShapeEvent;

    public enum PowerShapeType
    {
        FIRE_BALL,
        ICE_BALL,
        ROCK_SHILD,
        AIR_SHILD
    }

    public Color SpriteColor;
    public string DisplayedName;
    public string Description;
    public PowerShapeType CurrentPowerShapeType;

    public bool IsOnInventoryCase { get; set; }
    public bool IsMoving { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsMoving = true;
        OnUpdateSelectedPowerShapeEvent(gameObject);

        OnConfirmPowerShapeEvent(gameObject, false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsMoving = false;
        OnUpdateSelectedPowerShapeEvent(null);
        Debug.Log(IsOnInventoryCase);
        OnConfirmPowerShapeEvent(gameObject, IsOnInventoryCase);
    }

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = SpriteColor;
        }

        IsOnInventoryCase = false;
    }

    private void Update()
    {
        CheckIsMoving();
    }

    private void CheckIsMoving()
    {
        if (IsMoving)
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
