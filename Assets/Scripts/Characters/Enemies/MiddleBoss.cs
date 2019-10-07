using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss : CharacterController
{
    [SerializeField] private GameObject pokeballModel = null;
    [SerializeField] private List<GameObject> enemyModels = null;
    [SerializeField] private List<Transform> spawnOrigins = null;

    [SerializeField] private GameObject objects = null;

    private void Start()
    {
        characterSpeed = GameParameters.MIDDLE_BOSS_MOVEMENT_SPEED;
        StartCoroutine(Actions());
    }
    protected override void SetMaxHealth()
    {
        maxHealth = GameParameters.MIDDLE_BOSS_LIFE;
    }

    IEnumerator Actions()
    {
        Vector2 direction = new Vector2(Random.value - 0.5f, Random.value -0.5f);
        LookAt(direction);
        
        Move(direction.normalized);
        yield return new WaitForSeconds(GameParameters.SHOOTER_MOVEMENT_TIME);
        Move(Vector2.zero);
        
        Attack();

        yield return new WaitForSeconds(GameParameters.MIDDLE_BOSS_TIME_BETWEEN_ATTACKS);

        StartCoroutine(Actions());
    }

    public override void Attack()
    {
        int direction = Random.Range(0, spawnOrigins.Count - 1);
        Debug.Log(direction);

        // Generate fireball
        GameObject pokeball = Instantiate(pokeballModel);
        pokeball.transform.position = spawnOrigins[direction].position;
        pokeball.transform.up = spawnOrigins[direction].up;
        pokeball.transform.SetParent(objects.transform);

        PokeballController pokeballController = pokeball.GetComponent<PokeballController>();
        pokeballController.Shoot(pokeball.transform.up, gameObject.tag, GameParameters.MIDDLE_BOSS_PROJECTILE_SPEED, GameParameters.MIDDLE_BOSS_PROJECTILE_TIME, enemyModels[Random.Range(0, enemyModels.Count - 1)]);

        // Ignore collision between the player and the fireball (trigger ok)
        Physics2D.IgnoreCollision(mainCollider, pokeball.GetComponent<CircleCollider2D>());
    }
}
