using System;
using UnityEngine;

namespace Example.MySql
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
            _activePopup = Instantiate(popupLoadingPrefab, transform);
            _activePopup.Open();
        }
        
        public void ShowInfoPopup(string message, string title = "Warning")
        {
            var popup = Instantiate(popupInfoPrefab, transform);
            popup.Init(message, title);
            
            _activePopup = popup;
            _activePopup.Open();
        }
        
        public void ShowApprovePopup(string message, string title, Action action)
        {
            var popup = Instantiate(popupApprovePrefab, transform);
            popup.Init(message, title, action);
            
            _activePopup = popup;
            _activePopup.Open();
        }

        public void ClosePopup()
        {
            _activePopup.Close();
            Destroy(_activePopup.gameObject, 1f);
        }
        
        
    }
}