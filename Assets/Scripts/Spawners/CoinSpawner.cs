using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private SphereCollider _planet;
    [SerializeField] private Transform _car;
    [SerializeField] private List<Coin> _coinPrefab;
    [SerializeField] private LayerMask _mask;
    [Header("Config")]
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private float _frequence = 0.5f;
    [SerializeField] private bool _autoExpand = true;


    private PoolObjects<Coin> _pool;
    private float _radiusPlanet;
    private bool _isPlaying = false;
    

    private const float GROUND_OFFSET = 2f;


    private void Start()
    {
        GameController.Instance.GameStart += () => { _isPlaying = true; };
        GameController.Instance.GameOver += () => 
        {
            _isPlaying = false;
            _pool.DeactivateAll();
        };
        
        _radiusPlanet = _planet.radius * _planet.transform.localScale.x;
        _pool = new PoolObjects<Coin>(_coinPrefab, _poolCount, transform);
        _pool.AutoExpand = _autoExpand;
        StartCoroutine(Spawn());
    }
        
    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(_frequence);

            if (_isPlaying)
            {
                Vector3 offset = Vector3.right * Random.Range(-20f, 20f);
                Vector3 pos = _planet.transform.position + (_planet.transform.position - _car.position).normalized * (_radiusPlanet + GROUND_OFFSET);
                Vector3 offsetPos = pos + offset;
                Vector3 offsetPosNormal = _planet.transform.position + (offsetPos - _planet.transform.position).normalized * (_radiusPlanet + GROUND_OFFSET);
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, _planet.transform.position - offsetPosNormal);
                if (!Physics.CheckSphere(offsetPosNormal, 1.5f, _mask) && _pool.HasFreeElement(out Coin coin))
                {
                    coin.transform.position = offsetPosNormal;
                    coin.transform.rotation = rot;
                }
            }
        }
    }
}
