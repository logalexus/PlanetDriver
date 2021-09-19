using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapLoader : MonoBehaviour
{
    public static MapLoader Instance;

    [SerializeField] private MapsHolder _maps;
    [SerializeField] private Transform _planetContainer;
    [SerializeField] private Transform _planetsUIConteiner;
    
    private DataController _dataController;

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
        SetMap(_maps.GetMap(_dataController.Data.Map));
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
}
