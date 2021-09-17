using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private Maps _maps;
    [SerializeField] private Transform _planetContainer;

    private DataController _dataController;

    private void Start()
    {
        _dataController = DataController.Instance;
        SetMap(_maps.Earth);
    }

    public void SetMap(Map map)
    {
        _dataController.Data.Map = map;
        _dataController.Save();
        Instantiate(map.Prefab, _planetContainer);
    }
}
