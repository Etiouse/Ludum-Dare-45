using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamagePlayer(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        DamagePlayer(collision);
    }

    private void DamagePlayer(Collider2D collision)
    {
        //if () // can levitate lava
        {
            if (collision.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().Damage(GameParameters.LAVA_DAMAGE);
            }
        }
    }
}
