using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LoseScreen : UIScreen
{
    [SerializeField] private LoseTransition _loseTransition;
    [SerializeField] private CinemachineSwitcher _cinemachineSwitcher;
    [SerializeField] private RewAd _rewAd;
    [SerializeField] private GameObject _X2Element;
    [Header("Buttons")]
    [SerializeField] private Button _collect;
    [SerializeField] private Button _again;
    [SerializeField] private Button _continue;
    [SerializeField] private Button _rewardX2;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _distanceCounter;

    private Player _player;
    
    private void Start()
    {
        UIController uiController = UIController.Instance;
        GameController gameController = GameController.Instance;
        _player = Player.Instance;

        _collect.onClick.AddListener(()=> 
        {
            _loseTransition.ScrollResults();
            _player.Coins += _player.CollectedCoinsInGame;
            _player.AddExp(_player.Distance);
        });

        _again.onClick.AddListener(() =>
        {
            uiController.OpenScreen(uiController.GetScreen<InGameScreen>());
            gameController.OnGameRestart();
        });

        _continue.onClick.AddListener(() =>
        {
            uiController.OpenScreen(uiController.GetScreen<MainMenuScreen>());
            gameController.OnGameToInitial();
        });

        _rewardX2.onClick.AddListener(() =>
        {
            _rewAd.ShowAd();
            _X2Element.SetActive(false);
        });

        _player.CollectedCoinsInGameChanged += () =>
        {
            _coinCounter.text = $"{_player.CollectedCoinsInGame}$";
        };
    }
    
    public override void Close()
    {
        _loseTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }

    public override void Open()
    {
        base.Open();
        _X2Element.SetActive(true);
        _coinCounter.text = $"{_player.CollectedCoinsInGame}$";
        _distanceCounter.text = $"{_player.Distance}m";
        _loseTransition.OpenAnim().OnComplete(() => { _cinemachineSwitcher.SwitchAroundCamera(); });
    }


}
