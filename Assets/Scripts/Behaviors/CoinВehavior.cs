using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinÂehavior : MonoBehaviour
{
    [SerializeField] private CoinAnim _coinAnim;

    private GameObject parent;

    private void Start()
    {
        parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Player cb))
        {
            Player.Instance.CoinCollecting();
            _coinAnim.DoCollect().OnComplete(() => 
            {
                parent.SetActive(false);
                _coinAnim.DoDefault();
            });
        }
    }
    

}
