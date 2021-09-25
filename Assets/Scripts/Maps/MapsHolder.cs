using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Maps", order = 1)]
public class MapsHolder : ScriptableObject
{
    [SerializeField] private List<Map> _maps;

    public List<Map> Maps => _maps;
    
    
    public Map GetMap(string name)
    {
        return _maps.Find(x => x.Name == name);
    }
}

[System.Serializable]
public class Map
{
    public string Name;
    public Planet Prefab;
    public int Cost;
    public bool Access;
    public Sprite Preview;
    public Material SkyBox;
}
