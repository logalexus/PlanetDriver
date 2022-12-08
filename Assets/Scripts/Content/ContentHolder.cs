using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Data;
using NaughtyAttributes;

public abstract class ContentHolder<T> : ScriptableObject where T : Content
{
    [SerializeField] protected List<T> _contents;
    [SerializeField] protected DbConnection dbConnection;

    public List<T> Contents => _contents;

    public virtual T GetContent(string name)
    {
        return _contents.Find(x => x.Name == name);
    }

}
