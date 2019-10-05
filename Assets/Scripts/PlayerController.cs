using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : CharacterController
{

    [SerializeField] private GameObject fireballModel;
    [SerializeField] private Transform fireballOrigin;
    [SerializeField] private GameObject iceballModel;
    [SerializeField] private List<Transform> iceBallOrigins;

    [SerializeField] private GameObject rockShieldModel;
    [SerializeField] private Transform rockOrigin;
    [SerializeField] private int numberOfShield = 5;

    private List<GameObject> rockshield;

    private void Start()
    {
        rockshield = new List<GameObject>();
        GenerateRockShield();
    }

    public override void Attack()
    {
        //if (...) // TODO get bool parameter
        {
            FireballAttack();
        }

        // if (...) // TODO get bool parameter
        {

        }

        // if (...) // TODO get bool parameter
        {
            IceBallAttack();
        }

        // if (...) // TODO get bool parameter
        {
            ActivateRockShield();
        }

    }

    private void FireballAttack()
    {
        // Generate fireball
        GameObject fireball = Instantiate(fireballModel);
        fireball.transform.position = fireballOrigin.position;
        fireball.transform.up = spritesParent.transform.up;

        ProjectileController projectileController = fireball.GetComponent<ProjectileController>();
        projectileController.Shoot(fireball.transform.up);

        CircleCollider2D fireballCollider = fireball.GetComponent<CircleCollider2D>();
        // Ignore collision between the player and the fireball (trigger ok)
        Physics2D.IgnoreCollision(mainCollider, fireballCollider);
        IgnoreCollisionWithRockShield(fireballCollider);
    }

    private void IceBallAttack()
    {
        foreach (Transform t in iceBallOrigins)
        {
            GameObject iceball = Instantiate(iceballModel);
            iceball.transform.position = t.position;
            iceball.transform.up = spritesParent.transform.up;

            iceball.GetComponent<ProjectileController>().Shoot(t.position - transform.position);

            CircleCollider2D iceballCollider = iceball.GetComponent<CircleCollider2D>();
            // Ignore collision between the player and the bullet (trigger ok)
            Physics2D.IgnoreCollision(mainCollider, iceballCollider);

            IgnoreCollisionWithRockShield(iceballCollider);
        }

    }

    private void GenerateRockShield()
    {
        for (int i = 0; i < numberOfShield; i++)
        {
            float angle = 360 / numberOfShield * i;

            GameObject rock = Instantiate(rockShieldModel);
            float shieldDistance = (rockOrigin.transform.position - transform.position).magnitude;
            rock.transform.position = transform.position + Quaternion.Euler(0, 0, angle) * (transform.position - rockOrigin.position);
            Debug.Log(shieldDistance);
            rock.GetComponent<RockShield>().Init(transform, shieldDistance, angle);

            // Ignore collision between the player and the fireball (trigger ok)
            Physics2D.IgnoreCollision(mainCollider, rock.GetComponent<CircleCollider2D>());

            rockshield.Add(rock);
        }
    }

    private void IgnoreCollisionWithRockShield(Collider2D collider)
    {
        rockshield.Select(rock => rock.GetComponent<CircleCollider2D>()).ToList().ForEach(rock => Physics2D.IgnoreCollision(collider, rock));
    }

    private void ActivateRockShield()
    {
        rockshield.ForEach(rock => rock.SetActive(true));
    }

    private void DeactivateRockShield()
    {
        rockshield.ForEach(rock => rock.SetActive(false));
    }
}
