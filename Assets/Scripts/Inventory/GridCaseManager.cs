using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCaseManager : MonoBehaviour
{
    public bool IsPowerShapeOnCase { get; set; }

    private bool isMouseOnGridCase;
    private Rect rect;
    private GameObject currentPowerShape;

    private void OnEnable()
    {
        InventoryPowerBloc.OnUpdateSelectedPowerShapeEvent += UpdatePowerShape;
    }

    private void OnDisable()
    {
        InventoryPowerBloc.OnUpdateSelectedPowerShapeEvent -= UpdatePowerShape;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PowerBloc")
        {
            //Debug.Log("OK");
        }
    }

    private void Update()
    {
        rect = GetComponent<RectTransform>().rect;
        Vector3 casePos = transform.position;
        Vector3 mousePos = Input.mousePosition;

        isMouseOnGridCase = false;

        if (mousePos.x >= casePos.x - rect.width * SharedData.ScreenRatio / 2 &&
            mousePos.x <= casePos.x + rect.width * SharedData.ScreenRatio / 2 &&
            mousePos.y >= casePos.y - rect.height * SharedData.ScreenRatio / 2 &&
            mousePos.y <= casePos.y + rect.height * SharedData.ScreenRatio / 2)
        {
            isMouseOnGridCase = true;

            if (currentPowerShape != null)
            {
                currentPowerShape.transform.position = casePos;
                IsPowerShapeOnCase = true;
            }
        }
        else
        {
            if (currentPowerShape != null)
            {
                IsPowerShapeOnCase = false;
            }
        }
    }

    private void LateUpdate()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere((transform.position), 5);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere((Input.mousePosition), 5);

        if (isMouseOnGridCase)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere((transform.position), 5);
        }
    }

    private void UpdatePowerShape(GameObject powerShape)
    {
        currentPowerShape = powerShape;
    }
}
