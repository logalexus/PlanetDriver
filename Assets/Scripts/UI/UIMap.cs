using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    [SerializeField] private GameObject _accessPanel;
    [SerializeField] private Image _mapPreview;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;
    [Header("Buttons")]
    [SerializeField] private Button _selectButton;

    public Map CurrentMap => _map;

    private Enums.AccessType _access;
    private Map _map;

    private void Start()
    {
        UIController uIController = UIController.Instance;

        _selectButton.onClick.AddListener(() =>
        {
            MapLoader.Instance.SetMap(_map);
            uIController.OpenScreen(uIController.GetScreen<MainMenuScreen>());
        });
    }

    public void SetMapUI(Map map)
    {
        _map = map;
        _name.text = map.Name;
        _cost.text = map.Cost.ToString();
        _mapPreview.sprite = map.Preview;
        _access = map.Access;

        switch (_access)
        {
            case Enums.AccessType.Available:
                _accessPanel.SetActive(false);
                break;
            case Enums.AccessType.Locked:
                _accessPanel.SetActive(true);
                break;
        }
    }

}
