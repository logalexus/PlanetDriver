using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyBehavior _prefEnemy;
    [SerializeField] private GameObject _player;
    [SerializeField] private SphereCollider _planet;
    [SerializeField] private LayerMask _mask;
    [Header("Config")]
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private bool _autoExpand = true;


    private PoolObjects<EnemyBehavior> _pool;
    private float _diameterPlanet;
    private bool _isPlaying = false;


    void Start()
    {
        GameController.Instance.GameStart += () => { _isPlaying = true; };
        GameController.Instance.GameOver += () => { _isPlaying = false; };
        GameController.Instance.GameRestart += RestartSpawner;

        _pool = new PoolObjects<EnemyBehavior>(_prefEnemy, _poolCount, transform);
        _pool.AutoExpand = _autoExpand;
        _diameterPlanet = _planet.radius * _planet.transform.localScale.x * 2;
        StartCoroutine(Spawn());
    }
    
    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (_isPlaying)
            {
                Vector3 pos = _player.transform.position - _player.transform.up * (_diameterPlanet);
                Quaternion rot = _player.transform.rotation * Quaternion.Euler(180f, 180f, 0) * Quaternion.Euler(0, Random.Range(-90, 90), 0);
                Debug.DrawLine(_player.transform.position, pos, Color.red, 0.1f);
                if (!Physics.CheckSphere(pos, 5f, _mask) && _pool.HasFreeElement(out EnemyBehavior enemy))
                {
                    enemy.transform.position = pos;
                    enemy.transform.rotation = rot;
                }
                
            }
        }
    }

    private void RestartSpawner()
    {
        _pool.DeactivateAll();
    }
    
    
    
}
