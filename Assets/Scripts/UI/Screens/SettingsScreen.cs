using DG.Tweening;
using Lean.Gui;
using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : UIScreen
{
    [SerializeField] private SettingsScreenTransition _settingsScreenTransition;
    [Header("Buttons")]
    [SerializeField] private Button _back;
    [Header("Switchers")]
    [SerializeField] private LeanToggle _musicSwitcher;
    [SerializeField] private LeanToggle _soundSwitcher;
    [SerializeField] private LeanToggle _postProcSwitcher;
    [Header("Graphic")]
    [SerializeField] private GameObject _postProcessing;


    public bool MusicActive
    {
        get => _musicActive;
        set
        {
            _musicActive = value;
            _dataController.Data.SettingsData.MusicEnable = value;
            _dataController.Save();
            AudioController.Instance.SetMusicMute(!value);
        }
    }
    public bool SoundActive
    {
        get => _soundActive;
        set
        {
            _soundActive = value;
            _dataController.Data.SettingsData.SoundEnable = value;
            _dataController.Save();
            AudioController.Instance.SetSoundMute(!value);
        }
    }
    public bool PostProcActive
    {
        get => _postProcActive;
        set
        {
            _postProcActive = value;
            _dataController.Data.SettingsData.GraphicQuality = value;
            _dataController.Save();
            _postProcessing.SetActive(value);
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
        
        _back.onClick.AddListener(() =>
        {
            uiController.OpenScreen(uiController.GetScreen<MainMenuScreen>());
        });

        StartCoroutine(InitSettings());
    }

    private IEnumerator InitSettings()
    {
        yield return null;
        MusicActive = _dataController.Data.SettingsData.MusicEnable;
        SoundActive = _dataController.Data.SettingsData.SoundEnable;
        PostProcActive = _dataController.Data.SettingsData.GraphicQuality;
        _musicSwitcher.On = MusicActive;
        _soundSwitcher.On = SoundActive;
        _postProcSwitcher.On = PostProcActive;
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
}
