using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeballController : MonoBehaviour
{
    private float speed = GameParameters.DEFAULT_PROJECTILE_SPEED;
    private float maxTravelTime;

    private Rigidbody2D rigid2d;
    private string owner;
    private GameObject enemyModel;

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
        SpawnMob();
    }

    public void Shoot(Vector2 direction, string owner, float speed, float maxTravelTime, GameObject enemyModel)
    {
        this.enemyModel = enemyModel;
        this.owner = owner;
        this.maxTravelTime = maxTravelTime;
        rigid2d.velocity = direction * speed;

        StartCoroutine(WaitAndSpawn());
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(maxTravelTime);
        SpawnMob();
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

    private void SpawnMob()
    {
        GameObject mob = Instantiate(enemyModel);
        mob.transform.position = transform.position;

        Destroy(gameObject);
    }
}
