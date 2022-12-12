using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public event UnityAction GameOver;
    public event UnityAction GameStart;
    public event UnityAction GameRestart;
    public event UnityAction GameInitial;

    private AudioController _audioController;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);

        Application.targetFrameRate = 70;
    }

    private void Start()
    {
        Player.Instance.CarCrush += OnGameEnd;
        _audioController = AudioController.Instance;
    }

    private void OnGameEnd()
    {
        GameOver?.Invoke();
        _audioController.PlayMusic(_audioController.Sounds.MainTheme);
    }

    public void OnGameStart()
    {
        GameStart?.Invoke();
        _audioController.StopMusic();

    }

    public void OnGameRestart()
    {
        GameRestart?.Invoke();
        OnGameStart();
    }

    public void OnGameToInitial()
    {
        GameRestart?.Invoke();
        GameInitial?.Invoke();
    }
}
