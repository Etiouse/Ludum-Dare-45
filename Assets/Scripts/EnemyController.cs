using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{

    [SerializeField] private GameObject fireballModel;
    [SerializeField] private Transform fireballOrigin;

    float attackDelay = 5;
    float currentAttack = 0;

    // Update is called once per frame
    void Update()
    {
        currentAttack += Time.deltaTime;
        if (currentAttack >= attackDelay)
        {
            currentAttack = 0;
            Attack();
        }
    }

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
    }
}
