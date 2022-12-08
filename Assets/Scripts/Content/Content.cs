using UnityEngine;
using System.Collections;
using NaughtyAttributes;

[System.Serializable]
public class Content
{
    public int Id;
    public string Name;
    public int Cost;
    [System.NonSerialized] public bool Access;
    [ShowAssetPreview]
    public Sprite Preview;
}
