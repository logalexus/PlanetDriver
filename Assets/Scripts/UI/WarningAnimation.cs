using UnityEngine;
using DG.Tweening;

public class WarningAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;

    public void StartAnimation()
    {
        _rect.DOShakeAnchorPos(0.3f, new Vector3(50f,0,0), 10, 0, false, false);
    }
}
