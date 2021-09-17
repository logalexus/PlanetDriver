using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotation : MonoBehaviour
{
    [SerializeField] private Transform _car;
    public float value = 50;
    private void Update()
    {
        transform.rotation =  Quaternion.FromToRotation(_car.position - transform.position, ((_car.position + _car.forward * value) - transform.position)) * _car.rotation;
    }
}
