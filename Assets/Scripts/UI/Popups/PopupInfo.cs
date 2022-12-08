using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class PopupInfo : UIScreen
    {
        [SerializeField] private PopupScreenTransition _popupScreenTransition;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button okButton;


        public void Init(string message, string title)
        {
            titleText.text = title;
            messageText.text = message;
            
            okButton.onClick.AddListener(PopupFactory.Instance.ClosePopup);
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
}