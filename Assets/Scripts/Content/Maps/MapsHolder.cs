using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapsHolder", order = 1)]
public class MapsHolder : ContentHolder<Map>
{

}

[System.Serializable]
public class Map : Content
{
    public Planet Prefab;
    public Material SkyBox;
}
