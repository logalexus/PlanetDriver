using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;

public class PlanetsScreen : UIScreen
{
    [SerializeField] private PlanetsScreenTransition _planetsScreenTransition;
    [SerializeField] private MapsHolder _mapsHolder;
    [SerializeField] private UIMap _uiMap;
    [SerializeField] private VerticalScrollSnap _scroller;
    [Header("Buttons")]
    [SerializeField] private Button _back;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _levelCounter;
    

    private IEnumerator Start()
    {
        Player player = Player.Instance;
        UIController uiController = UIController.Instance;

        _back.onClick.AddListener(() =>
        {
            uiController.OpenScreen(uiController.GetScreen<MainMenuScreen>());
        });

        player.CoinsChanged += () =>
        {
            _coinCounter.text = $"{player.Coins}$";
        };

        player.LevelChanged += () =>
        {
            _levelCounter.text = $"lvl {player.Level}";
        };

        yield return null;
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
            _scroller.GoToScreen(_mapsHolder.Contents.Count - 1);
            base.Close();
        });
    }

    private void SetMapsUI()
    {
        for (int i = _mapsHolder.Contents.Count - 1; i >= 0; i--)
        {
            UIMap mapUI = Instantiate(_uiMap);
            mapUI.SetMapUI(_mapsHolder.Contents[i] as Map);
            _scroller.AddChild(mapUI.gameObject);
        }
        _scroller.GoToScreen(_mapsHolder.Contents.Count - 1);
    }
}
