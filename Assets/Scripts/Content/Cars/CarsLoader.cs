using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarsLoader : ContentLoader<Car>
{
    public static CarsLoader Instance;

    [SerializeField] private Transform _carContainer;

    public UnityAction<string> CarChanged;

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

        if (_dataController.Data.SavedCar != null)
            SetContent(_contentHolder.GetContent(_dataController.Data.SavedCar));
        else
            SetContent(_contentHolder.Contents[0]);
    }

    public override void SetContent(Content content)
    {
        Car car = content as Car;
        _dataController.Data.SavedCar = car.Name;
        _dataController.Save();

        if (_carContainer.childCount != 0)
            Destroy(_carContainer.GetChild(0).gameObject);

        Instantiate(car.Prefab, _carContainer);

        CarChanged?.Invoke(car.Name);
    }

    protected override void SetAvailableContents()
    {
        _availableContents = _dataController.Data.AvailableCars;
    }
}
