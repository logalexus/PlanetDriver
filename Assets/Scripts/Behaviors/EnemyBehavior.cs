using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody _enemy;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float _speed = 7f;

    private bool _isMoving = true;
    private bool _isDestroyable = false;
    
    private void Start()
    {
        GameController.Instance.GameOver += () => { _isMoving = false; };
        GameController.Instance.GameStart += () => { _isMoving = true; };
        StartCoroutine(WaitForDestroy());
    }

    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(6f);
        _isDestroyable = true;
    }

    private void FixedUpdate()
    {
        if (_isMoving)
            _enemy.MovePosition(_enemy.position + transform.forward * _speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out EnemyDestroyerMovement destroyer) && _isDestroyable)
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Crush"))
        {
            _isMoving = false;
            if (!Physics.CheckSphere(transform.position, 25f, _mask))
            {
                gameObject.SetActive(false);
            }
        }
            
    }

    private void OnEnable()
    {
        _isDestroyable = false;
        _isMoving = true;
        StartCoroutine(WaitForDestroy());
    }
    
}
