using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{

    [SerializeField] private GameObject fireballModel;
    [SerializeField] private Transform fireballOrigin;
    [SerializeField] private GameObject iceballModel;
    [SerializeField] private List<Transform> iceBallOrigins;

    public override void Attack()
    {
        // Generate fireball
        GameObject fireball = Instantiate(fireballModel);
        fireball.transform.position = fireballOrigin.position;
        fireball.transform.up = spritesParent.transform.up;

        ProjectileController projectileController = fireball.GetComponent<ProjectileController>();
        projectileController.Shoot(fireball.transform.up);


        // Ignore collision between the player and the fireball (trigger ok)
        Physics2D.IgnoreCollision(mainCollider, fireball.GetComponent<CircleCollider2D>());

        foreach (Transform t in iceBallOrigins)
        {
            GameObject iceball = Instantiate(iceballModel);
            iceball.transform.position = t.position;
            iceball.transform.up = spritesParent.transform.up;
            
            iceball.GetComponent<ProjectileController>().Shoot(t.position - transform.position);

            // Ignore collision between the player and the bullet (trigger ok)
            Physics2D.IgnoreCollision(mainCollider, iceball.GetComponent<CircleCollider2D>());
        }

    }
}
