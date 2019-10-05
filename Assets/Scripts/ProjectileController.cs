using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float damage = 5;
    [SerializeField] private float speed = 20;

    private Rigidbody2D rigid2d;

    // Start is called before the first frame update
    void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController character = collision.gameObject.GetComponent<CharacterController>();
        if (character != null)
        {
            character.Damage(damage);
        }
        Destroy(gameObject);
    }

    public void Shoot(Vector2 direction)
    {
        rigid2d.velocity = direction * speed;
    }
}
