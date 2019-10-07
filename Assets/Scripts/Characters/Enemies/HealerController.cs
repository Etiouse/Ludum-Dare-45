using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerController : CharacterController
{

    [SerializeField] private GameObject healwaveModel;

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
        yield return new WaitForSeconds(GameParameters.HEALER_MOVEMENT_TIME);
        Move(Vector2.zero);

        Heal();
        
        yield return new WaitForSeconds(GameParameters.HEALER_WAIT_TIME);

        StartCoroutine(Actions());
    }

    public void Heal()
    {
        // Generate fireball
        GameObject healWave = Instantiate(healwaveModel);
        healWave.transform.position = transform.position;

        HealShockwaveController healwaveController = healWave.GetComponent<HealShockwaveController>();
        healwaveController.Activate(transform.tag, GameParameters.healerShockwaveExpandingSpeed, GameParameters.healerhockwaveExpandingScale, GameParameters.healerShockwaveHeal);

        // Ignore collision between the player and the fireball (trigger ok)
        Physics2D.IgnoreCollision(mainCollider, healWave.GetComponent<CircleCollider2D>());
    }
}
