using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarsScreen : UIScreen
{
    [SerializeField] private CarsScreenTransition _carsScreenTransition;
    [Header("Buttons")]
    [SerializeField] private Button _back;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _levelCounter;

    

    private void Start()
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
}
