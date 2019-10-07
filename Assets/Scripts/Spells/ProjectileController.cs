using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float damage = GameParameters.DEFAULT_PROJECTILE_DAMAGE;
    private float speed = GameParameters.DEFAULT_PROJECTILE_SPEED;

    private Rigidbody2D rigid2d;
    private string owner;

    // Start is called before the first frame update
    void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collision between same entities (enemies projectile to enemies) and projectile to projectile
        if (collision.tag == "Projectile" || collision.tag == owner || collision.tag == "Lava" || collision.tag == "Water")
        {
            return;
        }

        CharacterController character = collision.gameObject.GetComponent<CharacterController>();
        if (character != null)
        {
            character.Damage(damage);
        }
        Destroy(gameObject);
    }

    public void Shoot(Vector2 direction, string owner, float speed, float damage)
    {
        this.damage = damage;
        this.owner = owner;
        rigid2d.velocity = direction * speed;
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
        //Destroy(gameObject);
    }
}
