using UnityEngine;
using DG.Tweening;

public class MapWarningAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _map;

    public void StartAnimation()
    {
        _map.DOShakeAnchorPos(0.3f, new Vector3(50f,0,0), 10, 0, false, false);
    }
}
