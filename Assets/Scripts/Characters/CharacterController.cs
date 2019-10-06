using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] protected GameObject spritesParent;
    protected CircleCollider2D mainCollider;

    protected float startHealth = 20;
    protected float characterSpeed = 20;
    
    private Rigidbody2D rigid2d;
    private float health;

    // Start is called before the first frame update
    void Awake()
    {
        if (spritesParent == null)
        {
            spritesParent = gameObject;
        }
        rigid2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CircleCollider2D>();
        health = startHealth;
    }

    public void Move(Vector2 direction)
    {
        rigid2d.velocity = direction * characterSpeed; // * UpgradeParameters.PlayerSpeed;
    }  

    public void LookAt(Vector2 direction)
    {
        spritesParent.transform.up = direction;
    }

    public virtual void Attack()
    {

    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
