using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class StoreScreen : UIScreen
{
    [SerializeField] private StoreScreenTransition _storeScreenTransition;
    [Header("Buttons")]
    [SerializeField] private Button _back;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _coinCounter;

    private void Start()
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
        
    }
    public override void Open()
    {
        base.Open();
        _storeScreenTransition.OpenAnim();
    }

    public override void Close()
    {
        _storeScreenTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }
}