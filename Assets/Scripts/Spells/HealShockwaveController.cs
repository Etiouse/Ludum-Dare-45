using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealShockwaveController : ShockwaveController
{
    private float heal;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            CharacterController character = collision.gameObject.GetComponent<CharacterController>();
            if (character != null)
            {
                character.Heal(heal);
            }
        }
    }


    private void OnEnable()
    {
        GameHandler.OnRestartGameEvent += RestartGame;
    }

    private void OnDisable()
    {
        GameHandler.OnRestartGameEvent -= RestartGame;
    }

    private void RestartGame()
    {
        Destroy(gameObject);
    }

    public override void Activate(string owner, float speed, float scale, float heal)
    {
        this.owner = owner;
        this.heal = heal;
        StartCoroutine(Scale(scale, speed));
    }


}
