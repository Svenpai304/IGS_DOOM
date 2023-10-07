using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : IPoolable
{
    private List<T> _activePool = new List<T>();
    private List<T> _inactivePool = new List<T>();
    private int maxCapacity;

    public ObjectPool(int _maxCapacity) 
    {
        maxCapacity = _maxCapacity;
        Debug.Log(maxCapacity);
    }

    public T RequestObject()
    {
        if (_inactivePool.Count > 0)
        {
            return ActivateObject(_inactivePool[0]);
        }
        if (_activePool.Count < maxCapacity)
        {
            return ActivateObject(AddNewObject());
        }
        else
        {
            return RecycleObject();
        }
    }

    private T AddNewObject()
    {
        T obj = (T)Activator.CreateInstance(typeof(T));
        _inactivePool.Add(obj);
        return obj;
    }

    private T ActivateObject(T obj) 
    {
        obj.Active = true;
        obj.OnActivate();
        if (_inactivePool.Contains(obj))
        {
            _inactivePool.Remove(obj);
        }
        _activePool.Add(obj);
        return obj;
    }

    private T RecycleObject()
    {
        T obj = _activePool[0];
        _activePool.Remove(obj);
        _activePool.Add(obj);
        return obj;
    }
}
