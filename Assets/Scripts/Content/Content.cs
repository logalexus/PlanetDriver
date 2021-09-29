using UnityEngine;
using System.Collections;

[System.Serializable]
public class Content
{
    public string Name;
    public int Cost;
    [System.NonSerialized] public bool Access;
    public Sprite Preview;
}
