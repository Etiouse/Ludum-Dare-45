using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCaseManager : MonoBehaviour
{
    public bool IsPowerShapeOnCase { get; set; }

    private bool isMouseOnGridCaseTMP;
    private Rect rect;
    private GameObject currentPowerShape;

    private void OnEnable()
    {
        PowerShape.OnUpdateSelectedPowerShapeEvent += UpdatePowerShape;
    }

    private void OnDisable()
    {
        PowerShape.OnUpdateSelectedPowerShapeEvent -= UpdatePowerShape;
    }
    
    private void Update()
    {
        rect = GetComponent<RectTransform>().rect;
    }

    private void LateUpdate()
    {
        Vector3 casePos = transform.position;
        Vector3 mousePos = Input.mousePosition;

        isMouseOnGridCaseTMP = false;

        if (mousePos.x >= casePos.x - rect.width * SharedData.GetScreenRatio() / 2 &&
            mousePos.x <= casePos.x + rect.width * SharedData.GetScreenRatio() / 2 &&
            mousePos.y >= casePos.y - rect.height * SharedData.GetScreenRatio() / 2 &&
            mousePos.y <= casePos.y + rect.height * SharedData.GetScreenRatio() / 2)
        {
            isMouseOnGridCaseTMP = true;

            if (currentPowerShape != null)
            {
                if (currentPowerShape.GetComponent<PowerShape>().NumberOfCollisionWithOtherPowerShapes <= 0 &&
                    currentPowerShape.GetComponent<PowerShape>().NumberOfCollisionWithGridLimits <= 0)
                {
                    currentPowerShape.transform.position = casePos;
                    IsPowerShapeOnCase = true;
                }
                else
                {
                    IsPowerShapeOnCase = false;
                }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere((transform.position), 5);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere((Input.mousePosition), 5);

        if (isMouseOnGridCaseTMP)
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
