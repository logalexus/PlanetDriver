using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Sound", order = 1)]
public class Sounds : ScriptableObject
{
    [SerializeField] private AudioClip _mainTheme;
    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _engine;
    [SerializeField] private AudioClip _coin;
    [SerializeField] private AudioClip _collision;
    
    public AudioClip MainTheme => _mainTheme;
    public AudioClip Click => _click;
    public AudioClip Engine => _engine;
    public AudioClip Coin => _coin;
    public AudioClip Collision => _collision;
}
