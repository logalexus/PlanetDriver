using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitation : MonoBehaviour
{
    private const double G = 9.8;
    private const double MASS_PLANET = 10000;
    
    private HashSet<Rigidbody> affectedBodies = new HashSet<Rigidbody>();

    private void FixedUpdate()
    {
        foreach (var body in affectedBodies)
        {
            if (body == null)
            {
                affectedBodies.Remove(body);
                break;
            }
            float power = (float)(G * body.mass * MASS_PLANET);
            Vector3 force = power * (transform.position - body.transform.position).normalized;
            force /= (transform.position - body.transform.position).sqrMagnitude;
            body.AddForce(force, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
            affectedBodies.Add(other.attachedRigidbody);
    }
    
}
