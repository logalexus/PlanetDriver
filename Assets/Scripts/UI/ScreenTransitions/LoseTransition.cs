using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoseTransition : ScreenTransition
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _resultGroup;

    private readonly float _durationScroll = 0.3f;
    
    public override Sequence CloseAnim()
    {
        Sequence s = DOTween.Sequence();
        s.Join(base.CloseAnim());
        s.Join(_image.transform.DOScale(Vector3.zero, 0));
        s.Append(_image.DOFade(0, 0));
        s.Append(_resultGroup.DOLocalMoveX(_resultGroup.localPosition.x + _resultGroup.rect.width / 2, 0));
        return s;
    }

    public override Sequence OpenAnim()
    {
        Sequence s = DOTween.Sequence();
        s.Join(_image.DOFade(1f, _durationOpen));
        s.Join(_image.transform.DOScale(new Vector3(20, 20, 0), _durationOpen).SetEase(Ease.Flash).OnComplete(()=> 
        {
            _image.DOFade(0f, 1.5f);
        }));
        base.OpenAnim();
        return s;
    }

    public void ScrollResults()
    {
        _canvasGroup.interactable = false;
        _resultGroup.DOLocalMoveX(_resultGroup.localPosition.x - _resultGroup.rect.width / 2, _durationScroll).SetEase(Ease.OutBack, _overshot).OnComplete(()=>
        {
            _canvasGroup.interactable = true;
        });
    }

}
