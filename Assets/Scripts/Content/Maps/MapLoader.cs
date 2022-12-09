using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MapLoader : ContentLoader<Map>
{
    public static MapLoader Instance;
    
    [SerializeField] private Transform _planetContainer;
    [SerializeField] private MapsHolder _mapsHolder;

    public Map CurrentMap { get; private set; }

    public UnityAction<string> PlanetChanged;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);
    }
    
    protected override void Start()
    {
        base.Start();

        // if (_dataController.Data.SavedMap != null)
        //     SetContent(_contentHolder.GetContent(_dataController.Data.SavedMap));
        // else
        //     SetContent(_contentHolder.Contents[0]);
    }

    protected override void SetAvailableContents()
    {
        //_availableContents = _dataController.Data.AvailableMaps;
    }
    
    public override void SetContent(Content content)
    {
        // CurrentMap = content as Map;
        // _dataController.Data.SavedMap = CurrentMap.Name;
        // _dataController.Save();
        //
        // if (_planetContainer.childCount != 0)
        //     Destroy(_planetContainer.GetChild(0).gameObject);
        //
        // Instantiate(CurrentMap.Prefab, _planetContainer);
        // RenderSettings.skybox = CurrentMap.SkyBox;
        //
        // PlanetChanged?.Invoke(CurrentMap.Name);
    }
    
}
