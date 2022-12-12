using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MapLoader : MonoBehaviour
{
    public static MapLoader Instance;
    
    [SerializeField] private Transform _planetContainer;
    [SerializeField] private MapsHolder _mapsHolder;

    public PlanetData CurrentPlanet { get; private set; }

    public UnityAction<string> PlanetChanged;
    
    private DataController _dataController;

    
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
        
        PlanetData planetData = _dataController.Data.AvailablePlanetsData.Find(x => x.IsCurrent);
        
        SetContent(planetData);
    }

    
    public void SetContent(PlanetData planetData)
    {
        Map planet = _mapsHolder.GetContent(planetData.Name);
        
        CurrentPlanet = planetData;
        
        _dataController.Data.AvailablePlanetsData.ForEach(x=>x.IsCurrent = false);
        _dataController.Data.AvailablePlanetsData.Find(x=>x.Name == planetData.Name).IsCurrent = true;
        
        if (_planetContainer.childCount != 0)
            Destroy(_planetContainer.GetChild(0).gameObject);
        
        Instantiate(planet.Prefab, _planetContainer);
        RenderSettings.skybox = planet.SkyBox;
        
        PlanetChanged?.Invoke(planetData.Name);
    }
}
