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
    
    [SerializeField] private GameObject airShieldModel;

    private bool canAttack = true;
    private List<GameObject> rockshield;
    private AirShield airshield;

    private bool invincibility = false;
    private List<SpriteRenderer> rendererList = new List<SpriteRenderer>();

    private void Start()
    {
        startHealth = GameParameters.playerStartHealth;
        characterSpeed = GameParameters.playerSpeed;
        rockshield = new List<GameObject>();
        GenerateRockShield();
        CreateAirShield();
        GetSpriteRenderer();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckForEnemyCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckForEnemyCollision(collision);
    }

    private void CheckForEnemyCollision(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Damage(GameParameters.COLLISION_DAMAGE);
        }
    }

    public override void Damage(float damage)
    {
        if (!invincibility)
        {
            base.Damage(damage);
            StartCoroutine(Invincibility());
        }
    }

    public override void Attack()
    {
        if (canAttack)
        {
            //if (...) // TODO get bool parameter
            {
                FireballAttack();
            }

            // if (...) // TODO get bool parameter
            {
                IceBallAttack();
            }

            // if (...) // TODO get bool parameter
            {
                ActivateRockShield();
            }
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(GameParameters.playerAttackSpeed);
        canAttack = true;
    }

    private IEnumerator Invincibility()
    {
        invincibility = true;
        bool blinkOn = true;
        int numberOfBlink = 20;

        for (int i = 0; i < numberOfBlink; i++)
        {
            SetAlphaAllSprites(blinkOn ? 0.5f : 1);
            yield return new WaitForSeconds(GameParameters.playerInvincibilityTime / numberOfBlink);
            blinkOn = !blinkOn;
        }

        SetAlphaAllSprites(1);
        invincibility = false;
    }

    private void SetAlphaAllSprites(float alpha)
    {
        foreach (SpriteRenderer r in rendererList)
        {
            r.color = new Color(r.color.r, r.color.g, r.color.b, alpha);
        }
    }

    private void GetSpriteRenderer()
    {
        foreach (SpriteRenderer objectRenderer in spritesParent.GetComponentsInChildren<SpriteRenderer>())
        {
            rendererList.Add(objectRenderer);
        }
    }

    private void FireballAttack()
    {
        // Generate fireball
        GameObject fireball = Instantiate(fireballModel);
        fireball.transform.position = fireballOrigin.position;
        fireball.transform.up = spritesParent.transform.up;

        ProjectileController projectileController = fireball.GetComponent<ProjectileController>();
        projectileController.Shoot(fireball.transform.up, gameObject.tag, GameParameters.fireballSpeed, GameParameters.fireballDamage);

        CircleCollider2D fireballCollider = fireball.GetComponent<CircleCollider2D>();
        // Ignore collision between the player and the fireball (trigger ok)
        Physics2D.IgnoreCollision(mainCollider, fireballCollider);
        IgnoreCollisionWithRockShield(fireballCollider);
        IgnoreCollisionWithAirShield(fireballCollider);
    }

    private void IceBallAttack()
    {
        foreach (Transform t in iceBallOrigins)
        {
            GameObject iceball = Instantiate(iceballModel);
            iceball.transform.position = t.position;
            iceball.transform.up = spritesParent.transform.up;

            iceball.GetComponent<ProjectileController>().Shoot(t.position - transform.position, gameObject.tag, GameParameters.iceballSpeed, GameParameters.iceballDamage);

            CircleCollider2D iceballCollider = iceball.GetComponent<CircleCollider2D>();
            // Ignore collision between the player and the bullet (trigger ok)
            Physics2D.IgnoreCollision(mainCollider, iceballCollider);

            IgnoreCollisionWithRockShield(iceballCollider);
            IgnoreCollisionWithAirShield(iceballCollider);
        }

    }

    private void GenerateRockShield()
    {
        for (int i = 0; i < GameParameters.numberOfRockShield; i++)
        {
            float angle = 360 / GameParameters.numberOfRockShield * i;

            GameObject rock = Instantiate(rockShieldModel);
            float shieldDistance = (rockOrigin.transform.position - transform.position).magnitude;
            rock.transform.position = transform.position + Quaternion.Euler(0, 0, angle) * (transform.position - rockOrigin.position);
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

    private void IgnoreCollisionWithAirShield(Collider2D collider)
    {
        Physics2D.IgnoreCollision(collider, airshield.GetComponent<CircleCollider2D>());
    }

    private void ActivateRockShield()
    {
        rockshield.ForEach(rock => rock.SetActive(true));
    }

    private void DeactivateRockShield()
    {
        rockshield.ForEach(rock => rock.SetActive(false));
    }

    private void CreateAirShield()
    {
        airshield = Instantiate(airShieldModel).GetComponent<AirShield>();
        airshield.Target = transform;
    }
}
