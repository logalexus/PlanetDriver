using UnityEngine;
using System.Collections;
using AnalyticsLogic;
using Cysharp.Threading.Tasks;
using Data;
using Data.Models;
using MySql.Data.MySqlClient;
using TMPro;
using UI.Popups;
using UnityEngine.UI;

public class UICar : MonoBehaviour
{
    [SerializeField] private GameObject _accessPanel;
    [SerializeField] private Image _carPreview;
    [SerializeField] private WarningAnimation _mapWarning;
    [Header("Fields")] [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;
    [Header("Buttons")] [SerializeField] private Button _selectButton;

    private Car _car;
    private UIController _uiController;
    private PopupFactory _popupFactory;
    private Player _player;
    private DataController _dataController;
    private AutoData _autoData;
    private bool _access;

    public bool AccessCar
    {
        get => _access;
        set
        {
            _access = value;
            _accessPanel.SetActive(value);
        }
    }

    public void Init(CarsHolder carsHolder, AutoData autoData)
    {
        _player = Player.Instance;
        _uiController = UIController.Instance;
        _dataController = DataController.Instance;
        _popupFactory = PopupFactory.Instance;
        _autoData = autoData;

        AccessCar = _dataController.Data.AvailableAutosData.Exists(x => x.Name == _autoData.Name);

        _car = carsHolder.GetContent(_autoData.Name);
        _name.text = _autoData.Name;
        _cost.text = _autoData.Cost.ToString();
        _carPreview.sprite = _car.Preview;

        _selectButton.onClick.AddListener(OnClickCar);
    }

    private void OnClickCar()
    {
        if (AccessCar)
            ShowCar();
        else
        {
            if (_player.Coins >= _autoData.Cost)
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
        _player.Coins -= _autoData.Cost;
        AccessCar = true;

        PopupFactory.Instance.ShowLoadingPopup();
        try
        {
            await _dataController.SaveProgress();
            await _dataController.AutoRepository.AddAutoToUser(_dataController.Data.UserData.Id, _autoData.IdAutoType,
                false);
            _dataController.Data.AvailableAutosData.Add(_autoData);
            Analytics.Instance.Write($"Bought auto - {_autoData.Name}");
            PopupFactory.Instance.ClosePopup();
        }
        catch (MySqlException e)
        {
            PopupFactory.Instance.ShowInfoPopup(e.Message);
            throw;
        }
    }

    private void ShowCar()
    {
        CarsLoader.Instance.SetContent(_autoData);
        _uiController.OpenScreen(_uiController.GetScreen<MainMenuScreen>());
        SaveCurrent();
    }


    private async UniTask SaveCurrent()
    {
        PopupFactory.Instance.ShowLoadingPopup();
        try
        {
            await _dataController.SaveAutos();
            PopupFactory.Instance.ClosePopup();
        }
        catch (MySqlException e)
        {
            PopupFactory.Instance.ShowInfoPopup(e.Message);
            throw;
        }
    }
}