using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    [SerializeField] private GameObject fireballModel;
    [SerializeField] private List<Transform> fireballOrigins;


    private void Awake()
    {
        StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        yield return new WaitForSeconds(GameParameters.FINAL_BOSS_MOVEMENT_TIME);

        Attack();

        StartCoroutine(Actions());
    }

    private void Attack()
    {
        FireballAttack();
    }

    private void FireballAttack()
    {
        foreach(Transform origin in fireballOrigins)
        {
            // Generate fireball
            GameObject fireball = Instantiate(fireballModel);
            fireball.transform.parent = transform;
            fireball.transform.position = origin.position;

            ProjectileController projectileController = fireball.GetComponent<ProjectileController>();
            projectileController.Shoot(origin.right, gameObject.tag, 1 / 2f, 5);

            CircleCollider2D fireballCollider = fireball.GetComponent<CircleCollider2D>();
            // Ignore collision between the player and the fireball (trigger ok)
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), fireballCollider);
        }
        
    }

}
