using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Maps", order = 1)]
public class Maps : ScriptableObject
{
    [SerializeField] private List<Map> _maps;
    //[SerializeField] private Map _earth;
    //[SerializeField] private Map _mars;

    //public Map Earth => _earth;
    //public Map Mars => _mars;

    public Map GetMap(string name)
    {
        return _maps.Find(x => x.Name == name);
    }
}

[System.Serializable]
public class Map
{
    public enum AccessType { Available = 1, Locked}

    public string Name;
    public GameObject Prefab;
    public int Cost;
    public AccessType Access;
    public Sprite Preview;
}
