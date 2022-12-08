using System;
using UnityEngine;

namespace Example.MySql
{
    public class PopupFactory : MonoBehaviour
    {
        [SerializeField] private PopupLoading popupLoading;

        public static PopupFactory Instance;

        private UIScreen _activePopup;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ShowLoadingPopup()
        {
            _activePopup = Instantiate(popupLoading, transform);
            _activePopup.Open();
        }

        public void ClosePopup()
        {
            _activePopup.Close();
            Destroy(_activePopup.gameObject, 1f);
        }
        
        
    }
}