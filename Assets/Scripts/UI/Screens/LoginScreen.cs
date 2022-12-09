using Cysharp.Threading.Tasks;
using Data;
using Data.Models;
using DG.Tweening;
using MySql.Data.MySqlClient;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScreen : UIScreen
{
    [SerializeField] private ScreenTransition _screenTransition;
    [SerializeField] private TMP_InputField mailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    
    private PopupFactory _popupFactory;
    private DataController _dataController;
    private UserRepository _userRepository;
    private PlanetRepository _planetRepository;
    private AutoRepository _autoRepository;
    private ProgressRepository _progressRepository;
    private SettingsRepository _settingsRepository;
    private AnalyticsRepository _analyticsRepository;
    private LoginUIController _loginUIController;
    
    public void Init(LoginUIController uiController)
    {
        _popupFactory = PopupFactory.Instance;
        _dataController = DataController.Instance;
        _userRepository = _dataController.UserRepository;
        _planetRepository = _dataController.PlanetRepository;
        _autoRepository = _dataController.AutoRepository;
        _progressRepository = _dataController.ProgressRepository;
        _settingsRepository = _dataController.SettingsRepository;
        _analyticsRepository = _dataController.AnalyticsRepository;
        _loginUIController = uiController;
        
        loginButton.onClick.AddListener(() => OnLoginWithMail());
        registerButton.onClick.AddListener(uiController.OpenRegisterScreen);
    }

    public async UniTask OnLoginWithMail()
    {
        if (InputValid())
        {
            string email = mailInput.text;
            string password = Utilities.GetHash(passwordInput.text);

            _popupFactory.ShowLoadingPopup();
            try
            {
                UserData user = await _userRepository.GetUserByEmail(email);

                if (user != null && user.Password == password)
                {
                    await _dataController.LoadData(user);
                    _popupFactory.ClosePopup();
                    SceneManager.LoadScene("Game");
                }
                else
                {
                    _popupFactory.ShowInfoPopup("Incorrect credentials");
                }
            }
            catch (MySqlException e)
            {
                _popupFactory.ShowInfoPopup(e.Message);
                throw;
            }
        }
        else
        {
        }
    }

    private bool InputValid()
    {
        bool mailValid = !string.IsNullOrEmpty(mailInput.text);
        bool passwordValid = !string.IsNullOrEmpty(passwordInput.text);
        
        if (!mailValid || !passwordValid)
        {
            _popupFactory.ShowInfoPopup("Not valid input");
            return false;
        }
        
        return true;
    }

    public override void Open()
    {
        base.Open();
        _screenTransition.OpenAnim();
    }

    public override void Close()
    {
        _screenTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }
}