using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private List<UIScreen> _screens = new List<UIScreen>();

    private UIScreen _activeScreen;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);
    }

    IEnumerator Start()
    {
        yield return null;

        GameController.Instance.GameOver += () =>
        {
            OpenScreen(GetScreen<LoseScreen>());
        };

        _activeScreen = GetScreen<MainMenuScreen>();
        _activeScreen.Open();
    }
    
    public void OpenScreen(UIScreen screen)
    {
        _activeScreen.Close();
        screen.Open();
        _activeScreen = screen;
    }

    public T GetScreen<T>() where T : UIScreen
    {
        return _screens.OfType<T>().FirstOrDefault();
    }
    
    public void PopupCall(UnityAction yesClick)
    {
        PopupScreen popup = GetScreen<PopupScreen>();
        popup.Open();
        popup.YesClick = yesClick;
        popup.YesClick += () => _activeScreen.SetInteractable(true);
        popup.NoClick += () => _activeScreen.SetInteractable(true);
        _activeScreen.SetInteractable(false);
    }
    
}
