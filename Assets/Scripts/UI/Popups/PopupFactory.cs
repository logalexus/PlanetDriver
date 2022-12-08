using System;
using UnityEngine;

namespace UI.Popups
{
    public class PopupFactory : MonoBehaviour
    {
        [SerializeField] private PopupLoading popupLoadingPrefab;
        [SerializeField] private PopupInfo popupInfoPrefab;
        [SerializeField] private PopupApprove popupApprovePrefab;

        public static PopupFactory Instance;

        private UIScreen _activePopup;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ShowLoadingPopup()
        {
            ClosePopup();
            _activePopup = Instantiate(popupLoadingPrefab, transform);
            _activePopup.Open();
        }

        public void ShowInfoPopup(string message, string title = "Warning")
        {
            ClosePopup();
            var popup = Instantiate(popupInfoPrefab, transform);
            popup.Init(message, title);

            _activePopup = popup;
            _activePopup.Open();
        }

        public void ShowApprovePopup(string message, string title, Action action)
        {
            ClosePopup();
            var popup = Instantiate(popupApprovePrefab, transform);
            popup.Init(message, title, action);

            _activePopup = popup;
            _activePopup.Open();
        }

        public void ClosePopup()
        {
            if (_activePopup == null)
                return;
            
            _activePopup.Close();
            Destroy(_activePopup.gameObject, 1f);
        }
    }
}