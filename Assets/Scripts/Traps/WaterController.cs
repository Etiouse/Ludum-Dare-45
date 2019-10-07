using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerCharacteristics.GetValue(PowerShape.Type.WATTER_WALK)) // player water perks
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>());
        }
    }

}
