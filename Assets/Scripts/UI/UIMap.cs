using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    [SerializeField] private GameObject _accessPanel;
    [SerializeField] private GameObject _lockLabel;
    [SerializeField] private GameObject _costLabel;
    [SerializeField] private Image _mapPreview;
    [SerializeField] private WarningAnimation _mapWarning;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _targetLevel;
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
            _lockLabel.SetActive(!value);
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
                    _mapWarning.StartAnimation(()=> _selectButton.interactable = true);
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
        _lockLabel.SetActive(!_map.Access);
        _costLabel.SetActive(false);
        _targetLevel.text = $"{_map.TargetLevel} lvl";
        Player.Instance.LevelChanged += CheckLevel;
        CheckLevel();
    }

    private void CheckLevel()
    {
        if (Player.Instance.Level >= _map.TargetLevel)
        {
            _costLabel.SetActive(true);
            _targetLevel.gameObject.SetActive(false);
        }
    }



    private void ShowMap()
    {
        MapLoader.Instance.SetContent(_map);
        uIController.OpenScreen(uIController.GetScreen<MainMenuScreen>());
    }


}
