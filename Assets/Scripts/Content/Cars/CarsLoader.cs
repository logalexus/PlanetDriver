using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Models;
using UnityEngine;
using UnityEngine.Events;

public class CarsLoader : MonoBehaviour
{
    public static CarsLoader Instance;

    [SerializeField] private Transform _carContainer;
    [SerializeField] private CarsHolder carsHolder;

    public UnityAction<BoxCollider> CarChanged;

    private DataController _dataController;

    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);
    }

    protected  void Start()
    {
        _dataController = DataController.Instance;
        
        AutoData planetData = _dataController.Data.AvailableAutosData.Find(x => x.IsCurrent);
        
        SetContent(planetData);
    }

    public void SetContent(AutoData autoData)
    {
        Car car = carsHolder.GetContent(autoData.Name);
        
        _dataController.Data.AvailableAutosData.ForEach(x=>x.IsCurrent = false);
        _dataController.Data.AvailableAutosData.Find(x => x.Name == autoData.Name).IsCurrent = true;
        
        if (_carContainer.childCount != 0)
            Destroy(_carContainer.GetChild(0).gameObject);
        
        var auto = Instantiate(car.Prefab, _carContainer);
        
        CarChanged?.Invoke(auto.GetComponent<BoxCollider>());
    }
}
