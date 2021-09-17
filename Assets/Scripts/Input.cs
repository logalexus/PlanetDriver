using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    [SerializeField] private CarMovement _movement;
    [SerializeField] private Joystick _joystick;

    private float _horizonalInput;
    private float _verticalInput;

    private void Start()
    {
        GameController.Instance.GameStart += () => { enabled = true; };
        GameController.Instance.GameOver += () => { enabled = false; };
        enabled = false;
    }

    private void Update()
    {
        _horizonalInput = _joystick.Horizontal;
        _verticalInput = _joystick.Vertical;
    }
    private void FixedUpdate()
    {
        _movement.MoveCar(_horizonalInput, _verticalInput);
    }
    
}
