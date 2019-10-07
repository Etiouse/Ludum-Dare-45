using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherController : CharacterController
{
    [SerializeField] private GameObject shockwaveModel;
    private GameObject target;

    private void Start()
    {
        characterSpeed = GameParameters.DASHER_MOVEMENT_SPEED;
        target = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        Vector2 direction = target.transform.position - transform.position;
        direction = Quaternion.Euler(0, 0, (Random.value - 0.5f) * 40f) * direction;
        LookAt(direction);
        
        Move(direction.normalized);
        yield return new WaitForSeconds(GameParameters.DASHER_MOVEMENT_TIME);
        Move(Vector2.zero);


        Attack();

        yield return new WaitForSeconds(GameParameters.DASHER_WAIT_TIME);


        //for(int i = 0; i < GameParameters.SHOOTER_NUMBER_OF_SHOOTS; i++)
        //{
        //    Attack();
        //    yield return new WaitForSeconds(GameParameters.SHOOTER_TIME_BETWEEN_ATTACKS);
        //}

        StartCoroutine(Actions());
    }

    public override void Attack()
    {
        // Generate fireball
        GameObject shockwave = Instantiate(shockwaveModel);
        shockwave.transform.position = transform.position;

        ShockwaveController shockwaveController = shockwave.GetComponent<ShockwaveController>();
        shockwaveController.Activate(gameObject.tag, GameParameters.dasherShockwaveExpandingSpeed, GameParameters.dasherShockwaveExpandingScale, GameParameters.dasherShockwaveDamage);

        // Ignore collision between the player and the fireball (trigger ok)
        Physics2D.IgnoreCollision(mainCollider, shockwave.GetComponent<CircleCollider2D>());
    }
}
