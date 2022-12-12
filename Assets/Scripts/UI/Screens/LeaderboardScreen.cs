using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using Data.Models;
using DG.Tweening;
using Leaderboards;
using MySql.Data.MySqlClient;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScreen : UIScreen
{
    [SerializeField] private StoreScreenTransition _storeScreenTransition;
    [SerializeField] private Transform leaderboardsCoonainer;
    [SerializeField] private Leaderboard leaderboardPrefab;
    [Header("Buttons")] 
    [SerializeField] private Button _back;

    private DataController _dataController;


    private void Start()
    {
        Player player = Player.Instance;
        UIController uiController = UIController.Instance;
        _dataController = DataController.Instance;

        _back.onClick.AddListener(() => { uiController.OpenScreen(uiController.GetScreen<MainMenuScreen>()); });
    }

    private async UniTask LoadLeaderboard()
    {
        PopupFactory.Instance.ShowLoadingPopup();

        ClearLeaderboardContainer();

        var planets = _dataController.Data.PlanetsData;

        try
        {
            foreach (var planet in planets)
            {
                List<LeaderData> leaders = await _dataController.UserRepository.GetLeadersByPlanet(planet.IdPlanetType);
                Leaderboard leaderboard = Instantiate(leaderboardPrefab, leaderboardsCoonainer);
                leaderboard.Init(planet.Name, leaders);

            }
            PopupFactory.Instance.ClosePopup();
        }
        catch (MySqlException e)
        {
            PopupFactory.Instance.ShowInfoPopup(e.Message);
            throw;
        }
    }

    private void CreateLeaderBoard(List<LeaderData> leaders, PlanetData planet)
    {
        Leaderboard leaderboard = Instantiate(leaderboardPrefab, leaderboardsCoonainer);
    }

    private void ClearLeaderboardContainer()
    {
        for (int i = leaderboardsCoonainer.childCount - 1; i >= 0; i--)
            Destroy(leaderboardsCoonainer.GetChild(i).gameObject);
    }

    public override void Open()
    {
        base.Open();
        _storeScreenTransition.OpenAnim();
        LoadLeaderboard();
    }

    public override void Close()
    {
        _storeScreenTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }
}