using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyerMovement : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private SphereCollider _planet;

    private float _diameterPlanet;

    private void Start()
    {
        StartCoroutine(MoveDestroyer());
    }
    
    private IEnumerator MoveDestroyer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            _diameterPlanet = _planet.radius * _planet.transform.localScale.x * 2;
            transform.position = _player.transform.position - _player.transform.up * _diameterPlanet;
            transform.rotation = _player.transform.rotation;
            transform.rotation *= Quaternion.Euler(180f, 180f, 0);
        }
    }

}
