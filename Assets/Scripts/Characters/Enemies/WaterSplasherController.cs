using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplasherController : CharacterController
{

    [SerializeField] private GameObject fireballModel;
    [SerializeField] private Transform fireballOrigin;

    private GameObject target;

    private void Start()
    {
        characterSpeed = GameParameters.WATERSPLASHER_MOVEMENT_SPEED;
        target = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        Vector2 direction = new Vector2(Random.value - 0.5f, Random.value -0.5f);
        LookAt(direction);
        
        Move(direction.normalized);
        yield return new WaitForSeconds(GameParameters.WATERSPLASHER_MOVEMENT_TIME);
        Move(Vector2.zero);

        Vector2 attackDirection = target.transform.position - transform.position;
        LookAt(attackDirection);
        float angle = 0;
        for(int i = 0; i < GameParameters.WATERSPLASHER_NUMBER_OF_SHOOTS; i++)
        {
            Attack();
            yield return new WaitForSeconds(GameParameters.WATERSPLASHER_TIME_BETWEEN_ATTACKS);
            angle += 360 / GameParameters.WATERSPLASHER_NUMBER_OF_SHOOTS;
            LookAt(Quaternion.Euler(0, 0, angle) * attackDirection);
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
        projectileController.Shoot(fireball.transform.up, gameObject.tag, GameParameters.waterSplasherProjectileSpeed, GameParameters.waterSplasherDamage);

        // Ignore collision between the player and the fireball (trigger ok)
        Physics2D.IgnoreCollision(mainCollider, fireball.GetComponent<CircleCollider2D>());
    }
}
