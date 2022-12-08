﻿using DG.Tweening;
using UnityEngine;

namespace Example.MySql
{
    public class PopupLoading : UIScreen
    {
        [SerializeField] private PopupScreenTransition _popupScreenTransition;


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
}