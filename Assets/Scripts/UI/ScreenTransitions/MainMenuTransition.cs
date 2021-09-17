using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuTransition : ScreenTransition
{
    public override Sequence OpenAnim()
    {
        Sequence s = base.OpenAnim();
        s.Join(_canvasGroup.DOFade(1, _durationOpen));
        return s;
    }
    public override Sequence CloseAnim()
    {
        Sequence s = base.CloseAnim();
        s.Join(_canvasGroup.DOFade(0, _durationClose));
        return s;
    }

    
}
