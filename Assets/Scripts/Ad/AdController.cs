using UnityEngine;
using System.Collections;

public class AdController : MonoBehaviour
{
    [SerializeField] private InterAd _interAd;

    private int _loseCounter = 0;

    private void Start()
    {
        GameController.Instance.GameOver += () =>
        {
            _loseCounter++;
            if (_loseCounter % 2 == 0)
                _interAd.ShowAd();
        };
    }
}
