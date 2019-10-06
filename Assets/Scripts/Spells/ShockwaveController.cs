using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveController : MonoBehaviour
{
    private float damage = GameParameters.DEFAULT_SHOCKWAVE_DAMAGE;
    private float speed = GameParameters.DEFAULT_SHOCKWAVE_EXPANDING_SPEED;
    private float scale = GameParameters.DEFAULT_SHOCKWAVE_EXPANDING_SCALE;

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
        if (collision.tag == "Projectile" || collision.tag == owner)
        {
            return;
        }

        CharacterController character = collision.gameObject.GetComponent<CharacterController>();
        if (character != null)
        {
            character.Damage(damage);
        }
    }

    public void Activate(string owner, float speed, float scale, float damage)
    {
        this.damage = damage;
        this.owner = owner;

        StartCoroutine(Scale(scale, speed));

    }

    private IEnumerator Scale(float finalScale, float time)
    {
        float scale = 1;
        float elapsedTime = 0;
        float startScale = transform.localScale.x;
        while (elapsedTime < time)
        {
            transform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.deltaTime;
            scale = Mathf.Lerp(startScale, startScale * finalScale, elapsedTime / time);
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
