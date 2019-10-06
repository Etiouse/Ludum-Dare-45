using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShield : MonoBehaviour
{

    public Transform Target { get; set; }
    private RangeInt activationRange = new RangeInt(3, 10);
    private float activationTime = 1;
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
        nextActivation = Random.Range(activationRange.start, activationRange.end);
        yield return new WaitForSeconds(nextActivation);
        Enabled(true);
        yield return new WaitForSeconds(activationTime);
        Enabled(false);

        StartCoroutine(ActivateShield());
    }

    private void Enabled(bool isEnabled)
    {
        renderer.enabled = isEnabled;
        collider.enabled = isEnabled;
    }

}
