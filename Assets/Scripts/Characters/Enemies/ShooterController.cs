using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : CharacterController
{

    [SerializeField] private GameObject fireballModel;
    [SerializeField] private Transform fireballOrigin;

    private GameObject target;

    private void Start()
    {
        characterSpeed = GameParameters.SHOOTER_MOVEMENT_SPEED;
        target = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        Vector2 direction = new Vector2(Random.value - 0.5f, Random.value -0.5f);
        LookAt(direction);
        
        Move(direction.normalized);
        yield return new WaitForSeconds(GameParameters.SHOOTER_MOVEMENT_TIME);
        Move(Vector2.zero);

        LookAt(target.transform.position - transform.position);
        for(int i = 0; i < GameParameters.SHOOTER_NUMBER_OF_SHOOTS; i++)
        {
            Attack();
            yield return new WaitForSeconds(GameParameters.SHOOTER_TIME_BETWEEN_ATTACKS);
        }

        StartCoroutine(Actions());
    }

    public override void Attack()
    {
        // Generate fireball
        GameObject fireball = Instantiate(fireballModel);
        fireball.transform.position = fireballOrigin.position;
        fireball.transform.up = spritesParent.transform.up;

        ProjectileController projectileController = fireball.GetComponent<ProjectileController>();
        projectileController.Shoot(fireball.transform.up, gameObject.tag, GameParameters.shooterProjectileSpeed, GameParameters.shooterDamage);

        // Ignore collision between the player and the fireball (trigger ok)
        Physics2D.IgnoreCollision(mainCollider, fireball.GetComponent<CircleCollider2D>());
    }
}
