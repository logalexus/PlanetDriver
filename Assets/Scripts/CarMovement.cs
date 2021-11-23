using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CarMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _carRB;
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _rotSpeed = 20f;
    [SerializeField] private float _verticalStrenght = 2f;


    private bool _canDriving = false;
    private AudioController _audioController;


    private void Start()
    {
        _audioController = AudioController.Instance;
        GameController.Instance.GameStart += () => 
        {
            _canDriving = true;
            _audioController.OnSoundEngine();
        };
        GameController.Instance.GameOver += () => 
        {
            _canDriving = false;
            _audioController.OffSoundEngine();
        };

    }

    public void MoveCar(float horizontal, float vertical)
    {
        if (_canDriving)
        {
            _carRB.MovePosition(_carRB.position + transform.forward * (_speed + vertical * _verticalStrenght) * Time.fixedDeltaTime );
          
            Vector3 yRotation = Vector3.up * horizontal * _rotSpeed * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(yRotation);
            Quaternion targetRotation = _carRB.rotation * deltaRotation;
            _carRB.MoveRotation(Quaternion.Slerp(_carRB.rotation, targetRotation, 100f * Time.deltaTime));

            _audioController.SetPitchEngine(vertical);
        }
    }
    
    


}
