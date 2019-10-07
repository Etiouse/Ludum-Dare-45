using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : CharacterController
{

    [SerializeField] private GameObject fireballModel = null;
    [SerializeField] private List<Transform> fireballOrigins = null;

    [SerializeField] private GameObject iceballModel = null;
    [SerializeField] private List<Transform> iceBallOrigins = null;

    [SerializeField] private GameObject rockShieldModel = null;
    [SerializeField] private Transform rockOrigin = null;
    
    [SerializeField] private GameObject airShieldModel = null;

    [SerializeField] private GameObject objects = null;

    private bool canAttack = true;
    private List<GameObject> rockshield;
    private AirShield airshield;

    private bool invincibility = false;
    private List<SpriteRenderer> rendererList = new List<SpriteRenderer>();

    private float attackSpeed;

    private void Start()
    {
        maxHealth = PlayerCharacteristics.GetValue(PowerShape.Type.MAX_HEALTH_UP1) ? 
            (GameParameters.playerMaxHealthUpgrade2) : // upgrade 2
            (PlayerCharacteristics.GetValue(PowerShape.Type.MAX_HEALTH) ? 
            GameParameters.playerMaxHealthUpgrade1 : // upgrade 1
            GameParameters.playerMaxHealthDefault); // no upgrade

        attackSpeed = PlayerCharacteristics.GetValue(PowerShape.Type.ATTACK_SPEED_UP1) ?
            (GameParameters.playerAttackSpeedUpgrade2) : // upgrade 2
            (PlayerCharacteristics.GetValue(PowerShape.Type.ATTACK_SPEED) ?
            GameParameters.playerAttackSpeedUpgrade1 : // upgrade 1
            GameParameters.playerAttackSpeedDefault); // no upgrade

        Debug.Log("MAX_HEALTH" + maxHealth);
        characterSpeed = GameParameters.playerSpeed;
        rockshield = new List<GameObject>();
        if (PlayerCharacteristics.GetValue(PowerShape.Type.ROCK_SHIELD))
        {
            GameParameters.numberOfRockShield = PlayerCharacteristics.GetValue(PowerShape.Type.ROCK_SHIELD_UP1) ? GameParameters.numberOfRockShieldUpgrade : GameParameters.numberOfRockShieldDefault;
            GenerateRockShield();
            ActivateRockShield();
        }
        if (PlayerCharacteristics.GetValue(PowerShape.Type.AIR_SHIELD))
        {
            GameParameters.airShieldActivationRange = PlayerCharacteristics.GetValue(PowerShape.Type.AIR_SHIELD) ? GameParameters.airShieldActivationRangeUpgrade : GameParameters.airShieldActivationRangeDefault;
            CreateAirShield();
        }
        GetSpriteRenderer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckForEnemyCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckForEnemyCollision(collision);
    }

    private void CheckForEnemyCollision(Collider2D collision)
    {
        if (collision.tag == "Enemy")
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
            if (PlayerCharacteristics.GetValue(PowerShape.Type.FIRE_BALL)) 
            {
                FireballAttack();
            }

            if (PlayerCharacteristics.GetValue(PowerShape.Type.ICE_BALL)) 
            {
                IceBallAttack();
            }
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
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
        int numberProjectile = PlayerCharacteristics.GetValue(PowerShape.Type.FIRE_BALL_UP1) ? 3 : 1;
        for (int i = 0; i < numberProjectile; i++)
        {
            // Generate fireball
            GameObject fireball = Instantiate(fireballModel);
            fireball.transform.position = fireballOrigins[i].position;
            fireball.transform.right = fireballOrigins[i].right;
            fireball.transform.SetParent(objects.transform);

            ProjectileController projectileController = fireball.GetComponent<ProjectileController>();
            projectileController.Shoot(fireball.transform.up, gameObject.tag, GameParameters.fireballSpeed, GameParameters.fireballDamage);

            CircleCollider2D fireballCollider = fireball.GetComponent<CircleCollider2D>();
            // Ignore collision between the player and the fireball (trigger ok)
            Physics2D.IgnoreCollision(mainCollider, fireballCollider);
            if (rockshield.Count > 0)
                IgnoreCollisionWithRockShield(fireballCollider);
            if (airshield != null)
                IgnoreCollisionWithAirShield(fireballCollider);
        }
    }

    private void IceBallAttack()
    {
        int numberProjectile = PlayerCharacteristics.GetValue(PowerShape.Type.ICE_BALL_UP1) ? 4 : 2;
        for (int i = 0; i < numberProjectile; i++)
        {
            GameObject iceball = Instantiate(iceballModel);
            iceball.transform.parent = transform;
            iceball.transform.position = iceBallOrigins[i].position;
            iceball.transform.up = spritesParent.transform.up;
            iceball.transform.SetParent(objects.transform);

            iceball.GetComponent<ProjectileController>().Shoot(iceBallOrigins[i].position - transform.position, gameObject.tag, GameParameters.iceballSpeed, GameParameters.iceballDamage);

            CircleCollider2D iceballCollider = iceball.GetComponent<CircleCollider2D>();
            // Ignore collision between the player and the bullet (trigger ok)
            Physics2D.IgnoreCollision(mainCollider, iceballCollider);

            if (rockshield.Count > 0)
                IgnoreCollisionWithRockShield(iceballCollider);
            if (airshield != null)
                IgnoreCollisionWithAirShield(iceballCollider);
        }

    }

    private void GenerateRockShield()
    {
        for (int i = 0; i < GameParameters.numberOfRockShield; i++)
        {
            float angle = 360 / GameParameters.numberOfRockShield * i;

            GameObject rock = Instantiate(rockShieldModel);
            rock.transform.SetParent(objects.transform);
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
        airshield.transform.SetParent(objects.transform);
        airshield.Target = transform;
    }
}
