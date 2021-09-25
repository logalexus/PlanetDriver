using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] protected RectTransform _topGroup;
    [SerializeField] protected RectTransform _bottomGroup;

    protected CanvasGroup _canvasGroup;
    protected float _overshot = 1f;
    protected float _durationOpen = 0.5f;
    protected float _durationClose = 0.4f;
    

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.interactable = false;
        _topGroup.localPosition = new Vector3(_topGroup.localPosition.x, _topGroup.localPosition.y + _topGroup.rect.height);
        _bottomGroup.localPosition = new Vector3(_bottomGroup.localPosition.x, _bottomGroup.localPosition.y - _bottomGroup.rect.height);
    }

    public virtual Sequence CloseAnim()
    {
        _canvasGroup.interactable = false;
        Sequence s = DOTween.Sequence();
        s.Join(_bottomGroup.DOLocalMoveY(_bottomGroup.localPosition.y - _bottomGroup.rect.height, _durationClose).SetEase(Ease.InBack, _overshot));
        s.Join(_topGroup.DOLocalMoveY(_topGroup.localPosition.y + _topGroup.rect.height, _durationClose).SetEase(Ease.InBack, _overshot));
        return s;
    }
    public virtual Sequence OpenAnim()
    {
        Sequence s = DOTween.Sequence();
        s.Join(_bottomGroup.DOLocalMoveY(_bottomGroup.localPosition.y + _bottomGroup.rect.height, _durationOpen).SetEase(Ease.OutBack, _overshot));
        s.Join(_topGroup.DOLocalMoveY(_topGroup.localPosition.y - _topGroup.rect.height, _durationOpen).SetEase(Ease.OutBack, _overshot));
        s.OnComplete(() =>
        {
            _canvasGroup.interactable = true; ;
        });
        return s;
    }
}
