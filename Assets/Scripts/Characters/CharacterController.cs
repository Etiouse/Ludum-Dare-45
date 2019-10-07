using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    [SerializeField] protected GameObject spritesParent;
    protected CircleCollider2D mainCollider;

    protected float maxHealth = GameParameters.DEFAULT_HEALTH;
    protected float characterSpeed = GameParameters.DEFAULT_MOVEMENT_SPEED;
    
    protected float health;
    private Rigidbody2D rigid2d;
    private GameObject lifebar;
    private float startLifeBarWidth;

    // Start is called before the first frame update
    void Awake()
    {
        if (spritesParent == null)
        {
            spritesParent = gameObject;
        }
        rigid2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CircleCollider2D>();

        SetMaxHealth();
        health = maxHealth;

        lifebar = transform.Find("LifeBar").Find("CurrentLifeParent").gameObject;
        transform.Find("LifeBar").gameObject.SetActive(PlayerCharacteristics.GetValue(PowerShape.Type.PRIM_LIFEBAR_VISION));
        startLifeBarWidth = lifebar.transform.localScale.x;

        Physics2D.IgnoreCollision(mainCollider, GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
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

    protected virtual void SetMaxHealth()
    {
        maxHealth = GameParameters.DEFAULT_HEALTH;
    }

    private void Update()
    {
        lifebar.transform.localScale = new Vector2(health / maxHealth * startLifeBarWidth, lifebar.transform.localScale.y);
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

    public virtual void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (tag != "Player" && PlayerCharacteristics.GetValue(PowerShape.Type.LIFE_STEAL))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().Heal(GameParameters.lifeSteal) ;
            }
            if (tag == "Player")
            {
                SceneManager.LoadScene("MainMenuScene");
            }
            Destroy(gameObject);
        }
    }

    public void Heal(float heal)
    {
        health = Mathf.Clamp(health + heal, 0, maxHealth);
    }
}
