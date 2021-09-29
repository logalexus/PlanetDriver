using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public class CarsScreen : UIScreen
{
    [SerializeField] private CarsScreenTransition _carsScreenTransition;
    [SerializeField] private CarsHolder _carsHolder;
    [SerializeField] private UICar _uiCar;
    [SerializeField] private HorizontalScrollSnap _scroller;

    [Header("Buttons")]
    [SerializeField] private Button _back;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _levelCounter;

    
    private IEnumerator Start()
    {
        Player player = Player.Instance;

        _back.onClick.AddListener(() =>
        {
            UIController.Instance.OpenScreen(UIController.Instance.GetScreen<MainMenuScreen>());
        });

        player.CoinsChanged += () =>
        {
            _coinCounter.text = $"{player.Coins}$";
        };

        player.LevelChanged += () =>
        {
            _levelCounter.text = $"lvl {player.Level}";
        };

        yield return null;
        SetCarsUI();
    }

    public override void Open()
    {
        base.Open();
        _carsScreenTransition.OpenAnim();
    }

    public override void Close()
    {
        _carsScreenTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }

    private void SetCarsUI()
    {
        for (int i = 0; i < _carsHolder.Contents.Count; i++)
        {
            UICar carUI = Instantiate(_uiCar);
            carUI.SetMapUI(_carsHolder.Contents[i] as Car);
            _scroller.AddChild(carUI.gameObject);
        }
    }
}
