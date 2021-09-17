using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinAnim : MonoBehaviour
{
    private Tween _tw;

    private void Start()
    {
        _tw = transform.DOLocalRotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    public Sequence DoCollect()
    {
        Sequence s = DOTween.Sequence();
        s.Join(transform.DOLocalMoveY(transform.localPosition.y - 5f, 0.2f));
        s.Join(transform.DOScale(0, 1f));
        return s;
    }
    public Sequence DoDefault()
    {
        Sequence s = DOTween.Sequence();
        s.Join(transform.DOLocalMoveY(transform.localPosition.y + 5f, 0));
        s.Join(transform.DOScale(1, 0));
        return s;
    }
    


}
