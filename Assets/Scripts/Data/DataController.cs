using UnityEngine;

namespace Data
{
    public class DataController : MonoBehaviour
    {
        [SerializeField] private DbConnection dbConnection;
        [SerializeField] private UserRepository userRepository;


        public static DataController Instance;
    
        public GameData Data { get; private set; }
        public UserRepository UserRepository => userRepository;

        private Storage _storage;
        private Player _player;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            dbConnection.Init();
            userRepository.Init(dbConnection);

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
}