using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private Maps _maps;
    [SerializeField] private Transform _planetContainer;
    [SerializeField] private Transform _planetsUIConteiner;
    
    private DataController _dataController;
    
    private void Start()
    {
        _dataController = DataController.Instance;
        SetMap(_maps.GetMap("Earth"));
    }

    public void SetMap(Map map)
    {
        _dataController.Data.Map = map.Name;
        _dataController.Save();
        Instantiate(map.Prefab, _planetContainer);
    }
}
