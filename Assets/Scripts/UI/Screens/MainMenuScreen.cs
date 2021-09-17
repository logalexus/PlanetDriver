using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class MainMenuScreen : UIScreen
{
    [SerializeField] private MainMenuTransition mainMenuTransition;
    [Header("Buttons")]
    [SerializeField] private Button _start;
    [SerializeField] private Button _planets;
    [SerializeField] private Button _cars;
    [SerializeField] private Button _store;
    [SerializeField] private Button _settings;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _levelCounter;
    [Header("Bars")]
    [SerializeField] private Slider _levelBar;
    
    private void Start()
    {
        Player player = Player.Instance;
        UIController uiController = UIController.Instance;

        _start.onClick.AddListener(()=> 
        {
            GameController.Instance.OnGameStart();
            uiController.OpenScreen(uiController.GetScreen<InGameScreen>());
        });

        _planets.onClick.AddListener(() =>
        {
            uiController.OpenScreen(uiController.GetScreen<PlanetsScreen>());
        });

        _cars.onClick.AddListener(() =>
        {
            uiController.OpenScreen(uiController.GetScreen<CarsScreen>());
        });

        _store.onClick.AddListener(() =>
        {
            uiController.OpenScreen(uiController.GetScreen<StoreScreen>());
        });

        _settings.onClick.AddListener(() =>
        {
            uiController.OpenScreen(uiController.GetScreen<SettingsScreen>());
        });

        player.CoinsChanged += () =>
        {
            _coinCounter.text = $"{player.Coins}$";
        };

        player.LevelChanged += () =>
        {
            _levelCounter.text = $"lvl {player.Level}";
        };

        player.ExpChanged += () =>
        {
            _levelBar.value = player.Exp;
        };
    }

    public override void Open()
    {
        base.Open();
        mainMenuTransition.OpenAnim();
    }

    public override void Close()
    {
        mainMenuTransition.CloseAnim().OnComplete(()=> { base.Close(); });
    }

    
}
