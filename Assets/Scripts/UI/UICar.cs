using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class UICar : MonoBehaviour
{
    [SerializeField] private GameObject _accessPanel;
    [SerializeField] private Image _carPreview;
    [SerializeField] private WarningAnimation _warning;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;
    [Header("Buttons")]
    [SerializeField] private Button _selectButton;
    
    private Car _car;
    private UIController uIController;

    public bool AccessCar
    {
        get => _car.Access;
        set
        {
            _car.Access = value;
            _accessPanel.SetActive(value);
        }
    }

    private void Start()
    {
        Player player = Player.Instance;
        uIController = UIController.Instance;

        _selectButton.onClick.AddListener(() =>
        {
            if (_car.Access)
                ShowCar();
            else
            {
                if (player.Coins >= _car.Cost)
                {
                    uIController.PopupCall(() =>
                    {
                        player.Coins -= _car.Cost;
                        AccessCar = true;
                    });
                    CarsLoader.Instance.SaveAvailableContents(_car.Name);
                }
                else
                {
                    _selectButton.interactable = false;
                    _warning.StartAnimation(() => { _selectButton.interactable = true; });
                }
            }

        });
    }

    public void SetMapUI(Car car)
    {
        _car = car;
        _name.text = car.Name;
        _cost.text = car.Cost.ToString();
        _carPreview.sprite = car.Preview;
        //_carPreview.SetNativeSize();
        _accessPanel.SetActive(_car.Access);
    }

    private void ShowCar()
    {
        CarsLoader.Instance.SetContent(_car);
        uIController.OpenScreen(uIController.GetScreen<MainMenuScreen>());
    }

}
