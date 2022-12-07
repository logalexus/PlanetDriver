using System;
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
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.GameOver += () => { OpenScreen(GetScreen<LoseScreen>()); };
        StartCoroutine(InitUI());
    }

    private IEnumerator InitUI()
    {
        yield return null;
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