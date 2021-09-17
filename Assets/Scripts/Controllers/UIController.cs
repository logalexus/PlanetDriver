using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameController.Instance.GameOver += () =>
        {
            OpenScreen(GetScreen<LoseScreen>());
        };
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



    
}
