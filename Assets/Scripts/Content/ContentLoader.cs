using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine.Events;

public abstract class ContentLoader<T> : MonoBehaviour where T : Content
{
    [SerializeField] protected ContentHolder<T> _contentHolder;

    protected DataController _dataController;
    protected List<string> _availableContents;


    protected virtual void Start()
    {
        _dataController = DataController.Instance;
        SetAvailableContents();
        SetAccessAvailableContents();
    }

    public abstract void SetContent(Content content);

    protected abstract void SetAvailableContents();
    
    protected virtual void SetAccessAvailableContents()
    {
        foreach (var name in _availableContents)
            _contentHolder.GetContent(name).Access = true;
    }
}
