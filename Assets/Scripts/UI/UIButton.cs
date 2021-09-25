using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButton : Button
{
    

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (IsActive() && IsInteractable())
        {
            AudioController.Instance.PlaySFX(AudioController.Instance.Sounds.Click);
        }
        base.OnPointerDown(eventData);
    }
}
