using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockShield : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360;
    public Transform target;
    private bool inited = false;
    private float angle;

    private float distance = 5;

    public void Init(Transform target, float distance, float angle)
    {
        this.target = target;
        inited = true;
        this.distance = distance;
        this.angle = angle;
    }
   
    private void FixedUpdate()
    {
        transform.position = Quaternion.Euler(0, 0, angle) * new Vector3(0, distance, 0) + target.position;
        angle += rotationSpeed * Time.deltaTime;
    }
}
