using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    [SerializeField] private GameObject _accessPanel;
    [SerializeField] private Image _mapPreview;
    [SerializeField] private WarningAnimation _warning;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;
    [Header("Buttons")]
    [SerializeField] private Button _selectButton;
    
    private Map _map;
    private UIController uIController;

    public bool AccessMap
    {
        get => _map.Access;
        set
        {
            _map.Access = value;
            _accessPanel.SetActive(value);
        }
    }

    private void Start()
    {
        Player player = Player.Instance;
        uIController = UIController.Instance;

        _selectButton.onClick.AddListener(() =>
        {
            if (_map.Access)
                ShowMap();
            else
            {
                if (player.Coins >= _map.Cost)
                {
                    uIController.PopupCall(() =>
                    {
                        player.Coins -= _map.Cost;
                        AccessMap = true;
                    });
                    MapLoader.Instance.SaveAvailableContents(_map.Name);
                }
                else
                {
                    _selectButton.interactable = false;
                    _warning.StartAnimation(()=> { _selectButton.interactable = true; });
                }
            }
            
        });
    }

    public void SetMapUI(Map map)
    {
        _map = map;
        _name.text = map.Name;
        _cost.text = map.Cost.ToString();
        _mapPreview.sprite = map.Preview;
        _accessPanel.SetActive(_map.Access);
    }

    private void ShowMap()
    {
        MapLoader.Instance.SetContent(_map);
        uIController.OpenScreen(uIController.GetScreen<MainMenuScreen>());
    }


}
