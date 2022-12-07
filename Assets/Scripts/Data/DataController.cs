using UnityEngine;
using System.Collections;


public class DataController : MonoBehaviour
{
    public static DataController Instance;
    public GameData Data { get; private set; }

    private Storage _storage;
    private Player _player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

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
