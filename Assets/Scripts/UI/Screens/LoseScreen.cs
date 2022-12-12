using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MySql.Data.MySqlClient;
using TMPro;
using UI.Popups;

public class LoseScreen : UIScreen
{
    [SerializeField] private LoseTransition _loseTransition;
    [SerializeField] private CinemachineSwitcher _cinemachineSwitcher;
    [SerializeField] private RewAd _rewAd;
    [SerializeField] private GameObject _X2Element;
    [Header("Buttons")] [SerializeField] private Button _collect;
    [SerializeField] private Button _again;
    [SerializeField] private Button _continue;
    [SerializeField] private Button _rewardX2;
    [Header("Fields")] [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _distanceCounter;

    private Player _player;
    private DataController _dataController;


    private void Start()
    {
        UIController uiController = UIController.Instance;
        GameController gameController = GameController.Instance;
        _player = Player.Instance;
        _dataController = DataController.Instance;

        _collect.onClick.AddListener(() => Collect());

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

        _player.CollectedCoinsInGameChanged += () => { _coinCounter.text = $"{_player.CollectedCoinsInGame}$"; };
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

    private async UniTask Collect()
    {
        _loseTransition.ScrollResults();
        _player.Coins += _player.CollectedCoinsInGame;
        _player.AddExp(_player.Distance);
        _player.CompletedDistance = _player.Distance;
        CheckRecord();

        await Save();
    }

    private void CheckRecord()
    {
        var currentPlanet = MapLoader.Instance.CurrentPlanet;

        if (_player.CompletedDistance > currentPlanet.Record)
            _dataController.Data.AvailablePlanetsData.Find(x => x.Name == currentPlanet.Name).Record =
                _player.CompletedDistance;
    }

    private async UniTask Save()
    {
        PopupFactory.Instance.ShowLoadingPopup();
        try
        {
            await _dataController.SaveProgress();
            await _dataController.SavePlanets();
            PopupFactory.Instance.ClosePopup();
        }
        catch (MySqlException e)
        {
            PopupFactory.Instance.ShowInfoPopup(e.Message);
            throw;
        }
    }
}