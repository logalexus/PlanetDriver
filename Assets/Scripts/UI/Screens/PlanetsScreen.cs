using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using UnityEngine;
using DG.Tweening;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using TMPro;
using UI.Popups;
using UnityEngine.UI.Extensions;

public class PlanetsScreen : UIScreen
{
    [SerializeField] private PlanetsScreenTransition _planetsScreenTransition;
    [SerializeField] private MapsHolder _mapsHolder;
    [SerializeField] private UIMap _uiMap;
    [SerializeField] private Transform uiPlanetContainer;
    [Header("Buttons")] 
    [SerializeField] private Button _back;
    [Header("Fields")] 
    [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _levelCounter;

    private DataController _dataController;

    private void Start()
    {
        Player player = Player.Instance;
        UIController uiController = UIController.Instance;
        _dataController = DataController.Instance;
        
        _back.onClick.AddListener(() => { uiController.OpenScreen(uiController.GetScreen<MainMenuScreen>()); });

        player.CoinsChanged += () => { _coinCounter.text = $"{player.Coins}$"; };
        player.LevelChanged += () => { _levelCounter.text = $"lvl {player.Level}"; };

        SetMapsUI();
    }

    public override void Open()
    {
        base.Open();
        _planetsScreenTransition.OpenAnim();
    }

    public override void Close()
    {
        _planetsScreenTransition.CloseAnim().OnComplete(() =>
        {
            base.Close();
        });
    }

    private void SetMapsUI()
    {
        for (int j = 0; j < _dataController.Data.PlanetsData.Count ; j++)
        {
            UIMap mapUI = Instantiate(_uiMap, uiPlanetContainer);
            mapUI.Init(_mapsHolder, _dataController.Data.PlanetsData[j]);
        }
    }
    
    
}