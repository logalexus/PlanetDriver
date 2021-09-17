using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPathOnCar : MonoBehaviour
{
    [SerializeField] private Transform _car;

    private float _cameraPathOffset = 5f;

    private void Start()
    {
        GameController.Instance.GameOver += SetCameraPathOnCar;
        GameController.Instance.GameInitial += SetCameraPathOnCar;

    }
    private void SetCameraPathOnCar()
    {
        transform.position = _car.position + _car.up * _cameraPathOffset;
        transform.rotation = _car.rotation;

    }
}
