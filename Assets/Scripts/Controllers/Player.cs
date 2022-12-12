using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Events;


public class Player : MonoBehaviour
{
    [SerializeField] private Transform _mainPosition;
    [SerializeField] private BoxCollider _collider;


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
            _dataController.Data.ProgressData.Money = value;
            CoinsChanged?.Invoke();
        }
    }
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            _dataController.Data.ProgressData.Level = value;
            LevelChanged?.Invoke();
        }
    }
    public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;
            _dataController.Data.ProgressData.Exp = value;
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
    
    public int CompletedDistance
    {
        get => _completedDistance;
        set
        {
            _completedDistance = value;
            CompletedDistanceChanged?.Invoke();
        }
    }

    public static Player Instance;

    public UnityAction CarCrush;
    public UnityAction CollectedCoinsInGameChanged;
    public UnityAction CoinsChanged;
    public UnityAction LevelChanged;
    public UnityAction ExpChanged;
    public UnityAction DistanceChanged;
    public UnityAction CompletedDistanceChanged;
    
    private bool _isCollisionAlready = false;
    private int _collectedCoinsInGame;
    private int _coins;
    private int _level;
    private int _exp;
    private int _distance;
    private int _completedDistance;
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
    }

    private void Start()
    {
        GameController.Instance.GameRestart += RestartPlayer;
        CarsLoader.Instance.CarChanged += ChangeCollider;
        _oldPos = transform.position;
        _dataController = DataController.Instance;
        _audioController = AudioController.Instance;
        StartCoroutine(CountMetres());
        StartCoroutine(InitStatsUI());
    }

    private void ChangeCollider(BoxCollider newCollider)
    {
        //var newCollider = transform.GetChild(0).GetComponent<BoxCollider>();
        _collider.size = newCollider.size;
        _collider.center = newCollider.center;
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
        transform.position = _mainPosition.position;
        transform.rotation = _mainPosition.rotation;
        _isCollisionAlready = false;
        CollectedCoinsInGame = 0;
        Distance = 0;
    }

    private IEnumerator InitStatsUI()
    {
        yield return null;
        Coins = _dataController.Data.ProgressData.Money;
        Level = _dataController.Data.ProgressData.Level;
        Exp = _dataController.Data.ProgressData.Exp;
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
        CollectedCoinsInGame += 10;
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
