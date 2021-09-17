using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class InGameScreen : UIScreen
{
    [SerializeField] private InGameTransition _inGameTransition;
    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI _coinCounter;
    [SerializeField] private TextMeshProUGUI _distanceCounter;


    private void Start()
    {
        Player player = Player.Instance;
        player.CollectedCoinsInGameChanged += ()=> 
        {
            _coinCounter.text = "x" + player.CollectedCoinsInGame.ToString();
        };
        player.DistanceChanged += () =>
        {
            _distanceCounter.text = player.Distance.ToString() + "m";
        };
    }

    public override void Close()
    {
        _inGameTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }
    public override void Open()
    {
        base.Open();
        _inGameTransition.OpenAnim();
    }
    
    
}
