using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraumController : CharacterController
{
    [SerializeField] private GameObject shield;
    private GameObject target;

    private void Start()
    {
        characterSpeed = GameParameters.BRAUM_MOVEMENT_SPEED;
        target = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Actions());
    }

    protected override void SetMaxHealth()
    {
        maxHealth = GameParameters.BRAUM_LIFE;
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
    }

    private void FixedUpdate()
    {
        Vector2 playerDirection = target.transform.position - transform.position;
        LookAt(playerDirection);
    }

    IEnumerator Actions()
    {
        Vector2 direction = new Vector2(Random.value - 0.5f, Random.value -0.5f);
        LookAt(direction);
        
        Move(direction.normalized);
        yield return new WaitForSeconds(GameParameters.BRAUM_MOVEMENT_TIME);
        Move(Vector2.zero);

        shield.SetActive(false);
        yield return new WaitForSeconds(GameParameters.BRAUM_SHIELD_DEACTIVATION_TIME);
        shield.SetActive(true);
        
        StartCoroutine(Actions());
    }
    
}
