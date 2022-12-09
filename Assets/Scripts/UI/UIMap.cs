using UnityEngine;
using System.Collections;
using Data;
using Data.Models;
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
        _planetData = planetData;

        AccessMap = _dataController.Data.AvailablePlanetsData.Exists(x => x.Name == _planetData.Name);

        Map map = mapsHolder.GetContent(_planetData.Name);

        _map = map;
        _mapPreview.sprite = map.Preview;
        _name.text = _planetData.Name;
        _cost.text = _planetData.Cost.ToString();
        _targetLevel.text = $"{_planetData.TargetLevel} lvl";
        Player.Instance.LevelChanged += CheckLevel;
        CheckLevel();

        _selectButton.onClick.AddListener(OnClickMap);
    }

    private void OnClickMap()
    {
        if (AccessMap)
        {
            ShowMap();
        }
        else
        {
            if (_player.Coins >= _planetData.Cost)
            {
                _popupFactory.ShowApprovePopup("Are you sure?", "Warning", () =>
                {
                    _player.Coins -= _planetData.Cost;
                    AccessMap = true;
                });

                //MapLoader.Instance.SaveAvailableContents(_planetData.Name);
            }
            else
            {
                _selectButton.interactable = false;
                _mapWarning.StartAnimation(() => _selectButton.interactable = true);
            }
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
        MapLoader.Instance.SetContent(_map);
        _uiController.OpenScreen(_uiController.GetScreen<MainMenuScreen>());
    }
}