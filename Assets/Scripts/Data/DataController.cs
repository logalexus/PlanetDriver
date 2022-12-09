using Cysharp.Threading.Tasks;
using Data.Models;
using UnityEngine;

namespace Data
{
    public class DataController : MonoBehaviour
    {
        [SerializeField] private DbConnection dbConnection;
        [SerializeField] private UserRepository userRepository;
        [SerializeField] private PlanetRepository planetRepository;
        [SerializeField] private AutoRepository autoRepository;
        [SerializeField] private AnalyticsRepository analyticsRepository;
        [SerializeField] private ProgressRepository progressRepository;
        [SerializeField] private SettingsRepository settingsRepository;


        public static DataController Instance;
    
        public GameData Data { get; private set; }
        public UserRepository UserRepository => userRepository;
        public PlanetRepository PlanetRepository => planetRepository;
        public AutoRepository AutoRepository => autoRepository;
        public AnalyticsRepository AnalyticsRepository => analyticsRepository;
        public ProgressRepository ProgressRepository => progressRepository;
        public SettingsRepository SettingsRepository => settingsRepository;

        
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
            analyticsRepository.Init(dbConnection);
            progressRepository.Init(dbConnection);
            settingsRepository.Init(dbConnection);
        }

        public async UniTask LoadData(UserData user)
        {
            Data = new GameData();
            Data.UserData = user;
            Data.ProgressData = await progressRepository.GetProgress(user.Id);
            Data.SettingsData = await settingsRepository.GetSettings(user.Id);
            Data.PlanetsData = await planetRepository.GetAllPlanets();
            Data.AutosData = await autoRepository.GetAllAutos();
            Data.AvailablePlanetsData = await planetRepository.GetAvailablePlanets(user.Id);
            Data.AvailableAutosData = await autoRepository.GetAvailableAutos(user.Id);
        }

        public void Save()
        {
            
        }

        public void Load()
        {
            
        }

        private void OnApplicationQuit()
        {
            
        }
    }
}