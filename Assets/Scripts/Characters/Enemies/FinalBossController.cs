using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinalBossController : CharacterController
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
    private float damage;

    private bool hasFireball;
    private bool hasIceball;
    private bool hasFireballUpgrade;
    private bool hasIceballUpgrade;
    private int numberOfRockShield;
    private RangeInt airshieldRange;

    private GameObject target;

    private void Start()
    {
        spritesParent.GetComponent<SpriteRenderer>().sprite = GameParameters.BossSprite;
        System.Random rnd = new System.Random();
        float[] attackspeeds = new float[] { GameParameters.playerAttackSpeedDefault, GameParameters.playerAttackSpeedUpgrade1, GameParameters.playerAttackSpeedUpgrade2 }.OrderBy(x => rnd.Next()).ToArray();
        float[] damages = new float[] { GameParameters.playerDamageDefault, GameParameters.playerDamageUpgrade1, GameParameters.playerDamageUpgrade2 }.OrderBy(x => rnd.Next()).ToArray();
        float[] moveSpeeds = new float[] { GameParameters.playerSpeedDefault, GameParameters.playerSpeedUpgrade1, GameParameters.playerSpeedUpgrade2 }.OrderBy(x => rnd.Next()).ToArray();
        float[] maxHealths = new float[] { GameParameters.playerMaxHealthDefault, GameParameters.playerMaxHealthUpgrade1, GameParameters.playerMaxHealthUpgrade2 }.OrderBy(x => rnd.Next()).ToArray();
        bool[] spells = new bool[] { false, true, true, true }.OrderBy(x => rnd.Next()).ToArray();
        bool[] spellUpgrade = new bool[] { true, false, false, false }.OrderBy(x => rnd.Next()).ToArray();

        attackSpeed = attackspeeds[0];
        damage = damages[0];
        characterSpeed = moveSpeeds[0];

        hasFireball = spells[2];
        hasIceball = spells[3];
        hasFireballUpgrade = spellUpgrade[2];
        hasIceballUpgrade = spellUpgrade[3];

        rockshield = new List<GameObject>();
        if (spells[0])
        {
            numberOfRockShield = spellUpgrade[0] ? GameParameters.numberOfRockShieldUpgrade : GameParameters.numberOfRockShieldDefault;
            GenerateRockShield();
            ActivateRockShield();
        }
        if (spells[1])
        {
            airshieldRange = spellUpgrade[1] ? GameParameters.airShieldActivationRangeUpgrade : GameParameters.airShieldActivationRangeDefault;
            CreateAirShield();
        }

        GetSpriteRenderer();

        target = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        Vector2 direction = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
        LookAt(direction);

        Move(direction.normalized);
        yield return new WaitForSeconds(GameParameters.FINAL_BOSS_MOVEMENT_TIME);
        Move(Vector2.zero);

        LookAt(target.transform.position - transform.position);
        for (int i = 0; i < 3; i++)
        {
            Attack();
            yield return new WaitForSeconds(attackSpeed);
        }

        StartCoroutine(Actions());
    }

    protected override void SetMaxHealth()
    {
        System.Random rnd = new System.Random();
        float[] maxHealths = new float[] { GameParameters.playerMaxHealthDefault, GameParameters.playerMaxHealthUpgrade1, GameParameters.playerMaxHealthUpgrade2 }.OrderBy(x => rnd.Next()).ToArray();
        maxHealth = maxHealths[0];
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
            if (hasFireball) 
            {
                FireballAttack();
            }

            if (hasIceball) 
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
        int numberOfBlink = 30;

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
        int numberProjectile = hasFireballUpgrade ? 3 : 1;
        for (int i = 0; i < numberProjectile; i++)
        {
            // Generate fireball
            GameObject fireball = Instantiate(fireballModel);
            fireball.transform.position = fireballOrigins[i].position;
            fireball.transform.right = fireballOrigins[i].right;
            fireball.transform.SetParent(objects.transform);

            ProjectileController projectileController = fireball.GetComponent<ProjectileController>();
            projectileController.Shoot(fireball.transform.up, gameObject.tag, GameParameters.fireballSpeed, damage);

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
        int numberProjectile = hasIceballUpgrade ? 4 : 2;
        for (int i = 0; i < numberProjectile; i++)
        {
            GameObject iceball = Instantiate(iceballModel);
            iceball.transform.parent = transform;
            iceball.transform.position = iceBallOrigins[i].position;
            iceball.transform.up = spritesParent.transform.up;
            iceball.transform.SetParent(objects.transform);

            iceball.GetComponent<ProjectileController>().Shoot(iceBallOrigins[i].position - transform.position, gameObject.tag, GameParameters.iceballSpeed, damage);

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
        for (int i = 0; i < numberOfRockShield; i++)
        {
            float angle = 360 / numberOfRockShield * i;

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
