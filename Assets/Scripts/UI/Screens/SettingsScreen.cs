using System;
using DG.Tweening;
using Lean.Gui;
using System.Collections;
using Cysharp.Threading.Tasks;
using Data;
using MySql.Data.MySqlClient;
using UI.Popups;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScreen : UIScreen
{
    [SerializeField] private SettingsScreenTransition _settingsScreenTransition;
    [Header("Buttons")] [SerializeField] private Button _back;
    [SerializeField] private Button _logoutButton;
    [Header("Switchers")] [SerializeField] private LeanToggle _musicSwitcher;
    [SerializeField] private LeanToggle _soundSwitcher;
    [SerializeField] private LeanToggle _postProcSwitcher;
    [Header("Graphic")] [SerializeField] private GameObject _postProcessing;


    public bool MusicActive
    {
        get => _musicActive;
        set
        {
            _musicActive = value;
            _dataController.Data.SettingsData.MusicEnable = value;
            AudioController.Instance.SetMusicMute(!value);
            Save();
        }
    }

    public bool SoundActive
    {
        get => _soundActive;
        set
        {
            _soundActive = value;
            _dataController.Data.SettingsData.SoundEnable = value;
            AudioController.Instance.SetSoundMute(!value);
            Save();
        }
    }

    public bool PostProcActive
    {
        get => _postProcActive;
        set
        {
            _postProcActive = value;
            _dataController.Data.SettingsData.GraphicQuality = value;
            _postProcessing.SetActive(value);
            Save();
        }
    }

    private DataController _dataController;
    private bool _musicActive;
    private bool _soundActive;
    private bool _postProcActive;


    private void Start()
    {
        UIController uiController = UIController.Instance;
        _dataController = DataController.Instance;

        _back.onClick.AddListener(() => { uiController.OpenScreen(uiController.GetScreen<MainMenuScreen>()); });

        _logoutButton.onClick.AddListener(() => SceneManager.LoadScene("Login"));

        StartCoroutine(InitSettings());
    }

    private IEnumerator InitSettings()
    {
        yield return null;
        _musicActive = _dataController.Data.SettingsData.MusicEnable;
        _soundActive = _dataController.Data.SettingsData.SoundEnable;
        _postProcActive = _dataController.Data.SettingsData.GraphicQuality;
        _musicSwitcher.On = _musicActive;
        _soundSwitcher.On = _soundActive;
        _postProcSwitcher.On = _postProcActive;
    }

    public override void Open()
    {
        base.Open();
        _settingsScreenTransition.OpenAnim();
    }

    public override void Close()
    {
        _settingsScreenTransition.CloseAnim().OnComplete(() => { base.Close(); });
    }

    private async UniTask Save()
    {
        PopupFactory.Instance.ShowLoadingPopup();
        try
        {
            await _dataController.SaveSettings();
            PopupFactory.Instance.ClosePopup();
        }
        catch (MySqlException e)
        {
            PopupFactory.Instance.ShowInfoPopup(e.Message);
            throw;
        }
    }
}