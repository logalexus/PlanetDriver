using UnityEngine;

namespace Data
{
    public class DataController : MonoBehaviour
    {
        [SerializeField] private DbConnection dbConnection;
        [SerializeField] private UserRepository userRepository;
        [SerializeField] private PlanetRepository planetRepository;
        [SerializeField] private AutoRepository autoRepository;


        public static DataController Instance;
    
        public GameData Data { get; private set; }
        public UserRepository UserRepository => userRepository;
        public PlanetRepository PlanetRepository => planetRepository;
        public AutoRepository AutoRepository => autoRepository;

        private Storage _storage;
        private Player _player;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            dbConnection.Init();
            userRepository.Init(dbConnection);
            planetRepository.Init(dbConnection);
            autoRepository.Init(dbConnection);

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