using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeController : CharacterController
{

    [SerializeField] private GameObject fireballModel;
    [SerializeField] private List<Transform> fireballOrigin;

    private GameObject target;

    private void Start()
    {
        characterSpeed = GameParameters.CONE_MOVEMENT_SPEED;
        target = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Actions());
    }
    
    protected override void SetMaxHealth()
    {
        maxHealth = GameParameters.CONE_LIFE;
    }

    IEnumerator Actions()
    {
        Vector2 direction = new Vector2(Random.value - 0.5f, Random.value -0.5f);
        LookAt(direction);
        
        Move(direction.normalized);
        yield return new WaitForSeconds(GameParameters.CONE_MOVEMENT_TIME);
        Move(Vector2.zero);

        Vector2 attackDirection = target.transform.position - transform.position;
        LookAt(attackDirection);
        
        Attack();
        yield return new WaitForSeconds(GameParameters.CONE_TIME_BETWEEN_ATTACKS);

        StartCoroutine(Actions());
    }

    public override void Attack()
    {
        foreach(Transform t in fireballOrigin)
        {
            // Generate fireball
            GameObject fireball = Instantiate(fireballModel);
            fireball.transform.position = t.position;
            fireball.transform.up = t.up;

            ProjectileController projectileController = fireball.GetComponent<ProjectileController>();
            projectileController.Shoot(fireball.transform.up, gameObject.tag, GameParameters.coneProjectileSpeed, GameParameters.coneDamage);

            // Ignore collision between the player and the fireball (trigger ok)
            Physics2D.IgnoreCollision(mainCollider, fireball.GetComponent<CircleCollider2D>());
        }
    }
}
