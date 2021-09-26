using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Player : MonoBehaviour
{
    [SerializeField] private Transform MainPosition;

    public Vector3 ContactPosition { get; private set; }
    public int CollectedCoinsInGame
    {
        get => _collectedCoinsInGame;
        set
        {
            _collectedCoinsInGame = value;
            CollectedCoinsInGameChanged?.Invoke();
        }
    }
    public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            _dataController.Data.Coins = value;
            _dataController.Save();
            CoinsChanged?.Invoke();
        }
    }
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            _dataController.Data.Level = value;
            _dataController.Save();
            LevelChanged?.Invoke();
        }
    }
    public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;
            _dataController.Data.Exp = value;
            _dataController.Save();
            ExpChanged?.Invoke();
        }
    }
    public int Distance
    {
        get => _distance;
        set
        {
            _distance = value;
            DistanceChanged?.Invoke();
        }
    }

    public static Player Instance;

    public UnityAction CarCrush;
    public UnityAction CollectedCoinsInGameChanged;
    public UnityAction CoinsChanged;
    public UnityAction LevelChanged;
    public UnityAction ExpChanged;
    public UnityAction DistanceChanged;
    
    private bool _isCollisionAlready = false;
    private int _collectedCoinsInGame;
    private int _coins;
    private int _level;
    private int _exp;
    private int _distance;
    private int _maxExp = 1000;
    private Vector3 _oldPos;
    private DataController _dataController;
    private AudioController _audioController;
    
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
        GameController.Instance.GameRestart += RestartPlayer;
        _oldPos = transform.position;
        _dataController = DataController.Instance;
        _audioController = AudioController.Instance;
        StartCoroutine(CountMetres());
        StartCoroutine(InitStatsUI());
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Crush") && !_isCollisionAlready)
        {
            _audioController.PlaySFX(_audioController.Sounds.Collision);
            ContactPosition = collision.contacts[0].point;
            CarCrush?.Invoke();
            StopCoroutine(CountMetres());
            _isCollisionAlready = true;
        }
    }
    
    private void RestartPlayer()
    {
        transform.position = MainPosition.position;
        transform.rotation = MainPosition.rotation;
        _isCollisionAlready = false;
        CollectedCoinsInGame = 0;
        Distance = 0;
    }

    private IEnumerator InitStatsUI()
    {
        yield return null;
        Coins = _dataController.Data.Coins;
        Level = _dataController.Data.Level;
        Exp = _dataController.Data.Exp;
    }

    private IEnumerator CountMetres()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            float deltaDist = (transform.position - _oldPos).magnitude;
            _oldPos = transform.position;
            Distance += (int)deltaDist;
        }
    }

    public void CoinCollecting()
    {
        CollectedCoinsInGame+=1000;
        _audioController.PlaySFX(_audioController.Sounds.Coin);
    }

    public void AddExp(int value)
    {
        Exp += value;
        if (Exp >= _maxExp)
        {
            Level++;
            Exp -= _maxExp;
        }
    }

   
    
}
