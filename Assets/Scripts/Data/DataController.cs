using UnityEngine;
using System.Collections;
using Example.MySql;


public class DataController : MonoBehaviour
{
    [SerializeField] private DatabaseContext context;

    public static DataController Instance;
    public GameData Data { get; private set; }

    private Storage _storage;
    private Player _player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);

        context.Connect();
        
        _storage = new Storage();
        Load();
    }

    public void Save()
    {
        _storage.Save(Data);
    }

    public void Load()
    {
        Data = _storage.Load(new GameData()) as GameData;
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}