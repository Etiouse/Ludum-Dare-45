using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlocManager : MonoBehaviour
{
    public int NumberOfCollisionWithOtherPowerBlocs { get; private set; }
    public int NumberOfCollisionWithGridLimits { get; private set; }
    public PowerShape.Type PowerShapeType { get; private set; }

    private void Awake()
    {
        PowerShapeType = transform.parent.GetComponent<PowerShape>().PowerShapeType;
        NumberOfCollisionWithOtherPowerBlocs = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PowerBloc")
        {
            if (PowerShapeType != collision.transform.parent.GetComponent<PowerShape>().PowerShapeType)
            {
                NumberOfCollisionWithOtherPowerBlocs++;
                Debug.Log(NumberOfCollisionWithOtherPowerBlocs + " " + transform.parent.name);
            }
        }

        if (collision.tag == "InventoryGrid")
        {
            NumberOfCollisionWithGridLimits++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PowerBloc")
        {
            if (PowerShapeType != collision.transform.parent.GetComponent<PowerShape>().PowerShapeType)
            {
                NumberOfCollisionWithOtherPowerBlocs--;
                Debug.Log(NumberOfCollisionWithOtherPowerBlocs + " " + transform.parent.name);
            }
        }

        if (collision.tag == "InventoryGrid")
        {
            NumberOfCollisionWithGridLimits--;
        }
    }
}
