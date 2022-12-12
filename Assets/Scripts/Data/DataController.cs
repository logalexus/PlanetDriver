using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Data.Models;
using UnityEngine;
using Analytics = AnalyticsLogic.Analytics;

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


        private Analytics _analytics;
        private Storage _storage;
        private Player _player;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _analytics = Analytics.Instance;

            dbConnection.Init();
            userRepository.Init(dbConnection);
            planetRepository.Init(dbConnection);
            autoRepository.Init(dbConnection);
            analyticsRepository.Init(dbConnection);
            progressRepository.Init(dbConnection);
            settingsRepository.Init(dbConnection);

            StartCoroutine(AnalyticsSender());
        }

        public async UniTask LoadData(UserData user)
        {
            Data = new GameData();
            Data.UserData = user;
            Data.ProgressData = await progressRepository.GetProgress(user.Id);
            Data.SettingsData = await settingsRepository.GetSettings(user.Id);
            Data.PlanetsData = await planetRepository.GetAllPlanetTypes();
            Data.AutosData = await autoRepository.GetAllAutoTypes();
            Data.AvailablePlanetsData = await planetRepository.GetAvailablePlanets(user.Id);
            Data.AvailableAutosData = await autoRepository.GetAvailableAutos(user.Id);
        }

        public async UniTask SaveAutos()
        {
            await autoRepository.Update(Data.UserData.Id, Data.AvailableAutosData);
        }

        public async UniTask SavePlanets()
        {
            await planetRepository.Update(Data.UserData.Id, Data.AvailablePlanetsData);
        }

        public async UniTask SaveSettings()
        {
            await settingsRepository.Update(Data.UserData.Id, Data.SettingsData);
        }

        public async UniTask SaveProgress()
        {
            await progressRepository.Update(Data.UserData.Id, Data.ProgressData);
        }

        private async UniTask SaveAnalytics()
        {
            try
            {
                foreach (var action in _analytics.GetActions())
                    await analyticsRepository.AddAnalytics(Data.UserData.Id, action.Action, action.Time);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }
            

            _analytics.ClearQueue();
        }

        private IEnumerator AnalyticsSender()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f);
                SaveAnalytics();
            }
        }
    }
}