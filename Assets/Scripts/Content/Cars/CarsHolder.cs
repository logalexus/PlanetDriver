using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CarsHolder", order = 1)]

public class CarsHolder : ContentHolder<Car>
{

    
}

[System.Serializable]
public class Car : Content
{
    public Auto Prefab;
}
