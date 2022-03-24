using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Destructable : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void DoDamage(Vector3 playerPosition, bool propulsion) {
        Vector3 direction;
        if (propulsion) direction = transform.position - playerPosition;
        else direction = playerPosition - transform.position;

        if (direction.magnitude > 10.0f);
        {
            rb.AddForce(direction.normalized * 0.5f, ForceMode.Impulse);
        }
    }
}
