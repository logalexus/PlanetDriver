using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MapLoader : MonoBehaviour
{
    public static MapLoader Instance;

    [SerializeField] private MapsHolder _mapsHolder;
    [SerializeField] private Transform _planetContainer;
    [SerializeField] private Transform _planetsUIConteiner;
    
    private DataController _dataController;
    private List<string> _availableMaps;
    
    public UnityAction<string> PlanetChanged;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);
    }

    private void Start()
    {
        _dataController = DataController.Instance;
        _availableMaps = _dataController.Data.AvailableMaps;
        SeAccessAvailableMaps(_availableMaps);

        if (_dataController.Data.Map != null)
            SetMap(_mapsHolder.GetMap(_dataController.Data.Map));
        else
            SetMap(_mapsHolder.Maps[0]);
    }

    public void SetMap(Map map)
    {
        _dataController.Data.Map = map.Name;
        _dataController.Save();

        if (_planetContainer.childCount != 0)
            Destroy(_planetContainer.GetChild(0).gameObject);
        
        Instantiate(map.Prefab, _planetContainer);
        RenderSettings.skybox = map.SkyBox;

        PlanetChanged?.Invoke(map.Name);
    }

    private void SeAccessAvailableMaps(List<string> availableMaps)
    {
        foreach (var map in availableMaps)
            _mapsHolder.GetMap(map).Access = true;
    }

    public void SaveAvailableMap(string mapName)
    {
        _availableMaps.Add(mapName);
        _dataController.Save();
    }
}
