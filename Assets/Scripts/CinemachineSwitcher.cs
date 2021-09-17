using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _aroundCamera;
    [SerializeField] private GameObject _playingCamera;
    

    private void Start()
    {
        GameController.Instance.GameStart += SwitchPlayingCamera;

        SwitchAroundCamera();
    }

    public void SwitchAroundCamera()
    {
        _aroundCamera.SetActive(true);
        _playingCamera.SetActive(false);
    }

    public void SwitchPlayingCamera()
    {
        _aroundCamera.SetActive(false);
        _playingCamera.SetActive(true);
    }
}
