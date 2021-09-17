using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PoolObjects<T> where T : MonoBehaviour
{
    public bool AutoExpand { get; set; }

    private T _prefab;
    private Transform _parent;
    private List<T> _pool;

    public PoolObjects(T prefab, int count)
    {
        _prefab = prefab;
        _parent = null;
        CreatePool(count);
    }

    public PoolObjects(T prefab, int count, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>();
        for (int i = 0; i < count; i++)
            CreateObject();

    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        T createdObject = Object.Instantiate(_prefab, _parent);
        createdObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var poolObject in _pool)
            if (!poolObject.gameObject.activeInHierarchy)
            {
                element = poolObject;
                poolObject.gameObject.SetActive(true);
                return true;
            }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out T element))
            return element;

        if (AutoExpand)
            return CreateObject(true);

        throw new System.Exception("No free elements");
    }
    
    public void DeactivateAll()
    {
        foreach (var element in _pool)
            element.gameObject.SetActive(false);
        
    }
}
