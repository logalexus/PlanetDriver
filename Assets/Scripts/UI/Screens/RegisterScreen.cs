using System;
using Cysharp.Threading.Tasks;
using Data;
using DG.Tweening;
using MySql.Data.MySqlClient;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreen : UIScreen
{
    [SerializeField] private ScreenTransition _screenTransition;
    [SerializeField] private TMP_InputField mailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField repeatedPasswordInput;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button backButton;

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

        registerButton.onClick.AddListener(() => OnRegister());
        backButton.onClick.AddListener(uiController.OpenLoginScreen);
    }

    public async UniTaskVoid OnRegister()
    {
        if (InputValid())
        {
            string email = mailInput.text;
            string password = Utilities.GetHash(passwordInput.text);

            _popupFactory.ShowLoadingPopup();
            try
            {
                if (await _userRepository.CheckExistEmail(email))
                {
                    _popupFactory.ShowInfoPopup("Email already exists");
                    return;
                }

                int userId = await _userRepository.AddUser(email, password);

                await _planetRepository.AddPlanetToUser(userId, 1);
                await _autoRepository.AddAutoToUser(userId, 1);
                await _progressRepository.AddProgress(userId, 1000, 1, 0);
                await _settingsRepository.AddSettings(userId, true, true, false);
                
                if (userId != -1)
                {
                    _popupFactory.ShowInfoPopup("Registeration success");
                    _loginUIController.OpenLoginScreen();
                    ClearInputs();
                }
                else
                {
                    _popupFactory.ShowInfoPopup("Registeration failed");
                }
            }
            catch (MySqlException e)
            {
                _popupFactory.ShowInfoPopup(e.Message);
                throw;
            }
        }
    }

    private bool InputValid()
    {
        bool mailValid = !string.IsNullOrEmpty(mailInput.text);
        bool passwordValid = !string.IsNullOrEmpty(passwordInput.text);
        bool repeatedPasswordValid = !string.IsNullOrEmpty(repeatedPasswordInput.text);
        bool passwordMuch = passwordInput.text == repeatedPasswordInput.text;

        if (!mailValid || !passwordValid || !repeatedPasswordValid)
        {
            _popupFactory.ShowInfoPopup("Not valid input");
            return false;
        }

        if (!passwordMuch)
        {
            _popupFactory.ShowInfoPopup("Passwords do not match");
            return false;
        }

        return true;
    }

    private void ClearInputs()
    {
        mailInput.text = String.Empty;
        passwordInput.text = String.Empty;
        repeatedPasswordInput.text = String.Empty;
    }
    
    public override void Open()
    {
        base.Open();
        _screenTransition.OpenAnim();
    }

    public override void Close()
    {
        ClearInputs();
        _screenTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }
}