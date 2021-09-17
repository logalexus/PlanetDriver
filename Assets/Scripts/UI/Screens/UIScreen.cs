using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    public void Awake()
    {
        _canvas.enabled = false;
    }

    public virtual void Open()
    {
        _canvas.enabled = true;
    }

    public virtual void Close()
    {
        _canvas.enabled = false;
    }
}
