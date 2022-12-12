using UnityEngine;
using System.Collections;
using System.Linq;
using AnalyticsLogic;
using Cysharp.Threading.Tasks;
using Data;
using Data.Models;
using MySql.Data.MySqlClient;
using TMPro;
using UI.Popups;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    [SerializeField] private GameObject _accessPanel;
    [SerializeField] private GameObject _lockLabel;
    [SerializeField] private GameObject _costLabel;
    [SerializeField] private Image _mapPreview;
    [SerializeField] private WarningAnimation _mapWarning;

    [Header("Fields")] [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _targetLevel;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private TextMeshProUGUI _record;

    [Header("Buttons")] [SerializeField] private Button _selectButton;

    private UIController _uiController;
    private DataController _dataController;
    private PopupFactory _popupFactory;
    private Player _player;
    private PlanetData _planetData;
    private Map _map;
    private bool _access;


    public bool AccessMap
    {
        get => _access;
        set
        {
            _access = value;
            _accessPanel.SetActive(value);
            _lockLabel.SetActive(!value);
        }
    }

    public void Init(MapsHolder mapsHolder, PlanetData planetData)
    {
        _player = Player.Instance;
        _uiController = UIController.Instance;
        _dataController = DataController.Instance;
        _popupFactory = PopupFactory.Instance;

        _planetData = _dataController.Data.AvailablePlanetsData.Find(x => x.Name == planetData.Name);

        if (_planetData != null)
        {
            AccessMap = true;
        }
        else
        {
            AccessMap = false;
            _planetData = planetData;
        }
        
        
        _map = mapsHolder.GetContent(_planetData.Name);
        _mapPreview.sprite = _map.Preview;
        _name.text = _planetData.Name;
        _cost.text = _planetData.Cost.ToString();
        _targetLevel.text = $"{_planetData.TargetLevel} lvl";
        _record.text = $"{_planetData.Record}m";
        Player.Instance.LevelChanged += CheckLevel;
        Player.Instance.CompletedDistanceChanged += CheckRecord;
        CheckLevel();

        _selectButton.onClick.AddListener(OnClickMap);
    }

    private void CheckRecord()
    {
        var currentPlanet = MapLoader.Instance.CurrentPlanet;

        if (_planetData.Name == currentPlanet.Name && _player.CompletedDistance > currentPlanet.Record)
            _record.text = $"{_player.CompletedDistance}m";
    }

    private void OnClickMap()
    {
        if (AccessMap)
        {
            ShowMap();
        }
        else
        {
            if (_player.Coins >= _planetData.Cost && _player.Level >= _planetData.TargetLevel)
            {
                _popupFactory.ShowApprovePopup("Are you sure?", "Warning", () => Buy());
            }
            else
            {
                _selectButton.interactable = false;
                _mapWarning.StartAnimation(() => _selectButton.interactable = true);
            }
        }
    }

    private async UniTask Buy()
    {
        _player.Coins -= _planetData.Cost;
        AccessMap = true;

        PopupFactory.Instance.ShowLoadingPopup();
        try
        {
            await _dataController.SaveProgress();
            await _dataController.PlanetRepository.AddPlanetToUser(_dataController.Data.UserData.Id,
                _planetData.IdPlanetType, false);
            _dataController.Data.AvailablePlanetsData.Add(_planetData);
            Analytics.Instance.Write($"Bought planet - {_planetData.Name}");
            PopupFactory.Instance.ClosePopup();
        }
        catch (MySqlException e)
        {
            PopupFactory.Instance.ShowInfoPopup(e.Message);
            throw;
        }
    }

    private void CheckLevel()
    {
        bool value = Player.Instance.Level >= _planetData.TargetLevel;
        _costLabel.SetActive(value);
        _targetLevel.gameObject.SetActive(!value);
    }

    private void ShowMap()
    {
        MapLoader.Instance.SetContent(_planetData);
        _uiController.OpenScreen(_uiController.GetScreen<MainMenuScreen>());
        SaveCurrent();
    }

    private async UniTask SaveCurrent()
    {
        PopupFactory.Instance.ShowLoadingPopup();
        try
        {
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