using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class PopupScreen : UIScreen
{
    [SerializeField] private PopupScreenTransition _popupScreenTransition;
    [Header("Buttons")]
    [SerializeField] private Button _yes;
    [SerializeField] private Button _no;

    public UnityAction YesClick;
    public UnityAction NoClick;

    private void Start()
    {
        Player player = Player.Instance;
        UIController uiController = UIController.Instance;

        _yes.onClick.AddListener(() =>
        {
            YesClick?.Invoke();
            Close();
        });
        _no.onClick.AddListener(() =>
        {
            NoClick?.Invoke();
            Close();
        });
    }

    public override void Open()
    {
        base.Open();
        _popupScreenTransition.OpenAnim();
    }

    public override void Close()
    {
        _popupScreenTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }
}
