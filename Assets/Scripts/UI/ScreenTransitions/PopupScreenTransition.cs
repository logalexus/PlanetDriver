using UnityEngine;
using System.Collections;

public class PopupScreenTransition : ScreenTransition
{
    private void Awake()
    {
        _durationOpen = 0.2f;
        _durationClose = 0.1f;
    }
}
