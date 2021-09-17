using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashMover : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Camera _camera;

    private void Start()
    {
        GameController.Instance.GameOver += MoveSprite;
    }

    private void MoveSprite()
    {
        Vector3 screenPos = _camera.WorldToScreenPoint(Player.Instance.ContactPosition);
        _image.transform.position = screenPos;
    }


    
    
}
