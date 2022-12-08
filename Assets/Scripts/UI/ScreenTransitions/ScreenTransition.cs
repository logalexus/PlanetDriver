using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] protected RectTransform _topGroup;
    [SerializeField] protected RectTransform _bottomGroup;

    protected CanvasGroup _canvasGroup;
    protected RectTransform _canvas;
    protected float _overshot = 1f;
    protected float _durationOpen = 0.5f;
    protected float _durationClose = 0.4f;
    
    public virtual void Init()
    {
        _canvas ??= GetComponent<RectTransform>();
        _canvasGroup ??= GetComponent<CanvasGroup>();
        _canvasGroup.interactable = false;
    }

    public virtual Sequence CloseAnim()
    {
        Init();
        _canvasGroup.interactable = false;
        Sequence s = DOTween.Sequence();
        s.Join(_bottomGroup.DOLocalMoveY(_bottomGroup.localPosition.y - _canvas.rect.height, _durationClose).SetEase(Ease.InBack, _overshot));
        s.Join(_topGroup.DOLocalMoveY(_topGroup.localPosition.y + _canvas.rect.height, _durationClose).SetEase(Ease.InBack, _overshot));
        return s;
    }
    public virtual Sequence OpenAnim()
    {
        Init();
        OffsetPositionGroups();
        Sequence s = DOTween.Sequence();
        s.Join(_bottomGroup.DOLocalMoveY(_bottomGroup.localPosition.y + _canvas.rect.height, _durationOpen).SetEase(Ease.OutBack, _overshot));
        s.Join(_topGroup.DOLocalMoveY(_topGroup.localPosition.y - _canvas.rect.height, _durationOpen).SetEase(Ease.OutBack, _overshot));
        s.OnComplete(() =>
        {
            _canvasGroup.interactable = true;
        });
        return s;
    }
    

    void OffsetPositionGroups()
    {
        _topGroup.localPosition = new Vector3(_topGroup.localPosition.x, _canvas.rect.height);
        _bottomGroup.localPosition = new Vector3(_bottomGroup.localPosition.x, -_canvas.rect.height);
    }
}
