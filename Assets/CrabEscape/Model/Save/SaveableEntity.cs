using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    [SerializeField] private string _id;
    public string Id => _id;

    private void Awake()
    {
        if (Id == String.Empty)
        {
            GenerateId();
        }
    }

    [ContextMenu("Generate ID")]
    private void GenerateId()
    {
        _id = Guid.NewGuid().ToString();
    }

    public object SaveState()
    {
        var state = new Dictionary<string, object>();
        foreach (var saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.SaveState();
        }

        return state;
    }

    public void LoadState(object state)
    {
        var stateDictionary = (Dictionary<string, object>)state;
        foreach (var saveable in GetComponents<ISaveable>())
        {
            string typeName = saveable.GetType().ToString();
            if (stateDictionary.TryGetValue(typeName, out object savedState))
            {
                saveable.LoadState(savedState);
            }
        }
    }
}
    
