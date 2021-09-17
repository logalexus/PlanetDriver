using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class PlanetsScreen : UIScreen
{
    [SerializeField] private PlanetsScreenTransition _planetsScreenTransition;
    [Header("Buttons")]
    [SerializeField] private Button _back;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _levelCounter;

    private void Start()
    {
        Player player = Player.Instance;
        UIController uiController = UIController.Instance;

        _back.onClick.AddListener(() =>
        {
            uiController.OpenScreen(uiController.GetScreen<MainMenuScreen>());
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
        _planetsScreenTransition.OpenAnim();
    }

    public override void Close()
    {
        _planetsScreenTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }
}
