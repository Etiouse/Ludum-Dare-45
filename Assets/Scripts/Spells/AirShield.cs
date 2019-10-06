using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShield : MonoBehaviour
{

    public Transform Target { get; set; }
    private int nextActivation;
    private SpriteRenderer renderer;
    private CircleCollider2D collider;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();
        StartCoroutine(ActivateShield());
    }

    private void Update()
    {
        transform.position = Target.position;
    }

    IEnumerator ActivateShield()
    {
        nextActivation = Random.Range(GameParameters.airShieldActivationRange.start, GameParameters.airShieldActivationRange.end);
        yield return new WaitForSeconds(nextActivation);
        Enabled(true);
        yield return new WaitForSeconds(GameParameters.AIR_SHIELD_ACTIVATION_TIME);
        Enabled(false);

        StartCoroutine(ActivateShield());
    }

    private void Enabled(bool isEnabled)
    {
        renderer.enabled = isEnabled;
        collider.enabled = isEnabled;
    }

}
